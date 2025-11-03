using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls; // This is needed for divNoQuizzes
using System.Web.UI.WebControls; // This is needed for the repeaters

namespace WAPP_assignment
{
    public partial class teacherdashboard : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "teacher")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int teacherId = Convert.ToInt32(Session["UserID"]);
                string teacherName = Session["FullName"].ToString();

                litWelcome.Text = "Welcome, " + teacherName;
                if (!string.IsNullOrEmpty(teacherName))
                {
                    litAvatar.Text = teacherName.Substring(0, 1).ToUpper();
                }

                LoadStats(teacherId);
                LoadQuizzes(teacherId);
                LoadRecentActivity(teacherId);
            }
        }

        private void LoadStats(int teacherId)
        {
            string query = @"
                SELECT
                    (SELECT COUNT(*) FROM Quizzes WHERE TeacherID = @TeacherID AND Status = 'Approved') AS ActiveQuizCount,
                    (SELECT COUNT(A.AttemptID) 
                     FROM QuizAttempts A 
                     JOIN Quizzes Q ON A.QuizID = Q.QuizID 
                     WHERE Q.TeacherID = @TeacherID AND A.CompletedAt IS NOT NULL) AS TotalCompletions";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            litActiveQuizzes.Text = reader["ActiveQuizCount"].ToString();
                            litTotalCompletions.Text = reader["TotalCompletions"].ToString();
                        }
                    }
                }
            }
        }

        private void LoadQuizzes(int teacherId)
        {
            string query = @"
                SELECT 
                    Q.QuizID, 
                    Q.Title, 
                    Q.Status,
                    (SELECT COUNT(DISTINCT QA.StudentID) FROM QuizAttempts QA WHERE QA.QuizID = Q.QuizID AND QA.CompletedAt IS NOT NULL) AS StudentCount,
                    (SELECT COUNT(*) FROM Questions QN WHERE QN.QuizID = Q.QuizID) AS QuestionCount
                FROM Quizzes Q
                WHERE Q.TeacherID = @TeacherID AND Q.Status != 'Archived'
                ORDER BY Q.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    rptQuizzes.DataSource = cmd.ExecuteReader();
                    rptQuizzes.DataBind();
                }
            }

            if (rptQuizzes.Items.Count == 0)
            {
                divNoQuizzes.Visible = true;
            }
        }

        private void LoadRecentActivity(int teacherId)
        {
            string query = @"
                SELECT TOP 5
                    A.AttemptID,
                    A.StudentID,
                    A.Score,
                    A.CompletedAt,
                    U.FirstName + ' ' + U.LastName AS StudentName,
                    SUBSTRING(U.FirstName, 1, 1) AS Initial,
                    Q.Title AS QuizTitle
                FROM QuizAttempts A
                JOIN Users U ON A.StudentID = U.UserID
                JOIN Quizzes Q ON A.QuizID = Q.QuizID
                WHERE Q.TeacherID = @TeacherID AND A.CompletedAt IS NOT NULL
                ORDER BY A.CompletedAt DESC";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    rptStudentActivity.DataSource = cmd.ExecuteReader();
                    rptStudentActivity.DataBind();
                }
            }

            if (rptStudentActivity.Items.Count == 0)
            {
                litNoActivity.Visible = true;
                litNoActivity.Text = @"<tr><td colspan='4' style='text-align:center; padding: 20px;'>
                                        No students have completed your quizzes yet.
                                      </td></tr>";
            }
        }

        protected void rptQuizzes_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ResubmitQuiz")
            {
                int quizId = Convert.ToInt32(e.CommandArgument);
                int teacherId = Convert.ToInt32(Session["UserID"]);

                string query = "UPDATE Quizzes SET Status = 'Pending' WHERE QuizID = @QuizID AND TeacherID = @TeacherID";

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuizID", quizId);
                        cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadStats(teacherId);
                LoadQuizzes(teacherId);
                LoadRecentActivity(teacherId);
            }
        }

        protected string GetStatusClass(object statusObj)
        {
            string status = statusObj.ToString().ToLower();
            switch (status)
            {
                case "approved":
                    return "active";
                case "pending":
                    return "draft";
                case "rejected":
                    return "rejected";
                case "archived":
                    return "archived";
                default:
                    return "draft";
            }
        }
    }
}