<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teacherProfile.aspx.cs" Inherits="WAPP_assignment.teacher.teacherProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Teacher Profile - KinderLearn</title>
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
                <a href="teacherDashboard.aspx" class="sidebar-link">
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
                    <h1 class="dashboard-title">My Profile</h1>
                    <p class="dashboard-subtitle">Manage your account settings</p>
                </div>
                <div class="user-profile">
                    <a href="teacherProfile.aspx" class="user-avatar-link">
                        <div class="user-avatar"><asp:Literal ID="litAvatarHeader" runat="server">T</asp:Literal></div>
                    </a>
                </div>
            </header>

            <div class="profile-container">
                <section class="profile-section">
                    <div class="profile-header-section">
                        <div class="profile-avatar-large"><asp:Literal ID="litAvatarLarge" runat="server">T</asp:Literal></div>
                        <div class="profile-header-info">
                            <h2 class="profile-name"><asp:Literal ID="litProfileName" runat="server">Teacher Name</asp:Literal></h2>
                            <p class="profile-role">Teacher</p>
                            <p class="profile-email"><asp:Literal ID="litProfileEmail" runat="server">teacher@email.com</asp:Literal></p>
                        </div>
                    </div>
                </section>

                <section class="profile-section">
                    <h3 class="profile-section-title">Personal Information</h3>
                    <div class="profile-form">
                        <div class="form-row">
                            <div class="form-group">
                                <label class="form-label">First Name</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-input"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." ForeColor="Red" Display="Dynamic" ValidationGroup="UpdateProfile" />
                            </div>
                            <div class="form-group">
                                <label class="form-label">Last Name</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-input"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." ForeColor="Red" Display="Dynamic" ValidationGroup="UpdateProfile" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group">
                                <label class="form-label">Email Address</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" TextMode="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" ValidationGroup="UpdateProfile" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ForeColor="Red" Display="Dynamic" ValidationGroup="UpdateProfile" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveChanges_Click" ValidationGroup="UpdateProfile" />
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" style="margin-left: 15px;"></asp:Label>
                        </div>
                    </div>
                </section>

                <section class="profile-section">
                    <h3 class="profile-section-title">Teaching Information</h3>
                    <div class="profile-stats-grid">
                        <div class="profile-stat-card">
                            <p class="profile-stat-label">Total Students</p>
                            <p class="profile-stat-value"><asp:Literal ID="litTotalStudents" runat="server">0</asp:Literal></p>
                        </div>
                        <div class="profile-stat-card">
                            <p class="profile-stat-label">Approved Quizzes</p>
                            <p class="profile-stat-value"><asp:Literal ID="litActiveQuizzes" runat="server">0</asp:Literal></p>
                        </div>
                    </div>
                </section>

                <section class="profile-section">
                    <h3 class="profile-section-title">Security</h3>
                    <div class="profile-security">
                        <asp:HyperLink ID="hlChangePassword" runat="server" CssClass="btn btn-outline" NavigateUrl="teacherChangePassword.aspx">Change Password</asp:HyperLink>
                    </div>
                </section>
            </div>
        </main>
    </div>
</form>
</body>
</html>