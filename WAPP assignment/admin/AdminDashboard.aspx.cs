using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_assignment.admin
{
    public partial class AdminDashboard : System.Web.UI.Page
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
                string adminName = Session["FirstName"] != null ? Session["FirstName"].ToString() : "Admin";
                litAdminName.Text = adminName;
                if (!string.IsNullOrEmpty(adminName))
                {
                    litAvatar.Text = adminName.Substring(0, 1).ToUpper();
                }

                LoadStats();
            }
        }

        private void LoadStats()
        {
            // Updated query to also get new users (joined in last 7 days)
            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM Users) AS TotalUsers,
                    (SELECT COUNT(*) FROM Quizzes WHERE Status = 'Pending') AS PendingQuizzes,
                    (SELECT COUNT(*) FROM Users WHERE CreatedAt >= DATEADD(day, -7, GETDATE())) AS NewUsers
            ";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            litTotalUsers.Text = reader["TotalUsers"].ToString();
                            litPendingQuizzes.Text = reader["PendingQuizzes"].ToString();
                            litNewUsers.Text = reader["NewUsers"].ToString();
                        }
                    }
                }
            }
        }
    }
}