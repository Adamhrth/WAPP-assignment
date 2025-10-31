<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teacherStudent.aspx.cs" Inherits="WAPP_assignment.teacherStudent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Students - KinderLearn</title>
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
                <a href="teacherCourse.aspx" class="sidebar-link">
                    <svg class="sidebar-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"/>
                    </svg>
                    <span>My Quizzes</span>
                </a>
                <a href="teacherStudent.aspx" class="sidebar-link active">
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
                    <h1 class="dashboard-title">Students</h1>
                    <p class="dashboard-subtitle">Monitor student progress and performance</p>
                </div>
                <div class="user-profile">
                    <a href="teacherProfile.aspx" class="user-avatar-link">
                        <div class="user-avatar"><asp:Literal ID="litAvatar" runat="server">T</asp:Literal></div>
                    </a>
                </div>
            </header>

            <div class="students-filter-section">
                <div class="section-header-row">
                    <div class="search-box">
                        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/>
                        </svg>
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name or email..." CssClass="search-input" />
                    </div>
                    <asp:DropDownList ID="ddlQuizFilter" runat="server" CssClass="form-input" style="max-width: 200px;" />
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                </div>
            </div>

            <section class="students-section">
                <div class="students-table">
                    <table>
                        <thead>
                            <tr>
                                <th>Student</th>
                                <th>Email</th>
                                <th>Quiz</th>
                                <th>Score</th>
                                <th>Completed On</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="studentsTableBody">
                            <asp:Repeater ID="rptStudents" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="student-info">
                                                <div class="student-avatar"><%# GetAvatarInitial(Eval("StudentName")) %></div>
                                                <span><%# Eval("StudentName") %></span>
                                            </div>
                                        </td>
                                        <td><%# Eval("Email") %></td>
                                        <td><%# Eval("QuizTitle") %></td>
                                        <td>
                                            <span class='grade-badge <%# GetScoreBadgeClass(Eval("Score")) %>'>
                                                <%# Eval("Score", "{0:F0}%") %>
                                            </span>
                                        </td>
                                        <td><%# Eval("CompletedAt", "{0:g}") %></td>
                                        <td>
                                            <asp:HyperLink runat="server" CssClass="btn btn-outline btn-sm"
                                                NavigateUrl='<%# "StudentDetails.aspx?StudentID=" + Eval("StudentID") %>'>
                                                View
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder ID="phNoStudents" runat="server" Visible="false">
                                <tr>
                                    <td colspan="6" style="text-align:center; padding: 20px;">
                                        No students have completed your quizzes matching this filter.
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </tbody>
                    </table>
                </div>
            </section>
        </main>
    </div>
</form>
</body>
</html>