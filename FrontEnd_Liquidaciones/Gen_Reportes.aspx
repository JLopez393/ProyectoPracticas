<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gen_Reportes.aspx.cs" Inherits="FrontEnd_Liquidaciones.Gen_Reportes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>FRONTEND - GENERACION REPORTES</title>
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
                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });
                $("#myDatepicker").keydown(function () {
                    return false;
                });
                
            });

            /*--- START functions ---*/
            
            function CheckAllEmp(Checkbox) {
                console.log(Checkbox);
                var GridView = document.getElementById("GridView_Data_Cobros");
                for (i = 1; i < GridView.rows.length; i++) {
                    GridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            function Search_DropdownList(strKey, strDD) {
                var d = document.getElementById("cmbAgency");
                var c = document.getElementById("cmbCet");
                var s = document.getElementById("agenciaSeleccionadaTXT").value;
                var strData = d.options[d.selectedIndex].value;
                for (var i = 0; i < d.length; i++) {
                    
                }
                
            }
            function open_tab() {
                window.open("Reporte_PDF.aspx");
            }

            function ocultarAvanzado(estado) {
                if (estado) {
                    $("#consultaAvanzadaDiv").fadeOut("slow");
                    estado = false;
                } else {
                    $("#consultaAvanzadaDiv").fadeIn("slow");
                    estado = true;
                }
            };
            function verificoVisibilidad() {
                if ($("#visibleVar").val() == "true" && $("#algoVisibleVar").val() == "true") {
                    // $("#consultaAvanzadaDiv").show();
                    $("#consultaAvanzadaDiv").fadeIn("slow");
                }
                
            };

        </script>
        <script type="text/javascript" async="async">
            window.onload = function lanzar() {
                verificoVisibilidad();              
            };


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
                max-height:53px;
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
            section{
                display:inline-block;
            }
            /*IDS autogenerados por el plugin de select2*/
            #select2-cmbAgency-container,
            #select2-cmbReporte-container,
            #select2-cmbCet-container{
                max-width:16em;
                min-width:16em;
            }
            #generate_icon ,#clip_icon ,#consult_icon{
                width:2em; 
                height:2em;
            
            }
            .hide {
                display: none;
            }
            .show {
                display: block;
            }

        </style>
    </head>
    <body >
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
                            <b class="titulo">GENERACION DE REPORTES</b>
                        </div>
                        <div id="inputs_Gen_Reportes" class="thumbnail">
                            <div style="text-align:center;">
                                <!-- INICIO FILA -->
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:20%" id="reporteControl" runat="server">
                                            <label>Reporte</label>
                                            <asp:DropDownList ID="cmbReporte" runat="server" AutoPostBack="true" CssClass="select_mui search_dropdown" OnSelectedIndexChanged="cmbReporte_SelectedIndexChanged"></asp:DropDownList>
                                        </section>
                                        <section style="width:20%" id="agenciaControl" runat="server">
                                            <label>Agencia</label>
                                            <asp:DropDownList ID="cmbAgency" runat="server"   AutoPostBack="true" CssClass="select_mui search_dropdown" OnSelectedIndexChanged="agendaChanged"></asp:DropDownList>
                                        </section>
                                        <section style="width:20%" id="cetControl" runat="server">
                                            <label>Cet</label>
                                            <asp:DropDownList ID="cmbCet" runat="server" AutoPostBack="true" CssClass="select_mui search_dropdown" OnSelectedIndexChanged="cmbCet_SelectedIndexChanged"></asp:DropDownList>
                                        </section>
                                        <section style="width:10%" id="rutaControl" runat="server">
                                            <label>Ruta</label>
                                            <asp:DropDownList ID="cmbRuta" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:10%" id="fechaDeControl" runat="server">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="myDatepicker" class="myDatepicker" placeholder="Fecha del" style="text-align: center;" required/>                                  
                                            </div>
                                        </section>
                                        <section style="width:10%" id="Section1" runat="server">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="datePickerFechaAl" class="myDatepicker" placeholder="Fecha al" style="text-align: center;" required/>
                                            </div>                                         
                                        </section>
                                    </div>
                                </div>
                                <!-- FIN FILA -->
                            </div>
                            <div style="text-align:center;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:15%" id="section_generate_icon">
                                            <asp:ImageButton id="generate_icon" runat="server" ImageUrl="Content/icon_png/notepad-2.png" OnClick="pdf_clicked"/>
                                            <br/>
                                            <button id="upload_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>GENERAR REPORTE</button>
                                        </section>
                                        <section style="width:5%">
                                            <asp:ImageButton id="clip_icon" runat="server" ImageUrl="Content/icon_png/attachment.png" disabled/>
                                            <br/>
                                            <button id="refresh_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>CLIP</button>
                                        </section>
                                        <section style="width:15%">
                                            <asp:ImageButton id="consult_icon" runat="server" ImageUrl="Content/icon_png/newspaper.png" OnClick="consult_icon_Click"/>
                                            <br/>
                                            <button id="consulta_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>CONSULTA AVANZADA</button>
                                        </section>
                                    </div>
                                </div>
                            </div>
                            <center><button id="advertencia_final" runat="server" class="mui-btn mui-btn--dark" visible="false" disabled>La fecha de finalizacion inicial debe ser menor que la fecha de finalizacion final.</button></center>
                        </div>
                    </div>
                </div>
            </div>
            <div class="not_wrapper" id="consultaAvanzadaDiv" style="display:none" runat="server"><!--ativo-->
                <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                    <div class="col-md-1"></div>
                    <div class="col-sm-12 col-md-11">
                        <div id="Div2" class="mui-btn mui-btn--primary btn-flat" style="width:100%; border-radius:0.5em; pointer-events: none; background-color: #0288D1">
                            <b class="titulo">CONSULTA AVANZADA</b>
                        </div>
                        <input id="visibleVar" runat="server" type="text" value="false" style="display:none" />
                        <input id="algoVisibleVar" runat="server" type="text" value="false" style="display:none" />
                        <div id="Div3" class="thumbnail">
                            <div style="text-align:center;margin-top:20px">
                                <!-- INICIO FILA -->
                                <div class="row">
                                    <div class="col-sm-12 col-md-12"> 
                                        <section style="width: 20%" id="clienteSapControl" runat="server">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="txtClienteSap" runat="server" type="text"/>
                                                <label>Cliente SAP</label>
                                            </div>
                                        </section>
                                        <section style="width:20%" id="giroNegocioControl" runat="server">
                                            <label>Giro negocio</label>
                                            <asp:DropDownList ID="cmbGiroNegocio" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:20%" id="cadenaControl" runat="server">
                                            <label>Cadena</label>
                                            <asp:DropDownList ID="cmbCadena"  runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>                                        
                                          <section style="width:10%" id="condicionPagoControll" runat="server">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="datePickerCondicionPago"  class="myDatepicker" placeholder="Condicion pago" style="text-align: center;"/>
                                            </div>
                                        </section>                                        
                                    </div>
                                </div>
                                <!-- FIN FILA -->
                            </div>
                            <div style="text-align:center;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12"> 
                                        <section style="width: 20%" id="clienteSioControl" runat="server">
                                             <div class="mui-textfield mui-textfield--float-label">
                                                <input id="txtClienteSIO" runat="server" type="text"/>
                                                <label>Cliente SIO</label>
                                            </div>
                                        </section>   
                                        <section style="width:20%" id="tipoMercadoControl" runat="server">
                                            <label>Tipo mercado</label>
                                            <asp:DropDownList ID="cmbTipoMercado" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width:20%" id="documentoF1Control" runat="server">
                                            <label>Documento F1</label>
                                            <asp:DropDownList ID="cmbDocumentoF1" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section style="width: 20%" id="centroControl" runat="server">
                                            <div class="mui-textfield mui-textfield--float-label">
                                                <input id="txtCentro" runat="server" type="text"/>
                                                <label>Centro</label>
                                            </div>
                                        </section>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-md-12"> 
                                        <section style="width: 20%" id="archivoControl" runat="server">
                                            <label>Archivo</label><br />
                                            <input id="archivo" runat="server" type="file"/>
                                        </section>
                                        <section style="width:20%" id="catProductoControl" runat="server">
                                            <label>Cat Producto</label>
                                            <asp:DropDownList ID="cmbCatProductos" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                         <section style="width:10%" id="contratoControl" runat="server">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="contratoPicker" required class="myDatepicker" placeholder="Contrato" style="text-align: center;"/>
                                            </div>
                                        </section>
                                         <section style="width:10%" id="utilizacionControl" runat="server">
                                            <div class="mui-textfield">
                                                <input type="text" runat="server" id="utilizacionPicker" required class="myDatepicker" placeholder="Utilizacion" style="text-align: center;"/>
                                            </div>
                                        </section>
                                    </div>
                                </div>                                
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">                                         
                                        <section style="width:30%" id="grupoMaterialControl" runat="server">
                                            <label>Grupo material</label>
                                            <asp:DropDownList ID="cmbGrupoMaterial" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>                                        
                                        <section style="width:30%" id="materialControl" runat="server">
                                            <label>Material</label>
                                            <asp:DropDownList ID="cmbMaterial" runat="server" AutoPostBack="false" CssClass="select_mui search_dropdown"></asp:DropDownList>
                                        </section>
                                        <section>
                                            <div style="display:none">
                                                <asp:DropDownList ID="cmbCetsPermitidos" ClientIDMode="Static" runat="server" Visible="true"></asp:DropDownList>
                                                <asp:DropDownList ID="cmbTodosLosCets" runat="server" Visible="true"></asp:DropDownList>
                                            </div>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:GridView runat="server" ID="hex_data" Visible="false"></asp:GridView>                  
        </form>
    </body>
</html>
