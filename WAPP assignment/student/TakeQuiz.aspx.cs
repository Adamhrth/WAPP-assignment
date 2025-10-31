using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.student
{
    // These two simple classes will help us store the questions in ViewState
    [Serializable]
    public class QuizQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public List<QuizOption> Options { get; set; }
        public int SelectedOptionID { get; set; } // To store the student's answer
    }

    [Serializable]
    public class QuizOption
    {
        public int OptionID { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }


    public partial class TakeQuiz : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        // We use ViewState to store the list of questions between button clicks
        private List<QuizQuestion> CurrentQuizQuestions
        {
            get => ViewState["QuizQuestions"] as List<QuizQuestion>;
            set => ViewState["QuizQuestions"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- SECURITY CHECK ---
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "student")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Get QuizID from the URL
                if (Request.QueryString["QuizID"] == null || !int.TryParse(Request.QueryString["QuizID"], out int quizId))
                {
                    ShowMessage("Invalid Quiz", "This quiz could not be found.");
                    return;
                }

                // 1. Create a new attempt in the database
                int studentId = Convert.ToInt32(Session["UserID"]);
                int attemptId = CreateQuizAttempt(quizId, studentId);
                if (attemptId == 0)
                {
                    ShowMessage("Error", "Could not start the quiz. Please try again.");
                    return;
                }
                hfAttemptID.Value = attemptId.ToString();

                // 2. Load all quiz questions and options into ViewState
                LoadQuiz(quizId);

                // 3. Display the first question
                if (CurrentQuizQuestions.Count > 0)
                {
                    DisplayQuestion(0); // Display the first question
                }
                else
                {
                    ShowMessage("Empty Quiz", "This quiz has no questions. Please let your teacher know.");
                }
            }
        }

        private int CreateQuizAttempt(int quizId, int studentId)
        {
            string query = "INSERT INTO QuizAttempts (StudentID, QuizID, StartedAt) VALUES (@StudentID, @QuizID, GETDATE()); SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@QuizID", quizId);
                        conn.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception) { return 0; }
        }

        private void LoadQuiz(int quizId)
        {
            var questions = new List<QuizQuestion>();
            string quizTitle = "";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                // First, get quiz title and its questions
                string queryQuestions = "SELECT QuizID, Title FROM Quizzes WHERE QuizID = @QuizID; " +
                                        "SELECT QuestionID, QuestionText FROM Questions WHERE QuizID = @QuizID";
                using (SqlCommand cmd = new SqlCommand(queryQuestions, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Read Quiz Title
                        if (reader.Read())
                        {
                            quizTitle = reader["Title"].ToString();
                        }

                        // Move to the next result set (Questions)
                        reader.NextResult();
                        while (reader.Read())
                        {
                            questions.Add(new QuizQuestion
                            {
                                QuestionID = Convert.ToInt32(reader["QuestionID"]),
                                QuestionText = reader["QuestionText"].ToString(),
                                Options = new List<QuizOption>()
                            });
                        }
                    }
                }

                // Second, get all options for all questions in this quiz
                string queryOptions = @"
                    SELECT O.OptionID, O.QuestionID, O.OptionText, O.IsCorrect 
                    FROM Options O
                    JOIN Questions Q ON O.QuestionID = Q.QuestionID
                    WHERE Q.QuizID = @QuizID";
                using (SqlCommand cmd = new SqlCommand(queryOptions, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int questionId = Convert.ToInt32(reader["QuestionID"]);
                            // Find the question this option belongs to
                            QuizQuestion matchingQuestion = questions.Find(q => q.QuestionID == questionId);
                            if (matchingQuestion != null)
                            {
                                matchingQuestion.Options.Add(new QuizOption
                                {
                                    OptionID = Convert.ToInt32(reader["OptionID"]),
                                    OptionText = reader["OptionText"].ToString(),
                                    IsCorrect = Convert.ToBoolean(reader["IsCorrect"])
                                });
                            }
                        }
                    }
                }
            }

            litQuizTitle.Text = quizTitle;
            CurrentQuizQuestions = questions; // Save the whole list to ViewState
        }

        private void DisplayQuestion(int questionIndex)
        {
            if (questionIndex >= CurrentQuizQuestions.Count) return;

            QuizQuestion question = CurrentQuizQuestions[questionIndex];

            // Update labels
            lblQuestionText.Text = question.QuestionText;
            litQuestionNumber.Text = (questionIndex + 1).ToString();
            litTotalQuestions.Text = CurrentQuizQuestions.Count.ToString();

            // Bind options to the RadioButtonList
            rblOptions.Items.Clear();
            rblOptions.DataSource = question.Options;
            rblOptions.DataTextField = "OptionText";
            rblOptions.DataValueField = "OptionID";
            rblOptions.DataBind();

            // Set the current question index in the hidden field
            hfCurrentQuestionIndex.Value = questionIndex.ToString();

            // Change button text on the last question
            if (questionIndex == CurrentQuizQuestions.Count - 1)
            {
                btnNext.Text = "Submit Quiz";
                btnNext.CssClass = "btn-quiz-next btn-quiz-submit";
            }
            else
            {
                btnNext.Text = "Next Question";
                btnNext.CssClass = "btn-quiz-next";
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            // 1. Get current question index
            int currentIndex = Convert.ToInt32(hfCurrentQuestionIndex.Value);

            // 2. Validate: Did the user select an answer?
            if (rblOptions.SelectedValue == "")
            {
                lblError.Visible = true;
                return;
            }
            lblError.Visible = false;

            // 3. Save the selected answer to our list in ViewState
            int selectedOptionId = Convert.ToInt32(rblOptions.SelectedValue);
            CurrentQuizQuestions[currentIndex].SelectedOptionID = selectedOptionId;

            // 4. Check if we are on the last question
            if (currentIndex == CurrentQuizQuestions.Count - 1)
            {
                // This was the "Submit" click. Grade the quiz.
                SubmitQuiz();
            }
            else
            {
                // 5. Not the last question. Show the next question.
                DisplayQuestion(currentIndex + 1);
            }
        }

        private void SubmitQuiz()
        {
            int correctCount = 0;
            // Loop through all questions in ViewState
            foreach (QuizQuestion question in CurrentQuizQuestions)
            {
                // Find the option the student selected
                QuizOption selectedOption = question.Options.Find(o => o.OptionID == question.SelectedOptionID);
                if (selectedOption != null && selectedOption.IsCorrect)
                {
                    correctCount++;
                }
            }

            // Calculate score
            decimal score = 0;
            if (CurrentQuizQuestions.Count > 0)
            {
                score = (decimal)correctCount / CurrentQuizQuestions.Count * 100;
            }

            // Save score to the database
            int attemptId = Convert.ToInt32(hfAttemptID.Value);
            string query = "UPDATE QuizAttempts SET Score = @Score, CompletedAt = GETDATE() WHERE AttemptID = @AttemptID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Redirect to a results page (we need to create this next)
            Response.Redirect($"QuizResults.aspx?AttemptID={attemptId}");
        }

        private void ShowMessage(string title, string body)
        {
            pnlQuiz.Visible = false; // Hide the quiz
            pnlMessage.Visible = true; // Show the message panel
            msgHeader.InnerText = title;
            msgBody.InnerText = body;
        }
    }
}