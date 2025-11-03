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
                <a href="teacherdashboard.aspx" class="sidebar-link"><span>Dashboard</span></a>
                <a href="teacherCourse.aspx" class="sidebar-link"><span>My Quizzes</span></a>
                <a href="teacherStudent.aspx" class="sidebar-link active"><span>Students</span></a>
            </nav>
            <div class="sidebar-footer">
                <a href="../loginsignup/login.aspx?action=logout" class="sidebar-link"><span>Logout</span></a>
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
                                        </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder ID="phNoStudents" runat="server" Visible="false">
                                <tr>
                                    <td colspan="5" style="text-align:center; padding: 20px;">
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