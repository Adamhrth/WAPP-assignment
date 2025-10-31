using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.teacher // Make sure this namespace matches your project
{
    public partial class ManageQuiz : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- SECURITY CHECK ---
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "teacher")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["QuizID"], out int quizId))
            {
                Response.Redirect("teacherdashboard.aspx"); // No ID, go back
                return;
            }

            int teacherId = Convert.ToInt32(Session["UserID"]);

            // --- OWNERSHIP CHECK ---
            // Ensure this teacher owns this quiz
            if (!IsPostBack) // Only run this check on the first load
            {
                if (!VerifyQuizOwnership(quizId, teacherId))
                {
                    Response.Redirect("teacherdashboard.aspx"); // Not your quiz, go back
                    return;
                }

                hfQuizID.Value = quizId.ToString();
                LoadQuizInfo(quizId);
                LoadQuestions(quizId);
            }
        }

        private bool VerifyQuizOwnership(int quizId, int teacherId)
        {
            string query = "SELECT COUNT(*) FROM Quizzes WHERE QuizID = @QuizID AND TeacherID = @TeacherID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void LoadQuizInfo(int quizId)
        {
            string query = "SELECT Title FROM Quizzes WHERE QuizID = @QuizID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    conn.Open();
                    litQuizTitle.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void LoadQuestions(int quizId)
        {
            // This query finds the question and its one correct answer
            string query = @"
                SELECT Q.QuestionID, Q.QuestionText, O.OptionText AS CorrectAnswer
                FROM Questions Q
                JOIN Options O ON Q.QuestionID = O.QuestionID
                WHERE Q.QuizID = @QuizID AND O.IsCorrect = 1
                ORDER BY Q.QuestionID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    rptQuestions.DataSource = reader;
                    rptQuestions.DataBind();

                    int questionCount = rptQuestions.Items.Count;
                    litQuestionCount.Text = questionCount.ToString();
                    pnlNoQuestions.Visible = questionCount == 0;
                }
            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            // Make sure all required fields are filled
            if (!Page.IsValid) return;

            // Only add options that are not empty
            var options = new List<Tuple<string, bool>>();
            int correctIndex = Convert.ToInt32(rblCorrectAnswer.SelectedValue);

            if (!string.IsNullOrWhiteSpace(txtOption1.Text))
                options.Add(new Tuple<string, bool>(txtOption1.Text.Trim(), correctIndex == 1));
            if (!string.IsNullOrWhiteSpace(txtOption2.Text))
                options.Add(new Tuple<string, bool>(txtOption2.Text.Trim(), correctIndex == 2));
            if (!string.IsNullOrWhiteSpace(txtOption3.Text))
                options.Add(new Tuple<string, bool>(txtOption3.Text.Trim(), correctIndex == 3));
            if (!string.IsNullOrWhiteSpace(txtOption4.Text))
                options.Add(new Tuple<string, bool>(txtOption4.Text.Trim(), correctIndex == 4));

            // Validate that the selected correct answer is not an empty textbox
            if (options.Find(o => o.Item2) == null)
            {
                lblMessage.Text = "The selected correct answer cannot be empty.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int quizId = Convert.ToInt32(hfQuizID.Value);
            string questionText = txtQuestionText.Text.Trim();

            // --- DATABASE TRANSACTION ---
            // This ensures that the Question AND all its Options are saved, or none are.
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Insert the Question and get its new ID
                    int newQuestionID;
                    string queryQuestion = "INSERT INTO Questions (QuizID, QuestionText) VALUES (@QuizID, @QuestionText); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmdQ = new SqlCommand(queryQuestion, conn, transaction))
                    {
                        cmdQ.Parameters.AddWithValue("@QuizID", quizId);
                        cmdQ.Parameters.AddWithValue("@QuestionText", questionText);
                        newQuestionID = Convert.ToInt32(cmdQ.ExecuteScalar());
                    }

                    // 2. Insert all the Options
                    foreach (var option in options)
                    {
                        string queryOption = "INSERT INTO Options (QuestionID, OptionText, IsCorrect) VALUES (@QuestionID, @OptionText, @IsCorrect);";
                        using (SqlCommand cmdO = new SqlCommand(queryOption, conn, transaction))
                        {
                            cmdO.Parameters.AddWithValue("@QuestionID", newQuestionID);
                            cmdO.Parameters.AddWithValue("@OptionText", option.Item1);
                            cmdO.Parameters.AddWithValue("@IsCorrect", option.Item2);
                            cmdO.ExecuteNonQuery();
                        }
                    }

                    // 3. If all succeeded, commit the transaction
                    transaction.Commit();

                    lblMessage.Text = "Question added successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    ClearForm();
                }
                catch (Exception ex)
                {
                    // 4. If any step failed, roll back all changes
                    transaction.Rollback();
                    lblMessage.Text = "Error adding question: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

            // 5. Reload the list of questions
            LoadQuestions(quizId);
        }

        protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteQuestion")
            {
                int questionId = Convert.ToInt32(e.CommandArgument);

                // Because of "ON DELETE CASCADE" in our database,
                // deleting the question will automatically delete all its options.
                string query = "DELETE FROM Questions WHERE QuestionID = @QuestionID";

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuestionID", questionId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh the list
                LoadQuestions(Convert.ToInt32(hfQuizID.Value));
            }
        }

        private void ClearForm()
        {
            txtQuestionText.Text = "";
            txtOption1.Text = "";
            txtOption2.Text = "";
            txtOption3.Text = "";
            txtOption4.Text = "";
            rblCorrectAnswer.ClearSelection();
        }
    }
}