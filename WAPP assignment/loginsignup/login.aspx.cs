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

            // Get the authentication status (it returns a string now)
            string authStatus = AuthenticateUser(username, password);

            if (authStatus == "SUCCESS")
            {
                // Authentication was successful. The AuthenticateUser method
                // already handled the session and redirect.
            }
            else if (authStatus == "INACTIVE")
            {
                // specific error message
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Your account is pending approval or has been deactivated. Please contact an admin.";
            }
            else // (authStatus == "INVALID")
            {
      
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid username or password.";
            }
        }

        // --- THIS METHOD IS UPDATED to return a string ---
        private string AuthenticateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();

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
                                // User was found. First, check if they are active.
                                bool isActive = Convert.ToBoolean(reader["IsActive"]);
                                if (!isActive)
                                {
                                    // --- UPDATED ---
                                    // Return the "INACTIVE" status code
                                    return "INACTIVE";
                                }

                                // User is active, so now check their password
                                string storedHash = reader["PasswordHash"].ToString();
                                string saltString = reader["Salt"].ToString();
                                byte[] salt = Convert.FromBase64String(saltString);
                                string enteredHash = HashPasswordPBKDF2(password, salt);

                                if (enteredHash == storedHash)
                                {
                                    // Password is correct! Create session.
                                    int userId = Convert.ToInt32(reader["UserID"]);
                                    string firstName = reader["FirstName"].ToString();
                                    string lastName = reader["LastName"].ToString();
                                    string userUsername = reader["Username"].ToString();
                                    string role = reader["Role"].ToString();

                                    Session["UserID"] = userId;
                                    Session["Username"] = userUsername;
                                    Session["FirstName"] = firstName;
                                    Session["LastName"] = lastName;
                                    Session["FullName"] = firstName + " " + lastName;
                                    Session["UserRole"] = role;
                                    Session["IsAuthenticated"] = true;

                                    // Redirect based on role
                                    RedirectUserByRole(role);

                                    // --- UPDATED ---
                                    return "SUCCESS";
                                }
                                else
                                {
                                    // --- UPDATED ---
                                    // Wrong password
                                    return "INVALID";
                                }
                            }
                            else
                            {
                                // --- UPDATED ---
                                // User not found
                                return "INVALID";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Login Error: " + ex.Message);
                    // --- UPDATED ---
                    return "INVALID";
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