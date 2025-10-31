using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment
{
    public partial class signup : System.Web.UI.Page
    {
        // Get connection string from Web.config
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear any previous messages when page loads
            if (!IsPostBack)
            {
                lblMessage.Text = "";
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Check if page validation passed
            if (!Page.IsValid)
                return;

            // Get form data
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // Check if username already exists
            if (!IsUsernameAvailable(username))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "This username is already taken.";
                return;
            }

            // Check if email already exists
            if (!IsEmailAvailable(email))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "This email is already registered.";
                return;
            }

            // Generate salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash password using PBKDF2
            string passwordHash = HashPasswordPBKDF2(password, salt);

            // Insert new user into database
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Users (FirstName, LastName, Username, Email, PasswordHash, Salt, Role, CreatedAt, IsActive) 
                                    VALUES (@FirstName, @LastName, @Username, @Email, @PasswordHash, @Salt, @Role, GETDATE(), 1)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Salt", Convert.ToBase64String(salt));
                        cmd.Parameters.AddWithValue("@Role", role);

                        cmd.ExecuteNonQuery();

                        // Success message
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Registration successful! You can now login.";

                        // Clear form fields
                        ClearFormFields();
                    }
                }
                catch (SqlException ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                    {
                        lblMessage.Text = "Email or username already exists.";
                    }
                    else
                    {
                        lblMessage.Text = "Registration failed. Please try again.";
                        // Log the error for debugging (optional)
                        System.Diagnostics.Debug.WriteLine("SQL Error: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "An error occurred. Please try again.";
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Server-side validation for username
        protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string username = args.Value.Trim();
            args.IsValid = IsUsernameAvailable(username);
        }

        // Check if username is available
        private bool IsUsernameAvailable(string username)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        int count = (int)cmd.ExecuteScalar();
                        return count == 0;
                    }
                }
                catch
                {
                    // If error occurs, assume username is not available for safety
                    return false;
                }
            }
        }

        // Check if email is available
        private bool IsEmailAvailable(string email)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count == 0;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        // Hash password with PBKDF2
        private string HashPasswordPBKDF2(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }

        // Clear all form fields after successful registration
        private void ClearFormFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            ddlRole.SelectedIndex = 0;
        }
    }
}