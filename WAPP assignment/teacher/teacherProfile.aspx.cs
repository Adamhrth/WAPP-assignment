using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_assignment.teacher // Make sure this namespace matches your project
{
    public partial class teacherProfile : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                LoadProfileInfo();
                LoadTeacherStats();
            }
        }

        private void LoadProfileInfo()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);
            string query = "SELECT FirstName, LastName, Email FROM Users WHERE UserID = @TeacherID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string email = reader["Email"].ToString();

                            // Set all the labels and textboxes
                            txtFirstName.Text = firstName;
                            txtLastName.Text = lastName;
                            txtEmail.Text = email;

                            litProfileName.Text = $"{firstName} {lastName}";
                            litProfileEmail.Text = email;

                            string initial = string.IsNullOrEmpty(firstName) ? "T" : firstName.Substring(0, 1).ToUpper();
                            litAvatarHeader.Text = initial;
                            litAvatarLarge.Text = initial;
                        }
                    }
                }
            }
        }

        private void LoadTeacherStats()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]);
            // This query gets both stats at once
            string query = @"
                SELECT
                    (SELECT COUNT(*) FROM Quizzes WHERE TeacherID = @TeacherID AND Status = 'Approved') AS ActiveQuizCount,
                    (SELECT COUNT(DISTINCT StudentID) 
                     FROM QuizAttempts A 
                     JOIN Quizzes Q ON A.QuizID = Q.QuizID 
                     WHERE Q.TeacherID = @TeacherID) AS TotalStudentCount";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            litActiveQuizzes.Text = reader["ActiveQuizCount"].ToString();
                            litTotalStudents.Text = reader["TotalStudentCount"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int teacherId = Convert.ToInt32(Session["UserID"]);
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE UserID = @TeacherID";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Update Session variables so the changes appear everywhere
                Session["FirstName"] = firstName;
                Session["FullName"] = $"{firstName} {lastName}";

                // Reload the header info
                litProfileName.Text = $"{firstName} {lastName}";
                litProfileEmail.Text = email;
                string initial = string.IsNullOrEmpty(firstName) ? "T" : firstName.Substring(0, 1).ToUpper();
                litAvatarHeader.Text = initial;
                litAvatarLarge.Text = initial;

                lblMessage.Text = "Profile updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            catch (SqlException ex)
            {
                // Handle duplicate email error
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    lblMessage.Text = "An account with this email already exists.";
                }
                else
                {
                    lblMessage.Text = "An error occurred. " + ex.Message;
                }
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}