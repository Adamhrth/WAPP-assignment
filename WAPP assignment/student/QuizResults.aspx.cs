using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;

namespace WAPP_assignment.student
{
    public partial class QuizResults : System.Web.UI.Page
    {
        // Simple class to hold a rule
        private class AchievementRule
        {
            public int AchievementID { get; set; }
            public int? CategoryID { get; set; }
            public int QuizCountThreshold { get; set; }
        }

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
            string query = @"
                SELECT
                    A.Score,
                    A.StudentID,
                    Q.Title AS QuizTitle,
                    Q.CategoryID, 
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
                            int? grantsAchievementId = reader["GrantsAchievementID"] as int?;
                            int categoryId = Convert.ToInt32(reader["CategoryID"]);

                            litQuizTitle.Text = "Results for '" + quizTitle + "'";
                            litScore.Text = score.ToString("F0") + "%";
                            litCorrectCount.Text = $"You got {correctCount} out of {totalQuestions} questions correct.";
                            litMessage.Text = GetResultMessage(score);

                            // 1. Check for 100%-specific badge (from the quiz itself)
                            if (score == 100.00m)
                            {
                                CheckForQuizSpecificAchievement(studentId, grantsAchievementId);
                            }

                            // 2. Check for global "completion" badges (if student passed)
                            if (score >= 50.00m) // "pass" is 50% or higher
                            {
                                CheckForGlobalAchievements(studentId, categoryId);
                            }
                        }
                        else
                        {
                            ShowError("Results Not Found", "We couldn't find the quiz results you were looking for.");
                        }
                    }
                }
            }
        }

        private void CheckForQuizSpecificAchievement(int studentId, int? achievementId)
        {
            if (achievementId.HasValue)
            {
                AwardAchievement(studentId, achievementId.Value);
            }
        }

        // --- UPDATED METHOD ---
        private void CheckForGlobalAchievements(int studentId, int quizCategoryId)
        {
            int totalCompleted = 0;
            int categoryCompleted = 0;
            var rules = new List<AchievementRule>();

            // --- THIS QUERY IS NOW FIXED ---
            string query = @"
                -- 1. Get all global rules the student does NOT have
                SELECT A.AchievementID, A.CategoryID, A.QuizCountThreshold 
                FROM Achievements A
                WHERE 
                    -- A 'Global' badge is one that is NOT linked to a quiz
                    A.AchievementID NOT IN (SELECT GrantsAchievementID FROM Quizzes WHERE GrantsAchievementID IS NOT NULL)
                    -- And the student doesn't have it yet
                    AND A.AchievementID NOT IN (SELECT AchievementID FROM StudentAchievements WHERE StudentID = @StudentID);

                -- 2. Get student's total *passed* quizzes
                SELECT COUNT(DISTINCT QuizID) 
                FROM QuizAttempts 
                WHERE StudentID = @StudentID AND Score >= 50.00;

                -- 3. Get student's *passed* quizzes in this specific category
                SELECT COUNT(DISTINCT Q.QuizID) 
                FROM QuizAttempts A JOIN Quizzes Q ON A.QuizID = Q.QuizID 
                WHERE A.StudentID = @StudentID AND Q.CategoryID = @QuizCategoryID AND A.Score >= 50.00;
            ";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@QuizCategoryID", quizCategoryId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Result 1: The rules
                        while (reader.Read())
                        {
                            rules.Add(new AchievementRule
                            {
                                AchievementID = Convert.ToInt32(reader["AchievementID"]),
                                CategoryID = reader["CategoryID"] as int?,
                                QuizCountThreshold = Convert.ToInt32(reader["QuizCountThreshold"])
                            });
                        }

                        // Result 2: Total count
                        reader.NextResult();
                        if (reader.Read())
                        {
                            totalCompleted = reader.GetInt32(0);
                        }

                        // Result 3: Category count
                        reader.NextResult();
                        if (reader.Read())
                        {
                            categoryCompleted = reader.GetInt32(0);
                        }
                    }
                }
            }

            // Now, check the rules
            foreach (var rule in rules)
            {
                if (rule.CategoryID == null) // "All Categories" rule
                {
                    if (totalCompleted >= rule.QuizCountThreshold)
                    {
                        AwardAchievement(studentId, rule.AchievementID);
                    }
                }
                else if (rule.CategoryID == quizCategoryId) // Specific category rule
                {
                    if (categoryCompleted >= rule.QuizCountThreshold)
                    {
                        AwardAchievement(studentId, rule.AchievementID);
                    }
                }
            }
        }

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