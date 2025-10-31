<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminManageBadges.aspx.cs" Inherits="WAPP_assignment.admin.AdminManageBadges" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Manage Badges - Admin</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <link rel="stylesheet" href="../stylesheet/admin.css"/>
    <style>
        .form-container { background-color: var(--color-background); padding: var(--spacing-xl); border-radius: var(--radius-lg); box-shadow: var(--shadow-sm); border: 2px solid var(--color-primary); margin-bottom: var(--spacing-xl); }
        .form-group { margin-bottom: var(--spacing-lg); }
        .form-label { display: block; font-weight: 600; margin-bottom: var(--spacing-sm); }
        .form-input { width: 100%; padding: var(--spacing-md); border: 2px solid var(--color-border); border-radius: var(--radius-md); font-size: 0.9375rem; }
        .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: var(--spacing-lg); }
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
                <a href="AdminUserManagement.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
                    <span>User Management</span>
                </a>
                <a href="AdminManageBadges.aspx" class="sidebar-link active">
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
                    <h1 class="dashboard-title">Manage Badges & Achievements</h1>
                    <p class="dashboard-subtitle">Create new global badges and manage all existing badges.</p>
                </div>
            </header>

            <section class="form-container">
                <h2 class="profile-section-title">Create New Global Badge</h2>
                <div class="profile-form">
                    <div class="form-group">
                        <label class="form-label" for="txtName">Badge Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-input" placeholder="e.g., Quiz Master"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Badge name is required." ForeColor="Red" Display="Dynamic" ValidationGroup="AddBadge" />
                    </div>
                    
                    <div class="form-row">
                        <div class="form-group">
                            <label class="form-label">Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-input"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Quizzes to Complete (at 100%)</label>
                            <asp:TextBox ID="txtQuizCount" runat="server" CssClass="form-input" TextMode="Number" Text="1"></asp:TextBox>
                            <asp:RangeValidator ID="rvQuizCount" runat="server" ControlToValidate="txtQuizCount" ErrorMessage="Must be 1 or more." ForeColor="Red" Type="Integer" MinimumValue="1" MaximumValue="1000" ValidationGroup="AddBadge" Display="Dynamic" />
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label class="form-label" for="txtDescription">Description (Auto-Generated)</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-input" placeholder="e.g., Complete 10 quizzes." ReadOnly="true" BackColor="#f4f6f9"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="txtBadgeImageURL">Badge Image URL</label>
                        <asp:TextBox ID="txtBadgeImageURL" runat="server" CssClass="form-input" Text="images/badges/default_badge.png"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBadgeImageURL" runat="server" ControlToValidate="txtBadgeImageURL" ErrorMessage="Image URL is required." ForeColor="Red" Display="Dynamic" ValidationGroup="AddBadge" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnAddBadge" runat="server" Text="Create Global Badge" 
                            CssClass="btn btn-primary" OnClick="btnAddBadge_Click" ValidationGroup="AddBadge" />
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" style="margin-left: 15px;"/>
                    </div>
                </div>
            </section>
            
            <section class="users-section">
                <h2 class="profile-section-title">All Existing Badges</h2>
                <div class="users-table">
                    <table>
                        <thead>
                            <tr>
                                <th>Badge</th>
                                <th>Name</th>
                                <th>Rule</th>
                                <th>Earned By</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptAchievements" runat="server" OnItemCommand="rptAchievements_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <img src='../<%# Eval("BadgeImageURL") %>' alt='<%# Eval("Name") %>' style="width: 50px; height: 50px; border-radius: 5px;" />
                                        </td>
                                        <td><%# Eval("Name") %></td>
                                        <td>
                                            <b><%# Eval("QuizCountThreshold") %></b> quiz(zes)
                                            in <b><%# Eval("CategoryName") %></b>
                                            <br/>
                                            <small style="color: #6c757d;"><%# Eval("Description") %></small>
                                        </td>
                                        <td><%# Eval("EarnedCount") %> Students</td>
                                        <td>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                                CommandName="DeleteBadge" CommandArgument='<%# Eval("AchievementID") %>'
                                                OnClientClick="return confirm('Are you sure you want to delete this badge? This will remove it from all students who have earned it.');">
                                                Delete
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                     <asp:Panel ID="pnlNoBadges" runat="server" Visible="false" Style="text-align: center; padding: 20px;">
                        <p>No achievements have been created yet.</p>
                    </asp:Panel>
                </div>
            </section>
        </main>
    </div>
</form>
</body>
</html>