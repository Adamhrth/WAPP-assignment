using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment // Ensure this matches your project's namespace
{
    public partial class teacherStudent : System.Web.UI.Page
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

                // Load the dropdown list of quizzes
                LoadQuizFilterDropdown();
                // Load the student list
                LoadStudentList();
            }
        }

        private void LoadQuizFilterDropdown()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);
            string query = "SELECT QuizID, Title FROM Quizzes WHERE TeacherID = @TeacherID ORDER BY Title";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    ddlQuizFilter.DataSource = cmd.ExecuteReader();
                    ddlQuizFilter.DataTextField = "Title";
                    ddlQuizFilter.DataValueField = "QuizID";
                    ddlQuizFilter.DataBind();
                }
            }
            // Add a default "All Quizzes" item at the top
            ddlQuizFilter.Items.Insert(0, new ListItem("All Quizzes", "0"));
        }

        // This runs when the "Filter" button is clicked
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadStudentList();
        }

        private void LoadStudentList()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);
            string searchTerm = txtSearch.Text.Trim();
            int quizId = Convert.ToInt32(ddlQuizFilter.SelectedValue);

            // Base query: gets all completed attempts for this teacher's quizzes
            string query = @"
                SELECT 
                    U.UserID AS StudentID, 
                    U.FirstName + ' ' + U.LastName AS StudentName,
                    U.Email,
                    Q.Title AS QuizTitle,
                    A.Score,
                    A.CompletedAt
                FROM QuizAttempts A
                JOIN Users U ON A.StudentID = U.UserID
                JOIN Quizzes Q ON A.QuizID = Q.QuizID
                WHERE Q.TeacherID = @TeacherID AND A.CompletedAt IS NOT NULL
            ";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                // We use a List<string> to build the WHERE clauses
                var whereClauses = new List<string>();
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TeacherID", teacherId)
                };

                // Add quiz filter if one is selected
                if (quizId > 0)
                {
                    whereClauses.Add("Q.QuizID = @QuizID");
                    parameters.Add(new SqlParameter("@QuizID", quizId));
                }

                // Add search filter if text is entered
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    whereClauses.Add("(U.FirstName LIKE @Search OR U.LastName LIKE @Search OR U.Email LIKE @Search)");
                    parameters.Add(new SqlParameter("@Search", $"%{searchTerm}%"));
                }

                // Append all filters to the base query
                if (whereClauses.Count > 0)
                {
                    query += " AND " + string.Join(" AND ", whereClauses);
                }

                query += " ORDER BY A.CompletedAt DESC";

                // Execute the query
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    conn.Open();
                    rptStudents.DataSource = cmd.ExecuteReader();
                    rptStudents.DataBind();
                }
            }

            // Show a message if no students were found
            phNoStudents.Visible = rptStudents.Items.Count == 0;
        }

        // --- HELPER FUNCTIONS for the Repeater ---

        protected string GetAvatarInitial(object studentName)
        {
            if (studentName != null && !string.IsNullOrEmpty(studentName.ToString()))
            {
                return studentName.ToString().Substring(0, 1).ToUpper();
            }
            return "S";
        }

        protected string GetScoreBadgeClass(object scoreObj)
        {
            if (scoreObj == DBNull.Value || scoreObj == null) return "grade-c"; // Default

            decimal score = Convert.ToDecimal(scoreObj);
            if (score >= 90) return "grade-a"; // A+ / A
            if (score >= 80) return "grade-b"; // B+ / B
            if (score >= 70) return "grade-c"; // C+ / C

            return "grade-f"; // D / F (You'll need to add .grade-f to your CSS)
        }
    }
}