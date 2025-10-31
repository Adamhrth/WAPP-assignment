using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.admin
{
    public partial class AdminQuizApproval : System.Web.UI.Page
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
                LoadPendingQuizzes();
            }
        }

        private void LoadPendingQuizzes()
        {
            string query = @"
                SELECT 
                    Q.QuizID, Q.Title, Q.Status,
                    (U.FirstName + ' ' + U.LastName) AS TeacherName,
                    C.Name AS CategoryName,
                    (SELECT COUNT(*) FROM Questions QN WHERE QN.QuizID = Q.QuizID) AS QuestionCount
                FROM Quizzes Q
                JOIN Users U ON Q.TeacherID = U.UserID
                JOIN Categories C ON Q.CategoryID = C.CategoryID
                WHERE Q.Status = 'Pending'
                ORDER BY Q.CreatedAt";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    rptPendingQuizzes.DataSource = cmd.ExecuteReader();
                    rptPendingQuizzes.DataBind();
                }
            }
            pnlNoPendingQuizzes.Visible = rptPendingQuizzes.Items.Count == 0;
        }

        protected void rptPendingQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int quizId = Convert.ToInt32(e.CommandArgument);
            string newStatus = "";

            if (e.CommandName == "ApproveQuiz")
            {
                newStatus = "Approved";
            }
            else if (e.CommandName == "RejectQuiz")
            {
                newStatus = "Rejected";
            }
            else
            {
                return;
            }

            string query = "UPDATE Quizzes SET Status = @Status WHERE QuizID = @QuizID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadPendingQuizzes(); // Refresh the list
        }
    }
}