<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="studentpage.aspx.cs" Inherits="WAPP_assignment.studentpage" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>KinderLearn - My Learning Space</title>
    <link href="https://fonts.googleapis.com/css2?family=Fredoka:wght@400;500;600;700&family=Nunito:wght@400;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="../stylesheet/student.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header class="header">
                <div class="logo">
                    <div class="logo-icon">
                        <img src="images/klogo.png" alt="KinderLearn Logo" />
                    </div>
                    KinderLearn
                </div>
                <div class="welcome-message">
                    Hello, <asp:Literal ID="litStudentName" runat="server" Text="Student"></asp:Literal>! 👋
                </div>
                <a href="studentprofile.aspx" class="profile-section">
                    <div class="profile-avatar">
                        <asp:Literal ID="litProfileAvatar" runat="server" Text="S"></asp:Literal>
                    </div>
                    <div class="profile-name">My Profile</div>
                </a>
            </header>

            <main class="main-content">
                <section>
                    <div class="section-header">
                        <h2 class="section-title">
                            <span class="section-icon">📚</span>
                            Choose Your Subject
                        </h2>
                    </div>
                    
                    <div class="subjects-grid">
                        <asp:Repeater ID="rptCategories" runat="server" OnItemCommand="rptCategories_ItemCommand">
                            <ItemTemplate>
                                <div class='subject-card <%# Eval("Name").ToString().ToLower().Split(' ')[0] %>'>
                                    <div class="subject-icon">
                                        <%# GetCategoryIcon(Eval("Name").ToString()) %>
                                    </div>
                                    <h3 class="subject-name"><%# Eval("Name") %></h3>
                                    <p class="subject-description"><%# Eval("Description") %></p>
                                    <asp:Button ID="btnSelectCategory" runat="server" 
                                        CssClass="subject-button" 
                                        Text='<%# GetButtonText(Eval("Name").ToString()) %>'
                                        CommandName="Select" 
                                        CommandArgument='<%# Eval("CategoryID") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="view-more-wrapper">
                        <asp:Button ID="btnViewMoreSubjects" runat="server" CssClass="view-more-button" Text="View More ➜" OnClick="btnViewMoreSubjects_Click" />
                    </div>
                </section>

                <div class="encouragement-banner">
                    <div class="encouragement-text">You're Doing Amazing! 🌟</div>
                    <div class="encouragement-subtext">Keep learning and having fun every day!</div>
                </div>
            </main>
        </div>
    </form>
</body>
</html>