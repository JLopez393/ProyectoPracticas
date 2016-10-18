using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public partial class Liquidacion : System.Web.UI.Page
    {
        Convertir convertir = new Convertir();
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;

        string S_lista = "";
        string S_sociedad = "";
        string S_ruta = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarUsuario();
                S_lista = (string)(Session["Lista"]);
                S_sociedad = (string)(Session["Sociedad"]);
                S_ruta = (string)(Session["Ruta"]);
                ClientScript.RegisterStartupScript(GetType(), "lista", "$('#centro').val(" + S_lista + ");", true);
                get_Data();
                validarLiquidacion();
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
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error Conexion() --> " + ex.Message + " -->");
            }
        }

        //Al hacer click si cumple todos los requisitos manda al metodo
        protected void validarLiquidacion()
        {
            try
            {
                string dato = datos_busqueda.Rows[0].Cells[0].Text;
                if (dato.Trim() != null || dato.Trim() != "")
                {
                    esstado.Value = "liquidar";

                    data_panel_heading.Attributes["class"] = "liquidadas-head";
                    ClientScript.RegisterStartupScript(GetType(), "clickLiquidar", "clickLiquidar();", true);
                }
                else
                {

                    data_panel_heading.Attributes["class"] = "no-liquidadas-head";
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error validarLiquidacion() --> " + ex.Message + " -->");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error validarLiquidacion() --> " + ex.Message + " -->");
            }
        }

        //busqueda en sap de los datos proporcionados (gridviews lado izquiero)
        public void get_Data()
        {
            Convertir convertir = new Convertir();
            DataSet ds = new DataSet();
            string name_function = "Zfv_Lista_Clientes";
            //string lista = "0000049377";
            string lista = S_lista;
            try
            {
                DataSet tb_select_key1 = new DataSet();
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction.SetValue("P_OBJ_ID", lista);
                rfcFunction.Invoke(rfcDestination);
                ds.Tables.Add(convertir.toGridView(rfcFunction.GetTable("SELECTVISIT_KEY"), datos_busqueda));
                ds.Tables.Add(convertir.toGridView(rfcFunction.GetTable("SELECTVISIT_KEY1"), datos_generales));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error get_Data() --> " + ex.Message + " -->");
            }
        }

        //esconde los rows que no hacen match con la busqueda
        protected void OnRowDataBound_Details(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search_visit_id")
                {

                    // Activo el boton Liberar
                    Button1.Enabled = true;
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = datos_generales.Rows[index];
                    string VISIT_ID = row.Cells[2].Text.ToString();
                    //System.Diagnostics.Debug.WriteLine("<-- VISIT_ID : " + VISIT_ID + " -->");

                    get_Data2(VISIT_ID);

                    for (int i = 0; i < datos_especificos.Rows.Count; i++)
                    {
                        if (!VISIT_ID.Equals(datos_especificos.Rows[i].Cells[3].Text))
                        {
                            datos_especificos.Rows[i].Visible = false;
                        }
                        else
                        {
                            datos_especificos.Rows[i].Visible = true;
                        }

                    }

                    for (int i = 0; i < datos_items.Rows.Count; i++)
                    {
                        if (!VISIT_ID.Equals(datos_items.Rows[i].Cells[1].Text))
                        {
                            datos_items.Rows[i].Visible = false;
                        }
                        else
                        {
                            datos_items.Rows[i].Visible = true;
                        }
                    }
                    //lleno un input con esos valores para no perder el estado del botón
                    if (esstado.Value == "liquidar")
                    {
                        //System.Diagnostics.Debug.WriteLine("liquidar");
                        String script = @"<script type='text/javascript'>
                            clickLiquidar();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiquidar", script, false);
                    }
                    else if (esstado.Value == "liberar")
                    {
                        String script = @"<script type='text/javascript'>
                            clickLiberar();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiberar", script, false);
                    }
                    else if (esstado.Value == "log")
                    {
                        String script = @"<script type='text/javascript'>
                            clickLog();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLog", script, false);
                    }
                    //oculto el lapicito
                    int index1 = Convert.ToInt32(e.CommandArgument);
                    for (int i = 0; i < datos_especificos.Rows.Count; i++)
                    {
                        GridViewRow row1 = datos_especificos.Rows[i];

                        row1.Cells[12].ControlStyle.CssClass = "hidden";
                    }

                    datos_especificos.Columns[12].HeaderStyle.CssClass = "hidden";

                }

                if (e.CommandName == "search_hh_delvry")
                {
                    string t = ml.Value;
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = datos_especificos.Rows[index];
                    Session["HH_DELVRY"] = row.Cells[4].Text.ToString();
                    row.Cells[12].ControlStyle.CssClass = "nohidden";
                    datos_especificos.Columns[12].HeaderStyle.CssClass = "nohidden";

                    for (int i = 0; i < datos_items.Rows.Count; i++)
                    {
                        if (!Session["HH_DELVRY"].Equals(datos_items.Rows[i].Cells[2].Text))
                        {
                            datos_items.Rows[i].Visible = false;

                        }
                        else
                        {
                            datos_items.Rows[i].Visible = true;

                        }
                    }
                    //lleno un input con esos valores para no perder el estado del botón
                    if (esstado.Value == "liquidar")
                    {
                        //System.Diagnostics.Debug.WriteLine("liquidar");
                        String script = @"<script type='text/javascript'>
                            clickLiquidar();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiquidar", script, false);
                    }
                    else if (esstado.Value == "liberar")
                    {
                        String script = @"<script type='text/javascript'>
                            clickLiberar();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiberar", script, false);
                        // System.Diagnostics.Debug.WriteLine("liberar");
                    }
                    else if (esstado.Value == "log")
                    {
                        String script = @"<script type='text/javascript'>
                            clickLog();</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLog", script, false);
                        //System.Diagnostics.Debug.WriteLine("log");
                    }

                    //Muestro el lapicito
                    for (int i = 0; i < datos_especificos.Rows.Count; i++)
                    {
                        if (i != index)
                        {
                            GridViewRow row1 = datos_especificos.Rows[i];

                            row1.Cells[12].ControlStyle.CssClass = "hidden";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("Error OnRowDataBound_Details() --> " + ex.Message + " -->");
            }
        }

        //busqueda en sap de los datos proporcionados (gridviews lado derecho)
        public void get_Data2(string Visit_ID)
        {
            DataSet ds = new DataSet();
            string name_function = "Zfv_Detalle_Clientes";
            Session["tour_id"] = datos_busqueda.Rows[0].Cells[1].Text;
            Session["visit"] = Visit_ID;
            try
            {
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction.SetValue("P_TOUR_ID", Session["tour_id"]);
                rfcFunction.SetValue("P_VISIT_ID", Session["visit"]);
                rfcFunction.Invoke(rfcDestination);
                ds.Tables.Add(convertir.toGridView(rfcFunction.GetTable("SELECT_DETALLE"), datos_especificos));
                ds.Tables.Add(convertir.toGridView(rfcFunction.GetTable("SELECT_DETALLE1"), datos_items));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error get_Data2() --> " + ex.Message + " -->");
            }
        }

        //libera con el P_TOUR_ID
        public void Liberar()
        {
            string name_function = "Zfv_Libera_Lista";
            try
            {
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(name_function.ToUpper());
                if (datos_busqueda.Rows.Count > 0)
                {
                    rfcFunction.SetValue("P_TOUR_ID", datos_busqueda.Rows[0].Cells[1].Text);
                }
                rfcFunction.Invoke(rfcDestination);
                buttonError.InnerText = "La liberación fue exitosa";
                buttonError.Visible = true;
            }
            catch (Exception ex)
            {
                buttonError.InnerText = "No se pudo liberar";
                buttonError.Visible = true;
                System.Diagnostics.Debug.WriteLine("Error Liberar() --> " + ex.Message + " -->");
            }
        }

        //liquida con P_TOUR_ID, F_SLD_ID (se obtienen del gridview) y los otros dos datos se mandan por session de cockpit
        public void Liquidar()
        {
            string name_function = "Zfv_Liquida_Lista";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(name_function.ToUpper());
                if (datos_busqueda.Rows.Count > 0)
                {
                    rfcFunction.SetValue("P_TOUR_ID", datos_busqueda.Rows[0].Cells[1].Text);
                    rfcFunction.SetValue("F_SLD_ID", datos_busqueda.Rows[0].Cells[0].Text);
                    rfcFunction.SetValue("F_RUTA", S_ruta);
                    rfcFunction.SetValue("F_FECHA", fecha);
                    rfcFunction.SetValue("F_SOCIEDAD", S_sociedad);
                }
                rfcFunction.Invoke(rfcDestination);
                buttonError.InnerText = "La liquidación fue exitosa";
                buttonError.Visible = true;
            }
            catch (Exception ex)
            {
                buttonError.InnerText = "No se pudo liquidar";
                buttonError.Visible = true;
                System.Diagnostics.Debug.WriteLine("Error Liquidar() --> " + ex.Message + " -->");
            }
        }

        //log con P_TOUR_ID, F_SLD_ID (se obtienen del gridview)
        public void Log()
        {
            DataSet ds = new DataSet();
            string name_function = "Zfv_Verif_Lista_Liq";
            string name_function2 = "Zfv_Msg_Liquidacion";
            try
            {
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(name_function.ToUpper());
                if (datos_busqueda.Rows.Count > 0)
                {
                    rfcFunction.SetValue("P_TOUR_ID", datos_busqueda.Rows[0].Cells[1].Text);
                    rfcFunction.SetValue("F_SLD_ID", datos_busqueda.Rows[0].Cells[0].Text);
                    rfcFunction.Invoke(rfcDestination);
                }

                IRfcFunction rfcFunction_MSG = rfcRepository.CreateFunction(name_function2.ToUpper());
                if (datos_busqueda.Rows.Count > 0)
                {
                    rfcFunction_MSG.SetValue("SL_SLD_ID", datos_busqueda.Rows[0].Cells[0].Text);
                    rfcFunction_MSG.Invoke(rfcDestination);
                    ds.Tables.Add(convertir.toGridView(rfcFunction_MSG.GetTable("MENSAJES"), datos_log));
                }

                String concatenacion = "<table class='tree' style='width: 550px'>";
                int[] a = new int[datos_log.Rows.Count];

                for (int i = 0; i < datos_log.Rows.Count; i++)
                {
                    int prueba = Convert.ToInt32(datos_log.Rows[i].Cells[2].Text);
                    string datos = datos_log.Rows[i].Cells[1].Text;
                    string estado = datos_log.Rows[i].Cells[0].Text;
                    //System.Diagnostics.Debug.WriteLine("DATOS TREE: " + datos_log.Rows[i].Cells[0].Text + " de " + datos + " y a[" + prueba + "]");

                    string circulo = "";
                    if (estado.ToUpper().Equals("I") || estado.ToUpper().Equals("S"))
                    {
                        circulo = "<span class='circulo green'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span>&nbsp;&nbsp;";
                    }
                    else if (estado.ToUpper().Equals("E"))
                    {
                        circulo = "<span class='circulo red'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span>&nbsp;&nbsp;";
                    }

                    a[i] = prueba;

                    if (a[i] == 1)
                    {

                        concatenacion += "<tr class='treegrid-1'><td>" + circulo + datos + "</span></td></tr>";
                    }
                    if (a[i] == 2)
                    {
                        concatenacion += "<tr class='treegrid-2 treegrid-parent-1'><td>" + circulo + datos + "</span></td></tr>";
                    }

                    if (a[i] == 3)
                    {
                        concatenacion += "<tr class='treegrid-parent-2'><td>" + circulo + datos + "</span></td></tr>";
                    }

                    if (a[i] == 4)
                    {
                        concatenacion += "<tr class='treegrid-3 treegrid-parent-2'><td>" + circulo + datos + "</span></td></tr>";
                    }

                    if (a[i] == 6)
                    {
                        concatenacion += "<tr class='treegrid-parent-3'><td>" + circulo + datos + "</span></td></tr>";
                    }
                }
                concatenacion += "</table><br /><br /><br /><br />";
                //System.Diagnostics.Debug.WriteLine("prueba de concatenacion: " + concatenacion);

                arbol_js.InnerHtml = concatenacion;
                buttonError.InnerText = "Log generado con exito";
                buttonError.Visible = true;
            }
            catch (Exception ex)
            {
                buttonError.InnerText = "No se pudo generar el Log";
                buttonError.Visible = true;
                System.Diagnostics.Debug.WriteLine("Error Log() --> " + ex.Message + " -->");
            }
        }

        //boton que llama a Liberar()
        protected void Liberar_Clicked(object sender, EventArgs e)
        {
            esstado.Value = "liberar";
            String script = @"<script type='text/javascript'>
                            clickLiberar();</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiberar", script, false);
            Liberar();
            ClientScript.RegisterStartupScript(GetType(), "lista_libera", "$('#centro').val(" + S_lista + ");", true);
        }

        //boton que llama a Liquidar()
        protected void Liquidar_Clicked(object sender, EventArgs e)
        {
            esstado.Value = "liquidar";
            String script = @"<script type='text/javascript'>
                            clickLiquidar();</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiquidar", script, false);
            Liquidar();
            ClientScript.RegisterStartupScript(GetType(), "lista_liquidar", "$('#centro').val(" + S_lista + ");", true);
        }

        //boton que llama a Log()
        protected void Log_Clicked(object sender, EventArgs e)
        {
            String var = data_panel_heading.InnerHtml;

            esstado.Value = "log";
            String script = @"<script type='text/javascript'>
                            clickLog();</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLog", script, false);
            datos_busqueda.Visible = false;
            datos_generales.Visible = false;
            datos_especificos.Visible = false;
            datos_items.Visible = false;
            Log();
            ClientScript.RegisterStartupScript(GetType(), "lista_log", "$('#centro').val(" + S_lista + ");", true);
            data_panel_heading.InnerHtml = var;
        }

        //cambia la columna de NO. Pedido por un textbox y lo edita
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            datos_especificos.EditIndex = e.NewEditIndex;
            get_Data2(Session["visit"].ToString());
            if (esstado.Value == "liquidar")
            {
                String script = @"<script type='text/javascript'>
                            clickLiquidar();</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiquidar", script, false);
            }
            else if (esstado.Value == "liberar")
            {
                String script = @"<script type='text/javascript'>
                            clickLiberar();</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiberar", script, false);
            }
            else if (esstado.Value == "log")
            {
                String script = @"<script type='text/javascript'>
                            clickLog();</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLog", script, false);
            }

            for (int i = 0; i < datos_especificos.Rows.Count; i++)
            {
                if (i != e.NewEditIndex)
                {
                    GridViewRow row1 = datos_especificos.Rows[i];

                    row1.Cells[12].ControlStyle.CssClass = "hidden";
                }
            }
        }

        //Guarda lo que hay en el textbox del la columno NO.Pedido y lo muestra en un label
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            for (int i = 0; i < datos_especificos.Rows.Count; i++)
            {
                if (i != e.RowIndex)
                {
                    GridViewRow row1 = datos_especificos.Rows[i];

                    row1.Cells[12].ControlStyle.CssClass = "hidden";
                }
            }
            try
            {
                //Finding the controls from Gridview for the row which is going to update  
                TextBox name = datos_especificos.Rows[e.RowIndex].FindControl("txt_Name") as TextBox;
                string hola = name.Text;
                string name_function = "Zfv_Mod_Cabecera";
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("P_TOUR_ID", Session["tour_id"]);
                rfcFunction_Company.SetValue("P_VISIT_ID", Session["visit"]);
                rfcFunction_Company.SetValue("P_HH_DELVRY", Session["HH_DELVRY"]);
                rfcFunction_Company.SetValue("P_BSTKD", hola);
                rfcFunction_Company.Invoke(rfcDestination);
                datos_especificos.EditIndex = -1;
                get_Data2(Session["visit"].ToString());
                if (esstado.Value == "liquidar")
                {
                    String script = @"<script type='text/javascript'>
                            clickLiquidar();</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiquidar", script, false);
                }
                else if (esstado.Value == "liberar")
                {
                    String script = @"<script type='text/javascript'>
                            clickLiberar();</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLiberar", script, false);
                }
                else if (esstado.Value == "log")
                {
                    String script = @"<script type='text/javascript'>
                            clickLog();</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "clickLog", script, false);
                }

                for (int i = 0; i < datos_especificos.Rows.Count; i++)
                {

                    GridViewRow row1 = datos_especificos.Rows[i];

                    row1.Cells[12].ControlStyle.CssClass = "hidden";
                }
                datos_especificos.Columns[12].HeaderStyle.CssClass = "hidden";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GridView1_RowUpdating() --> " + ex.Message + " -->");
            }
        }

        //boton que regresa a Cockpit
        protected void Out_Clicked(object sender, EventArgs e)
        {
            Response.Redirect("Cockpit.aspx", false);
        }

    } 
}