using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ClosedXML;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml.Serialization;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public class DatosPermitidos
    {
        public string mandt { get; set; }
        public string sala_venta { get; set; }
        public string cet { get; set; }
        public string ruta { get; set; }
    }

    public partial class Cockpit : System.Web.UI.Page
    {
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;
        List<DatosPermitidos> datos = new List<DatosPermitidos>();
        Convertir convert = new Convertir(); // Clase donde estan las funciones para convertir las tablas de SAP :)

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarUsuario();
                get_routes();
                svent();
                ccets();
                show_all();
                show_Errors();
            }
        }

        // Verificar la sesion de usuario
        protected void verificarUsuario()
        {
            if (Session["usuario"] == null || (string)Session["usuario"] == "")
            {
                Response.Redirect("login.aspx");
            }
        }

        //prueba la conexion con SAP
        public void Conexion()
        {
            try
            {
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                    //System.Diagnostics.Debug.WriteLine("God Job Try!!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connection Failure Error --> " + ex.Message);
            }
        }

        //busqueda en sap de los datos proporcionados
        public void get_routes()
        {
            DataSet ds = new DataSet();
            string name_function = "Zmaestro_Rutas";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Company.GetTable("R_RUTA")));
                DataSet dsCet = new DataSet();
                DataTable dtCet = new DataTable();
                DataSet cetsPermitidos = new DataSet();
                string sqlQuery = "SELECT d.detalle_op FROM usuario u inner join detalle_usuario d "
                                + "ON u.codigo_usuario = d.codigo_usuario WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=4";
                dataAdapter = new SqlDataAdapter(sqlQuery, connection);
                dataAdapter.Fill(dsCet);
                DataTable table = new DataTable("Agencias");
                table.Columns.Add("codigo", typeof(Int32));
                table = dsCet.Tables[0];
                DataRow[] foundRow;
                DataRow[] foundRow1;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    foundRow = table.Select("detalle_op = '" + dr[1].ToString() + "'");
                    foundRow1 = table.Select("detalle_op = 'ATOTAL'");
                    if (foundRow1.Length == 1)
                    {
                        DatosPermitidos dp = new DatosPermitidos();
                        dp.sala_venta = dr[1].ToString();
                        dp.cet = dr[2].ToString();
                        dp.ruta = dr[3].ToString();
                        datos.Add(dp);


                    }
                    else
                    {
                        for (int q = 0; q < foundRow.Length; q++)
                        {
                            DatosPermitidos dp = new DatosPermitidos();
                            dp.sala_venta = dr[1].ToString();
                            dp.cet = dr[2].ToString();
                            dp.ruta = dr[3].ToString();
                            datos.Add(dp);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- get_routes Error: " + ex.Message + " -->");
            }
        }

        DataSet CustomDataSet(List<DatosPermitidos> lista)
        {
            DataSet ds = new DataSet("datos");
            DataTable dt = ds.Tables.Add("datos");
            dt.Columns.Add("agencia", typeof(string));
            dt.Columns.Add("cet", typeof(string));
            dt.Columns.Add("ruta", typeof(string));

            foreach (DatosPermitidos s in lista)
            {
                dt.Rows.Add(s.sala_venta, s.cet, s.ruta);
            }
            ds.AcceptChanges();
            return ds;
        }

        //muestra todos los datos
        public void show_all()
        {
            

            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            string nSVent = null;
            string nCets = null;
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "REFRESH");
                IRfcTable i_tbl_refresh = rfcFunction_Company.GetTable("I_Tbl_Refresh");


                foreach (DatosPermitidos dp in datos)
                {

                    i_tbl_refresh.Append();
                    i_tbl_refresh.SetValue("SALA_VENTA", dp.sala_venta);
                    i_tbl_refresh.SetValue("CET", dp.cet);
                    i_tbl_refresh.SetValue("RUTA", dp.ruta);
                    i_tbl_refresh.SetValue("FECHA_LIQUIDA", fecha);

                }


                rfcFunction_Company.Invoke(rfcDestination);
                //ds.Tables.Add(convert.toGridView(rfcFunction_Company.GetTable("O_Tbl_Refresh"), prueba_Dataso));
                DataTable dtDatos = convert.toDataTable(rfcFunction_Company.GetTable("O_Tbl_Refresh"));

                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("COD_SALA_VENTA", typeof(string));
                dtFinal.Columns.Add("SALA_VENTA", typeof(string));
                dtFinal.Columns.Add("COD_CET", typeof(string));
                dtFinal.Columns.Add("CET", typeof(string));
                dtFinal.Columns.Add("RUTA", typeof(string));
                dtFinal.Columns.Add("FECHA_LIQUIDA", typeof(DateTime));
                dtFinal.Columns.Add("LISTA_VISITA", typeof(string));
                dtFinal.Columns.Add("MENSAJE", typeof(string));
                dtFinal.Columns.Add("STATUS", typeof(string));
                dtFinal.Columns.Add("REPORT1", typeof(string));
                dtFinal.Columns.Add("LIQUIDA", typeof(string));
                dtFinal.Columns.Add("REPORT2", typeof(string));
                dtFinal.Columns.Add("STATUSF", typeof(string));
                
                for (int x = 0; x < dtDatos.Rows.Count; x++)
                {
                    //System.Diagnostics.Debug.WriteLine(CSVENT.Rows[0].Cells[0].Text);
                    for (int y = 0; y < CSVENT.Rows.Count; y++) {
                        if (CSVENT.Rows[y].Cells[0].Text == dtDatos.Rows[x][0].ToString())
                        {

                            nSVent = CSVENT.Rows[y].Cells[1].Text;
                            break;
                        }
                    }
                    for (int zx = 0; zx < CCETS.Rows.Count; zx++) {
                        if (CCETS.Rows[zx].Cells[0].Text == dtDatos.Rows[x][1].ToString()) {
                            nCets = CCETS.Rows[zx].Cells[1].Text;
                            break;
                        }
                    }
                    
                    //System.Diagnostics.Debug.WriteLine(CSVENT.Rows[0].Cells[0].Text +" -- "+dtDatos.Rows[x][0].ToString());
                    //System.Diagnostics.Debug.WriteLine("prueba: -- " + nSVent);
                    DataRow data = dtFinal.NewRow();
                    data[0] = nSVent;
                    data[1] = dtDatos.Rows[x][0].ToString();
                    data[2] = nCets;
                    data[3] = dtDatos.Rows[x][1].ToString();
                    data[4] = dtDatos.Rows[x][2].ToString();
                    data[5] = dtDatos.Rows[x][3].ToString();
                    data[6] = dtDatos.Rows[x][4].ToString();
                    data[7] = dtDatos.Rows[x][5].ToString();
                    data[8] = dtDatos.Rows[x][6].ToString();
                    data[9] = dtDatos.Rows[x][7].ToString();
                    data[10] = dtDatos.Rows[x][8].ToString();
                    data[11] = dtDatos.Rows[x][9].ToString();
                    data[12] = dtDatos.Rows[x][10].ToString();
                    dtFinal.Rows.Add(data);
                }

                All_Data.DataSource = dtFinal;
                All_Data.DataBind();
                Semaforo();

                //All_Data.Columns[0].ControlStyle.CssClass = "Column0";
                //All_Data.Columns[1].ControlStyle.CssClass = "Column1";
                //All_Data.Columns[2].ControlStyle.CssClass = "Column2";
                //All_Data.Columns[3].ControlStyle.CssClass = "Column3";
                //All_Data.Columns[4].ControlStyle.CssClass = "Column4";
                //All_Data.Columns[5].ControlStyle.CssClass = "Column5";
                //All_Data.Columns[6].ControlStyle.CssClass = "Column6";
                //All_Data.Columns[7].ControlStyle.CssClass = "Column7";
                //All_Data.Columns[8].ControlStyle.CssClass = "Column8";
                //All_Data.Columns[9].ControlStyle.CssClass = "Column9";
                //All_Data.Columns[10].ControlStyle.CssClass = "Column10";
                //All_Data.Columns[11].ControlStyle.CssClass = "Column11";
                //All_Data.Columns[12].ControlStyle.CssClass = "Column12";
                //All_Data.Columns[13].ControlStyle.CssClass = "Column13";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- show_all Error: " + ex.Message + " -->");
            }
        }


//************************************************************************************************************************************************************************************************************************************



        protected void svent()
        {
            DataTable dt = new DataTable();
            //DataColumn dc;
            //int cont = 0;
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "SVENT");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Company.GetTable("It_Datos"), CSVENT));
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error svent() --> " + ex);
            }
            
        }

        public void ccets() {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "CETS");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Company.GetTable("It_Datos"), CCETS));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error svent() --> " + ex);
            }
        }

        //************************************************************************************************************************************************************************************************************************************

        //revisa que ButtonField es y realiza la accion
        protected void OnRowDataBound(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = All_Data.Rows[index];
                string verde = row.Cells[8].Text;

                if (e.CommandName == "proceso1" && verde.Equals("1"))
                {
                    Reporte_1(e);
                    Semaforo();
                    Save_Search();
                }
                else if (e.CommandName == "proceso2" && verde.Equals("1"))
                {
                    Reporte_Liq(e);
                    //Semaforo();
                    Response.Redirect("Liquidacion.aspx", false);
                }
                else if (e.CommandName == "proceso3" && verde.Equals("1"))
                {
                    Reporte_2(e);
                    Semaforo();
                    Save_Search();
                }
                else
                {
                    Semaforo();
                    Save_Search();
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("<-- OnRowDataBound Error: " + ex.Message + " -->");
                Save_Search();
            }
        }

        //genera el reporte #1
        protected void Reporte_1(GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = All_Data.Rows[index];
            string lista = row.Cells[7].Text;
            string verde = row.Cells[8].Text;
            //System.Diagnostics.Debug.WriteLine("<-- proceso1 LISTA: " + lista + " -->");

            DataSet ds = new DataSet();
            string name_function2 = "Zdsd_Pedidos_Cokpit";
            string name_function3 = "Zfv_Status";
            try
            {
                if (!lista.Trim().Equals("&nbsp;") && verde.Equals("1"))
                {
                    IRfcFunction rfcFunction_status = rfcRepository.CreateFunction(name_function3.ToUpper());
                    rfcFunction_status.SetValue("I_Lista_Visita", lista);
                    rfcFunction_status.SetValue("I_Rep", "2");
                    rfcFunction_status.Invoke(rfcDestination);

                    IRfcFunction rfcFunction_proceso1 = rfcRepository.CreateFunction(name_function2.ToUpper());
                    rfcFunction_proceso1.SetValue("I_Lista_Visita", lista);
                    rfcFunction_proceso1.SetValue("I_Tipo", "20");
                    rfcFunction_proceso1.Invoke(rfcDestination);
                    ds.Tables.Add(ConvertToDonNet_Excel(rfcFunction_proceso1.GetTable("O_Ped_Cokpit"), 1));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Reporte_1 Error: " + ex.Message + " -->");
            }

        }

        //genera el reporte #2
        protected void Reporte_2(GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = All_Data.Rows[index];
            string lista = row.Cells[7].Text;
            string verde = row.Cells[8].Text;
            //System.Diagnostics.Debug.WriteLine("<-- proceso3 LISTA: " + lista + " -->");
            DataSet ds = new DataSet();
            string name_function2 = "Zdsd_Ped_Confir_Liq";
            string name_function3 = "Zfv_Status";
            try
            {
                if (!lista.Trim().Equals("&nbsp;") && verde.Equals("1"))
                {
                    IRfcFunction rfcFunction_status = rfcRepository.CreateFunction(name_function3.ToUpper());
                    rfcFunction_status.SetValue("I_Lista_Visita", lista);
                    rfcFunction_status.SetValue("I_Rep", "4");
                    rfcFunction_status.Invoke(rfcDestination);

                    IRfcFunction rfcFunction_proceso3 = rfcRepository.CreateFunction(name_function2.ToUpper());
                    rfcFunction_proceso3.SetValue("I_Lista_Visita", lista);
                    rfcFunction_proceso3.SetValue("I_Tipo", "");
                    rfcFunction_proceso3.Invoke(rfcDestination);
                    ds.Tables.Add(ConvertToDonNet_Excel(rfcFunction_proceso3.GetTable("O_Ped_Liquidados"), 2));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Reporte_2 Error: " + ex.Message + " -->");
            }

        }

        //genera el reporte liquidacion
        protected void Reporte_Liq(GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = All_Data.Rows[index];
            string sociedad = row.Cells[2].Text;
            string ruta = row.Cells[5].Text;
            string lista = row.Cells[7].Text;
            string verde = row.Cells[8].Text;
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            DataSet ds = new DataSet();

            try
            {
                if (!lista.Trim().Equals("&nbsp;") && verde.Equals("1"))
                {
                    string name_function2 = "Ysdrep_Gps_Envio_Xml_Real";
                    IRfcFunction rfcFunction_status = rfcRepository.CreateFunction(name_function2.ToUpper());
                    rfcFunction_status.SetValue("GPSDATE1", fecha);
                    rfcFunction_status.SetValue("RUTA", ruta);
                    rfcFunction_status.Invoke(rfcDestination);

                    string name_function3 = "Ysdrep_Cierre_Agencia_Jr_Rui";
                    IRfcFunction rfcFunction_proceso3 = rfcRepository.CreateFunction(name_function3.ToUpper());
                    rfcFunction_proceso3.SetValue("FECHA", fecha);
                    rfcFunction_proceso3.SetValue("RUTA", ruta);
                    rfcFunction_proceso3.Invoke(rfcDestination);
                    //ds.Tables.Add(ConvertToDonNet(rfcFunction_proceso3.GetTable("IT_RUTAS_CET_JR"), ));

                    Session["Lista"] = lista;
                    Session["Sociedad"] = sociedad;
                    Session["Ruta"] = ruta;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Reporte_Liq Error: " + ex.Message + " -->");
            }

        }

        //sube la data a sap
        protected void upload()
        {
            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "SUBIDA");
                IRfcTable i_tbl_refresh = rfcFunction_Company.GetTable("I_Tbl_Refresh");
                foreach (GridViewRow row in All_Data.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        //mete la data en una tabla de sap
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        if (chkRow.Checked)
                        {

                            Session["Sal_Venta"] = row.Cells[2].Text;
                            Session["Cet"] = row.Cells[4].Text;
                            Session["ruta"] = row.Cells[5].Text;

                            i_tbl_refresh.Append();
                            i_tbl_refresh.SetValue("SALA_VENTA", row.Cells[2].Text);
                            i_tbl_refresh.SetValue("CET", row.Cells[4].Text);
                            i_tbl_refresh.SetValue("RUTA", row.Cells[5].Text);
                            i_tbl_refresh.SetValue("FECHA_LIQUIDA", fecha);
                        }
                    }
                }
                rfcFunction_Company.Invoke(rfcDestination);
                //ds.Tables.Add(ConvertToDonNet(rfcFunction_Company.GetTable("O_Tbl_Refresh"), GridView1));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Upload Error: " + ex.Message + " -->");
            }
        }

        //muestra la tabla de errores
        public void show_Errors()
        {
            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "ERROR");
                IRfcTable i_tbl_refresh = rfcFunction_Company.GetTable("I_Tbl_Refresh");
                for (int i = 0; i < routes.Rows.Count; i++)
                {
                    i_tbl_refresh.Append();
                    i_tbl_refresh.SetValue("SALA_VENTA", routes.Rows[i].Cells[1].Text);
                    i_tbl_refresh.SetValue("CET", routes.Rows[i].Cells[2].Text);
                    i_tbl_refresh.SetValue("RUTA", routes.Rows[i].Cells[3].Text);
                    i_tbl_refresh.SetValue("FECHA_LIQUIDA", fecha);
                }
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Company.GetTable("O_Tbl_Lerror"), errors));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- show_Errors Error: " + ex.Message + " -->");
            }
        }

        //pone los colores del semaforo segun el status
        public void Semaforo()
        {
            foreach (GridViewRow links in All_Data.Rows)
            {
                if (!links.Cells[8].Text.Equals("&nbsp;"))
                {
                    //pone el circulo dependiendo del texto del Estado
                    HtmlGenericControl div_green = new HtmlGenericControl("div");
                    HtmlGenericControl div_yellow = new HtmlGenericControl("div");
                    HtmlGenericControl div_red = new HtmlGenericControl("div");
                    div_green.Attributes["class"] = "circle_green";
                    div_yellow.Attributes["class"] = "circle_yellow";
                    div_red.Attributes["class"] = "circle_red";
                    div_red.Attributes["sala_venta"] = links.Cells[2].Text;
                    div_red.Attributes["cet"] = links.Cells[4].Text;
                    div_red.Attributes["ruta"] = links.Cells[5].Text;
                    if (links.Cells[8].Text.Equals("0"))
                    {
                        div_green.Style["opacity"] = "0.1";
                        div_yellow.Style["opacity"] = "0.1";
                    }
                    else if (links.Cells[8].Text.Equals("1"))
                    {
                        div_yellow.Style["opacity"] = "0.1";
                        div_red.Style["opacity"] = "0.1";
                    }
                    else if (!links.Cells[8].Text.Equals("&nbsp;") && !links.Cells[8].Text.Equals("0") && !links.Cells[8].Text.Equals("1"))
                    {
                        div_green.Style["opacity"] = "0.1";
                        div_red.Style["opacity"] = "0.1";
                    }
                    links.Cells[8].Controls.Add(div_green);
                    links.Cells[8].Controls.Add(div_yellow);
                    links.Cells[8].Controls.Add(div_red);
                }

                HtmlGenericControl status_F = new HtmlGenericControl("div");
                if (links.Cells[11].Text.Equals("X"))
                {
                    status_F.Attributes["class"] = "flag_color";
                    links.Cells[15].Controls.Add(status_F);
                }

                if (links.Cells[15].Text.Equals("X"))
                {
                    status_F.Attributes["class"] = "success";
                    links.Cells[15].Controls.Add(status_F);
                }
            }
        }

        //boton que llama a show_all()
        protected void Refresh_Clicked(object sender, EventArgs e)
        {
            get_routes();
            show_all();
            Save_Search();
        }

        //boton que llama a upload()
        protected void Upload_Clicked(object sender, EventArgs e)
        {
            upload();
            get_routes();
            show_all();
            Save_Search();
        }

        // boton que llama a Download();
        protected void Download_Clicked(object sender, EventArgs e)
        {
            int contar = 0;
            int contando = 0;
            string ruta = "";
            try
            {
                foreach (GridViewRow row in All_Data.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        //verifica que checkbox está marcado
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        if (chkRow.Checked)
                        {
                            Session["bajada_Sal_Venta"] = row.Cells[2].Text;
                            Session["bajada_Cet"] = row.Cells[4].Text;
                            Session["bajada_ruta"] = row.Cells[5].Text;

                            contar = contar + 1;
                            //System.Diagnostics.Debug.WriteLine("Ha seleccionado un checkbox");
                        }
                    }
                }
                if (contar > 1 | contar < 1)
                {
                    buttonError.Visible = true;
                    buttonError.InnerText = "Seleccione una";
                    Semaforo();
                }
                else if (contar == 1)
                {
                    Response.Redirect("Descarga.aspx", false);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Download_Clicked Error: " + ex.Message + " -->");
            }
        }

        // boton que llama a Save
        protected void Save_Search()
        {
            String var = ruta.Value;
            //System.Diagnostics.Debug.WriteLine("GUADADO: "+var);
            String script = @"<script type='text/javascript'>
                            Search_Gridview_by_Value('" + ruta.Value + "','All_Data');</script>";

            ScriptManager.RegisterStartupScript(this, typeof(Page), "Search_Gridview_by_Value", script, false);

        }

        // boton que llama a get session errors
        protected void get_session_errors()
        {
            //System.Diagnostics.Debug.WriteLine("get_session_errors funciono");
            Session["detalle_sala_venta"] = sessions_sala_venta.Value;
            Session["detalle_cet"] = sessions_cet.Value;
            Session["detalle_ruta"] = sessions_ruta.Value;
            Response.Redirect("Log_De_Errores.aspx", true);
        }

        // boton que llama a sessions
        protected void Sessions_Clicked(object sender, EventArgs e)
        {
            get_session_errors();
        }

        //convierte a excel
        public DataTable ConvertToDonNet_Excel(IRfcTable RFCTable, int No_reporte)
        {
            DataTable dtTable = new DataTable();
            try
            {
                for (int item = 0; item < RFCTable.ElementCount; item++)
                {
                    RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                    dtTable.Columns.Add(metadata.Name);
                }

                foreach (IRfcStructure row in RFCTable)
                {
                    DataRow dr = dtTable.NewRow();
                    for (int item = 0; item < RFCTable.ElementCount; item++)
                    {
                        RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                        if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                        {
                            dr[item] = row.GetInt(metadata.Name);
                        }
                        else
                        {
                            dr[item] = row.GetString(metadata.Name);
                        }
                    }
                    dtTable.Rows.Add(dr);
                }

                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dtTable, "Reporte");
                HttpResponse httpResponse = Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                if (No_reporte == 1)
                {
                    httpResponse.AddHeader("content-disposition", "attachment;filename=\"Reporte1.xlsx\"");
                }
                else if (No_reporte == 2)
                {
                    httpResponse.AddHeader("content-disposition", "attachment;filename=\"Reporte2.xlsx\"");
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                //httpResponse.End();
                httpResponse.Flush(); // Sends all currently buffered output to the client.
                httpResponse.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Excel Error: " + ex.Message + " -->");
            }
            return dtTable;
        }

    }
}