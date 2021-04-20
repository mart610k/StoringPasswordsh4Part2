<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="StoringPasswordsH4Part2.WebForm1"%>


<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function DisableEnable() {
            document.getElementById("btn").disabled = true;
            setTimeout(function () {
                document.getElementById("btn").disabled = false;

                var input = document.createElement("input");
                input.type = "hidden";
                input.name = "btn";
                input.value = "Confirm and Forward To Checker";
                document.getElementById("form1").appendChild(input);
                document.getElementById("form1").submit();
            }, 2000);
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="UserNameLabel" runat="server" Text="UserName:"></asp:Label><br />
            <asp:TextBox ID="UserName" runat="server"></asp:TextBox><br />
            <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label><br />
            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:Button ID="SubmitButton" runat="server" Text="Login" OnClick="SubmitButton_Click" />
        </div>
    </form>
</body>
</html>
