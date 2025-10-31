<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminUserManagement.aspx.cs" Inherits="WAPP_assignment.admin.AdminUserManagement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>User Management - Admin</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <link rel="stylesheet" href="../stylesheet/admin.css"/>
    <style>
        /* New style for the permanent remove button */
        .btn-remove-user {
            background: none;
            border: 1px solid var(--color-danger);
            color: var(--color-danger);
            margin-left: 5px;
        }
        .btn-remove-user:hover {
            background: var(--color-danger);
            color: white;
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
                <a href="AdminDashboard.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/></svg>
                    <span>Dashboard</span>
                </a>
                <a href="AdminQuizApproval.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/></svg>
                    <span>Quiz Approval</span>
                </a>
                <a href="AdminUserManagement.aspx" class="sidebar-link active">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
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
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/></svg>
                    <span>Logout</span>
                </a>
            </div>
        </aside>

        <main class="main-content">
            <header class="dashboard-header">
                <div>
                    <h1 class="dashboard-title">User Management</h1>
                    <p class="dashboard-subtitle">Activate, deactivate, or permanently remove users.</p>
                </div>
            </header>

            <section class="users-section" id="users">
                <div class="users-table">
                    <table>
                        <thead>
                            <tr>
                                <th>User</th>
                                <th>Role</th>
                                <th>Status</th>
                                <th>Joined</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="user-info">
                                                <div class="user-avatar-small"><%# GetAvatarInitial(Eval("FirstName")) %></div>
                                                <div>
                                                    <div class="user-name"><%# Eval("FirstName") %> <%# Eval("LastName") %></div>
                                                    <div class="user-email"><%# Eval("Email") %></div>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <span class='role-badge <%# GetRoleClass(Eval("Role")) %>'>
                                                <%# Eval("Role") %>
                                            </span>
                                        </td>
                                        <td>
                                            <span class='status-badge <%# GetStatusClass(Eval("IsActive")) %>'>
                                                <%# GetStatusText(Eval("IsActive")) %>
                                            </span>
                                        </td>
                                        <td><%# Eval("CreatedAt", "{0:MMM d, yyyy}") %></td>
                                        <td>
                                            <asp:LinkButton ID="btnDeactivate" runat="server" CssClass="btn btn-danger btn-sm"
                                                CommandName="DeactivateUser" CommandArgument='<%# Eval("UserID") %>'
                                                Visible='<%# (bool)Eval("IsActive") %>'
                                                OnClientClick="return confirm('Are you sure you want to deactivate this user?');">
                                                Deactivate
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnActivate" runat="server" CssClass="btn btn-primary btn-sm"
                                                CommandName="ActivateUser" CommandArgument='<%# Eval("UserID") %>'
                                                Visible='<%# !(bool)Eval("IsActive") %>'>
                                                Activate
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-sm btn-remove-user"
                                                CommandName="RemoveUser" CommandArgument='<%# Eval("UserID") %>'
                                                Visible='<%# Convert.ToInt32(Eval("UserID")) != Convert.ToInt32(Session["UserID"]) %>'
                                                OnClientClick="return confirm('WARNING: You are about to PERMANENTLY DELETE this user and all their data (quizzes, attempts, badges).\n\nThis action cannot be undone. Are you sure?');">
                                                Remove
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </section>
        </main>
    </div>
</form>
</body>
</html>