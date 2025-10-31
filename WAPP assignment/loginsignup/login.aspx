<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WAPP_assignment.login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" dir="ltr">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>KinderLearn Login</title>
    <link rel="stylesheet" href="../stylesheet/main.css" />
    <link rel="stylesheet" href="../stylesheet/login.css" />
</head>
<body>
    <div class="page-wrapper">
        <form id="form1" runat="server">
            <div class="container">
                <!-- Logo Section -->
                <div class="logo-section">
                    <img src="images/klogo.png" alt="KinderLearn Logo" class="logo-image" />
                    <h1 class="logo-text">KinderLearn</h1>
                </div>
                
                <h2>Welcome Back!</h2>
                
                <!-- Username Field -->
                <div class="form-field">
                    <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" 
                        placeholder="Enter your username"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername" 
                        ErrorMessage="Username is required."
                        ForeColor="Red" 
                        Display="Dynamic"
                        SetFocusOnError="false"></asp:RequiredFieldValidator>
                </div>

                <!-- Password Field -->
                <div class="form-field">
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" 
                        TextMode="Password" 
                        placeholder="Enter your password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword" 
                        ErrorMessage="Password is required."
                        ForeColor="Red" 
                        Display="Dynamic"
                        SetFocusOnError="false"></asp:RequiredFieldValidator>
                </div>

                <!-- Message Label -->
                <asp:Label ID="lblMessage" runat="server" CssClass="message-label" ForeColor="Red"></asp:Label>

                <!-- Login Button -->
                <asp:Button ID="loginBtn" runat="server" Text="Login" 
                    CssClass="btn-login" 
                    OnClick="loginBtn_Click" />

                <!-- Register Link -->
                <div class="register-link">
                    Don't have an account?
                    <a href="signup.aspx">Register here</a>
                </div>
            </div>
        </form>
    </div>
</body>
</html>