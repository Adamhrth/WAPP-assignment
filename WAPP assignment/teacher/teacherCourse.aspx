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
<style>
    .course-badge-archived {
        background-color: var(--color-muted);
        color: var(--color-muted-foreground);
        border: 1px solid var(--color-border);
    }
    .btn-success {
        background-color: #2ecc71; 
        color: white;
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
                <a href="teacherdashboard.aspx" class="sidebar-link"><span>Dashboard</span></a>
                <a href="teacherCourse.aspx" class="sidebar-link active"><span>My Quizzes</span></a>
                <a href="teacherStudent.aspx" class="sidebar-link"><span>Students</span></a>
            </nav>
            <div class="sidebar-footer">
                <a href="../loginsignup/login.aspx?action=logout" class="sidebar-link"><span>Logout</span></a>
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
                        <asp:LinkButton ID="btnAllQuizzes" runat="server" CssClass="filter-tab active" Text="All" OnClick="btnAllQuizzes_Click" />
                        <asp:LinkButton ID="btnActiveQuizzes" runat="server" CssClass="filter-tab" Text="Approved" OnClick="btnActiveQuizzes_Click" />
                        <asp:LinkButton ID="btnDraftQuizzes" runat="server" CssClass="filter-tab" Text="Pending" OnClick="btnDraftQuizzes_Click" />
                        <asp:LinkButton ID="btnArchivedQuizzes" runat="server" CssClass="filter-tab" Text="Archived" OnClick="btnArchivedQuizzes_Click" />
                    </div>
                    
                    <asp:HyperLink ID="btnCreateQuiz" runat="server" CssClass="btn btn-primary" NavigateUrl="CreateQuiz.aspx">
                         <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" style="margin-right: 8px;">
                             <line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/>
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
                        
                        <asp:LinkButton ID="btnArchive" runat="server" CssClass="btn btn-outline btn-sm" 
                            CommandName="ArchiveQuiz" CommandArgument='<%# Eval("QuizID") %>'
                            OnClientClick="return confirm('Are you sure you want to archive this quiz? It will be hidden from you and your students.');"
                            Visible='<%# !Eval("Status").ToString().Equals("Archived") %>'>
                            Archive
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnRestore" runat="server" CssClass="btn btn-success btn-sm" 
                            CommandName="RestoreQuiz" CommandArgument='<%# Eval("QuizID") %>'
                            Visible='<%# Eval("Status").ToString().Equals("Archived") %>'>
                            Restore
                        </asp:LinkButton>
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