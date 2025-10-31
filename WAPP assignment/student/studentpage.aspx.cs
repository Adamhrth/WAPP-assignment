using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment
{
    public partial class studentpage : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- SECURITY CHECK ---
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "student")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }
            // --- END SECURITY CHECK ---

            if (!IsPostBack)
            {
                LoadStudentInfo();
                LoadCategories();
            }
        }

        private void LoadStudentInfo()
        {
            // Load student's name and initial from Session
            string studentName = Session["FirstName"] != null ? Session["FirstName"].ToString() : "Student";
            litStudentName.Text = studentName;

            if (!string.IsNullOrEmpty(studentName))
            {
                litProfileAvatar.Text = studentName.Substring(0, 1).ToUpper();
            }
        }

        private void LoadCategories()
        {
            // Fetch categories that have at least one 'Approved' quiz
            string query = @"
                SELECT DISTINCT
                    C.CategoryID,
                    C.Name,
                    C.Description
                FROM Categories C
                JOIN Quizzes Q ON C.CategoryID = Q.CategoryID
                WHERE Q.Status = 'Approved'
                ORDER BY C.Name";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    rptCategories.DataSource = cmd.ExecuteReader();
                    rptCategories.DataBind();
                }
            }
        }

        // This event handler runs when any button inside the repeater is clicked
        protected void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Get the CategoryID that we stored in the CommandArgument
                string categoryId = e.CommandArgument.ToString();

                // Redirect to the new page, passing the CategoryID in the URL
                Response.Redirect($"QuizList.aspx?CategoryID={categoryId}");
            }
        }

        // Helper function for the "View More" button
        protected void btnViewMoreSubjects_Click(object sender, EventArgs e)
        {
            // You can make this page later, for now it just re-routes
            Response.Redirect("QuizList.aspx");
        }

        // --- Helper functions for styling the repeater ---
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

        protected string GetButtonText(string categoryName)
        {
            switch (categoryName.ToLower())
            {
                case "english":
                    return "Let's Read!";
                case "maths":
                    return "Let's Count!";
                case "science":
                    return "Let's Explore!";
                case "art & craft":
                    return "Let's Create!";
                default:
                    return "Start Learning!";
            }
        }
    }
}