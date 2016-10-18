<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cockpit.aspx.cs" Inherits="FrontEnd_Liquidaciones.Cockpit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>FRONTEND - COCKPIT</title>
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
        <script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();
                $('.myDatepicker').datepicker({
                    format: "dd/mm/yyyy",
                });
                $(".myDatepicker").datepicker('setDate', 'today');

                $(".circle_red").click(function logerror() {
                    $("#sessions_sala_venta").val($(this).attr("sala_venta"));
                    $("#sessions_cet").val($(this).attr("cet"));
                    $("#sessions_ruta").val($(this).attr("ruta"));
                    $("#sessions_button").click();
                });

                setTimeout(function () {
                    $("#buttonError").fadeOut("slow");
                }, 3000);

                /*Hace que el header se quede estatico y se pueda hacer scroll(magicamente)*/
                $('#<%=All_Data.ClientID %>').Scrollable({
                    ScrollHeight: 400,
                    Width: 1200
                });
            });

            /*--- START functions ---*/
            function CheckAllEmp(Checkbox) {
                console.log(Checkbox);
                var GridView = document.getElementById("All_Data");
                for (i = 0; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            //Filtra la info en el gridview
            function Search_Gridview(strKey, strGV) {
                var strData = strKey.value.toLowerCase().split(" ");
                var tblData = document.getElementById(strGV);
                var rowData;
                for (var i = 0; i < tblData.rows.length; i++) {
                    rowData = tblData.rows[i].innerHTML;
                    var styleDisplay = 'none';
                    for (var j = 0; j < strData.length; j++) {
                        if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                            styleDisplay = '';
                        else {
                            styleDisplay = 'none';
                            break;
                        }
                    }
                    tblData.rows[i].style.display = styleDisplay;
                }
            }
            // ------------------ NO BORRAR: Guarda el estado al resfrescar
            //Filtra la info en el gridview
            function Search_Gridview_by_Value(strKey, strGV) {
                var strData = strKey.split(" ");
                var tblData = document.getElementById(strGV);
                var rowData;
                for (var i = 1; i < tblData.rows.length; i++) {
                    rowData = tblData.rows[i].innerHTML;
                    var styleDisplay = 'none';
                    for (var j = 0; j < strData.length; j++) {
                        if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                            styleDisplay = '';
                        else {
                            styleDisplay = 'none';
                            break;
                        }
                    }
                    tblData.rows[i].style.display = styleDisplay;
                }
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
            .Columna0 {
                width:27px;
                max-width:500px;
            }
            .Columna1 {
                width:125px;
                max-width:125px;
                text-align:center;
            }
            .Columna2, .Columna4, .Columna9, .Columna10, .Columna11 {
                min-width:50px;
                width:50px;
                max-width:50px;
                text-align:center;
            }
            .Columna3 {
                min-width:150px;
                width:150px;
                max-width:150px;  
                text-align:center;           
            }
            .Y {
                -webkit-transform: translateY(15%);  
                -o-transform: translateY(15%);    
                transform: translateY(15%); 
                
            }

            .Columna5, .Columna12 {
                min-width:73px;
                width:73px;
                max-width:73px;
                text-align:center;
            }
            .Columna6, .Columna7, .Columna8 {
                min-width:103px;
                width:103px;
                max-width:103px;
                text-align:center;
            }
            .Columna13 {
                min-width:190px;
                width:190px;
                max-width:190px;
                text-align:center;
            }
            
            
              
            #refresh_icon ,#upload_icon{
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
                float: left;
                margin-right: 0.2em;
            }
            .circle_yellow {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: yellow;
                float: left;
                margin-right: 0.2em;
            }
            .circle_red {
	            border-radius: 50%;
	            width: 1em;
	            height: 1em;
                background: red;
                float: left;
                margin-right: 0.2em;
                cursor: pointer;
            }
            #myDatepicker{
                pointer-events:none;
            }
            .hidden{
                display:none;
            }
            .flag_color{
                background-image: url("Content/icon_png/flag-3.png");
                background-size: 1.5em 1.2em;
                background-repeat: no-repeat;
	            height: 1.5em;
                margin-left: 1.5em;
            }
            .success {
                background-image: url("Content/icon_png/success.png");
                background-size: 1.2em 1.2em;
                background-repeat: no-repeat;
                height: 1.5em;
                margin-left: 1.5em;
            }  
            .tamano1 {
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
                            <b class="titulo">COCKPIT</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center; ">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:3%"></section>
                                        <section style="width:18%">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="myDatepicker" class="myDatepicker" placeholder="Fecha" style="text-align: center;"/>
                                            </div>
                                        </section>
                                        <section style="width:2%"></section>
                                        <section style="width:18%">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="ruta" runat="server" type="text" onkeyup="Search_Gridview(this, 'All_Data')"/>
                                                <label>Filtrar por SV, Ruta o Cet</label>
                                            </div>
                                        </section>
                                        <section style="width:2%"></section>
                                        <section style="width:18%">
                                            <asp:ImageButton id="refresh_icon" runat="server" ImageUrl="Content/icon_png/repeat.png" OnClick="Refresh_Clicked"/>
                                            <br/>
                                            <button id="refresh_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>REFRESCAR</button>
                                        </section>
                                        <section style="width:18%">
                                            <asp:ImageButton id="upload_icon" runat="server" ImageUrl="Content/icon_png/cloud-computing-2.png" OnClick="Upload_Clicked"/>
                                            <br/>
                                            <button id="upload_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>SUBIR</button>
                                        </section>
                                        <section style="width:18%">
                                            <asp:ImageButton id="download_icon" runat="server" CssClass="tamano1" ImageUrl="Content/icon_png/cloud-computing.png" OnClick="Download_Clicked"/>
                                            <br/>
                                            <button id="download_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>BAJAR</button>
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
                        <div id="get_data_sessions" style="display:none;">
                            <input id="sessions_sala_venta" runat="server"/>
                            <input id="sessions_cet" runat="server"/>
                            <input id="sessions_ruta" runat="server"/>
                            <asp:Button runat="server" ID="sessions_button" OnClick="Sessions_Clicked"/>
                        </div>
                        <asp:GridView runat="server" ID="pruebadt"></asp:GridView>
                        <center><button id="buttonError" runat="server" visible="false" class="mui-btn mui-btn--dark" disabled></button></center>
                        <asp:GridView ID="CSVENT" runat="server" Visible="false"></asp:GridView>
                        <asp:GridView ID="CCETS" runat="server" Visible="false"></asp:GridView>
                        <asp:GridView ID="All_Data" runat="server"  ForeColor="#333333" GridLines="none" CssClass="table" AutoGenerateColumns="false" OnRowCommand="OnRowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="Columna0">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="COD_SALA_VENTA" HeaderText="SALA VTA" ItemStyle-CssClass="Columna1" HeaderStyle-CssClass="Columna1"/>
                                <asp:BoundField DataField="SALA_VENTA" HeaderText="C.S.V" ItemStyle-CssClass="Columna2 Y" HeaderStyle-CssClass="Columna2"/>
                                <asp:BoundField DataField="COD_CET" HeaderText="CETS" ItemStyle-CssClass="Columna3 Y" HeaderStyle-CssClass="Columna3"/>
                                <asp:BoundField DataField="CET" HeaderText="C.C" ItemStyle-CssClass="Columna4 Y" HeaderStyle-CssClass="Columna4"/>
                                <asp:BoundField DataField="RUTA" HeaderText="RUTA" ItemStyle-CssClass="Columna5 Y" HeaderStyle-CssClass="Columna5"/>
                                <asp:BoundField DataField="FECHA_LIQUIDA" HeaderText="FECHA LIQ" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="Columna6 Y" HeaderStyle-CssClass="Columna6"/>
                                <asp:BoundField DataField="LISTA_VISITA" HeaderText="LISTA" ItemStyle-CssClass="Columna7 Y" HeaderStyle-CssClass="Columna7"/>
                                <asp:BoundField DataField="STATUS" HeaderText="SEMAFORO" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Columna8 Y" HeaderStyle-CssClass="Columna8"/>
                                <asp:BoundField DataField="REPORT1" HeaderText="A" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                <asp:BoundField DataField="LIQUIDA" HeaderText="B" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                <asp:BoundField DataField="REPORT2" HeaderText="C" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                <asp:ButtonField CommandName="proceso1" HeaderText="REP1" Text="VER" ItemStyle-CssClass="Columna9 Y" HeaderStyle-CssClass="Columna9" />
                                <asp:ButtonField CommandName="proceso2" HeaderText="LIQ" Text="LIQ" ItemStyle-CssClass="Columna10 Y" HeaderStyle-CssClass="Columna10" />
                                <asp:ButtonField CommandName="proceso3" HeaderText="REP2" Text="VER" ItemStyle-CssClass="Columna11 Y" HeaderStyle-CssClass="Columna11" />
                                <asp:BoundField DataField="STATUSF" HeaderText="ESTATUS" ItemStyle-CssClass="Columna12 Y" HeaderStyle-CssClass="Columna12"/>
                                <asp:BoundField DataField="MENSAJE" HeaderText="ESTADO" ItemStyle-CssClass="Columna13" HeaderStyle-CssClass="Columna13"/>
                            </Columns>
                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                        </asp:GridView>
                        <asp:GridView ID="routes" runat="server" Visible="false"></asp:GridView>
                        <center>
                            <asp:GridView ID="errors" runat="server" AutoGenerateColumns="false" Visible="false">
                                <Columns>
                                    <asp:BoundField DataField="RUTA" HeaderText="RUTA"/>
                                    <asp:BoundField DataField="FECHA_PROCESO" HeaderText="FECHA"/>
                                    <asp:BoundField DataField="MESSAGE" HeaderText="ERROR"/>
                                </Columns>
                            </asp:GridView>
                        </center>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>