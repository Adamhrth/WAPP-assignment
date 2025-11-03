<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teacherdashboard.aspx.cs" Inherits="WAPP_assignment.teacherdashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Teacher Dashboard - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    
    <style>
        .btn-success {
            background-color: #2ecc71; /* Green */
            color: white;
            padding: 5px 10px;
            border-radius: var(--radius-md);
            text-decoration: none;
            font-size: 0.875rem;
            border: none;
            cursor: pointer;
            font-weight: 600;
        }
        .btn-success:hover {
            background-color: #27ae60;
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
                <a href="teacherdashboard.aspx" class="sidebar-link active"><span>Dashboard</span></a>
                <a href="teacherCourse.aspx" class="sidebar-link"><span>My Quizzes</span></a>
                <a href="teacherStudent.aspx" class="sidebar-link"><span>Students</span></a>
            </nav>
            <div class="sidebar-footer">
                <a href="../loginsignup/login.aspx?action=logout" class="sidebar-link"><span>Logout</span></a>
            </div>
        </aside>

        <main class="main-content">
            <header class="dashboard-header">
                <div>
                    <h1 class="dashboard-title"><asp:Literal ID="litWelcome" runat="server">Teacher Dashboard</asp:Literal></h1>
                    <p class="dashboard-subtitle">Manage your quizzes and students</p>
                </div>
                <div class="user-profile">
                    <a href="teacherProfile.aspx" class="user-avatar-link">
                        <div class="user-avatar"><asp:Literal ID="litAvatar" runat="server">T</asp:Literal></div>
                    </a>
                </div>
            </header>

            <div class="stats-grid">
                <div class="stat-card stat-card-secondary">
                    <div class="stat-content">
                        <p class="stat-label">Active Quizzes</p>
                        <p class="stat-value"><asp:Literal ID="litActiveQuizzes" runat="server">0</asp:Literal></p>
                    </div>
                </div>
                <div class="stat-card stat-card-primary">
                    <div class="stat-content">
                        <p class="stat-label">Total Completions</p>
                        <p class="stat-value"><asp:Literal ID="litTotalCompletions" runat="server">0</asp:Literal></p>
                    </div>
                </div>
            </div>

            <section class="courses-section">
                <div class="section-header-row">
                    <h2 class="section-heading">My Quizzes</h2>
                    <a href="CreateQuiz.aspx" class="btn btn-primary">Create Quiz</a>
                </div>
                
                <div class="courses-grid">
                    <asp:Repeater ID="rptQuizzes" runat="server" OnItemCommand="rptQuizzes_ItemCommand">
                        <ItemTemplate>
                            <div class="course-card">
                                <div class="course-header">
                                    <h3 class="course-title"><%# Eval("Title") %></h3>
                                    <span class='course-badge course-badge-<%# GetStatusClass(Eval("Status")) %>'>
                                        <%# Eval("Status") %>
                                    </span>
                                </div>
                                <p class="course-meta">
                                    <%# Eval("StudentCount") %> Students • <%# Eval("QuestionCount") %> Questions
                                </p>
                                <div class="course-actions">
                                    <asp:HyperLink runat="server" CssClass="btn btn-primary btn-sm" 
                                        NavigateUrl='<%# "ManageQuiz.aspx?QuizID=" + Eval("QuizID") %>'
                                        Visible='<%# !Eval("Status").ToString().Equals("Rejected") %>'>Manage</asp:HyperLink>
                                    
                                    <asp:LinkButton ID="btnResubmit" runat="server" 
                                        CssClass="btn-success btn-sm"
                                        CommandName="ResubmitQuiz" 
                                        CommandArgument='<%# Eval("QuizID") %>'
                                        Visible='<%# Eval("Status").ToString().Equals("Rejected") %>'>
                                        Resubmit
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <div id="divNoQuizzes" runat="server" class="empty-placeholder" visible="false">
                        <p>You haven't created any quizzes yet.</p>
                        <a href="CreateQuiz.aspx" class="btn btn-primary">Create Your First Quiz</a>
                    </div>
                </div>
            </section>

            <section class="students-section">
                <h2 class="section-heading">Recent Student Activity</h2>
                <div class="students-table">
                    <table>
                        <thead>
                            <tr>
                                <th>Student</th>
                                <th>Quiz</th>
                                <th>Score</th>
                                <th>Completed On</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptStudentActivity" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="student-info">
                                                <div class="student-avatar"><%# Eval("Initial") %></div>
                                                <span><%# Eval("StudentName") %></span>
                                            </div>
                                        </td>
                                        <td><%# Eval("QuizTitle") %></td>
                                        <td>
                                            <span class="score-badge"><%# Eval("Score", "{0:F0}%") %></span>
                                        </td>
                                        <td><%# Eval("CompletedAt", "{0:g}") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Literal ID="litNoActivity" runat="server" Visible="false" />
                        </tbody>
                    </table>
                </div>
            </section>
        </main>
    </div>
</form>
</body>
</html>