using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment.admin
{
    public partial class AdminManageBadges : System.Web.UI.Page
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
                LoadCategories();
                LoadAchievements();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT CategoryID, Name FROM Categories ORDER BY Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    ddlCategory.DataSource = cmd.ExecuteReader();
                    ddlCategory.DataTextField = "Name";
                    ddlCategory.DataValueField = "CategoryID";
                    ddlCategory.DataBind();
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("All Categories", "0"));
        }

        private void LoadAchievements()
        {
            // UPDATED: Removed BadgeImageURL, added BadgeType
            string query = @"
                SELECT 
                    A.AchievementID, A.Name, A.Description, A.BadgeType,
                    A.QuizCountThreshold,
                    ISNULL(C.Name, 'All Categories') AS CategoryName,
                    (SELECT COUNT(*) FROM StudentAchievements SA WHERE SA.AchievementID = A.AchievementID) AS EarnedCount
                FROM Achievements A
                LEFT JOIN Categories C ON A.CategoryID = C.CategoryID
                ORDER BY A.Name";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    rptAchievements.DataSource = cmd.ExecuteReader();
                    rptAchievements.DataBind();
                }
            }
            pnlNoBadges.Visible = rptAchievements.Items.Count == 0;
        }

        protected void btnAddBadge_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string name = txtName.Text.Trim();
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            int quizCount = Convert.ToInt32(txtQuizCount.Text);
            string description = $"Complete {quizCount} quiz(zes) in {ddlCategory.SelectedItem.Text}.";

            // UPDATED: Removed ImageURL, added BadgeType
            string query = @"
                INSERT INTO Achievements (Name, Description, QuizCountThreshold, CategoryID, BadgeType) 
                VALUES (@Name, @Description, @QuizCountThreshold, @CategoryID, 'Global')";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@QuizCountThreshold", quizCount);

                        if (categoryId == 0)
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                        }

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                lblMessage.Text = "Achievement added successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                ClearForm();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    lblMessage.Text = "An achievement with this name already exists.";
                }
                else
                {
                    lblMessage.Text = "An error occurred. " + ex.Message;
                }
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

            LoadAchievements(); // Refresh the list
        }

        protected void rptAchievements_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBadge")
            {
                int achievementId = Convert.ToInt32(e.CommandArgument);

                // Use a transaction to delete from all 3 tables safely
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        // 1. Delete from StudentAchievements
                        string querySA = "DELETE FROM StudentAchievements WHERE AchievementID = @AchievementID";
                        using (SqlCommand cmd = new SqlCommand(querySA, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AchievementID", achievementId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Unlink from Quizzes
                        string queryQ = "UPDATE Quizzes SET GrantsAchievementID = NULL WHERE GrantsAchievementID = @AchievementID";
                        using (SqlCommand cmd = new SqlCommand(queryQ, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AchievementID", achievementId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Delete from Achievements
                        string queryA = "DELETE FROM Achievements WHERE AchievementID = @AchievementID";
                        using (SqlCommand cmd = new SqlCommand(queryA, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AchievementID", achievementId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        // Show an error
                    }
                }
                LoadAchievements();
            }
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtDescription.Text = "";
            txtQuizCount.Text = "1";
            ddlCategory.SelectedIndex = 0;
        }

        // --- NEW HELPER ---
        protected string GetBadgeColor(object badgeType)
        {
            if (badgeType.ToString() == "Global")
            {
                return "#3498db"; // Blue
            }
            return "#f39c12"; // Yellow
        }
    }
}