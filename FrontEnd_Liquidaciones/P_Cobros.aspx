<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="P_Cobros.aspx.cs" Inherits="FrontEnd_Liquidaciones.P_Cobros" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>FRONTEND - P.COBROS</title>
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
        <script type="text/javascript" src="Scripts/ScrollableGridPlugin.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();

                var date = new Date();
                date.setDate(date.getDate() + 1);
                var day = date.getDay();
                if (day == 0) {
                    $("#domingo").show();
                } else {
                    $("#domingo").hide();
                }
                
                $('.myDatepicker').datepicker({
                    format: "dd/mm/yyyy",
                    startDate: date,
                    autoclose: true,
                });

                $(".myDatepicker").datepicker('setDate', date);

                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });

                $("#delete_icon").click(function () {
                    $('#GridView_Data_Cobros_chkboxSelectAll').prop('checked', false);
                });

                $("#myDatepicker").keydown(function () {
                    return false;
                });
            });
            
            /*--- START functions ---*/
            function CheckAllEmp(Checkbox) {
                var GridView = document.getElementById("GridView_Data_Cobros");
                for (i = 1; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            function data() {
                $("#data_not_found").hide();
                $("#GridView_Data_Cobros").show();
            }
            function not_data() {
                $("#data_not_found").show();
                $("#GridView_Data_Cobros").hide();
            }
            function rows_gridview_data() {
                $("#not_data").hide();
            }
            function not_rows_gridview_data() {
                $("#not_data").show();
            }
            function notificacion_exito(numero_rows) {
                var show_notificacion = "";
                if(numero_rows == 0){
                    show_notificacion = '<center><button id="rows_0" class="mui-btn mui-btn--dark">No se ha programado ningun Cobro</button></center>';
                } else if(numero_rows == 1) {
                    show_notificacion = '<center><button id="rows_one" class="mui-btn mui-btn--intro">Se ha programado correctamente ' + numero_rows + ' Cobro</button></center>';
                }else{
                    show_notificacion = '<center><button id="rows_infinite" class="mui-btn mui-btn--intro">Se han programado correctamente ' + numero_rows + ' Cobros</button></center>';
                }
                document.getElementById('notificacion_cobros').innerHTML = show_notificacion;
            }
            /*Hace que el header se quede estatico y se pueda hacer scroll(magicamente)*/
            function grid_fixed_header() {
                $('#<%=GridView_Data_Cobros.ClientID %>').Scrollable({
                    ScrollHeight: (window.innerHeight - $('#GridView_Data_Cobros').offset().top - 85),
                });
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
            th
            {
               text-align:center;
            }
            #search_icon ,#delete_icon ,#save_icon{
                width:2em; 
                height:2em;
            }
            .select2-search__field{
                text-transform: uppercase;
            }
            .circle_green {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: green;
            }
            .circle_red {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: red;
            }
            .table thead>tr>th, 
            .table tbody>tr>th, 
            .table tfoot>tr>th, 
            .table thead>tr>td, 
            .table tbody>tr>td, 
            .table tfoot>tr>td{
                font-size: x-small;
            }
            section{
                display:inline-block;
            }
            #rows_0,#rows_one,#rows_infinite{
                pointer-events:none;
            }
            .center{
                text-align:center;
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
                            <b class="titulo">P. COBROS</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center;">
                                <div class="row" style="margin-top:0.5em;">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:8%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="centro" type="text" runat="server"/>
                                                <label>Centro</label>
                                            </div>
                                        </section>
                                        <section style="width:8%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="Cet" type="text" runat="server"/>
                                                <label>Cet</label>
                                            </div>
                                        </section>
                                        <section style="width:8%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="ruta" type="text" runat="server" value="112101"/>
                                                <label>Ruta</label>
                                            </div>
                                        </section>
                                        <section style="width:8%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="cliente" type="text" runat="server"/>
                                                <label>Cliente</label>
                                            </div>
                                        </section>
                                        <section style="width:8%">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="myDatepicker" class="myDatepicker" placeholder="Fecha" style="text-align: center;"/>
                                            </div>
                                        </section>
                                        <section style="width:2%"></section>
                                        <section style="width:12%;">
                                            <label>Sociedad</label>
                                            <asp:DropDownList ID="cmbSociety" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:10%">
                                            <label>Oficina</label>
                                            <asp:DropDownList ID="cmbOffice" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:10%">
                                            <asp:ImageButton id="search_icon" runat="server" ImageUrl="Content/icon_png/search.png" OnClick="searck_clicked"/>
                                            <br />
                                            <button id="search_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>BUSCAR</button>
                                        </section>
                                        <section style="width:10%">
                                            <asp:ImageButton id="delete_icon" runat="server" ImageUrl="Content/icon_png/error.png" OnClick="deleted_clicked"/>
                                            <br />
                                            <button id="delete_icon_title" class="mui-btn mui-btn--flat mui-btn--danger" disabled>ELIMINAR</button>
                                        </section>
                                        <section style="width:10%">
                                            <asp:ImageButton id="save_icon" runat="server" ImageUrl="Content/icon_png/save.png" OnClick="saved_clicked"/>
                                            <br />
                                            <button id="save_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>GUARDAR</button>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <center style="margin-top:0.5em;"><button id="domingo" class="mui-btn mui-btn--dark" style="display:none" disabled>La programacion se realizara el dia Domingo.</button></center>
                <center style="margin-top:0.5em;"><button id="data_not_found" class="mui-btn mui-btn--dark" style="display:none" disabled>No se encontraron Datos con la Informacion proporcionados.</button></center>
                <center style="margin-top:0.5em;"><button id="not_data" class="mui-btn mui-btn--dark" style="display:none" disabled>No se han seleccionado Datos.</button></center>
                <center style="margin-top:0.5em;"><div id="notificacion_cobros"></div></center>
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <asp:GridView ID="GridView_Data_Cobros" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="CENTRO" HeaderText="CENTRO" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="OFIVTA" HeaderText="OF. VENTAS" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="GRPVTA" HeaderText="GRUPO VENTAS" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="RUTA" HeaderText="RUTA" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="CODCLI" HeaderText="COD. CLIENTE" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="DESCLI" HeaderText="NOMBRE" HeaderStyle-CssClass="center"/>
                                <asp:BoundField DataField="DIACRE" HeaderText="DIAS CREDI." HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="FACTUR" HeaderText="FACTURA" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="NUMFAC" HeaderText="NO. FACTURA" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="MONTO" HeaderText="MONTO" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="SALDO" HeaderText="SALDO" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="FECFAC" HeaderText="FECHA FACT" HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
                                <asp:BoundField DataField="DIASV" HeaderText="DIAS VENC." HeaderStyle-CssClass="center" ItemStyle-CssClass="center"/>
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