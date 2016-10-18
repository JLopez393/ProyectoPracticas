<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Estatus_Liquidacion.aspx.cs" Inherits="FrontEnd_Liquidaciones.Estatus_Liquidacion" %>
 
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
    <title>FRONTEND - ESTATUS LIQUIDACION</title>
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
        
        <script src="Scripts/highcharts.js"></script>
        <script type="text/javascript" src="Scripts/ScrollableGridPlugin.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                $(".touch").click();
                $(".search_dropdown").select2({
                    placeholder: "SELECCIONAR",
                    width: '100%'
                });
            });

            /*--- START functions ---*/
            function fill_chart(a, b, c) {
                $('#container').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'RUTAS LIQUIDADAS',
                        style: {
                            color: 'smokewhite'
                        }
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            }
                        }
                    },
                    series: [{
                        name: 'Rutas',
                        colorByPoint: true,
                        data: [{
                            name: 'No Liquidadas',
                            y: a,
                            id: 0
                        }, {
                            name: 'Pendientes',
                            y: c,
                            id: 1
                        }, {
                            name: 'Liquidadas',
                            y: b,
                            id: 2,
                            sliced: true,
                            selected: true
                        }],
                        point: {
                            events: {
                                click: function (event) {
                                    if (this.id == 0) {
                                        document.getElementById('NLiq_pie').click();
                                    } else if (this.id == 1) {
                                        document.getElementById('Pend_Pie').click();
                                    } else if (this.id == 2) {
                                        document.getElementById('Liq_Pie').click();
                                    }
                                }
                            }
                        }
                    }]
                })
            }

            //Probando colores 
            Highcharts.theme = {
                colors: ["#E74C3C", "#2c3e50", "#3498db", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
                   "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
                chart: {
                    backgroundColor: null,
                    style: {
                        fontFamily: "Dosis, sans-serif"
                    }
                },
                title: {
                    style: {
                        fontSize: '16px',
                        fontWeight: 'bold',
                        textTransform: 'uppercase'
                    }
                },
                tooltip: {
                    borderWidth: 0,
                    backgroundColor: 'rgba(219,219,216,0.8)',
                    shadow: false
                },
                legend: {
                    itemStyle: {
                        fontWeight: 'bold',
                        fontSize: '13px'
                    }
                },
                xAxis: {
                    gridLineWidth: 1,
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    }
                },
                yAxis: {
                    minorTickInterval: 'auto',
                    title: {
                        style: {
                            textTransform: 'uppercase'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    }
                },
                plotOptions: {
                    candlestick: {
                        lineColor: '#404048'
                    }
                },
                // General
                background2: '#3399FF'
            };
            // Apply the theme
            Highcharts.setOptions(Highcharts.theme);

            /*Hace que el header se quede estatico y se pueda hacer scroll(magicamente)*/
            function grid_fixed_header() {
                $('#<%=All_Data.ClientID %>').Scrollable({
                    ScrollHeight: (window.innerHeight - $('#All_Data').offset().top - 85),
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
            .prueba {
                color:black;
                height:80px;
                width:80px;
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
            #refresh_icon ,#upload_icon{
                width:2em; 
                height:2em;
            }
            #search_icon {
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
            .prueba {
                display:none;
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
                            <b class="titulo">ESTATUS LIQUIDACION</b>
                        </div>
                        <div class="thumbnail">
                            <div style="text-align:center; margin-top:0.5em;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <section style="width:18%">
                                            <label>Agencias</label>
                                            <asp:DropDownList ID="cmbAgency" runat="server" AutoPostBack="true" CssClass="select_mui search_dropdown" OnSelectedIndexChanged="seleccionarAgencia"></asp:DropDownList><!--GS12-->
                                        </section>
                                        <section style="width:18%">
                                            <asp:ImageButton id="refresh_icon" runat="server" ImageUrl="Content/icon_png/repeat.png" OnClick="Refresh_Clicked"/>
                                            <br/>
                                            <button id="refresh_icon_title" class="mui-btn mui-btn--flat mui-btn--primary" disabled>REFRESCAR</button>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-top:0.5em; margin-left: 1em;">
                            <div class="col-sm-8">
                                <div style="text-align:center; margin-top:0.5em;" runat="server" id="PieGrafica">
                                    <div id="container" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto"></div>
                                </div>
                            </div>
                            <div class="col-sm-3" id="gridview-left">
                                <asp:GridView ID="All_Data" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="SALA_VENTA" HeaderText="SALA VTA" Visible="false"/>
                                        <asp:BoundField DataField="CET" HeaderText="CET" Visible="false" />
                                        <asp:BoundField DataField="RUTA" HeaderText="RUTA"/>
                                        <asp:BoundField DataField="STATUS" HeaderText="SEMAFORO" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="prueba"/>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <HeaderStyle BackColor="#0288D1" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                </asp:GridView>
                                
                                <asp:GridView ID="routes" runat="server" Visible="false"></asp:GridView>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="All_Data1" Visible="false" Width="100px" runat="server" ForeColor="#333333" GridLines="None" CssClass="table" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="SALA_VENTA" HeaderText="SALA VTA" />
                            <asp:BoundField DataField="CET" HeaderText="CET" />
                            <asp:BoundField DataField="RUTA" HeaderText="RUTA"/>
                            <asp:BoundField DataField="STATUS" HeaderText="SEMAFORO" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="prueba"/>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <%--NO LIQUIDADAS--%>
            <a onclick="CallButtonClick2()" style="display:none"  href="javascript:void('0');">Call Server Function</a>
	        <asp:Button ID="NLiq_pie" runat="server"  Text="Call Button Click"  Style="display:none;" OnClick="NLiq_Clicked" />
            <%--PENDIENTES--%>
            <a onclick="CallButtonClick3()" style="display:none"  href="javascript:void('0');">Call Server Function</a>
	        <asp:Button ID="Pend_Pie" runat="server"  Text="Call Button Click"  Style="display:none;" OnClick="Pend_Clicked" />
            <%--LIQUIDADAS--%>
            <a onclick="CallButtonClick4()" style="display:none"  href="javascript:void('0');">Call Server Function</a>
	        <asp:Button ID="Liq_Pie" runat="server"  Text="Call Button Click"  Style="display:none;" OnClick="Liq_Clicked" />
        </form>
    </body>
</html>