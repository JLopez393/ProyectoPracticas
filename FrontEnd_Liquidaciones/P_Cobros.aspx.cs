using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public partial class P_Cobros : System.Web.UI.Page
    {

        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;
        Convertir convert = new Convertir();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarUsuario();
                fill_society();
                fill_office();
            }
        }

        // Verificar la sesion de usuario
        protected void verificarUsuario() {
            if (Session["usuario"] == null || (string)Session["usuario"] == "" )
            {
                Response.Redirect("login.aspx");
            }
        }

        //llena el combo de sociedad en P_Cobros
        protected void fill_society()
        {
            DataTable dt = new DataTable();
            DataTable tabla_sociedad = new DataTable();
            string sql_society = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                 " WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=1 ORDER BY detalle_op ASC ";
            try
            {
                dataAdapter = new SqlDataAdapter(sql_society, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);

                DataRow[] detalle_op = dt.Select("detalle_op = 'ATOTAL'");

                if (detalle_op.Length == 1)
                {
                    string sql_society_all = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                 " WHERE d.maestro=1 ORDER BY detalle_op ASC ";
                    dataAdapter = new SqlDataAdapter(sql_society_all, connection);
                    commandBuilder = new SqlCommandBuilder(dataAdapter);
                    dataAdapter.Fill(tabla_sociedad);
                    foreach (DataRow dr in tabla_sociedad.Rows)
                    {
                        if (dr["detalle_op"].ToString().Equals("ATOTAL"))
                        {
                            dr.Delete();
                        }
                    }
                    tabla_sociedad.AcceptChanges();
                    cmbSociety.DataSource = tabla_sociedad;
                }
                else
                {
                    cmbSociety.DataSource = dt;
                    
                }
                cmbSociety.DataTextField = "detalle_op";
                cmbSociety.DataValueField = "detalle_op";
                cmbSociety.DataBind();
               
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error fill_society() --> " + ex.Message);
            }
        }

        //llena el combo de agencia en P_Cobros
        protected void fill_office()
        {
            string sql_office = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                " WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=4 ORDER BY detalle_op ASC ";
            try
            {
                DataTable dt = new DataTable();
                DataTable table_office = new DataTable();
                dataAdapter = new SqlDataAdapter(sql_office, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);

                DataRow[] detalle_op = dt.Select("detalle_op = 'ATOTAL'");
                if (detalle_op.Length == 1)
                {
                    string sql_office_all = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                " WHERE d.maestro=4 ORDER BY detalle_op ASC ";
                    dataAdapter = new SqlDataAdapter(sql_office_all, connection);
                    commandBuilder = new SqlCommandBuilder(dataAdapter);
                    dataAdapter.Fill(table_office);
                    foreach (DataRow dr in table_office.Rows)
                    {
                        if (dr["detalle_op"].ToString().Equals("ATOTAL"))
                        {
                            dr.Delete();
                        }
                    }
                    table_office.AcceptChanges();
                    cmbOffice.DataSource = table_office;
                }
                else
                {
                    cmbOffice.DataSource = dt;
                }
                cmbOffice.DataTextField = "detalle_op";
                cmbOffice.DataValueField = "detalle_op";
                cmbOffice.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error fill_office() --> " + ex.Message);
            }
        }

        //prueba la conexion con SAP
        protected void Conexion()
        {
            try
            {
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error Conexion() --> " + ex.Message);
            }
        }
        
        //busqueda en sap de los datos proporcionados
        protected void Data_search()
        {
            DataSet ds = new DataSet();
            string name_function = "Zdsd_Func_Progra_Cobros";
            try
            {
                String fecha_original = myDatepicker.Value.ToString();
                Char delimitador = '/';
                String[] substrings = fecha_original.Split(delimitador);
                string fecha = substrings[2] + substrings[1] + substrings[0];

                IRfcFunction rfcFunction_Data_Search = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Centro", centro.Value.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Cet", Cet.Value.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Cliente", cliente.Value.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Fecha", fecha);
                rfcFunction_Data_Search.SetValue("I_Oficinaventa", cmbOffice.Text.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Ruta", ruta.Value.ToUpper());
                rfcFunction_Data_Search.SetValue("I_Sociedad", cmbSociety.Text.ToUpper());
                rfcFunction_Data_Search.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Data_Search.GetTable("O_Detalle_Doc"), GridView_Data_Cobros));
                
                //revisa cuantos valores tiene para mostrar o quitar un mensaje y darle la propiedad de fixed_headers al Gridview
                if (convert.toGridView(rfcFunction_Data_Search.GetTable("O_Detalle_Doc"), GridView_Data_Cobros).Rows.Count == 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "not_data", "not_data();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "data", "data(); grid_fixed_header();", true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error Data_search() --> " + ex.Message + " -->");
            }
        }

        //pone los circulos dependiendo el estado
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try 
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    TableCell statusCell = e.Row.Cells[1];
                    if (statusCell.Text == "RO")
                    {
                        div.Attributes["class"] = "circle_red";
                    }
                    if (statusCell.Text == "VE")
                    {
                        div.Attributes["class"] = "circle_green";
                    }
                    e.Row.Cells[1].Controls.Add(div);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error OnRowDataBound() --> " + ex.Message + " -->");
            }
        }

        //manda a llamar Data_search
        protected void searck_clicked(object sender, EventArgs e)
        {
            Data_search();
            ClientScript.RegisterStartupScript(GetType(), "all_selected", "$('#GridView_Data_Cobros_chkboxSelectAll').click();", true);
        }

        //esconde los rows que tienen checkbox seleccionado
        protected void Hide_rows()
        {
            try
            {
                foreach (GridViewRow row in GridView_Data_Cobros.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        //esconde las rows que estan seleccionadas
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        if (chkRow.Checked)
                        {
                            GridView_Data_Cobros.Rows[row.RowIndex].Visible = false;
                        }

                        //pone el circulo dependiendo del texto del Estado
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        TableCell statusCell = row.Cells[1];
                        if (statusCell.Text == "RO")
                        {
                            div.Attributes["class"] = "circle_red";
                        }
                        if (statusCell.Text == "VE")
                        {
                            div.Attributes["class"] = "circle_green";
                        }
                        row.Cells[1].Controls.Add(div);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error OnRowDataBound() --> " + ex.Message + " -->");
            }
        }

        //manda a llamar hide_rows 
        protected void deleted_clicked(object sender, EventArgs e)
        {
            try 
            {
                if (GridView_Data_Cobros.Rows.Count > 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "not_data_hide", "rows_gridview_data();", true);
                    Hide_rows();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "not_data_show", "not_rows_gridview_data();", true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error hide_rows() --> " + ex.Message + " -->");
            }  
        }

        //manda los datos de los checkbox no seleccionados para sap
        protected void send_SAP_sobrantes()
        {
            string name_function = "Zdsd_Recibe_Prog_Cobros_Road";
            int cont = 0;
            try
            {
                String fecha_original = myDatepicker.Value.ToString();
                Char delimitador = '/';
                String[] substrings = fecha_original.Split(delimitador);
                string fecha = substrings[2] + substrings[1] + substrings[0];

                IRfcFunction rfcFunction_Surplus = rfcRepository.CreateFunction(name_function.ToUpper());
                IRfcTable i_tbl_refresh = rfcFunction_Surplus.GetTable("I_Rec_Prog_Cob");

                foreach (GridViewRow row in GridView_Data_Cobros.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        string of_ventas = row.Cells[3].Text;
                        string ruta = row.Cells[5].Text;
                        string cod_cliente = row.Cells[6].Text;
                        string dias_credito = row.Cells[8].Text;
                        string factura = row.Cells[9].Text;
                        string monto = row.Cells[11].Text;
                        string saldo = row.Cells[12].Text;
                        if (!chkRow.Checked)
                        {                           
                            i_tbl_refresh.Append();
                            i_tbl_refresh.SetValue("VKBUR",of_ventas);
                            i_tbl_refresh.SetValue("ROUTEPPP", ruta);
                            i_tbl_refresh.SetValue("KUNNR", cod_cliente);
                            i_tbl_refresh.SetValue("DIAS", dias_credito);
                            i_tbl_refresh.SetValue("XBLNR", factura);
                            i_tbl_refresh.SetValue("MONTO", monto);
                            i_tbl_refresh.SetValue("SALDO", saldo);
                            i_tbl_refresh.SetValue("FECHA", fecha);
                            cont = cont + 1;
                        }
                    }

                    //pone el circulo dependiendo del texto del Estado
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    TableCell statusCell = row.Cells[1];
                    if (statusCell.Text == "RO")
                    {
                        div.Attributes["class"] = "circle_red";
                    }
                    if (statusCell.Text == "VE")
                    {
                        div.Attributes["class"] = "circle_green";
                    }
                    row.Cells[1].Controls.Add(div);
                }
                rfcFunction_Surplus.Invoke(rfcDestination);
                ClientScript.RegisterStartupScript(GetType(), "exito", "notificacion_exito(" + cont + ");", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error send_SAP_sobrantes() --> " + ex.Message + " -->");
            }
        }

        //manda a llamar send_SAP_sobrantes cuando se da click en el icono de guardar
        protected void saved_clicked(object sender, EventArgs e)
        {
            send_SAP_sobrantes();
        }

    }
}