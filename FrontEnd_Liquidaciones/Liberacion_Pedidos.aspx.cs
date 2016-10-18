using FrontEnd_Liquidaciones.functions;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontEnd_Liquidaciones
{
    public partial class Liberacion_Pedidos : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
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
                fill_Agency();
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

        //llena las agencias con SAP
        public void fill_Agency()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            DataTable dt = new DataTable();
            string sql = "SELECT d.detalle_op FROM usuario u inner join detalle_usuario d "
                             + "ON u.codigo_usuario = d.codigo_usuario WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=4;";
            try
            {
                //Acá recupero los códigos de las agencias por usuario
                dataAdapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);//contar rows
                //Acá recupero la tabla de sap que contiene codigo y descripcion de las agencias
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "SVENT");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Company.GetTable("It_Datos")));
                //Acá hago match con codigo de sql y sap, seteo la descripcion al dropdown
                int cont1 = 0;
                DataRow[] foundRow1;
                foreach (DataRow dr in dt.Rows)
                {

                    string permiso = dr[0].ToString();
                    if (permiso == "ATOTAL")
                    {
                        foreach (DataRow dr12 in ds.Tables[0].Rows)
                        {

                            //System.Diagnostics.Debug.WriteLine(dr12[1].ToString());
                            //System.Diagnostics.Debug.WriteLine(cont1);
                            cmbAgency.Items.Insert(cont1, new ListItem(dr12[1].ToString(), dr12[0].ToString()));
                            cont1++;
                        }
                    }
                    break;
                }
                DataRow[] foundRow;
                int cont = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    foundRow = dt.Select("detalle_op = '" + dr[0].ToString() + "'");

                    for (int q = 0; q < foundRow.Length; q++)
                    {
                        cmbAgency.Items.Insert(cont, new ListItem(dr[1].ToString(), dr[0].ToString()));
                        cont++;
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error fill_agency() --> " + ex.Message);
            }
        }

        //busqueda en SAP de los datos proporcionados
        protected void Data_search()
        {
            DataSet ds = new DataSet();
            string name_function = "Zfv_Liberapedidos";
            try
            {
                String fecha_original = fecha_liberacion.Value.ToString();
                Char delimitador = '/';
                String[] substrings = fecha_original.Split(delimitador);
                string fecha = substrings[2] + substrings[1] + substrings[0];

                IRfcFunction rfcFunction_Search_Orders = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Search_Orders.SetValue("I_Agencia", cmbAgency.SelectedValue.ToUpper());
                rfcFunction_Search_Orders.SetValue("I_Fecha", fecha);
                rfcFunction_Search_Orders.SetValue("I_Usuario", Session["usuario"]);
                rfcFunction_Search_Orders.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Search_Orders.GetTable("Ot_Libera_Pedidos"), get_pedidos));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error Data_search() --> " + ex.Message);
            }
        }

        //manda los datos de los checkbox no seleccionados para sap
        protected void send_SAP_liberacion()
        {
            string name_function = "Zfv_Genera_Liberacionpedidos";
            int cont = 0;
            try
            {
                IRfcFunction rfcFunction_Liberation = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Liberation.SetValue("I_Agencia", cmbAgency.SelectedValue.ToUpper());
                rfcFunction_Liberation.SetValue("I_Usuario", Session["usuario"]);
                IRfcTable i_tbl_refresh = rfcFunction_Liberation.GetTable("IT_LIBERA_PEDIDOS");

                foreach (GridViewRow row in get_pedidos.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        if (!chkRow.Checked)
                        {
                            i_tbl_refresh.Append();
                            i_tbl_refresh.SetValue("VBELN", row.Cells[1].Text);
                            i_tbl_refresh.SetValue("KUNNR", row.Cells[2].Text);
                            i_tbl_refresh.SetValue("NAME1", row.Cells[3].Text);
                            i_tbl_refresh.SetValue("BZIRK", row.Cells[4].Text);
                            i_tbl_refresh.SetValue("AUART", row.Cells[5].Text);
                            i_tbl_refresh.SetValue("Z1", row.Cells[6].Text);
                            i_tbl_refresh.SetValue("Z2", row.Cells[7].Text);
                            i_tbl_refresh.SetValue("Z3", row.Cells[8].Text);
                            i_tbl_refresh.SetValue("Z4", row.Cells[9].Text);
                            i_tbl_refresh.SetValue("Z5", row.Cells[10].Text);
                            i_tbl_refresh.SetValue("Z6", row.Cells[11].Text);
                            i_tbl_refresh.SetValue("Z7", row.Cells[12].Text);                            
                            i_tbl_refresh.SetValue("Z9", row.Cells[13].Text);
                            i_tbl_refresh.SetValue("ZA", row.Cells[14].Text);
                            i_tbl_refresh.SetValue("ZB", row.Cells[15].Text);
                            cont = cont + 1;
                        }
                    }
                }
                rfcFunction_Liberation.Invoke(rfcDestination);
                ClientScript.RegisterStartupScript(GetType(), "exito", "notificacion_exito(" + cont + ");", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error send_SAP_liberacion() --> " + ex.Message);
            }
        }

        //manda a llamar Data_search
        protected void searck_clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(cmbAgency.SelectedValue.ToString());
            Data_search();
        }

        //manda a llamar send_SAP_liberacion
        protected void liberate_clicked(object sender, EventArgs e)
        {
            send_SAP_liberacion();
        }
    }
}