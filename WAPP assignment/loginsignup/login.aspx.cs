using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace WAPP_assignment
{
    public partial class login : System.Web.UI.Page
    {
        // Get connection string from Web.config
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- THIS IS THE UPDATED CODE ---
            // Check if the user clicked a "Logout" link
            if (Request.QueryString["action"] == "logout")
            {
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Destroy the session
                Response.Redirect("login.aspx"); // Redirect back to the login page
                return; // Stop the page from loading further
            }
            // --- END OF UPDATE ---

            // Clear message on page load
            if (!IsPostBack)
            {
                lblMessage.Text = "";
            }
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            // Validate input
            if (!Page.IsValid)
                return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Try to authenticate user
            if (AuthenticateUser(username, password))
            {
                // Authentication successful - user is redirected in AuthenticateUser method
            }
            else
            {
                // Authentication failed
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid username or password.";
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();

                    // Query to get user by username (not email)
                    string query = @"SELECT UserID, FirstName, LastName, Username, PasswordHash, Salt, Role, IsActive 
                                     FROM Users 
                                     WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check if account is active
                                bool isActive = Convert.ToBoolean(reader["IsActive"]);
                                if (!isActive)
                                {
                                    lblMessage.Text = "Your account has been deactivated. Please contact admin.";
                                    return false;
                                }

                                // Get stored password hash and salt
                                string storedHash = reader["PasswordHash"].ToString();
                                string saltString = reader["Salt"].ToString();
                                byte[] salt = Convert.FromBase64String(saltString);

                                // Hash the entered password with the stored salt
                                string enteredHash = HashPasswordPBKDF2(password, salt);

                                // Compare hashes
                                if (enteredHash == storedHash)
                                {
                                    // Password is correct - create session
                                    int userId = Convert.ToInt32(reader["UserID"]);
                                    string firstName = reader["FirstName"].ToString();
                                    string lastName = reader["LastName"].ToString();
                                    string userUsername = reader["Username"].ToString();
                                    string role = reader["Role"].ToString();

                                    // Store user info in session
                                    Session["UserID"] = userId;
                                    Session["Username"] = userUsername;
                                    Session["FirstName"] = firstName;
                                    Session["LastName"] = lastName;
                                    Session["FullName"] = firstName + " " + lastName;
                                    Session["UserRole"] = role;
                                    Session["IsAuthenticated"] = true;

                                    // Redirect based on role
                                    RedirectUserByRole(role);
                                    return true;
                                }
                                else
                                {
                                    // Incorrect password
                                    return false;
                                }
                            }
                            else
                            {
                                // User not found
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error for debugging
                    System.Diagnostics.Debug.WriteLine("Login Error: " + ex.Message);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "An error occurred. Please try again.";
                    return false;
                }
            }
        }

        private void RedirectUserByRole(string role)
        {
            switch (role.ToLower())
            {
                case "student":
                    Response.Redirect("../student/studentpage.aspx", false); // The correct path
                    break;
                case "teacher":
                    Response.Redirect("../teacher/teacherDashboard.aspx", false);
                    break;
                case "admin":
                    Response.Redirect("../admin/AdminDashboard.aspx", false); // <-- THE CORRECT PATH
                    break;
                default:
                    Response.Redirect("Default.aspx", false); // A default page
                    break;
            }
            Context.ApplicationInstance.CompleteRequest();
        }

        // Hash password with PBKDF2 (same method as signup)
        private string HashPasswordPBKDF2(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }
    }
}