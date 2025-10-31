using System;
using System.Configuration;
using System.Data; // Required for SqlTransaction
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

        // --- NEW METHOD ---
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
            // Add a default "All Categories" item
            ddlCategory.Items.Insert(0, new ListItem("All Categories", "0"));
        }


        // --- UPDATED METHOD ---
        private void LoadAchievements()
        {
            // This query now joins Category to get the name of the rule
            string query = @"
                SELECT 
                    A.AchievementID, 
                    A.Name, 
                    A.Description, 
                    A.BadgeImageURL,
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

        // --- UPDATED METHOD ---
        protected void btnAddBadge_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string name = txtName.Text.Trim();
            string imageUrl = txtBadgeImageURL.Text.Trim();

            // Get new rule data
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            int quizCount = Convert.ToInt32(txtQuizCount.Text);

            // Auto-generate description
            string description = $"Complete {quizCount} quiz(zes) in {ddlCategory.SelectedItem.Text}.";

            // We only insert global badges (where GrantsAchievementID is NULL)
            string query = @"
                INSERT INTO Achievements (Name, Description, BadgeImageURL, QuizCountThreshold, CategoryID) 
                VALUES (@Name, @Description, @BadgeImageURL, @QuizCountThreshold, @CategoryID)";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@BadgeImageURL", imageUrl);
                        cmd.Parameters.AddWithValue("@QuizCountThreshold", quizCount);

                        // If "All Categories" (value 0) is chosen, insert NULL into the DB
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

        // --- This method is unchanged ---
        protected void rptAchievements_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBadge")
            {
                int achievementId = Convert.ToInt32(e.CommandArgument);
                string query = "DELETE FROM Achievements WHERE AchievementID = @AchievementID";

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AchievementID", achievementId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
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
            txtBadgeImageURL.Text = "images/badges/default_badge.png";
        }
    }
}