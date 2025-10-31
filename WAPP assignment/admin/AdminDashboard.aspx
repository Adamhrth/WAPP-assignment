<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="WAPP_assignment.admin.AdminDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Admin Dashboard - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <link rel="stylesheet" href="../stylesheet/admin.css"/>
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
                <a href="AdminDashboard.aspx" class="sidebar-link active">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/>
                    </svg>
                    <span>Dashboard</span>
                </a>
                <a href="AdminQuizApproval.aspx" class="sidebar-link">
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
                        <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8"V
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

        <main class="main-content">
            <header class="dashboard-header">
                <div>
                    <h1 class="dashboard-title">Admin Dashboard</h1>
                    <p class="dashboard-subtitle">Welcome, <asp:Literal ID="litAdminName" runat="server"></asp:Literal>!</p>
                </div>
                <div class="user-profile">
                    <a href="#" class="user-avatar-link">
                        <div class="user-avatar"><asp:Literal ID="litAvatar" runat="server">A</asp:Literal></div>
                    </a>
                </div>
            </header>

            <div class="hub-grid">
                <asp:HyperLink ID="hlQuizApproval" runat="server" CssClass="hub-card" NavigateUrl="AdminQuizApproval.aspx">
                    <div class="hub-card-header">
                        <div class="hub-card-icon pending">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/>
                                <line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/>
                            </svg>
                        </div>
                        <span>Quiz Approval</span>
                    </div>
                    <div class="hub-card-body">
                        <asp:Literal ID="litPendingQuizzes" runat="server">0</asp:Literal>
                    </div>
                    <div class="hub-card-footer">
                        <span>View Pending Quizzes &rarr;</span>
                    </div>
                </asp:HyperLink>

                <asp:HyperLink ID="hlUserManagement" runat="server" CssClass="hub-card" NavigateUrl="AdminUserManagement.aspx">
                    <div class="hub-card-header">
                        <div class="hub-card-icon users">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/>
                            </svg>
                        </div>
                        <span>User Management</span>
                    </div>
                    <div class="hub-card-body">
                        <asp:Literal ID="litTotalUsers" runat="server">0</asp:Literal>
                    </div>
                    <div class="hub-card-footer">
                        <span>Manage All Users &rarr;</span>
                    </div>
                </asp:HyperLink>

                <div class="hub-card" style="border-color: var(--color-success);">
                    <div class="hub-card-header">
                        <div class="hub-card-icon new-users">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="8.5" cy="7" r="4"/><line x1="20" y1="8" x2="20" y2="14"/><line x1="23" y1="11" x2="17" y2="11"/>
                            </svg>
                        </div>
                        <span>New Users This Week</span>
                    </div>
                    <div class="hub-card-body" style="color: var(--color-success);">
                        <asp:Literal ID="litNewUsers" runat="server">0</asp:Literal>
                    </div>
                    <div class="hub-card-footer">
                        <span>Joined in the last 7 days</span>
                    </div>
                </div>
            </div>
        </main>
    </div>
</form>
</body>
</html>