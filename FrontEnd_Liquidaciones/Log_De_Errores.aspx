<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Log_De_Errores.aspx.cs" Inherits="FrontEnd_Liquidaciones.Log_De_Errores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>FRONTEND - LOG DE ERRORES</title>
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
            });
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
            #out_icon{
                width:2em; 
                height:2em;
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
                            <b class="titulo">LOG DE ERRORES</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:18%">
                                             <div class="mui-textfield">
                                                <input type="text" runat="server" id="input_agencia" style="text-align:center;" readonly="readonly"/>
                                                <label>Sala de Ventas</label>
                                              </div>
                                        </section>
                                        <section style="width:18%">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="input_cet" style="text-align:center;" readonly="readonly"/>
                                                <label>Cet</label>
                                              </div>
                                        </section>
                                        <section style="width:18%">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="input_ruta" style="text-align:center;" readonly="readonly"/>
                                                <label>Ruta</label>
                                              </div>
                                        </section> 
                                        <section style="width:10%">
                                            <asp:ImageButton id="out_icon" runat="server" ImageUrl="Content/icon_png/back.png" OnClick="Out_Clicked"/>
                                            <br/>
                                            <button id="out_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>REGRESAR</button>
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
                        <center>
                            <asp:GridView ID="errors" runat="server" AutoGenerateColumns="false" Visible="true" ForeColor="#333333" GridLines="None" CssClass="table">
                                <Columns>
                                    <asp:BoundField DataField="PROCESO" HeaderText="PROCESO"/>
                                    <asp:BoundField DataField="CORRELATIVO" HeaderText="CORRELATIVO"/>
                                    <asp:BoundField DataField="RUTA" HeaderText="RUTA" Visible="false"/>
                                    <asp:BoundField DataField="FECHA_PROCESO" HeaderText="FECHA" Visible="false"/>
                                    <asp:BoundField DataField="TYPE" HeaderText="TYPE" Visible="false"/>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"/>
                                    <asp:BoundField DataField="ZNUMBER" HeaderText="NRO"/>
                                    <asp:BoundField DataField="MESSAGE" HeaderText="MENSAJE"/>
                                    <asp:BoundField DataField="LOG_NO" HeaderText="NRO LOG"/>
                                    <asp:BoundField DataField="LOG_MSG_NO" HeaderText="LOG MS"/>
                                    <asp:BoundField DataField="MESSAGE_V1" HeaderText="MENSAJE 1"/>
                                    <asp:BoundField DataField="MESSAGE_V2" HeaderText="MENSAJE 2"/>
                                    <asp:BoundField DataField="MESSAGE_V3" HeaderText="MENSAJE 3"/>
                                    <asp:BoundField DataField="MESSAGE_V4" HeaderText="MENSAJE 4"/>
                                    <asp:BoundField DataField="ZPARAMETER" HeaderText="PARAMETRO"/>
                                    <asp:BoundField DataField="ZROW" HeaderText="LINEA"/>
                                    <asp:BoundField DataField="FIELD" HeaderText="CAMPO"/>
                                </Columns>
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                            </asp:GridView>
                        </center>
                    </div>
                </div>
            </div>     
        </form>
    </body>
</html>