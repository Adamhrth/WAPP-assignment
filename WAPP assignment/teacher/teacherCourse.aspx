<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teacherCourse.aspx.cs" Inherits="WAPP_assignment.teacherCourse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>My Quizzes - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
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
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/>
                    </svg>
                    <span>Dashboard</span>
                </a>
                <a href="teacherCourse.aspx" class="sidebar-link active">
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
                    <h1 class="dashboard-title">My Quizzes</h1>
                    <p class="dashboard-subtitle">Create and manage your quizzes</p>
                </div>

                <div class="user-profile">
                    <a href="teacherProfile.aspx" class="user-avatar-link">
                        <div class="user-avatar"><asp:Literal ID="litAvatar" runat="server">T</asp:Literal></div>
                    </a>
                </div>
            </header>

            <div class="courses-filter-section">
                <div class="section-header-row">
                    <div class="filter-tabs">
                        <asp:LinkButton ID="btnAllQuizzes" runat="server" CssClass="filter-tab active" Text="All Quizzes" OnClick="btnAllQuizzes_Click" />
                        <asp:LinkButton ID="btnActiveQuizzes" runat="server" CssClass="filter-tab" Text="Approved" OnClick="btnActiveQuizzes_Click" />
                        <asp:LinkButton ID="btnDraftQuizzes" runat="server" CssClass="filter-tab" Text="Pending" OnClick="btnDraftQuizzes_Click" />
                        <asp:LinkButton ID="btnRejectedQuizzes" runat="server" CssClass="filter-tab" Text="Rejected" OnClick="btnRejectedQuizzes_Click" />
                    </div>
                    <asp:HyperLink ID="btnCreateQuiz" runat="server" CssClass="btn btn-primary" NavigateUrl="CreateQuiz.aspx">
                         <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" style="margin-right: 8px;">
                             <line x1="12" y1="5" x2="12" y2="19"/>
                             <line x1="5" y1="12" x2="19" y2="12"/>
                         </svg>
                         Create Quiz
                    </asp:HyperLink>
                </div>
            </div>

            <section class="courses-section">
                <div class="courses-grid" id="coursesGrid">
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
                                        NavigateUrl='<%# "ManageQuiz.aspx?QuizID=" + Eval("QuizID") %>'>Manage</asp:HyperLink>
                                    
                                    <asp:LinkButton runat="server" CssClass="btn btn-outline btn-sm" 
                                        CommandName="DeleteQuiz" CommandArgument='<%# Eval("QuizID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this entire quiz?');">Delete</asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Panel ID="pnlNoQuizzes" runat="server" CssClass="empty-placeholder" Visible="false">
                        <p>No quizzes found for this filter.</p>
                    </asp:Panel>
                </div>
            </section>
        </main>
    </div>

    </form>
</body>
</html>