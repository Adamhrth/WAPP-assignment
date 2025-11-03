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

        // --- NEW --- Stores the current filter in ViewState
        private string CurrentFilter
        {
            get { return ViewState["QuizFilter"] as string ?? "all_active"; }
            set { ViewState["QuizFilter"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "teacher")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string teacherName = Session["FullName"] != null ? Session["FullName"].ToString() : "T";
                if (!string.IsNullOrEmpty(teacherName))
                {
                    litAvatar.Text = teacherName.Substring(0, 1).ToUpper();
                }

                // --- UPDATED --- Load "All" (which is now all non-archived) by default
                LoadQuizzes(CurrentFilter);
                SetActiveFilterTab(btnAllQuizzes);
            }
        }

        private void LoadQuizzes(string filter)
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);

            string query = @"
                SELECT 
                    Q.QuizID, Q.Title, Q.Status,
                    (SELECT COUNT(DISTINCT QA.StudentID) FROM QuizAttempts QA WHERE QA.QuizID = Q.QuizID AND QA.CompletedAt IS NOT NULL) AS StudentCount,
                    (SELECT COUNT(*) FROM Questions QN WHERE QN.QuizID = Q.QuizID) AS QuestionCount
                FROM Quizzes Q
                WHERE Q.TeacherID = @TeacherID";

            // --- UPDATED FILTER LOGIC ---
            if (filter == "all_active")
            {
                // "All" now means all *except* archived
                query += " AND Q.Status != 'Archived'";
            }
            else
            {
                // Filter for a specific status
                query += " AND Q.Status = @Status";
            }
            query += " ORDER BY Q.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    if (filter != "all_active")
                    {
                        cmd.Parameters.AddWithValue("@Status", filter);
                    }

                    conn.Open();
                    rptQuizzes.DataSource = cmd.ExecuteReader();
                    rptQuizzes.DataBind();
                }
            }

            pnlNoQuizzes.Visible = rptQuizzes.Items.Count == 0;
            CurrentFilter = filter; // Save the current filter
        }

        // --- FILTER BUTTON CLICK HANDLERS (Updated) ---

        protected void btnAllQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("all_active"); // "All" now means all non-archived
            SetActiveFilterTab(btnAllQuizzes);
        }

        protected void btnActiveQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Approved");
            SetActiveFilterTab(btnActiveQuizzes);
        }

        protected void btnDraftQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Pending");
            SetActiveFilterTab(btnDraftQuizzes);
        }

        // --- NEW BUTTON HANDLER ---
        protected void btnArchivedQuizzes_Click(object sender, EventArgs e)
        {
            LoadQuizzes("Archived");
            SetActiveFilterTab(btnArchivedQuizzes);
        }


        // --- UPDATED HELPER METHOD ---
        private void SetActiveFilterTab(LinkButton activeBtn)
        {
            btnAllQuizzes.CssClass = "filter-tab";
            btnActiveQuizzes.CssClass = "filter-tab";
            btnDraftQuizzes.CssClass = "filter-tab";
            btnArchivedQuizzes.CssClass = "filter-tab"; // Added
            activeBtn.CssClass = "filter-tab active";
        }

        // --- REPEATER COMMAND HANDLER (Updated) ---

        protected void rptQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int quizId = Convert.ToInt32(e.CommandArgument);
            int teacherId = Convert.ToInt32(Session["UserID"]);

            if (e.CommandName == "ArchiveQuiz")
            {
 
                string query = "UPDATE Quizzes SET Status = 'Archived' WHERE QuizID = @QuizID AND TeacherID = @TeacherID";

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
            }
            else if (e.CommandName == "RestoreQuiz")
            {
 
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
            }

            LoadQuizzes(CurrentFilter);
        }

        // --- UPDATED HELPER METHOD ---
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
                    return "archived"; // New style
                default:
                    return "draft";
            }
        }
    }
}