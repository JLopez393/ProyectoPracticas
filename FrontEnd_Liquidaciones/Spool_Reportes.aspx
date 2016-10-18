<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Spool_Reportes.aspx.cs" Inherits="FrontEnd_Liquidaciones.Spool_Reportes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>FRONTEND - SPOOL REPORTES</title>
        <link rel="icon" href="Img/cbc.ico"/>
        <!--CSS-->
        <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/Menu.css" rel="stylesheet" type="text/css" />
        <link href="Content/mui.css" rel="stylesheet" type="text/css" />
        <link href="Content/select2.css" rel="stylesheet" type="text/css" />
        <!--JS-->
        <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>
        <script type="text/javascript" src="Scripts/Menu.js"></script>
        <script type="text/javascript" src="Scripts/mui.min.js"></script>
        <script type="text/javascript" src="Scripts/select2.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();
                $('.myDatepicker').datepicker({
                    format: "dd/mm/yyyy",
                    autoclose: true
                });

                $("#_partial_nav_bar").load("Menu.aspx");
            });

            /*--- START functions ---*/
            function CheckAllEmp(Checkbox) {
                console.log(Checkbox);
                var GridView = document.getElementById("GridView_Spool_Reportes");
                for (i = 1; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            function open_tab() {
                window.open("Reporte_PDF.aspx");
            }
        </script>
        <style type="text/css">
            @-moz-document url-prefix() {
                #generate_icon{
                          margin-top:12px;
                }
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
            #refresh_icon, #delete_icon{
                width:2em; 
                height:2em;
            }
            section{
                display:inline-block;
            }
            .circle_green {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: green;
            }
            .circle_yellow {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: yellow;
            }
            .circle_red {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: red;
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

            <div class="not_wrapper"><!--wrapper ativo-->
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div id="alpoee" class="mui-btn mui-btn--primary btn-flat" style="width:100%; border-radius:0.5em; pointer-events: none; background-color: #0288D1">
                            <b class="titulo">SPOOL REPORTES</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center; margin-top:0.5em;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:12%">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="myDatepicker" class="myDatepicker" placeholder="Fecha" style="text-align: center;"/>
                                            </div>
                                        </section>
                                        <section style="width:12%">
                                            <asp:ImageButton id="refresh_icon" runat="server" ImageUrl="Content/icon_png/repeat.png" OnClick="Refresh_Clicked"/>
                                            <br />
                                            <button id="search_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>REFRESCAR</button>
                                        </section>
                                        <section style="width:12%">
                                            <asp:ImageButton id="delete_icon" runat="server" ImageUrl="Content/icon_png/error.png" OnClick="Delete_Clicked"/>
                                            <br />
                                            <button id="delete_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>BORRAR</button>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <asp:GridView ID="GridView_Spool_Reportes" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" OnRowCommand="OnRowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NUM_JOB" HeaderText="NO. JOB"/>
                                <asp:BoundField DataField="FECHA" HeaderText="FECHA REPORTE" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="NOM_PROGRAMA" HeaderText="NOMBRE PROGRAMA"/>
                                <asp:BoundField DataField="RUTA" HeaderText="RUTA"/>
                                <asp:ButtonField CommandName="num_spool" DataTextField="NUM_SPOOL" HeaderText="NO. SPOOL" ButtonType="Link"/>
                                <asp:BoundField DataField="FECHA_GENERADO" HeaderText="FECHA GENERADO" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="HORA" HeaderText="HORA"/>
                                <asp:BoundField DataField="ESTATUS_JOB" HeaderText="ESTATUS"/>
                            </Columns>
                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:GridView runat="server" ID="hex_data" Visible="false"></asp:GridView>
        </form>
    </body>
</html>
