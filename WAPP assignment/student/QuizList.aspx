<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuizList.aspx.cs" Inherits="WAPP_assignment.QuizList" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Quizzes - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/student.css">
    <style>
        /* Add some specific styles for this list page */
        .quiz-card {
            background-color: #fff;
            border-radius: 20px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .quiz-info h3 {
            font-family: 'Fredoka', sans-serif;
            font-size: 1.5rem;
            color: #333;
            margin: 0 0 5px 0;
        }
        .quiz-info p {
            font-family: 'Nunito', sans-serif;
            font-size: 1rem;
            color: #777;
            margin: 0;
        }
        .quiz-button {
            background-color: #5cb85c; /* Green */
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
        }
        .quiz-button:hover {
            background-color: #4cae4c;
        }
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .back-button {
            font-family: 'Nunito', sans-serif;
            text-decoration: none;
            color: #555;
            font-weight: 600;
        }
        .empty-message {
            text-align: center;
            padding: 50px;
            background-color: #fff;
            border-radius: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header class="header">
                <div class="logo">
                    <div class="logo-icon">
                        <img src="images/klogo.png" alt="KinderLearn Logo" />
                    </div>
                    KinderLearn
                </div>
            </header>

            <main class="main-content">
                <div class="page-header">
                    <h2 class="section-title">
                        <span class="section-icon">
                            <asp:Literal ID="litCategoryIcon" runat="server">📚</asp:Literal>
                        </span>
                        <asp:Literal ID="litCategoryName" runat="server">Quizzes</asp:Literal>
                    </h2>
                    <a href="studentpage.aspx" class="back-button">← Back to Subjects</a>
                </div>

                <div class="quiz-list">
                    <asp:Repeater ID="rptQuizzes" runat="server" OnItemCommand="rptQuizzes_ItemCommand">
                        <ItemTemplate>
                            <div class="quiz-card">
                                <div class="quiz-info">
                                    <h3><%# Eval("Title") %></h3>
                                    <p>A fun quiz from <%# Eval("TeacherName") %></p>
                                </div>
                                <asp:LinkButton ID="btnStartQuiz" runat="server" 
                                    CssClass="quiz-button" 
                                    Text="Start Quiz!"
                                    CommandName="Start"
                                    CommandArgument='<%# Eval("QuizID") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Panel ID="pnlNoQuizzes" runat="server" CssClass="empty-message" Visible="false">
                        <h3>No Quizzes Here Yet!</h3>
                        <p>Your teachers are still working on it. Check back soon!</p>
                    </asp:Panel>
                </div>
            </main>
        </div>
    </form>
</body>
</html>