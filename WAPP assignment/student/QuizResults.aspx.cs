using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_assignment.student
{
    public partial class QuizResults : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                if (Request.QueryString["AttemptID"] == null || !int.TryParse(Request.QueryString["AttemptID"], out int attemptId))
                {
                    ShowError("Results Not Found", "We couldn't find the quiz results you were looking for.");
                    return;
                }

                int studentId = Convert.ToInt32(Session["UserID"]);
                LoadResults(attemptId, studentId);
            }
        }

        private void LoadResults(int attemptId, int studentId)
        {
            // --- UPDATED QUERY ---
            // We now select GrantsAchievementID
            string query = @"
                SELECT
                    A.Score,
                    A.StudentID,
                    Q.Title AS QuizTitle,
                    Q.GrantsAchievementID, 
                    (SELECT COUNT(*) FROM Questions WHERE QuizID = Q.QuizID) AS TotalQuestions
                FROM QuizAttempts A
                JOIN Quizzes Q ON A.QuizID = Q.QuizID
                WHERE A.AttemptID = @AttemptID";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AttemptID", attemptId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int dbStudentId = Convert.ToInt32(reader["StudentID"]);
                            if (dbStudentId != studentId)
                            {
                                ShowError("Access Denied", "You do not have permission to view these results.");
                                return;
                            }

                            decimal score = Convert.ToDecimal(reader["Score"]);
                            string quizTitle = reader["QuizTitle"].ToString();
                            int totalQuestions = Convert.ToInt32(reader["TotalQuestions"]);
                            int correctCount = (int)Math.Round(totalQuestions * (score / 100));

                            // Get the Achievement ID. It might be null.
                            int? grantsAchievementId = reader["GrantsAchievementID"] as int?;

                            // Populate the labels
                            litQuizTitle.Text = "Results for '" + quizTitle + "'";
                            litScore.Text = score.ToString("F0") + "%";
                            litCorrectCount.Text = $"You got {correctCount} out of {totalQuestions} questions correct.";
                            litMessage.Text = GetResultMessage(score);

                            // --- NEW ACHIEVEMENT LOGIC ---
                            CheckForAchievements(studentId, grantsAchievementId, score);
                        }
                        else
                        {
                            ShowError("Results Not Found", "We couldn't find the quiz results you were looking for.");
                        }
                    }
                }
            }
        }

        // --- COMPLETELY REPLACED METHOD ---
        // This is the new, simple logic that checks for 100% and a specific badge.
        private void CheckForAchievements(int studentId, int? achievementId, decimal score)
        {
            // Check 1: Does this quiz even grant a badge? (Is the ID null?)
            if (!achievementId.HasValue)
            {
                return; // No badge associated with this quiz.
            }

            // Check 2: Did the student get 100%?
            if (score == 100.00m)
            {
                // Yes. Award this specific badge.
                AwardAchievement(studentId, achievementId.Value);
            }
        }

        // --- This method is unchanged and works perfectly ---
        private void AwardAchievement(int studentId, int achievementId)
        {
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM StudentAchievements WHERE StudentID = @StudentID AND AchievementID = @AchievementID)
                BEGIN
                    INSERT INTO StudentAchievements (StudentID, AchievementID, EarnedAt)
                    VALUES (@StudentID, @AchievementID, GETDATE())
                END";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@AchievementID", achievementId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception) { /* Log error if needed */ }
        }

        // --- Helper functions (no changes) ---
        private string GetResultMessage(decimal score)
        {
            if (score >= 90) return "Excellent! 🌟";
            if (score >= 70) return "Great Job! 👍";
            if (score >= 50) return "Good Try! 😊";
            return "Keep Practicing! 📚";
        }

        private void ShowError(string title, string body)
        {
            pnlResults.Visible = false;
            pnlError.Visible = true;
            errorHeader.InnerText = title;
            errorBody.InnerText = body;
        }
    }
}