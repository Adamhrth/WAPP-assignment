using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.admin
{
    public partial class AdminManageQuestions : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "admin")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAllQuestions();
            }
        }

        private void LoadAllQuestions()
        {
            // This query joins Questions, Quizzes, and Users to get all info
            string query = @"
                SELECT 
                    Q.QuestionID, 
                    Q.QuestionText,
                    QZ.Title AS QuizTitle,
                    (U.FirstName + ' ' + U.LastName) AS TeacherName
                FROM Questions Q
                JOIN Quizzes QZ ON Q.QuizID = QZ.QuizID
                JOIN Users U ON QZ.TeacherID = U.UserID
                ORDER BY QZ.Title, Q.QuestionID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    rptQuestions.DataSource = cmd.ExecuteReader();
                    rptQuestions.DataBind();
                }
            }
            pnlNoQuestions.Visible = rptQuestions.Items.Count == 0;
        }

        protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteQuestion")
            {
                int questionId = Convert.ToInt32(e.CommandArgument);

                // Because of "ON DELETE CASCADE" in our 'Options' table,
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
                LoadAllQuestions();
            }
        }

        // --- ASPX Helper Function ---
        protected string GetAvatarInitial(object name)
        {
            if (name != null && !string.IsNullOrEmpty(name.ToString()))
            {
                return name.ToString().Substring(0, 1).ToUpper();
            }
            return "?";
        }
    }
}