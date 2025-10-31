using System;
using System.Configuration;
using System.Data; // Required for SqlTransaction
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.admin
{
    public partial class AdminUserManagement : System.Web.UI.Page
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
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            string query = "SELECT UserID, FirstName, LastName, Email, Role, IsActive, CreatedAt FROM Users ORDER BY Role, LastName";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    rptUsers.DataSource = cmd.ExecuteReader();
                    rptUsers.DataBind();
                }
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userIdToChange = Convert.ToInt32(e.CommandArgument);
            int currentAdminId = Convert.ToInt32(Session["UserID"]);

            // Security check: Admin cannot change their own status or remove themselves
            if (userIdToChange == currentAdminId)
            {
                return;
            }

            if (e.CommandName == "DeactivateUser" || e.CommandName == "ActivateUser")
            {
                bool newActiveState = (e.CommandName == "ActivateUser");
                string query = "UPDATE Users SET IsActive = @IsActive WHERE UserID = @UserID";

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsActive", newActiveState);
                        cmd.Parameters.AddWithValue("@UserID", userIdToChange);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            // --- NEW CODE BLOCK ---
            else if (e.CommandName == "RemoveUser")
            {
                // Call the new method to permanently delete the user
                PermanentlyRemoveUser(userIdToChange);
            }
            // --- END NEW CODE BLOCK ---

            LoadUsers(); // Refresh the list
        }

        // --- NEW METHOD ---
        private void PermanentlyRemoveUser(int userId)
        {
            // We must use a transaction to ensure all data is deleted or none is.
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get the user's role first
                    string role = "";
                    string queryGetRole = "SELECT Role FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmdRole = new SqlCommand(queryGetRole, conn, transaction))
                    {
                        cmdRole.Parameters.AddWithValue("@UserID", userId);
                        object result = cmdRole.ExecuteScalar();
                        if (result != null)
                        {
                            role = result.ToString();
                        }
                    }

                    if (role == "student")
                    {
                        // 1. Delete student's achievements
                        string queryAch = "DELETE FROM StudentAchievements WHERE StudentID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(queryAch, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Delete student's quiz attempts
                        string queryAtt = "DELETE FROM QuizAttempts WHERE StudentID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(queryAtt, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (role == "teacher")
                    {
                        // 1. Delete the teacher's quizzes. This will cascade delete
                        // Questions and Options due to our database rules.
                        string queryQuiz = "DELETE FROM Quizzes WHERE TeacherID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(queryQuiz, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Finally, delete the user from the Users table
                    string queryUser = "DELETE FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(queryUser, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    // 4. If all commands succeeded, commit the transaction
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // If any command failed, roll back all changes
                    transaction.Rollback();
                    // You could show an error message here
                }
            }
        }

        // --- ASPX Helper Functions (Unchanged) ---
        protected string GetAvatarInitial(object firstName)
        {
            if (firstName != null && !string.IsNullOrEmpty(firstName.ToString()))
            {
                return firstName.ToString().Substring(0, 1).ToUpper();
            }
            return "?";
        }

        protected string GetRoleClass(object role)
        {
            string roleStr = role.ToString().ToLower();
            if (roleStr == "student") return "role-badge-student";
            if (roleStr == "teacher") return "role-badge-teacher";
            if (roleStr == "admin") return "role-badge-admin";
            return "";
        }

        protected string GetStatusClass(object isActive)
        {
            if (isActive != null && (bool)isActive)
            {
                return "status-active";
            }
            return "status-inactive";
        }

        protected string GetStatusText(object isActive)
        {
            if (isActive != null && (bool)isActive)
            {
                return "Active";
            }
            return "Inactive";
        }
    }
}