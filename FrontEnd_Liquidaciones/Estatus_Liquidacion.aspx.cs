using System;
using System.Net;
using System.Configuration;
//Librería del proxy de Pepsi
using System.Collections.Generic;
using SAP.Middleware.Connector;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public class Objeto2
    {
        public string codigo { get; set; }
        public Objeto2() { }
    }
    public partial class Estatus_Liquidacion : System.Web.UI.Page
    {
        int cero = 0;
        int uno = 0;
        int vacio = 0;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        Convertir convertir = new Convertir();

        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        static SqlDataAdapter da;
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;

        List<Objeto2> cets = new List<Objeto2>();
        List<Objeto2> rutas = new List<Objeto2>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarUsuario();
                show_all1();
                fill_Agency();
                seleccion();
                s();


                for (int i = 0; i < All_Data.Rows.Count; i++)
                {
                    string status = All_Data.Rows[i].Cells[3].Text;
                    string cod = All_Data.Rows[i].Cells[2].Text;

                    if (status.Equals("1"))
                    {

                        cero += 1;
                        Session["NLiq"] = cod;

                    }
                    else
                    {
                        All_Data.Rows[i].Visible = false;
                    }
                }
                
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

        //llena el dropdown de agencia por usuario
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
                ds.Tables.Add(convertir.toDataTable(rfcFunction_Company.GetTable("It_Datos")));
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
                System.Diagnostics.Debug.WriteLine("Error fill_agency() --> " + ex);
            }
        }

		//Acá hago match con las rutas de All_data y los cets con la agencia seleccionada
        public void seleccion()
        {
            Boolean bol = false;
            string agencia = cmbAgency.SelectedValue.ToString();

            for (int i = 0; i < All_Data1.Rows.Count; i++)
            {
                if (agencia == All_Data1.Rows[i].Cells[0].Text)
                {
                    string cetss = All_Data1.Rows[i].Cells[1].Text;
                    Objeto2 a = new Objeto2();
                    a.codigo = cetss.ToString();
                    bol = true;
                    cets.Add(a);
                    //System.Diagnostics.Debug.WriteLine("asdfasdf: " + cetss);
                }

            }
            if (bol == false)
            {
                PieGrafica.Visible = false;
                All_Data.Visible = false;
            }
            else
            {
                PieGrafica.Visible = true;
                All_Data.Visible = true;
            }
        }

        //Verifica a que agencia está seleccionada
        protected void seleccionarAgencia(Object sender, EventArgs e)
        {
            seleccion();
            s();
			//Acá hago que por recarga se muestren en el grid solo las liquidadas
            for (int i = 0; i < All_Data.Rows.Count; i++)
            {
                string status = All_Data.Rows[i].Cells[3].Text;
                string cod = All_Data.Rows[i].Cells[2].Text;

                if (status.Equals("1"))
                {

                    cero += 1;
                    Session["NLiq"] = cod;

                }
                else
                {
                    All_Data.Rows[i].Visible = false;
                }
            }


        }
		//Acá obtengo los valores para llenar la grafica de pie
        protected void s()
        {
            //System.Diagnostics.Debug.WriteLine("Agencia seleccionada: " + cmbAgency.SelectedIndex.ToString());
            //fill_cets(cmbAgency.SelectedValue);

            All_Data.DataSourceID = null;
            All_Data.DataSource = null;
            All_Data.DataBind();

            try
            {

                obtieneRutas(cets);
                show_all();
                for (int i = 0; i < All_Data.Rows.Count; i++)
                {
                    string status = All_Data.Rows[i].Cells[3].Text;
                    string cod = All_Data.Rows[i].Cells[2].Text;

                    if (status.Equals("0"))
                    {
                        cero += 1;
                        Session["NLiq"] = cod;
                    }
                    else if (status.Equals("1"))
                    {
                        uno += 1;
                        Session["Liq"] = cod;
                    }
                    else
                    {
                        vacio += 1;
                        Session["Pend"] = cod;
                    }
                }

                fill_pie();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- seleccionarAgencia Error: " + ex.Message + " -->");
            }
        }
		
		//Obtengo rutas por usuario y se las asigno a routes
        protected void obtieneRutas(List<Objeto2> grid_view)
        {
            string name_function = "ZMAESTRO_RUTAS";
            DataSet ds = new DataSet();
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());


                foreach (Objeto2 obj in grid_view)
                {
                    // Mando tabla agencia
                    IRfcTable i_agencia = rfcFunction_Company.GetTable("I_Agencia");
                    i_agencia.Append();
                    i_agencia.SetValue("MANDT", "600");
                    i_agencia.SetValue("AGENCIA", cmbAgency.SelectedItem.Value);
                    IRfcTable i_cet = rfcFunction_Company.GetTable("I_cet");
                    i_cet.Append();
                    i_cet.SetValue("CET", obj.codigo.ToString());
                }
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convertir.toGridView(rfcFunction_Company.GetTable("R_RUTA"), routes));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- obtieneRutas Error: " + ex.Message + " -->");
            }
        }
		
		//Acá recorro routes y hago match, luego las seteo a All_Data(Esta oculto)
        public void show_all()
        {
            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            //System.Diagnostics.Debug.WriteLine("tiempo" + fecha);
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "REFRESH");
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
                ds.Tables.Add(convertir.toGridView(rfcFunction_Company.GetTable("O_Tbl_Refresh"), All_Data));
                Semaforo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- show_all Error: " + ex.Message + " -->");
            }
        }

		//Acá recorro routes y hago match, luego las seteo a All_Data(Esta Visible)
        public void show_all1()
        {
            obtieneRutas(cets);
            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            //System.Diagnostics.Debug.WriteLine("tiempo" + fecha);
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "REFRESH");
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
                ds.Tables.Add(convertir.toGridView(rfcFunction_Company.GetTable("O_Tbl_Refresh"), All_Data1));
                Semaforo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- show_all Error: " + ex.Message + " -->");
            }
        }

        //pone los colores del semaforo segun el status
        public void Semaforo()
        {
            foreach (GridViewRow links in All_Data.Rows)
            {
                if (!links.Cells[3].Text.Equals("&nbsp;"))
                {
                    //pone el circulo dependiendo del texto del Estado
                    HtmlGenericControl div_green = new HtmlGenericControl("div");
                    HtmlGenericControl div_yellow = new HtmlGenericControl("div");
                    HtmlGenericControl div_red = new HtmlGenericControl("div");
                    div_green.Attributes["class"] = "circle_green";
                    div_yellow.Attributes["class"] = "circle_yellow";
                    div_red.Attributes["class"] = "circle_red";
                    if (links.Cells[3].Text.Equals("0"))
                    {
                        div_green.Style["opacity"] = "0.1";
                        div_yellow.Style["opacity"] = "0.1";
                    }
                    else if (links.Cells[3].Text.Equals("1"))
                    {
                        div_yellow.Style["opacity"] = "0.1";
                        div_red.Style["opacity"] = "0.1";
                    }
                    else if (links.Cells[3].Text.Equals("&nbsp;") && !links.Cells[3].Text.Equals("0") && !links.Cells[3].Text.Equals("1"))
                    {
                        div_green.Style["opacity"] = "0.1";
                        div_red.Style["opacity"] = "0.1";
                    }
                    links.Cells[3].Controls.Add(div_green);
                    links.Cells[3].Controls.Add(div_yellow);
                    links.Cells[3].Controls.Add(div_red);
                }
            }
        }


        //Acá le seteo los valores al javascript para llenar la grafica de pie
        protected void fill_pie()
        {
            ClientScript.RegisterStartupScript(GetType(), "fill_chart", "fill_chart(" + cero + "," + uno + "," + vacio + ");", true);
        }

        protected void Refresh_Clicked(object sender, EventArgs e)
        {
            seleccion();
            s();
            for (int i = 0; i < All_Data.Rows.Count; i++)
            {
                string status = All_Data.Rows[i].Cells[3].Text;
                string cod = All_Data.Rows[i].Cells[2].Text;

                if (status.Equals("1"))
                {

                    cero += 1;
                    Session["NLiq"] = cod;

                }
                else
                {
                    All_Data.Rows[i].Visible = false;
                }
            }
        }
		
		//Le da click a una parte del pie y muestra en el grid las rutas No liquidadas
        protected void NLiq_Clicked(object sender, EventArgs e)
        {
            seleccion();
            s();
            All_Data.Visible = true;

            for (int i = 0; i < All_Data.Rows.Count; i++)
            {
                string status = All_Data.Rows[i].Cells[3].Text;
                string cod = All_Data.Rows[i].Cells[2].Text;

                if (status.Equals("0"))
                {

                    cero += 1;
                    Session["NLiq"] = cod;

                }
                else
                {
                    All_Data.Rows[i].Visible = false;
                }
            }
            ClientScript.RegisterStartupScript(GetType(), "grid_fixed_header_NLiq_Clicked", "grid_fixed_header();", true);
        }
		//Le da click a una parte del pie y muestra en el grid las rutas Pendientes
        protected void Pend_Clicked(object sender, EventArgs e)
        {
            seleccion();
            s();
            All_Data.Visible = true;

            for (int i = 0; i < All_Data.Rows.Count; i++)
            {
                string status = All_Data.Rows[i].Cells[3].Text;
                string cod = All_Data.Rows[i].Cells[2].Text;

                if (status.Equals(""))
                {
                    uno += 1;
                    Session["Liq"] = cod;
                }
                else if (status.Equals("1"))
                {
                    All_Data.Rows[i].Visible = false;
                }
                else if (status.Equals("0"))
                {
                    All_Data.Rows[i].Visible = false;
                }
            }
            ClientScript.RegisterStartupScript(GetType(), "grid_fixed_header_Pend_Clicked", "grid_fixed_header();", true);
        }
        protected void Liq_Clicked(object sender, EventArgs e)
        {
            seleccion();
            s();
            All_Data.Visible = true;

            for (int i = 0; i < All_Data.Rows.Count; i++)
            {
                string status = All_Data.Rows[i].Cells[3].Text;
                string cod = All_Data.Rows[i].Cells[2].Text;

                if (status.Equals("1"))
                {
                    uno += 1;
                    Session["Liq"] = cod;
                }
                else if (status.Equals("0"))
                {
                    All_Data.Rows[i].Visible = false;
                }
                else
                {
                    All_Data.Rows[i].Visible = false;
                }
            }
            ClientScript.RegisterStartupScript(GetType(), "grid_fixed_header_Liq_Clicked", "grid_fixed_header();", true);
        }

    }
}