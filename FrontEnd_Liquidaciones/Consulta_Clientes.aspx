<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Consulta_Clientes.aspx.cs" Inherits="FrontEnd_Liquidaciones.Consulta_Clientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>FRONTEND - CONSULTA CLIENTES</title>
        <link rel="icon" href="Img/cbc.ico"/>
        <!--CSS-->
        <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/Menu.css" rel="stylesheet" type="text/css" />
        <link href="Content/mui.css" rel="stylesheet" type="text/css" />
        <link href="Content/select2.css" rel="stylesheet" type="text/css" />
        <!--JS-->
        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyCb56DVGEE14XyD1LoVB6cgatitjL_f0ME"></script><!--la key se tiene q generar en www.console.developers.google.com-->
        <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>
        <script type="text/javascript" src="Scripts/Menu.js"></script>
        <script type="text/javascript" src="Scripts/mui.min.js"></script>
        <script type="text/javascript" src="Scripts/select2.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();
                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });

                $("#_partial_nav_bar").load("Menu.aspx");

                $("#cliente").keypress(function (e) {
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        return false;
                    }
                });

                $("#cliente").keyup(function () {
                    if ($("#cliente").val() !='') {
                        $('#search_icon').removeAttr('disabled');
                        $("#search_icon").removeClass("deshabilitado");
                    } else {
                        
                        $('#search_icon').attr('disabled', 'disabled');
                        $("#search_icon").addClass("deshabilitado");
                    }
                });

                $(window).load(function () {
                    if ($("#cliente").val() != '') {
                        $('#search_icon').removeAttr('disabled');
                        $("#search_icon").removeClass("deshabilitado");
                    } else {
                        
                        $('#search_icon').attr('disabled', 'disabled');
                        $("#search_icon").addClass("deshabilitado");
                    }
                });
            });

            /*--- START functions ---*/
            var marker;
            function initialize() {
                var latitud = localStorage.getItem("latitud_T");
                var longitud = localStorage.getItem("longitud_T");
                var mapProp = {
                    center: new google.maps.LatLng(latitud, longitud),
                    zoom: 13,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(latitud, longitud)
                });
                marker.setMap(map);
            }
            google.maps.event.addDomListener(window, 'load', initialize);

            function data() {
                $("#data_not_found").hide();
                $("#Simple_Data").show();
            }
            function not_data() {
                $("#data_not_found").show();
                $("#explicit_data").hide();
                $("#Simple_Data").hide();
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
            .all_info {
                width:1.5em; 
                height:1.5em;
            }
            .select2-search__field{
                text-transform: uppercase;
            }
            .deshabilitado{
                cursor:not-allowed !important;
            }
            section{
                display:inline-block;
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
                            <b class="titulo">CONSULTA CLIENTES</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:10%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="cliente" runat="server" type="text" value="1013005" maxlength="10"/>
                                                <label>Cliente</label>
                                            </div>
                                        </section>
                                        <section style="width:3%"></section>
                                        <section style="width:12%">
                                            <label>Agencia</label>
                                            <asp:DropDownList ID="cmbAgency" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:10%">
                                            <asp:ImageButton id="search_icon" runat="server" ImageUrl="Content/icon_png/search.png" OnClick="search_clicked"/>
                                            <br />
                                            <button id="search_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>BUSCAR</button>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <center><button id="data_not_found" class="mui-btn mui-btn--dark" style="display:none" disabled>No se encontraron Datos con la Informacion proporcionada.</button></center>
                
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <asp:GridView ID="Simple_Data" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_add" class="all_info" runat="server" ImageUrl="Content/icon_png/add-1.png" OnClick="plus_clicked"/>
                                        <asp:ImageButton ID="btn_minus" class="all_info" runat="server" ImageUrl="Content/icon_png/minus-1.png" OnClick="minus_clicked" Visible="false"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Kunnr" HeaderText="Codigo"/>
                                <asp:BoundField DataField="Name1" HeaderText="Nombre"/>
                                <asp:BoundField DataField="Ort01" HeaderText="Municipio"/>
                                <asp:BoundField DataField="Sortl" HeaderText="No. Ruta"/>
                                <asp:BoundField DataField="Stras" HeaderText="Direccion"/>
                                <asp:BoundField DataField="Vkbur" HeaderText="Agencia"/>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                        <!--Gridview para obtener la informacion de los Labels-->
                        <asp:GridView ID="Complete_Data" runat="server" Visible="false"></asp:GridView>
                        <!--Gridview para obtener la informacion de las coordenadas-->
                        <asp:GridView ID="Coordenadas" runat="server" Visible="false"></asp:GridView>
                    </div>
                </div>

                <div id="explicit_data" class="row" style="display:none; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div class="row">
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <ul class="list-group">
                                    <li class="list-group-item">No Cliente: <asp:Label ID="Label1" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Nombre 1: <asp:Label ID="Label2" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Nombre 2: <asp:Label ID="Label3" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Nombre 3: <asp:Label ID="Label4" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Nombre 4: <asp:Label ID="Label5" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Direccion 1: <asp:Label ID="Label6" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Direccion 2: <asp:Label ID="Label7" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">NIT: <asp:Label ID="Label8" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Clasificacion de Clientes: <asp:Label ID="Label9" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Fecha de creacion del Registro: <asp:Label ID="Label19" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Cedula/DPI: <asp:Label ID="Label16" runat="server" Font-Bold="True"></asp:Label></li>
                                </ul>
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <ul class="list-group">
                                    <li class="list-group-item">Clave de condiciones de pago: <asp:Label ID="Label10" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Lista de las vias de pago a tener en cuenta: <asp:Label ID="Label11" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Zona de ventas: <asp:Label ID="Label12" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Oficina de ventas: <asp:Label ID="Label13" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Grupo de vendedores: <asp:Label ID="Label14" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Grupo de clientes: <asp:Label ID="Label20" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Tipo de lista de precios: <asp:Label ID="Label15" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Centro: <asp:Label ID="Label17" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Limite de credito del cliente: <asp:Label ID="Label18" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Numero de telefono: <asp:Label ID="Label21" runat="server" Font-Bold="True"></asp:Label></li>
                                    <li class="list-group-item">Numero de Fax: <asp:Label ID="Label22" runat="server" Font-Bold="True"></asp:Label></li>
                                </ul>
                            </div>  
                        </div>
                    </div>
                </div>

                <div id="maps_container" class="row" style="margin-top:0.5em; margin-left: 1em; margin-bottom:1em; display:none;">
                    <div class="col-xs-1 col-sm-1 col-md-2"></div>
                    <div class="col-xs-10 col-sm-10 col-md-9">
                        <div id="googleMap" style="width:auto; height:35em;"></div>
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-2"></div>
                </div>
            </div>
        </form>
    </body>
</html>