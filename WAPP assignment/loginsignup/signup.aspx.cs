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
            if (!Page.IsValid)
                return;

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // (The username/email check methods are fine)

            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            string passwordHash = HashPasswordPBKDF2(password, salt);

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();

                    // --- UPDATED QUERY ---
                    // Removed 'IsActive' from the query. It will now use the database default (which is 0).
                    string query = @"INSERT INTO Users (FirstName, LastName, Username, Email, PasswordHash, Salt, Role, CreatedAt) 
                             VALUES (@FirstName, @LastName, @Username, @Email, @PasswordHash, @Salt, @Role, GETDATE())";

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

                        // --- UPDATED MESSAGE ---
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Registration successful! Your account is now pending admin approval.";

                        ClearFormFields();
                    }
                }
                catch (SqlException ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        lblMessage.Text = "Email or username already exists.";
                    }
                    else
                    {
                        lblMessage.Text = "Registration failed. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "An error occurred. Please try again.";
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