<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Liberacion_Pedidos.aspx.cs" Inherits="FrontEnd_Liquidaciones.Liberacion_Pedidos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>FRONTEND - LIBERACION PEDIDOS</title>
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
                });

                $(".myDatepicker").datepicker('setDate', 'today');

                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });
            });

            /*--- START functions ---*/
            function CheckAllEmp(Checkbox) {
                console.log(Checkbox);
                var GridView = document.getElementById("get_pedidos");
                for (i = 1; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            function notificacion_exito(numero_rows) {
                var show_notificacion = "";
                if (numero_rows == 0) {
                    show_notificacion = '<center><button id="rows_0" class="mui-btn mui-btn--dark">No se ha liberado ningun Pedido</button></center>';
                } else if (numero_rows == 1) {
                    show_notificacion = '<center><button id="rows_one" class="mui-btn mui-btn--intro">Se ha liberado correctamente ' + numero_rows + ' Pedido</button></center>';
                } else{
                    show_notificacion = '<center><button id="rows_infinite" class="mui-btn mui-btn--intro">Se han liberado correctamente ' + numero_rows + ' Pedidos</button></center>';
                }
                document.getElementById('notificacion_pedido').innerHTML = show_notificacion;
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
            #search_icon{
                width:2em; 
                height:2em;
            }
            
            section{
                display:inline-block;
            }
            #rows_0,#rows_one,#rows_infinite{
                pointer-events:none;
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

            <!--Contenido de la pagina-->
            <div class="not_wrapper"><!--wrapper ativo-->
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div id="alpoee" class="mui-btn mui-btn--primary btn-flat" style="width:100%; border-radius:0.5em; pointer-events: none; background-color: #0288D1">
                            <b class="titulo">LIBERACION PEDIDOS</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center; margin-top:1em;">
                                <input type="text" runat="server" id="fecha_liberacion" class="myDatepicker" placeholder="Fecha" style="display:none"/>
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:18%">
                                            <label>Agencia</label>
                                            <asp:DropDownList ID="cmbAgency" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:13%">
                                            <asp:ImageButton id="search_icon" runat="server" ImageUrl="Content/icon_png/search.png" OnClick="searck_clicked"/>
                                            <br/>
                                            <button id="search_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>BUSCAR</button>
                                        </section>
                                        <section style="width:15%">
                                            <asp:Button ID="liberar" runat="server" Text="LIBERAR PEDIDOS" class="mui-btn mui-btn--intro mui-btn--raised"  OnClick="liberate_clicked"/>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <center style="margin-top:0.5em;"><div id ="notificacion_pedido"></div></center>
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <asp:GridView ID="get_pedidos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="table">
                            <Columns>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </form>
    </body>
</html>
