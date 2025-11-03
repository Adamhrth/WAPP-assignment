using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_assignment // Your project's namespace
{
    public partial class CreateQuiz : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EducationDB"].ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"] || Session["UserRole"].ToString() != "teacher")
            {
                Response.Redirect("../loginsignup/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
            }

            // --- THIS IS THE FIX ---
            // Set validator state based on checkbox
            rfvBadgeName.Enabled = chkCreateBadge.Checked;
            // The line for 'rfvBadgeImageURL' has been removed.
        }

        private void LoadCategories()
        {
            // (This method is unchanged)
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT CategoryID, Name FROM Categories ORDER BY Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    ddlCategories.DataSource = cmd.ExecuteReader();
                    ddlCategories.DataTextField = "Name";
                    ddlCategories.DataValueField = "CategoryID";
                    ddlCategories.DataBind();
                }
            }
            ddlCategories.Items.Insert(0, new ListItem("-- Select a Category --", ""));
        }

        protected void chkCreateBadge_CheckedChanged(object sender, EventArgs e)
        {
            pnlBadgeFields.Visible = chkCreateBadge.Checked;
            rfvBadgeName.Enabled = chkCreateBadge.Checked;
        }

        protected void btnCreateQuiz_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int teacherId = Convert.ToInt32(Session["UserID"]);
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            int categoryId = Convert.ToInt32(ddlCategories.SelectedValue);

            int? newAchievementId = null;

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. If checkbox is checked, create the Achievement first
                    if (chkCreateBadge.Checked)
                    {
                        string badgeName = txtBadgeName.Text.Trim();
                        string badgeDescription = $"100% on '{title}'"; // Auto-description

                        // UPDATED: No image, BadgeType is 'Teacher'
                        string queryAch = "INSERT INTO Achievements (Name, Description, BadgeType) VALUES (@Name, @Description, 'Teacher'); SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmdAch = new SqlCommand(queryAch, conn, transaction))
                        {
                            cmdAch.Parameters.AddWithValue("@Name", badgeName);
                            cmdAch.Parameters.AddWithValue("@Description", badgeDescription);
                            newAchievementId = Convert.ToInt32(cmdAch.ExecuteScalar());
                        }
                    }

                    // 2. Create the Quiz, linking the new Achievement ID
                    string queryQuiz = @"
                        INSERT INTO Quizzes (Title, Description, CategoryID, TeacherID, Status, GrantsAchievementID) 
                        VALUES (@Title, @Description, @CategoryID, @TeacherID, 'Pending', @GrantsAchievementID);
                        SELECT SCOPE_IDENTITY();";

                    int newQuizId;
                    using (SqlCommand cmdQuiz = new SqlCommand(queryQuiz, conn, transaction))
                    {
                        cmdQuiz.Parameters.AddWithValue("@Title", title);
                        cmdQuiz.Parameters.AddWithValue("@Description", description);
                        cmdQuiz.Parameters.AddWithValue("@CategoryID", categoryId);
                        cmdQuiz.Parameters.AddWithValue("@TeacherID", teacherId);

                        if (newAchievementId.HasValue)
                        {
                            cmdQuiz.Parameters.AddWithValue("@GrantsAchievementID", newAchievementId.Value);
                        }
                        else
                        {
                            cmdQuiz.Parameters.AddWithValue("@GrantsAchievementID", DBNull.Value);
                        }

                        newQuizId = Convert.ToInt32(cmdQuiz.ExecuteScalar());
                    }

                    transaction.Commit();

                    if (newQuizId > 0)
                    {
                        Response.Redirect($"ManageQuiz.aspx?QuizID={newQuizId}");
                    }
                    else
                    {
                        lblMessage.Text = "An error occurred. The quiz was not created.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = "An error occurred. Please try again. Details: " + ex.Message;
                }
            }
        }
    }
}