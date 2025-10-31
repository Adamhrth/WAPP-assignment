<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="WAPP_assignment.signup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" dir="ltr">
<head runat="server">
    <title>Register to KinderLearn</title>
    <!-- Updated to use the new kid-friendly CSS file -->
    <link rel="stylesheet" href="../stylesheet/signup.css" />
</head>
<body>
    <div class="page-wrapper">
        <form id="form1" runat="server">
            <div class="container">
                <!-- KinderLearn logo section -->
                <div class="logo-section">
                    
                    <h1 class="logo-text">KinderLearn</h1>
                </div>
                
                <h2>Join the Fun!</h2>

                <!-- Wrapped First Name field in div for better layout control -->
                <div class="form-field">
                    <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                    <!-- Removed inline styles to allow CSS to control styling -->
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-input" placeholder="Enter your first name"></asp:TextBox>
                    <!-- Added SetFocusOnError="false" to prevent auto-validation on page load -->
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" 
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                </div>

                <!-- Wrapped Last Name field in div for better layout control -->
                <div class="form-field">
                    <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                    <!-- Removed inline styles -->
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-input" placeholder="Enter your last name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic"
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                </div>

                <!-- Wrapped Username field in div for better layout control -->
                <div class="form-field">
                    <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
                    <!-- Removed inline styles -->
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" placeholder="Choose a unique username"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic"
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvUsername" runat="server" ControlToValidate="txtUsername"
                        ErrorMessage="This username is taken." ForeColor="Red" Display="Dynamic"
                        ClientValidationFunction="validateUsername" SetFocusOnError="false" 
                        EnableClientScript="true"></asp:CustomValidator>
                </div>

                <!-- Wrapped Email field in div for consistency -->
                <div class="form-field">
                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic"
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Invalid Email format." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ForeColor="Red" Display="Dynamic" SetFocusOnError="false" 
                        EnableClientScript="true"></asp:RegularExpressionValidator>
                </div>

                <!-- Wrapped Password field in div for consistency -->
                <div class="form-field">
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic"
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                </div>

                <!-- Wrapped Confirm Password field in div for consistency -->
                <div class="form-field">
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input" TextMode="Password" placeholder="Confirm Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic"
                        SetFocusOnError="false" EnableClientScript="true"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPassword" runat="server"
                        ControlToCompare="txtPassword"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Passwords do not match"
                        ForeColor="Red" Display="Dynamic" SetFocusOnError="false" 
                        EnableClientScript="true"></asp:CompareValidator>
                </div>

                <!-- Wrapped Role field in div and added CssClass to DropDownList -->
                <div class="form-field">
                    <asp:Label ID="lblRole" runat="server" Text="I am a..."></asp:Label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Student" Value="student"></asp:ListItem>
                        <asp:ListItem Text="Teacher" Value="teacher"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="admin"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Added CssClass to Register button -->
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn-register" OnClick="btnRegister_Click" />
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                <!-- To direct to Login page -->
                <div class="login-link">
                    Already have an account? <a href="Login.aspx">Login</a>
                </div>
            </div>
        </form>
    </div>

    <!-- Added JavaScript for username validation -->
    <script type="text/javascript">
        // List of taken usernames for validation
        const takenUsernames = ['admin', 'teacher', 'student', 'test', 'user', 'kinderlearn', 'demo'];

        function validateUsername(source, args) {
            const username = args.Value.toLowerCase().trim();

            // Check if username is in the taken list
            if (takenUsernames.includes(username)) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
    </script>
</body>
</html>
