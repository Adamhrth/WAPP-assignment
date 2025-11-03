<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="studentprofile.aspx.cs" Inherits="WAPP_assignment.student.studentprofile" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My Profile - KinderLearn</title>
    <link rel="stylesheet" href="../stylesheet/studentprofile.css">
    
    <style>
        .profile-section {
            background-color: #fff;
            border-radius: 30px;
            padding: 40px;
            margin-bottom: 30px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
        }
        .profile-form {
            display: flex;
            flex-direction: column;
            gap: 20px;
            max-width: 800px;
            margin: 0 auto;
        }
        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
        }
        .form-group {
            margin-bottom: 5px;
        }
        .form-label {
            display: block;
            font-weight: 600;
            margin-bottom: 8px;
            color: #333;
        }
        .form-input {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 12px;
            font-size: 1rem;
            font-family: "Nunito", sans-serif;
            transition: all 0.3s ease;
        }
        .form-input:focus {
            outline: none;
            border-color: #ff6b9d;
            box-shadow: 0 0 0 4px rgba(255, 107, 157, 0.1);
        }
        .save-profile-button {
             background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
             color: white;
             border: none;
             padding: 15px 30px;
             border-radius: 50px;
             font-size: 18px;
             font-weight: 700;
             cursor: pointer;
             transition: all 0.3s ease;
             font-family: "Nunito", sans-serif;
             box-shadow: 0 4px 15px rgba(79, 172, 254, 0.3);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header class="header">
                <div class="logo">
                    <div class="logo-icon">
                        <img src="../images/klogo.png" alt="KinderLearn Logo" />
                    </div>
                    KinderLearn
                </div>
                <div class="header-actions">
                    <a href="studentpage.aspx" class="back-button">← Back to Subjects</a>
                    <a href="../loginsignup/login.aspx?action=logout" class="logout-button">Logout</a>
                </div>
            </header>

            <main class="main-content">
                
                <section class="profile-section">
                    <div class="profile-header-section" style="justify-content: center; text-align: center; flex-direction: column; gap: 20px;">
                        <div class="profile-avatar-large">
                            <span class="avatar-display">
                                <asp:Literal ID="litAvatar" runat="server">😊</asp:Literal>
                            </span>
                        </div>
                        <div class="profile-header-info">
                            <h2 class="profile-name"><asp:Literal ID="litProfileName" runat="server">Student Name</asp:Literal></h2>
                            <p class="profile-role">Student</p>
                        </div>
                    </div>
                </section>

                <section class="profile-section">
                    <h2 class="section-title" style="text-align:center; justify-content:center;">
                        <span class="section-icon">✏️</span>
                        Edit My Profile
                    </h2>
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
                        <div class="form-group">
                            <label class="form-label">Username</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required." ForeColor="Red" Display="Dynamic" ValidationGroup="UpdateProfile" />
                        </div>
                        <div class="form-group" style="text-align:center; margin-top: 15px;">
                            <asp:Button ID="btnSaveProfile" runat="server" Text="Save Profile Changes" 
                                CssClass="save-profile-button" 
                                OnClick="btnSaveProfile_Click" ValidationGroup="UpdateProfile" />
                            <br />
                            <asp:Label ID="lblProfileMessage" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                </section>

                <section class="character-section">
                    <h2 class="section-title">
                        <span class="section-icon">🎨</span>
                        Choose Your Character
                    </h2>
                    <asp:HiddenField ID="hfSelectedAvatar" runat="server" Value="😊" />
                    <div class="characters-grid">
                        <div class="character-option" onclick="selectAvatar(this, '😊')">
                            <div class="character-emoji">😊</div>
                            <div class="character-name">Smiley</div>
                        </div>
                        <div class="character-option" onclick="selectAvatar(this, '🚀')">
                            <div class="character-emoji">🚀</div>
                            <div class="character-name">Rocket</div>
                        </div>
                        <div class="character-option" onclick="selectAvatar(this, '🦄')">
                            <div class="character-emoji">🦄</div>
                            <div class="character-name">Unicorn</div>
                        </div>
                        <div class="character-option" onclick="selectAvatar(this, '⭐')">
                            <div class="character-emoji">⭐</div>
                            <div class="character-name">Star</div>
                        </div>
                        <div class="character-option" onclick="selectAvatar(this, '🐶')">
                            <div class="character-emoji">🐶</div>
                            <div class="character-name">Dog</div>
                        </div>
                        <div class="character-option" onclick="selectAvatar(this, '🤖')">
                            <div class="character-emoji">🤖</div>
                            <div class="character-name">Robot</div>
                        </div>
                    </div>
                    <div class="save-character-wrapper">
                        <asp:Button ID="btnSaveAvatar" runat="server" Text="Save Character" 
                            CssClass="save-character-button" 
                            OnClick="btnSaveAvatar_Click" />
                    </div>
                </section>

                <section class="achievements-section">
                    <h2 class="section-title">
                        <span class="section-icon">🏆</span>
                        My Achievements
                    </h2>
                    <div class="achievements-grid">
                        <asp:Repeater ID="rptAchievements" runat="server">
                             <ItemTemplate>
                                <div class='achievement-card <%# GetAchievementClass(Eval("EarnedAt")) %>'>
                                    <div class="achievement-badge">
                                        <%# GetBadgeIcon(Eval("EarnedAt"), Eval("BadgeType")) %>
                                    </div>
                                    <h3 class="achievement-title"><%# Eval("Name") %></h3>
                                    <p class="achievement-desc"><%# Eval("Description") %></p>
                                    <p class="achievement-date">
                                        <%# GetEarnedDate(Eval("EarnedAt")) %>
                                    </p>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </section>
            </main>
        </div>
    </form>
    
    <script type="text/javascript">
        function selectAvatar(element, avatar) {
            var allOptions = document.querySelectorAll('.character-option');
            allOptions.forEach(function (opt) {
                opt.classList.remove('selected');
            });
            element.classList.add('selected');
            var hf = document.getElementById('<%= hfSelectedAvatar.ClientID %>');
            hf.value = avatar;
        }

        function setInitialAvatar(avatar) {
            var allOptions = document.querySelectorAll('.character-option');
            allOptions.forEach(function (opt) {
                var emoji = opt.querySelector('.character-emoji').innerText;
                if (emoji === avatar) {
                    opt.classList.add('selected');
                }
            });
        }
    </script>
</body>
</html>