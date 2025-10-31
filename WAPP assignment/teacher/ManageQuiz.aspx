<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageQuiz.aspx.cs" Inherits="WAPP_assignment.teacher.ManageQuiz" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Manage Quiz - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <style>
        /* Styles for the forms on this page */
        .form-container {
            background-color: #fff;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            margin-bottom: 30px;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .form-group label {
            display: block;
            font-weight: 600;
            margin-bottom: 8px;
            color: #333;
        }
        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-sizing: border-box; 
        }
        .btn-submit {
            padding: 10px 20px;
            font-size: 16px;
            cursor: pointer;
        }
        
        /* Styles for the radio button list */
        .correct-answer-list label {
            display: inline-block;
            margin-right: 15px;
            font-weight: normal;
        }

        /* Styles for the existing questions list */
        .question-list-item {
            padding: 15px;
            border: 1px solid #eee;
            border-radius: 5px;
            margin-bottom: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .question-text {
            font-weight: 600;
            color: #333;
        }
        .correct-answer-text {
            color: #2c9049;
            font-style: italic;
        }
        .btn-delete {
            background-color: #e74c3c;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="dashboard">
        <aside class="sidebar">
            <div class="sidebar-header">
                <img src="../images/klogo.png" alt="KinderLearn Logo" class="sidebar-logo"/>
                <span class="sidebar-title">KinderLearn</span>
            </div>
            <nav class="sidebar-nav">
                <a href="teacherdashboard.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/></svg>
                    <span>Dashboard</span>
                </a>
                <a href="teacherCourse.aspx" class="sidebar-link active">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"/></svg>
                    <span>My Quizzes</span>
                </a>
                <a href="teacherStudent.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
                    <span>Students</span>
                </a>

            </nav>
            <div class="sidebar-footer">
                <a href="../loginsignup/login.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/></svg>
                    <span>Logout</span>
                </a>
            </div>
        </aside>

        <main class="main-content">
            <header class="dashboard-header">
                <div>
                    <h1 class="dashboard-title">Manage Quiz: <asp:Literal ID="litQuizTitle" runat="server"></asp:Literal></h1>
                    <p class="dashboard-subtitle">Add or remove questions for this quiz.</p>
                </div>
                <a href="teacherdashboard.aspx" class="btn btn-outline">Done</a>
            </header>

            <asp:HiddenField ID="hfQuizID" runat="server" />

            <div class="form-container">
                <h2 class="section-heading">Add New Question</h2>
                
                <div class="form-group">
                    <asp:Label ID="lblQuestionText" runat="server" Text="Question Text" AssociatedControlID="txtQuestionText" />
                    <asp:TextBox ID="txtQuestionText" runat="server" CssClass="form-control" placeholder="e.g., What is 2 + 2?"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvQuestionText" runat="server" ControlToValidate="txtQuestionText" ErrorMessage="Question text is required." ForeColor="Red" Display="Dynamic" ValidationGroup="AddQuestion" />
                </div>

                <div class="form-group">
                    <asp:Label ID="lblOption1" runat="server" Text="Option 1" AssociatedControlID="txtOption1" />
                    <asp:TextBox ID="txtOption1" runat="server" CssClass="form-control" placeholder="Answer choice 1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOption1" runat="server" ControlToValidate="txtOption1" ErrorMessage="At least two options are required." ForeColor="Red" Display="Dynamic" ValidationGroup="AddQuestion" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblOption2" runat="server" Text="Option 2" AssociatedControlID="txtOption2" />
                    <asp:TextBox ID="txtOption2" runat="server" CssClass="form-control" placeholder="Answer choice 2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOption2" runat="server" ControlToValidate="txtOption2" ErrorMessage="At least two options are required." ForeColor="Red" Display="Dynamic" ValidationGroup="AddQuestion" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblOption3" runat="server" Text="Option 3 (Optional)" AssociatedControlID="txtOption3" />
                    <asp:TextBox ID="txtOption3" runat="server" CssClass="form-control" placeholder="Answer choice 3"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblOption4" runat="server" Text="Option 4 (Optional)" AssociatedControlID="txtOption4" />
                    <asp:TextBox ID="txtOption4" runat="server" CssClass="form-control" placeholder="Answer choice 4"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Label ID="lblCorrectAnswer" runat="server" Text="Correct Answer" />
                    <asp:RadioButtonList ID="rblCorrectAnswer" runat="server" CssClass="correct-answer-list" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Option 1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Option 2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Option 3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Option 4" Value="4"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvCorrectAnswer" runat="server" ControlToValidate="rblCorrectAnswer" ErrorMessage="You must select a correct answer." ForeColor="Red" Display="Dynamic" ValidationGroup="AddQuestion" />
                </div>

                <div class="form-group">
                    <asp:Button ID="btnAddQuestion" runat="server" Text="Add Question" 
                        CssClass="btn btn-primary btn-submit" 
                        OnClick="btnAddQuestion_Click" ValidationGroup="AddQuestion" />
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                </div>
            </div>

            <div class="form-container">
                <h2 class="section-heading">Existing Questions (<asp:Literal ID="litQuestionCount" runat="server">0</asp:Literal>)</h2>
                <asp:Repeater ID="rptQuestions" runat="server" OnItemCommand="rptQuestions_ItemCommand">
                    <ItemTemplate>
                        <div class="question-list-item">
                            <div>
                                <span class="question-text"><%# Eval("QuestionText") %></span>
                                <br />
                                <span class="correct-answer-text">Correct Answer: <%# Eval("CorrectAnswer") %></span>
                            </div>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                                CssClass="btn-delete" 
                                CommandName="DeleteQuestion" 
                                CommandArgument='<%# Eval("QuestionID") %>' 
                                OnClientClick="return confirm('Are you sure you want to delete this question?');" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlNoQuestions" runat="server" Visible="false" Style="text-align: center; padding: 20px;">
                    <p>There are no questions in this quiz yet. Add one above!</p>
                </asp:Panel>
            </div>

        </main>
    </div>
</form>
</body>
</html>