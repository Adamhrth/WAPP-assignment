<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateQuiz.aspx.cs" Inherits="WAPP_assignment.CreateQuiz" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Create New Quiz - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/teachermain.css"/>
    <link rel="stylesheet" href="../stylesheet/teacherdashboard.css"/>
    <link rel="stylesheet" href="../stylesheet/teacher.css"/>
    <style>
        .form-container { background-color: #fff; padding: 25px; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }
        .form-group { margin-bottom: 20px; }
        .form-group label { display: block; font-weight: 600; margin-bottom: 8px; color: #333; }
        .form-control { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 5px; box-sizing: border-box; }
        .btn-submit { padding: 10px 20px; font-size: 16px; cursor: pointer; }
        
        /* --- NEW STYLES --- */
        .badge-section {
            border: 2px dashed #ddd;
            border-radius: 8px;
            padding: 20px;
            margin-top: 20px;
            background-color: #fcfcfc;
        }
        .badge-section-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 15px;
        }
        .badge-section-header h3 {
            margin: 0;
            color: #555;
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
                    <h1 class="dashboard-title">Create a New Quiz</h1>
                    <p class="dashboard-subtitle">Set up the quiz details and its achievement badge.</p>
                </div>
            </header>

            <div class="form-container">
                <div class="form-group">
                    <asp:Label ID="lblTitle" runat="server" Text="Quiz Title" AssociatedControlID="txtTitle" />
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="e.g., Basic Addition"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="A title is required." ForeColor="Red" Display="Dynamic" ValidationGroup="CreateQuiz" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblDescription" runat="server" Text="Description (Optional)" AssociatedControlID="txtDescription" />
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="e.g., A quick quiz on adding numbers from 1 to 10."></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblCategory" runat="server" Text="Category" AssociatedControlID="ddlCategories" />
                    <asp:DropDownList ID="ddlCategories" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategories" ErrorMessage="You must select a category." InitialValue="" ForeColor="Red" Display="Dynamic" ValidationGroup="CreateQuiz" />
                </div>

                <div class="badge-section">
                    <div class="badge-section-header">
                        <h3>🏆 Achievement Badge (Optional)</h3>
                        <asp:CheckBox ID="chkCreateBadge" runat="server" Text="Create a badge for this quiz" AutoPostBack="true" OnCheckedChanged="chkCreateBadge_CheckedChanged" />
                    </div>
                    
                    <asp:Panel ID="pnlBadgeFields" runat="server" Visible="false">
                        <p style="color: #555; margin-bottom: 15px;">
                            This badge will be awarded to students who get **100%** on this quiz.
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblBadgeName" runat="server" Text="Badge Name" AssociatedControlID="txtBadgeName" />
                            <asp:TextBox ID="txtBadgeName" runat="server" CssClass="form-control" placeholder="e.g., Addition Champion!"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBadgeName" runat="server" ControlToValidate="txtBadgeName" ErrorMessage="Badge name is required." ForeColor="Red" Display="Dynamic" ValidationGroup="CreateQuiz" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblBadgeImageURL" runat="server" Text="Badge Image URL" AssociatedControlID="txtBadgeImageURL" />
                            <asp:TextBox ID="txtBadgeImageURL" runat="server" CssClass="form-control" Text="images/badges/default_badge.png"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBadgeImageURL" runat="server" ControlToValidate="txtBadgeImageURL" ErrorMessage="Badge image URL is required." ForeColor="Red" Display="Dynamic" ValidationGroup="CreateQuiz" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="form-group" style="margin-top: 20px;">
                    <asp:Button ID="btnCreateQuiz" runat="server" Text="Create Quiz and Add Questions" 
                        CssClass="btn btn-primary btn-submit" 
                        OnClick="btnCreateQuiz_Click" ValidationGroup="CreateQuiz" />
                    <br />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                </div>
            </div>
        </main>
    </div>
</form>
</body>
</html>