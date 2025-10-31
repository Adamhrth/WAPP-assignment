<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuizResults.aspx.cs" Inherits="WAPP_assignment.student.QuizResults" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Quiz Results - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/student.css">
    <style>
        .results-container {
            max-width: 700px;
            margin: 20px auto;
            background-color: #fff;
            border-radius: 20px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
            padding: 30px 40px;
            text-align: center;
        }
        .results-header {
            font-family: 'Fredoka', sans-serif;
            font-size: 2.5rem;
            color: #333;
            margin-top: 0;
        }
        .results-score {
            font-family: 'Fredoka', sans-serif;
            font-size: 4.5rem;
            font-weight: 700;
            margin: 10px 0;
            color: #4285f4; /* Blue */
        }
        .results-summary {
            font-family: 'Nunito', sans-serif;
            font-size: 1.25rem;
            color: #555;
            margin-bottom: 30px;
        }
        .results-quiz-title {
            font-family: 'Nunito', sans-serif;
            font-size: 1rem;
            color: #777;
            margin-bottom: 25px;
        }
        .btn-results {
            background-color: #34a853; /* Green */
            color: white;
            padding: 12px 25px;
            border: none;
            border-radius: 25px;
            font-family: 'Fredoka', sans-serif;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            text-decoration: none;
            transition: background-color 0.3s ease;
            margin: 0 10px;
        }
        .btn-results.secondary {
            background-color: #f8f9fa;
            border: 2px solid #ddd;
            color: #555;
        }
        .btn-results.secondary:hover {
            background-color: #f1f1f1;
        }
        .btn-results:hover {
            background-color: #2c9049;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header class="header">
                <div class="logo">
                    <div class="logo-icon">
                        <img src="../images/klogo.png" alt="KinderLearn Logo" />
                    </div>
                    KinderLearn
                </div>
            </header>

            <main class="main-content">
                
                <asp:Panel ID="pnlResults" runat="server">
                    <div class="results-container">
                        <h2 class="results-header">
                            <asp:Literal ID="litMessage" runat="server">Great Job!</asp:Literal>
                        </h2>
                        <p class="results-quiz-title">
                            <asp:Literal ID="litQuizTitle" runat="server">Results for 'Quiz Title'</asp:Literal>
                        </p>
                        
                        <div class="results-score">
                            <asp:Literal ID="litScore" runat="server">100%</asp:Literal>
                        </div>

                        <p class="results-summary">
                            <asp:Literal ID="litCorrectCount" runat="server">You got 10 out of 10 correct.</asp:Literal>
                        </p>

                        <div class="results-actions">
                            <a href="studentpage.aspx" class="btn-results secondary">Back to Subjects</a>
                            <a href="studentprofile.aspx" class="btn-results">View My Badges</a>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="results-container">
                    <h2 class="results-header" id="errorHeader" runat="server">Error</h2>
                    <p class="results-summary" id="errorBody" runat="server">
                        We couldn't find your quiz results.
                    </p>
                    <a href="studentpage.aspx" class="btn-results">Back to Subjects</a>
                </asp:Panel>

            </main>
        </div>
    </form></body>
</html>