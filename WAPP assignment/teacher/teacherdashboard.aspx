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
</head>
<body>
<form id="form1" runat="server">
    <div class="dashboard">
        <aside class="sidebar">
            <div class="sidebar-header">
                <img src="images/klogo.png" alt="KinderLearn Logo" class="sidebar-logo"/>
                <span class="sidebar-title">KinderLearn</span>
            </div>
            <nav class="sidebar-nav">
                <a href="teacherdashboard.aspx" class="sidebar-link active">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/>
                    </svg>
                    <span>Dashboard</span>
                </a>
                <a href="teacherCourse.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"/>
                    </svg>
                    <span>My Quizzes</span>
                </a>
                <a href="teacherStudent.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>
                    </svg>
                    <span>Students</span>
                </a>
            </nav>
            <div class="sidebar-footer">
                <a href="../loginsignup/login.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/>
                    </svg>
                    <span>Logout</span>
                </a>
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
                    <div class="stat-icon">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"/>
                        </svg>
                    </div>
                    <div class="stat-content">
                        <p class="stat-label">Active Quizzes</p>
                        <p class="stat-value"><asp:Literal ID="litActiveQuizzes" runat="server">0</asp:Literal></p>
                    </div>
                </div>
                <div class="stat-card stat-card-primary">
                    <div class="stat-icon">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                             <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path>
                        </svg>
                    </div>
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
                    <asp:Repeater ID="rptQuizzes" runat="server">
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
                                    <a href='ManageQuiz.aspx?QuizID=<%# Eval("QuizID") %>' class="btn btn-primary btn-sm">Manage</a>
                                </div>
                            </div>
                        </ItemTemplate>
                        <%-- REMOVED EmptyDataTemplate --%>
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
                                <th>Actions</th>
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
                                        <td><a href='StudentDetails.aspx?StudentID=<%# Eval("StudentID") %>' class="link">View Details</a></td>
                                    </tr>
                                </ItemTemplate>
                                <%-- REMOVED EmptyDataTemplate --%>
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