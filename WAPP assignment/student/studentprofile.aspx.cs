using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.student
{
    public partial class studentprofile : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "student")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            lblProfileMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadProfileInfo();
                LoadMyAchievements();
            }
        }

        private void LoadProfileInfo()
        {
            int studentId = Convert.ToInt32(Session["UserID"]);
            string query = "SELECT FirstName, LastName, Username, Email, Avatar FROM Users WHERE UserID = @StudentID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string username = reader["Username"].ToString();

                            txtFirstName.Text = firstName;
                            txtLastName.Text = lastName;
                            txtUsername.Text = username;
                            litProfileName.Text = $"{firstName} {lastName}";

                            string avatar = "😊";
                            if (reader["Avatar"] != DBNull.Value)
                            {
                                avatar = reader["Avatar"].ToString();
                            }

                            litAvatar.Text = avatar;
                            hfSelectedAvatar.Value = avatar;

                            ScriptManager.RegisterStartupScript(this, GetType(), "SetAvatar",
                                $"setInitialAvatar('{avatar}');", true);
                        }
                    }
                }
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int studentId = Convert.ToInt32(Session["UserID"]);
            string newFirstName = txtFirstName.Text.Trim();
            string newLastName = txtLastName.Text.Trim();
            string newUsername = txtUsername.Text.Trim();

            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND UserID != @StudentID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(checkUserQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", newUsername);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    int userCount = (int)cmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        lblProfileMessage.Text = "That username is already taken. Please choose another.";
                        lblProfileMessage.CssClass = "profile-message error";
                        lblProfileMessage.Visible = true;
                        return;
                    }
                }

                string updateQuery = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Username = @Username WHERE UserID = @StudentID";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@FirstName", newFirstName);
                    updateCmd.Parameters.AddWithValue("@LastName", newLastName);
                    updateCmd.Parameters.AddWithValue("@Username", newUsername);
                    updateCmd.Parameters.AddWithValue("@StudentID", studentId);
                    updateCmd.ExecuteNonQuery();
                }
            }

            Session["FirstName"] = newFirstName;
            Session["LastName"] = newLastName;
            Session["Username"] = newUsername;
            Session["FullName"] = $"{newFirstName} {newLastName}";

            litProfileName.Text = $"{newFirstName} {newLastName}";

            lblProfileMessage.Text = "Profile updated successfully!";
            lblProfileMessage.CssClass = "profile-message success";
            lblProfileMessage.Visible = true;
        }

        protected void btnSaveAvatar_Click(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(Session["UserID"]);
            string newAvatar = hfSelectedAvatar.Value;

            string query = "UPDATE Users SET Avatar = @Avatar WHERE UserID = @StudentID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Avatar", newAvatar);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            litAvatar.Text = newAvatar;
            lblProfileMessage.Text = "Avatar updated!";
            lblProfileMessage.CssClass = "profile-message success";
            lblProfileMessage.Visible = true;
        }

        // --- THIS METHOD IS NOW FIXED ---
        private void LoadMyAchievements()
        {
            int studentId = Convert.ToInt32(Session["UserID"]);

            // This query now correctly gets ALL achievements and
            // joins the ones the student has earned (SA.EarnedAt)
            string query = @"
                SELECT 
                    A.Name,
                    A.Description,
                    A.BadgeType,
                    SA.EarnedAt
                FROM Achievements A
                LEFT JOIN StudentAchievements SA ON A.AchievementID = SA.AchievementID 
                                                AND SA.StudentID = @StudentID
                ORDER BY SA.EarnedAt DESC, A.Name;";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    conn.Open();
                    rptAchievements.DataSource = cmd.ExecuteReader();
                    rptAchievements.DataBind();
                }
            }


            if (rptAchievements.Items.Count == 0)
            {
                // pnlNoBadges.Visible = true; 
            }
        }

        // --- HELPER FUNCTIONS ---

        protected string GetAchievementClass(object earnedAt)
        {
            if (earnedAt == DBNull.Value || earnedAt == null)
            {
                return "locked";
            }
            return "earned";
        }

        protected string GetBadgeIcon(object earnedAt, object badgeType)
        {
            if (earnedAt == DBNull.Value || earnedAt == null)
            {
                return "❓"; // Locked
            }

            if (badgeType != null && badgeType.ToString() == "Global")
            {
                return "<span class='badge-icon' style='color: #3498db; font-size: 2.5rem;'>★</span>"; // Blue Star
            }
            return "<span class='badge-icon' style='color: #f39c12; font-size: 2.5rem;'>★</span>"; // Yellow Star
        }

        protected string GetEarnedDate(object earnedAt)
        {
            if (earnedAt == DBNull.Value || earnedAt == null)
            {
                return "Keep playing to unlock!";
            }
            DateTime date = Convert.ToDateTime(earnedAt);
            return $"Earned: {date:MMM d, yyyy}";
        }
    }
}