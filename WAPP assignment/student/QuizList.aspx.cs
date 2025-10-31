using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment
{
    public partial class QuizList : System.Web.UI.Page
    {
        private int categoryId = 0;

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- SECURITY CHECK ---
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] ||
                Session["UserRole"].ToString() != "student")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }
            // --- END SECURITY CHECK ---

            // Try to get the CategoryID from the URL (Query String)
            if (Request.QueryString["CategoryID"] == null || !int.TryParse(Request.QueryString["CategoryID"], out categoryId))
            {
                // If no valid ID, just show all quizzes
                // You could also redirect: Response.Redirect("studentpage.aspx");
            }

            if (!IsPostBack)
            {
                LoadCategoryInfo();
                LoadQuizList();
            }
        }

        private void LoadCategoryInfo()
        {
            if (categoryId == 0)
            {
                litCategoryName.Text = "All Available Quizzes";
                litCategoryIcon.Text = "🌟";
                return;
            }

            string query = "SELECT Name FROM Categories WHERE CategoryID = @CategoryID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string categoryName = result.ToString();
                        litCategoryName.Text = "Quizzes in " + categoryName;
                        litCategoryIcon.Text = GetCategoryIcon(categoryName); // Use helper
                    }
                }
            }
        }

        private void LoadQuizList()
        {
            // This query joins Quizzes with Users to get the teacher's name
            string query = @"
                SELECT 
                    Q.QuizID, 
                    Q.Title, 
                    Q.Description, 
                    (U.FirstName + ' ' + U.LastName) AS TeacherName 
                FROM Quizzes Q 
                JOIN Users U ON Q.TeacherID = U.UserID 
                WHERE Q.Status = 'Approved'"; // <-- ONLY show approved quizzes

            // If a specific category was selected, add it to the query
            if (categoryId > 0)
            {
                query += " AND Q.CategoryID = @CategoryID";
            }

            query += " ORDER BY Q.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (categoryId > 0)
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    }
                    conn.Open();
                    rptQuizzes.DataSource = cmd.ExecuteReader();
                    rptQuizzes.DataBind();
                }
            }

            // Show the "No Quizzes" message if the repeater is empty
            if (rptQuizzes.Items.Count == 0)
            {
                pnlNoQuizzes.Visible = true;
            }
        }

        protected void rptQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Start")
            {
                string quizId = e.CommandArgument.ToString();
                // Redirect to the page that will let the student take the quiz
                // We will build this page next!
                Response.Redirect($"TakeQuiz.aspx?QuizID={quizId}");
            }
        }

        // Helper function to show the right icon
        protected string GetCategoryIcon(string categoryName)
        {
            switch (categoryName.ToLower())
            {
                case "english":
                    return "📖";
                case "maths":
                    return "🔢";
                case "science":
                    return "🔬";
                case "art & craft":
                    return "🎨";
                default:
                    return "📚";
            }
        }
    }
}