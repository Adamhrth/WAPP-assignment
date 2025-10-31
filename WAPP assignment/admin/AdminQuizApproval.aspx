<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminQuizApproval.aspx.cs" Inherits="WAPP_assignment.admin.AdminQuizApproval" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Quiz Approval - Admin</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <link rel="stylesheet" href="../stylesheet/admin.css"/>
</head>
<body>
<form id="form1" runat="server">
    <div class="dashboard">
        <!-- ADMIN SIDEBAR -->
<aside class="sidebar">
    <div class="sidebar-header">
        <img src="../images/klogo.png" alt="KinderLearn Logo" class="sidebar-logo"/>
        <span class="sidebar-title">KinderLearn</span>
    </div>
    <nav class="sidebar-nav">
        <a href="AdminDashboard.aspx" class="sidebar-link">
            <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/>
            </svg>
            <span>Dashboard</span>
        </a>
        <a href="AdminQuizApproval.aspx" class="sidebar-link active">
            <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/>
            </svg>
            <span>Quiz Approval</span>
        </a>
        <a href="AdminUserManagement.aspx" class="sidebar-link">
            <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>
            </svg>
            <span>User Management</span>
        </a>
        
        <a href="AdminManageBadges.aspx" class="sidebar-link">
             <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path>
            </svg>
            <span>Manage Badges</span>
        </a>
        <a href="AdminManageQuestions.aspx" class="sidebar-link">
             <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
               <path d="M21.5 2v6h-6M2.5 22v-6h6M2 11.5a10 10 0 0 1 18.9-2.73L22 10M2 12.5a10 10 0 0 0 18.9 2.73L22 14"/>
            </svg>
            <span>Manage Questions</span>
        </a>
    </nav>
    <div class="sidebar-footer">
        <a href="../loginsignup/login.aspx?action=logout" class="sidebar-link">
            <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/>
            </svg>
            <span>Logout</span>
        </a>
    </div>
</aside>
        <!-- MAIN CONTENT -->
        <main class="main-content">
            <header class="dashboard-header">
                <div>
                    <h1 class="dashboard-title">Quiz Approval</h1>
                    <p class="dashboard-subtitle">Approve or reject quizzes submitted by teachers.</p>
                </div>
            </header>

            <section class="content-section" id="quizzes">
                <div class="content-grid">
                    <asp:Repeater ID="rptPendingQuizzes" runat="server" OnItemCommand="rptPendingQuizzes_ItemCommand">
                        <ItemTemplate>
                            <div class="content-card">
                                <div class="content-header">
                                    <h3 class="content-title"><%# Eval("Title") %></h3>
                                    <span class="content-badge content-badge-pending">Pending</span>
                                </div>
                                <p class="content-meta">By: <%# Eval("TeacherName") %></p>
                                <p class="content-type">Category: <%# Eval("CategoryName") %> • <%# Eval("QuestionCount") %> Questions</p>
                                <div class="content-actions">
                                    <asp:LinkButton ID="btnApprove" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="ApproveQuiz" CommandArgument='<%# Eval("QuizID") %>'>Approve</asp:LinkButton>
                                    <asp:LinkButton ID="btnReject" runat="server" CssClass="btn btn-danger btn-sm"
                                        CommandName="RejectQuiz" CommandArgument='<%# Eval("QuizID") %>'>Reject</asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlNoPendingQuizzes" runat="server" Visible="false" 
                        style="text-align: center; padding: 20px; background: #fff; border-radius: 8px;">
                        <p>No quizzes are pending approval. Great job!</p>
                    </asp:Panel>
                </div>
            </section>
        </main>
    </div>
</form>
</body>
</html>