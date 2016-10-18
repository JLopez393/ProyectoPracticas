<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Descarga.aspx.cs" Inherits="FrontEnd_Liquidaciones.Descarga" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title></title>
        <link rel="icon" href="Img/cbc.ico"/>
        <!--CSS-->
        <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/Menu.css" rel="stylesheet" type="text/css" />
        <link href="Content/mui.css" rel="stylesheet" type="text/css" />
        <link href="Content/select2.css" rel="stylesheet" type="text/css" />
        <link href="Content/jquery.treegrid.css" rel="stylesheet" type="text/css"/>
        <!--<link href="Content/styles.css" rel="stylesheet" type="text/css"/>-->
        <!--JS-->
        <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>
        <script type="text/javascript" src="Scripts/Menu.js"></script>
        <script type="text/javascript" src="Scripts/mui.min.js"></script>
        <script type="text/javascript" src="Scripts/select2.js"></script>
        <script type="text/javascript" src="Scripts/jquery.treegrid.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();
                //$("#_partial_nav_bar").load("Menu.aspx");
                $('.myDatepicker').datepicker({
                    format: "dd/mm/yyyy",
                    /*startView: "year",
                    minViewMode: "months",*/
                    autoclose: true
                });
                $("#myDatepicker").keydown(function () {
                    return false;
                });
            });
        </script>
        <style type="text/css">
            @-moz-document url-prefix() {
                #generate_icon{
                          margin-top:12px;
                }
            }
            .tamano1 {
                width:1.2em; 
                height:1.2em;
            }
            .label {
                margin-left:30px;
                color:black;
                font-size:medium;
            }
            .style1
            {
                width: 97%;
                height: 115px;
            }
            #alpoee  {
                background-image: url("Img/logo-cbc.png");
                background-size: 100px 50px;
                background-repeat: no-repeat;
                padding-bottom: 4px; 
                padding-top: 10px; 
            }
                .side-menu > li a.disabled {
                cursor: not-allowed;
            }
            .color{
                width:14.7em;
                font-size: xx-small;
            }
            .color2{
                width:13em;
                font-size: xx-small;
                margin-left: -2.4em;
            }
            section{
                display:inline-block;
            }
            .tamano{
                width:1.2em; 
                height:1.2em;
            }
        </style>
    </head>
    <body>
        <form id="form1" runat="server">

             <!--Navbar-->
            <div id="_partial_nav_bar">
                <button id="boton_menu" type="button" class="menu-trigger btn btn-primary" style="margin-left:-1em;"><img src="Img/cbc.ico" width="20" height="20"/><!--<span class="glyphicon glyphicon glyphicon-list"></span>--> &nbsp; Menú Principal</button>
                <nav id="menu_principal" class="side-menu-wrapper mnu-open-part mnu-open-all" style="top:0px;" ><!--mnu-open-all-->
                    <div class="side-menu-scroller">
                        <ul class="side-menu">
                            <li class='content-slider'>
                                <ul class='menu-submenu'>
                                    <li><a class="submenu-toggler color touch">MENU</a></li>
                                    <li class="content-slider" style="display: list-item;">
                                        <ul class="menu-submenu">
                                            <li><a class="menu-item-main color2" href="P_Cobros.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Cobros%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Liquidacion.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Liquidacion%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Liberacion_Pedidos.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Liberacion_pedidos%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Cockpit.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Cockpit_hh%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Consulta_Clientes.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Consulta_clientes%>"/></a></li>
                                        </ul>
                                    </li>
                                    <li><a class="submenu-toggler color touch">REPORTES</a></li>
                                    <li class="content-slider" style="display: list-item;">
                                        <ul class="menu-submenu">
                                            <li><a class="menu-item-main color2" href="Gen_Reportes.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Gen_Reportes%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Spool_Reportes.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Spool_reportes%>"/></a></li>
                                            <li><a class="menu-item-main color2" href="Estatus_Liquidacion.aspx"><asp:Literal runat="server" Text="<%$Resources:Menu_aspx, Estatus_liquidacion%>"/></a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </nav>
            </div>
            <div class="not_wrapper"><!--ativo-->
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div id="alpoee" class="mui-btn mui-btn--primary btn-flat" style="width:100%; border-radius:0.5em; pointer-events: none; background-color: #0288D1">
                            <b class="titulo">Proceso de descarga para datos HH</b>
                        </div>
                        <div style="margin-top:200px; height:200px; width:500px; margin-left:370px;">
                            <table class="style1">
                                <tr>
                                    <td>
                                        <div style="margin-left:145px;">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="label" Text="Ruta: "></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="mui-textfield">
                                            <input id="asdf" runat="server" type="text" style="text-align:center;" readonly="readonly" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="true" Text ="Ingrese fecha de bajada: " CssClass="label"></asp:Label>
                                    </td>
                                    <td>
                                        <div class="mui-textfield">
                                            <input type="text" runat="server" id="myDatepicker" class="myDatepicker" placeholder="Fecha" style="text-align: center;" required="required"/>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <div style="margin-left:25px;">
                                            <asp:Button ID="Button1" runat="server" Text="DESCARGAR" class="mui-btn mui-btn--intro mui-btn--raised" OnClick="download"/>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>