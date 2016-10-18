 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Liquidacion.aspx.cs" Inherits="FrontEnd_Liquidaciones.Liquidacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>FRONTEND - LIQUIDACION</title>
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
                
                $('.myDatepicker').datepicker({
                    format: "dd/mm/yyyy",
                    autoclose: true
                });

                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });

                $('.tree').treegrid();

                var datos_busqueda_td_1 = $("#datos_busqueda tbody>tr").find('td').eq(2).text();
                var datos_busqueda_td_2 = $("#datos_busqueda tbody>tr").find('td').eq(3).text();
                var datos_busqueda_td_3 = $("#datos_busqueda tbody>tr").find('td').eq(4).text();
                var a = "<p style='display:inline-block'> " + datos_busqueda_td_1 + " - </p>&#32;<p style='display:inline-block'>  " + datos_busqueda_td_2 + " - </p>&#32;<p style='display:inline-block'> " + datos_busqueda_td_3 + " </p>"
                $("#data_panel_heading").html(a);
                
                $('#centro').val(datos_busqueda_td_1);

                if ($("#esstado").val == "liberar") {
                    clickLiberar();
                } else if ($("#esstado").val == "liquidar") {
                    clickLiquidar();
                } else if ($("#esstado").val == "log") {
                    clickLog();
                }
            });

            /*--- START functions ---*/
            var concatenacion;
            function clickLiberar() {
                $('#Button1').attr("disabled", "disabled");
                $('#Button2').removeAttr('disabled');
            }

            function clickLiquidar() {
                $('#Button1').attr("disabled", "disabled");
                $('#Button2').attr("disabled", "disabled");
                $('#Button3').removeAttr('disabled');
            }

            function clickLog() {
                $('#Button1').attr("disabled", "disabled");
                $('#Button2').attr("disabled", "disabled");
                $("#data_panel_heading").html(a);
            }

            function verError(texto) {
                var msgError = $("#buttonError");
                document.getElementById("buttonError").innerHTML = texto;
            }
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
            .hidden{
                display:none;
            }
            #out_icon{
                width:2em; 
                height:2em;
            }
            #plus_panel{
                width:0.8em; 
                height:0.8em;
            }
            #datos_generales thead>tr>th, 
            #datos_generales tbody>tr>th, 
            #datos_generales tfoot>tr>th, 
            #datos_generales thead>tr>td, 
            #datos_generales tbody>tr>td, 
            #datos_generales tfoot>tr>td{
                font-size: x-small;
            }
            #datos_especificos thead>tr>th, 
            #datos_especificos tbody>tr>th, 
            #datos_especificos tfoot>tr>th, 
            #datos_especificos thead>tr>td, 
            #datos_especificos tbody>tr>td, 
            #datos_especificos tfoot>tr>td,
            /*----------------------------*/
            #datos_items thead>tr>th, 
            #datos_items tbody>tr>th, 
            #datos_items tfoot>tr>th, 
            #datos_items thead>tr>td, 
            #datos_items tbody>tr>td, 
            #datos_items tfoot>tr>td
            {
                font-size: x-small;
            }

            #datos_generales tbody>tr:first-child{
                display:none;
            }

            .liquidadas-head {
                color: #0288D1;
            }

            .no-liquidadas-head {
                color: black;
            }
            .circulo {
                border-radius:50%;
                width: 15px;
                height: 15px;
            }
            .red {
                background: #e80000;
            }

            .green {
                background: #00c63e;
            }

            .nohidden {
                display:grid;
            }
            .fondoHeader {
                background-color: white;
            }

            .fondoAzul {
                background-color: #0288D1;
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
                            <b class="titulo">LIQUIDACION</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center;  margin-top:1em;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:18%;">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="centro" type="text" style="text-align:center;" disabled/>
                                                <label>Lista Visitas No</label>
                                            </div>
                                        </section>
                                        <section style="width:15%">
                                            <asp:Button ID="Button1" runat="server" Text="LIBERAR" class="mui-btn mui-btn--intro mui-btn--raised" OnClick="Liberar_Clicked"/>
                                        </section>
                                        <section style="width:15%">
                                            <asp:Button ID="Button2" runat="server" Text="LIQUIDAR" class="mui-btn mui-btn--intro mui-btn--raised" OnClick="Liquidar_Clicked" disabled/>
                                        </section>
                                        <section style="width:10%">
                                            <asp:Button ID="Button3" runat="server" Text="LOG" class="mui-btn mui-btn--intro mui-btn--raised" OnClick="Log_Clicked" disabled/>
                                        </section>
                                        <section style="width:10%">
                                            <asp:ImageButton id="out_icon" runat="server" ImageUrl="Content/icon_png/back.png" OnClick="Out_Clicked"/>
                                            <br/>
                                            <button id="out_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>REGRESAR</button>
                                        </section>
                                    </div>
                                </div>
                                <center><button id="buttonError" runat="server" visible="false" class="mui-btn mui-btn--dark" disabled></button></center>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div class="row">
                            <input runat="server" type="text" id="esstado" readonly="readonly" visible="false"/>
                            <input runat="server" type="text" id="ml" value="1" visible="false"/>
                            <!--------------------------------------------------------------------------------------------1RA PARTE(LADO IZQUIERDO)----------------------------------------------------------------------------------------------------------------------------------------------->
                            <div class="col-md-6">
                                <div class="col-sm-12 col-md-12">
                                    <asp:GridView ID="datos_busqueda" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false" RowStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                        <Columns>
                                            <asp:BoundField DataField="SLD_ID" HeaderText="SLD_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="TOUR_ID" HeaderText="TOUR_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="OBJ_ID" HeaderText="OBJ_ID"/>
                                            <asp:BoundField DataField="OBJ_TYP" HeaderText="OBJ_TYP"/>
                                            <asp:BoundField DataField="DRIVER" HeaderText="DRIVER"/>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
        
                                    <center><h2 id="data_panel_heading" runat="server" style="display:inline-block;"></h2></center>
        
                                    <asp:GridView ID="datos_generales" runat="server" GridLines="None" CssClass="table" AutoGenerateColumns="false" OnRowCommand="OnRowDataBound_Details">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton id="search_details_1" runat="server" ImageUrl="Content/icon_png/file-2.png" CssClass="tamano" CommandName="search_visit_id" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TOUR_ID" HeaderText="TOUR_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="VISIT_ID" HeaderText="VISIT_ID" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="CUSTNR" HeaderText="CUSTNR" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="NOK" HeaderText="NOK" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="VISCOD" HeaderText="VISCOD" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="NAME1" HeaderText="NAME1" HeaderStyle-CssClass="hidden"/>
                                            <asp:BoundField DataField="STRAS" HeaderText="STRAS" HeaderStyle-CssClass="hidden"/>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" />
                                        <RowStyle BackColor="#EFF3FB" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <!--------------------------------------------------------------------------------------------2DA PARTE(LADO DERECHO)----------------------------------------------------------------------------------------------------------------------------------------------->
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-sm-11 col-md-11">
                                       <asp:GridView ID="datos_especificos" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false" OnRowCommand="OnRowDataBound_Details" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton id="search_details_2" runat="server" ImageUrl="Content/icon_png/file-2.png" CssClass="tamano" CommandName="search_hh_delvry" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CLIENT" HeaderText="CLIENT" Visible="false" ReadOnly="true"/>
                                                <asp:BoundField DataField="TOUR_ID" HeaderText="TOUR_ID" Visible="false" ReadOnly="true"/>
                                                <asp:BoundField DataField="VISIT_ID" HeaderText="VISIT_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" ReadOnly="true"/>
                                                <asp:BoundField DataField="HH_DELVRY" HeaderText="NO.DOCUMENTO" ReadOnly="true"/>
                                                <asp:BoundField DataField="BE_DELVRY" HeaderText="ENTREGA" ReadOnly="true"/>
                                                <asp:TemplateField HeaderText="NO. PEDIDO">  
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server"  Text='<%#Eval("BSTKD")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>  
                                                        <asp:TextBox ID="txt_Name" runat="server"  Text='<%#Eval("BSTKD")%>' Width="100px"></asp:TextBox>  
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BSTDK" HeaderText="FECHA PEDIDO" ReadOnly="true"/>
                                                <asp:BoundField DataField="OBJ_TYP" HeaderText="OBJ_TYP" Visible="false" ReadOnly="true"/>
                                                <asp:BoundField DataField="PLANT" HeaderText="PLANT" Visible="false" ReadOnly="true"/>
                                                <asp:TemplateField HeaderText="FECHA CREACION">    
                                                     <ItemTemplate>    
                                                           <asp:Label ID="CRE_TSTAMP" runat="server"
                                                                 Text='<%#(DateTime.ParseExact((string)Eval("CRE_TSTAMP")+",531", "yyyyMMddHHmmss,fff", System.Globalization.CultureInfo.InvariantCulture)) %>'></asp:Label>
                                                      </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TZONE" HeaderText="TZONE" Visible="false" ReadOnly="true"/>
                                                <asp:CommandField AccessibleHeaderText="qwer" Visible="true"  ButtonType="Image" ControlStyle-CssClass="tamano1" CancelText="" DeleteText="" EditImageUrl="Content/icon_png/edit.png" EditText="Editar" InsertText="" NewText="" ShowEditButton="true" UpdateImageUrl="Content/icon_png/save.png" UpdateText="Actualizar" ShowCancelButton="false" />
                                                <asp:BoundField DataField="TA_STATUS" HeaderText="TA_STATUS" Visible="false"/>
                                                <asp:BoundField DataField="REASON" HeaderText="REASON" Visible="false"/>
                                                <asp:BoundField DataField="STATUS" HeaderText="STATUS" Visible="false"/>
                                                <asp:BoundField DataField="NOK" HeaderText="NOK" Visible="false"/>
                                            </Columns>
                                            <AlternatingRowStyle BackColor="White" />
                                            <EditRowStyle BackColor="#EAF1FB" />
                                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-11 col-md-11">
                                        <asp:GridView ID="datos_items" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="TOUR_ID" HeaderText="TOUR_ID" Visible="false"/>
                                                <asp:BoundField DataField="VISIT_ID" HeaderText="VISIT_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                                <asp:BoundField DataField="HH_DELVRY" HeaderText="HH_DELVRY" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                                                <asp:BoundField DataField="HH_DELVRY_IT" HeaderText="POSICION"/>
                                                <asp:BoundField DataField="MATNR" HeaderText="MATERIAL"/>
                                                <asp:BoundField DataField="QUAN" HeaderText="CANTIDAD"/>
                                                <asp:BoundField DataField="UOM" HeaderText="UM"/>
                                                <asp:BoundField DataField="MAKTX" HeaderText="TEXTO BREVE MATERIAL"/>
                                                <asp:BoundField DataField="TA_COD" HeaderText="TA_COD" Visible="false"/>
                                                <asp:BoundField DataField="REASON" HeaderText="REASON" Visible="false"/>                                            
                                            </Columns>
                                            <AlternatingRowStyle BackColor="White" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>    
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div class="row">
                            <div class="col-md-4"></div>
                            <div class="col-md-4">
                                <asp:GridView runat="server" ID="datos_log" Visible="false"></asp:GridView>
                                <div id="arbol_js" runat="server"></div>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                    </div>
                </div>
            </div> 
        </form>
    </body>
</html>