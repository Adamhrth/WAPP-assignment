using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment // Ensure this matches your project's namespace
{
    public partial class teacherCourse : System.Web.UI.Page
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
            // --- END SECURITY CHECK ---

            if (!IsPostBack)
            {
                // Set Avatar
                string teacherName = Session["FullName"] != null ? Session["FullName"].ToString() : "T";
                if (!string.IsNullOrEmpty(teacherName))
                {
                    litAvatar.Text = teacherName.Substring(0, 1).ToUpper();
                }

                // Load all quizzes by default
                LoadQuizzes("all");
            }
        }

        private void LoadQuizzes(string filter)
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);

            // This query is from your dashboard, it gets all the counts
            string query = @"
                SELECT 
                    Q.QuizID, 
                    Q.Title, 
                    Q.Status,
                    (SELECT COUNT(DISTINCT QA.StudentID) FROM QuizAttempts QA WHERE QA.QuizID = Q.QuizID AND QA.CompletedAt IS NOT NULL) AS StudentCount,
                    (SELECT COUNT(*) FROM Questions QN WHERE QN.QuizID = Q.QuizID) AS QuestionCount
                FROM Quizzes Q
                WHERE Q.TeacherID = @TeacherID";

            // Add the filter logic
            if (filter != "all")
            {
                query += " AND Q.Status = @Status";
            }
            query += " ORDER BY Q.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    if (filter != "all")
                    {
                        cmd.Parameters.AddWithValue("@Status", filter);
                    }

                    conn.Open();
                    rptQuizzes.DataSource = cmd.ExecuteReader();
                    rptQuizzes.DataBind();
                }
            }

            // Show a message if no quizzes were found
            pnlNoQuizzes.Visible = rptQuizzes.Items.Count == 0;
        }

        // --- FILTER BUTTON CLICK HANDLERS ---

        protected void btnAllQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("all");
            SetActiveFilterTab(btnAllQuizzes);
        }

        protected void btnActiveQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Approved"); // "Active" in the UI is "Approved" in the DB
            SetActiveFilterTab(btnActiveQuizzes);
        }

        protected void btnDraftQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Pending"); // "Draft" in the UI is "Pending" in the DB
            SetActiveFilterTab(btnDraftQuizzes);
        }

        protected void btnRejectedQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Rejected"); // "Archived" in your UI, but "Rejected" is in our DB
            SetActiveFilterTab(btnRejectedQuizzes);
        }

        // Helper to set the 'active' class on the clicked tab
        private void SetActiveFilterTab(LinkButton activeBtn)
        {
            btnAllQuizzes.CssClass = "filter-tab";
            btnActiveQuizzes.CssClass = "filter-tab";
            btnDraftQuizzes.CssClass = "filter-tab";
            btnRejectedQuizzes.CssClass = "filter-tab";
            activeBtn.CssClass = "filter-tab active";
        }

        // --- REPEATER COMMAND HANDLER ---

        protected void rptQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteQuiz")
            {
                int quizId = Convert.ToInt32(e.CommandArgument);
                int teacherId = Convert.ToInt32(Session["UserID"]);

                // We must delete from Questions first if cascade isn't set,
                // but our DB design has ON DELETE CASCADE, so we just delete the quiz.
                string query = "DELETE FROM Quizzes WHERE QuizID = @QuizID AND TeacherID = @TeacherID";

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuizID", quizId);
                        cmd.Parameters.AddWithValue("@TeacherID", teacherId); // Security check
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh the list
                LoadQuizzes("all");
                SetActiveFilterTab(btnAllQuizzes);
            }
        }

        // Helper function for ASPX page to set CSS class for quiz status
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
                    return "rejected"; // You'll need to add a .course-badge-rejected class to your CSS
                default:
                    return "draft";
            }
        }
    }
}