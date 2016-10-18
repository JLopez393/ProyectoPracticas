<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="menu.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Login</title>
        <link rel="icon" href="Img/cbc.ico"/>
        <meta charset="UTF-8"/>
         <!--CSS--> 
        <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/Menu.css" rel="stylesheet" type="text/css" />
        <link href="Content/mui.css" rel="stylesheet" type="text/css" />
        
        <!--Scripts-->
        <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src="Scripts/Menu.js"></script>
        <script type="text/javascript" src="Scripts/mui.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(":input").keyup(function () {
                    var user = $("#user_lg").val().trim();
                   
                    if (user == "" || $("#pass").val() == "" || user.length < 1) {
                        $("#login_go").attr("disabled", "disabled");
                    } else {
                        $('#login_go').removeAttr('disabled');
                    }
                });
            });
            /*--- START functions ---*/
            /*--- START functions ---*/
            function user_not_found() {
                $("#not_user").show();
            }
        </script>
        <style>
            @-moz-document url-prefix() {
                #generate_icon{
                          margin-top:12px;
                }
            }
        .Centrar-H-V{
                position:absolute;
                margin:0;
                top:50%;
                left:50%;
                margin-right:-50%;
                transform: translate(-50%, -50%)
            }
        </style>
    </head>
    <body>
        <form id="form1" runat="server">       
            <div class="container Centrar-H-V" >
                 <div class="panel panel-primary" style="width:25em;">
                    <div class="panel-heading titulo">
                        <center><img src="Img/logo_completo.png"/></center>
                    </div>
                    <div class="panel-body" style="padding:2em 2em 2em 2em">
                        <div class="mui-textfield mui-textfield--float-label">
                            <input id="user_lg" type="text" runat="server"/>
                            <label>Usuario</label>
                        </div>
                        <div class="mui-textfield mui-textfield--float-label">
                            <input id="pass" type="password" runat="server"/>
                            <label>Contraseña</label>
                        </div>
                        <center>
                            <asp:Button ID="login_go" class="mui-btn mui-btn--intro mui-btn--raised" runat="server" onclick="login_succes" Text="LOGIN" disabled/>
                        </center>
                    </div>
                 </div>
                <center style="margin-top:0.5em;"><button id="not_user" class="mui-btn mui-btn--dark" style="display:none" disabled>Usuario Invalido</button></center>
            </div>
        </form>
    </body>
</html>