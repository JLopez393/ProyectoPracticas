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
    public partial class Consulta_Clientes : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        Convertir convertir = new Convertir();
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;

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

        //llena el combo de agencia en ConsultaClientes
        protected void fill_Agency()
        {
            string sql_office = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                " WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=4 ORDER BY detalle_op ASC ";
            try
            {
                DataTable dt = new DataTable();
                DataTable table_Agency = new DataTable();
                dataAdapter = new SqlDataAdapter(sql_office, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);

                DataRow[] detalle_op = dt.Select("detalle_op = 'ATOTAL'");
                if (detalle_op.Length == 1)
                {
                    string sql_Agency_all = " SELECT DISTINCT d.detalle_op FROM usuario u inner join detalle_usuario d ON u.codigo_usuario = d.codigo_usuario " +
                                " WHERE d.maestro=4 ORDER BY detalle_op ASC ";
                    dataAdapter = new SqlDataAdapter(sql_Agency_all, connection);
                    commandBuilder = new SqlCommandBuilder(dataAdapter);
                    dataAdapter.Fill(table_Agency);
                    foreach (DataRow dr in table_Agency.Rows)
                    {
                        if (dr["detalle_op"].ToString().Equals("ATOTAL"))
                        {
                            dr.Delete();
                        }
                    }
                    table_Agency.AcceptChanges();
                    cmbAgency.DataSource = table_Agency;
                }
                else
                {
                    cmbAgency.DataSource = dt;
                }
                cmbAgency.DataTextField = "detalle_op";
                cmbAgency.DataValueField = "detalle_op";
                cmbAgency.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error fill_Agency --> " + ex.Message);
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

        //Busca seis datos del cliente
        public void Data_search()
        {
            DataSet ds = new DataSet();
            string name_function = "Zfv_Busca_Cliente";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("P_Name", cliente.Value.ToUpper());
                rfcFunction_Company.SetValue("P_Vkbur", cmbAgency.Text.ToUpper());
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(ConvertToDonNet(rfcFunction_Company.GetTable("Clientes"), Simple_Data));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Search Error: " + ex.Message + " -->");
            }
        }

        //Busca todos los datos del cliente
        public void Data_search_complete()
        {
            DataSet ds = new DataSet();
            string name_function = "Zfv_Recupera_Cliente";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("P_Kunnr", Simple_Data.Rows[0].Cells[1].Text);
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(ConvertToDonNet(rfcFunction_Company.GetTable("Clientes"), Complete_Data));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Search Error: " + ex.Message + " -->");
            }
        }

        //Llena los labels con la informacion del Gridview --> Complete_Data
        public void Populate_Labels()
        {
            Label1.Text = Complete_Data.Rows[0].Cells[0].Text;
            Label2.Text = Complete_Data.Rows[0].Cells[1].Text;
            Label3.Text = Complete_Data.Rows[0].Cells[2].Text;
            Label4.Text = Complete_Data.Rows[0].Cells[3].Text;
            Label5.Text = Complete_Data.Rows[0].Cells[4].Text;
            Label6.Text = Complete_Data.Rows[0].Cells[5].Text;
            Label7.Text = Complete_Data.Rows[0].Cells[6].Text;
            Label8.Text = Complete_Data.Rows[0].Cells[7].Text;
            Label9.Text = Complete_Data.Rows[0].Cells[8].Text;
            Label10.Text = Complete_Data.Rows[0].Cells[9].Text;
            Label11.Text = Complete_Data.Rows[0].Cells[10].Text;
            Label12.Text = Complete_Data.Rows[0].Cells[11].Text;
            Label13.Text = Complete_Data.Rows[0].Cells[12].Text;
            Label14.Text = Complete_Data.Rows[0].Cells[13].Text;
            Label15.Text = Complete_Data.Rows[0].Cells[14].Text;
            Label16.Text = Complete_Data.Rows[0].Cells[15].Text;
            Label17.Text = Complete_Data.Rows[0].Cells[16].Text;
            Label18.Text = Complete_Data.Rows[0].Cells[17].Text;
            Label19.Text = Complete_Data.Rows[0].Cells[18].Text;
            Label20.Text = Complete_Data.Rows[0].Cells[19].Text;
            Label21.Text = Complete_Data.Rows[0].Cells[20].Text;
            Label22.Text = Complete_Data.Rows[0].Cells[21].Text;
        }

        //Busca la ubicacion del cliente
        public void ubication_search()
        {
            DataSet ds = new DataSet();
            string name_function = "YBPM_CLI_RECUPERACOORDENADAS";
            try
            {
                IRfcFunction rfcFunction_Ubication = rfcRepository.CreateFunction(name_function.ToUpper());
                IRfcTable something = rfcFunction_Ubication.GetTable("COORDENADAS");
                something.Append();
                something.SetValue("KUNNR", Simple_Data.Rows[0].Cells[1].Text);
                rfcFunction_Ubication.Invoke(rfcDestination);
                ds.Tables.Add(ConvertToDonNet(rfcFunction_Ubication.GetTable("COORDENADAS"), Coordenadas));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- Search Error: " + ex.Message + " -->");
            }
        }

        //Manda la ubicacion del cliente
        protected void set_coordinates()
        {
            var latitud = Coordenadas.Rows[0].Cells[0].Text;
            var longitud = Coordenadas.Rows[0].Cells[1].Text;
            ClientScript.RegisterStartupScript(GetType(), "latitud_call", "localStorage.setItem('latitud_T', " + latitud + ");", true);
            ClientScript.RegisterStartupScript(GetType(), "longitud_call", "localStorage.setItem('longitud_T', " + longitud + ");", true);
        }

        //llena los gridview 
        public DataTable ConvertToDonNet(IRfcTable RFCTable, GridView ID_GridView)
        {
            DataTable dtTable = new DataTable();

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

            if (dtTable.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "not_data", "not_data();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "data", "data();", true);
                ID_GridView.DataSource = dtTable;
                ID_GridView.DataBind();
            }
            return dtTable;
        }

        //Click en lupa
        public void search_clicked(object sender, EventArgs e)
        {
            Data_search();
            ClientScript.RegisterStartupScript(GetType(), "hide_explicit", "$('#explicit_data').hide();", true);
            ClientScript.RegisterStartupScript(GetType(), "hide_maps", "$('#maps_container').hide();", true);
        }

        //Click en signo mas
        public void plus_clicked(object sender, EventArgs e)
        {

            ImageButton btn_add = Simple_Data.Rows[0].FindControl("btn_add") as ImageButton;
            ImageButton btn_minus = Simple_Data.Rows[0].FindControl("btn_minus") as ImageButton;
            btn_add.Visible = false;
            btn_minus.Visible = true;
            Data_search_complete();
            Populate_Labels();
            ubication_search();
            ClientScript.RegisterStartupScript(GetType(), "show_explicit", "$('#explicit_data').show();", true);
            ClientScript.RegisterStartupScript(GetType(), "show_maps", "$('#maps_container').show();", true);
            set_coordinates();
        }

        public void minus_clicked(object sender, EventArgs e)
        {
            ImageButton btn_add = Simple_Data.Rows[0].FindControl("btn_add") as ImageButton;
            ImageButton btn_minus = Simple_Data.Rows[0].FindControl("btn_minus") as ImageButton;
            btn_add.Visible = true;
            btn_minus.Visible = false;
        }
    }
}