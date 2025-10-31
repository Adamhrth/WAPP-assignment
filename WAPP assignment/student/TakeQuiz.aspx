<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeQuiz.aspx.cs" Inherits="WAPP_assignment.student.TakeQuiz" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Taking Quiz - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/student.css">
    <style>
        .quiz-container {
            max-width: 800px;
            margin: 20px auto;
            background-color: #fff;
            border-radius: 20px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
            padding: 30px 40px;
        }
        .quiz-header h2 {
            font-family: 'Fredoka', sans-serif;
            font-size: 2rem;
            color: #333;
            margin-top: 0;
        }
        .quiz-header p {
            font-family: 'Nunito', sans-serif;
            font-size: 1.1rem;
            color: #666;
        }
        .question-panel {
            margin-top: 25px;
        }
        .question-text {
            font-family: 'Fredoka', sans-serif;
            font-size: 1.5rem;
            color: #444;
            margin-bottom: 20px;
        }
        .options-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .options-list li {
            margin-bottom: 15px;
        }
        /* Style for the Radio Buttons */
        .options-list label {
            display: block;
            background: #f8f9fa;
            border: 2px solid #ddd;
            border-radius: 12px;
            padding: 15px 20px;
            font-family: 'Nunito', sans-serif;
            font-size: 1.1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
        }
        .options-list input[type="radio"] {
            display: none; /* Hide the default radio button */
        }
        .options-list input[type="radio"]:checked + label {
            border-color: #34a853; /* Green */
            background-color: #e6f6e9;
            color: #34a853;
        }
        .options-list label:hover {
            border-color: #aaa;
        }
        .quiz-footer {
            margin-top: 30px;
            text-align: right;
        }
        .btn-quiz-next {
            background-color: #4285f4; /* Blue */
            color: white;
            padding: 12px 25px;
            border: none;
            border-radius: 25px;
            font-family: 'Fredoka', sans-serif;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }
        .btn-quiz-next:hover {
            background-color: #357ae8;
        }
        .btn-quiz-submit {
             background-color: #34a853; /* Green */
        }
        .btn-quiz-submit:hover {
            background-color: #2c9049;
        }
        .error-message {
            color: #ea4335; /* Red */
            font-family: 'Nunito', sans-serif;
            font-weight: 600;
            margin-right: 20px;
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
                <div class="quiz-container">
                    
                    <asp:Panel ID="pnlQuiz" runat="server">
                        <div class="quiz-header">
                            <h2><asp:Literal ID="litQuizTitle" runat="server">Quiz Title</asp:Literal></h2>
                            <p>Question <asp:Literal ID="litQuestionNumber" runat="server">1</asp:Literal> of <asp:Literal ID="litTotalQuestions" runat="server">10</asp:Literal></p>
                        </div>
                        
                        <div class="question-panel">
                            <asp:Label ID="lblQuestionText" runat="server" CssClass="question-text" Text="What is 2 + 2?"></asp:Label>
                            
                            <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="options-list" RepeatLayout="UnorderedList">
                                </asp:RadioButtonList>
                        </div>

                        <div class="quiz-footer">
                            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" Text="Please select an answer."></asp:Label>
                            
                            <asp:Button ID="btnNext" runat="server" Text="Next Question" CssClass="btn-quiz-next" OnClick="btnNext_Click" />
                            
                            <asp:HiddenField ID="hfCurrentQuestionIndex" runat="server" Value="0" />
                            <asp:HiddenField ID="hfAttemptID" runat="server" Value="0" />
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" style="text-align: center;">
                        <h2 id="msgHeader" runat="server">Loading Quiz...</h2>
                        <p id="msgBody" runat="server">Please wait.</p>
                        <a href="studentpage.aspx" class="btn-quiz-next" style="text-decoration: none;">Back to Subjects</a>
                    </asp:Panel>

                </div>
            </main>
        </div>
    </form>
</body>
</html>