using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public class Objeto {
        public string codigo { get;set;}
        public string nombre { get; set; }
        public string codigo_sociedad { get; set; }

        public Objeto() { }
    }

    public class Cets {
        public string codigo { get; set; }
    }

    public partial class Gen_Reportes : System.Web.UI.Page
    {
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;
        ArrayList codigosSociedad = new ArrayList();
        Convertir convert = new Convertir();

        // Declarando variables de Reportes
        DataTable dataTableReporte;
        // Declarando variables de Agencias
        List<Objeto> agencias = new List<Objeto>();
        // Declarando variables Cets
        List<Objeto> cets = new List<Objeto>();
        List<String> cetsPermitidosList = new List<String>();
        DataTable rep = new DataTable();
        Boolean algoVisible = false;
        ArrayList visibilidadAvanzados = new ArrayList();
       
        protected void Page_Load(object sender, EventArgs e)
        {
                
            if (!IsPostBack)
            {
                verificarUsuario();                            // Verifico que el usuario haya iniciado sesión y que la sesión exista;
                Conexion();                                    // Verificación de la conexión con SAP
                cargaReporte();                                // Lleno el dropdown de reporte       
                cargoAgencia();                                // Verifico las agencias que el usuario puede ver y las cargo al dropdown
                mostrarCets();
                CetsPermitidos();                              // Verifico los cets que el usuario puede ver y los cargo en el dropdown               
                obtengoRutas();                                // Cargo las rutas dependiendo de lo que este seleccionado
                cargoGiroNegocio();                            // Cargo los datos para Giro Negocio
                cargoCatProductos();                           // Cargo los datos para categoria productos
                cargoCadena();                                 // Cargo los datos para cadena
                cargoTipoMercado();                            // Cargo los datos de tipo mercado
                cargoDocumentoF1();                            // Cargo los datos para Documento F1
                cargoGrupoMaterial();                          // Cargo los datos para Grupo Material
                cargoMaterial();                               // Cargo los datos para Material
                habilitarCampos();                             // Habilito los datos del reporte seleccionado
                cargarCets(cmbAgency.SelectedValue);
                obtengoRutas();
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

        // Verificando la conexion con SAP
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
                System.Diagnostics.Debug.WriteLine("Error Conexion() --> " + ex.Message);
            }
        }

        // Cargo los reportes al dropdown
        protected void cargaReporte()
        {
            DataTable dt = new DataTable();
            string sql_society = "SELECT * FROM Programas_R3 WHERE tipo_Programa = 'R'";
            try
            {
                dataAdapter = new SqlDataAdapter(sql_society, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);
                cmbReporte.DataSource = dt;
                cmbReporte.DataTextField = "descripcion";
                cmbReporte.DataValueField = "programa";
                cmbReporte.DataBind();
                dataTableReporte = dt;
                    Session["reportes"] = dt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargaReporte() --> " + ex.Message);
            }
        }
 
        //llena el combo de agencia
        public void cargoAgencia()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            DataTable dt = new DataTable();
            string sql = "SELECT d.detalle_op FROM usuario u inner join detalle_usuario d "
                             + "ON u.codigo_usuario = d.codigo_usuario WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=4";
            try
            {
                dataAdapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);//contar rows

                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "SVENT");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Company.GetTable("It_Datos")));
                DataRow[] foundRow;
                DataRow[] foundRow1;
                int cont = 0;
                Session["sociedades"] = ds.Tables[0];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    foundRow = dt.Select("detalle_op = '" + dr[0].ToString() + "'");
                    foundRow1 = dt.Select("detalle_op = 'ATOTAL'");
                    if (foundRow1.Length == 1)
                    {
                        cmbAgency.Items.Insert(cont, new ListItem(dr[1].ToString(), dr[0].ToString()));
                        cont++;

                    }
                    else
                    {
                        for (int q = 0; q < foundRow.Length; q++)
                        {
                            cmbAgency.Items.Insert(cont, new ListItem(dr[1].ToString(), dr[0].ToString()));
                            cont++;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoAgencia() --> " + ex.Message);
            }
        }

        // Verificar cets permitidos para el usuario
        protected void CetsPermitidos()
        {
            try
            {
                DataSet dsCet = new DataSet();
                DataTable dtCet = new DataTable();
                DataSet cetsPermitidos = new DataSet();
                string sqlQuery = "SELECT d.detalle_op FROM usuario u inner join detalle_usuario d " +
                                  "ON u.codigo_usuario = d.codigo_usuario WHERE u.usuario = '" + Session["usuario"] + "' AND d.maestro=5";
                dataAdapter = new SqlDataAdapter(sqlQuery, connection);
                dataAdapter.Fill(dsCet);
                DataTable dt = dsCet.Tables[0];
                int cont = 0;
                DataRow[] detalle_op = dt.Select("detalle_op = 'ATOTAL'");
                if (detalle_op.Length == 1)
                {
                    foreach (ListItem li in cmbTodosLosCets.Items)
                    {
                        cmbCetsPermitidos.Items.Add(li);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string cod = cmbTodosLosCets.Items.FindByValue(dr["descripcion"].ToString()).Value;
                        string desc = cmbTodosLosCets.Items.FindByValue(dr["descripcion"].ToString()).Text;
                        cmbCetsPermitidos.Items.Insert(cont, new ListItem(desc, cod));
                    }
                }
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error CetsPermitidos() --> " + ex.Message);
            }
        }

        // Cargo los cets y los mando a obtener todos los datos
        protected void cargarCets(String codigo) {
            string name_function = "Zconsulta_Maestro_Detalle";
            string descripcion_it_datos = "DESCRIPCION";
            string codigo_it_datos = "CODIGO";
            DataSet ds = new DataSet();
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "RECET");
                IRfcTable tablaEnv = rfcFunction_Company.GetTable("TG_OFICINAS");
                tablaEnv.Append();
                tablaEnv.SetValue("VKBUR", codigo);
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Company.GetTable("It_Datos")));
                


                DataTable dt = ds.Tables[0];
                int cont = 0;
                foreach (DataRow dr in dt.Rows){

                    ListItem item = cmbCetsPermitidos.Items.FindByValue(dr[1].ToString());
                    cmbCet.Items.Insert(cont,new ListItem(item.Text,item.Value));
                    cont++;
                }
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargarCets() --> " + ex.Message);
            }
        }

        // Obtengo los Cets ya filtrados para mostrarlos en el dropdown
        protected void mostrarCets() {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            string descripcion_it_datos = "DESCRIPCION";
            string codigo_it_datos = "CODIGO";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "CETS");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Company.GetTable("It_Datos")));
                convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"),cmbTodosLosCets,descripcion_it_datos,codigo_it_datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error mostrarCets() --> " + ex.Message);
            }
        }

        // Obteniendo rutas
        protected void obtengoRutas()
        {
            DataSet ds = new DataSet();
            string name_function = "ZMAESTRO_RUTAS";
   
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                // Mando tabla agencia
                IRfcTable i_agencia = rfcFunction_Company.GetTable("I_Agencia");
                i_agencia.Append();
                i_agencia.SetValue("MANDT","600");
                i_agencia.SetValue("AGENCIA",cmbAgency.SelectedItem.Value);
                i_agencia.SetValue("NOMBRE_AGENCIA", cmbAgency.SelectedItem.Text);
                i_agencia.SetValue("PAIS", "GT");
                i_agencia.SetValue("SOCIEDAD", "");
                // Mando tabla cets
                IRfcTable i_cet = rfcFunction_Company.GetTable("I_cet");
                i_cet.Append();
                i_cet.SetValue("CET",cmbCet.SelectedItem.Value);
                i_cet.SetValue("NOMBRE_CET", cmbCet.SelectedItem.Text);
                i_cet.SetValue("AGENCIA", cmbAgency.SelectedItem.Value);
                i_cet.SetValue("PAIS", "GT");
                i_cet.SetValue("SOCIEDAD", "");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("R_RUTA"), cmbRuta, "RUTA", "RUTA"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error obtengoRutas() --> " + ex.Message);
            }
        }

        // Obtengo y lleno Giro Negocio
        protected void cargoGiroNegocio() {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "GNEGO");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbGiroNegocio, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoGiroNegocio() --> " + ex.Message);
            }
        }

        // Obtengo y lleno Categorias Productos
        protected void cargoCatProductos()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "CTPRO");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbCatProductos, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoCatProductos() --> " + ex.Message);
            }
        }

        // Obtengo y lleno Cadenas
        protected void cargoCadena()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "CADEN");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbCadena, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoCadena() --> " + ex.Message);
            }
        }

        // Obtengo y lleno Tipo  mercado
        protected void cargoTipoMercado()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "TMERC");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbTipoMercado, "DESCRIPCION", "CODIGO"));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoTipoMercado() --> " + ex.Message);
            }
        }

        // Obtengo y lleno DocumetosF1
        protected void cargoDocumentoF1()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "DOCF1");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbDocumentoF1, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoDocumentoF1() --> " + ex.Message);
            }
        }

        // Obtengo y lleno Grupo material
        protected void cargoGrupoMaterial()
        {
            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "GRMAT");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbGrupoMaterial, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoGrupoMaterial() --> " + ex.Message);
            }
        }

        // Obtengo y lleno material
        protected void cargoMaterial()
        {

            DataSet ds = new DataSet();
            string name_function = "Zconsulta_Maestro_Detalle";
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Parametro", "MATER");
                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDropdownList(rfcFunction_Company.GetTable("It_Datos"), cmbMaterial, "DESCRIPCION", "CODIGO"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargoMaterial() --> " + ex.Message);
            }
        }

        // Mando los datos a SAP para crear el PDF del reporte  
        public void create_PDF()
        {
            try
            {
                DataSet ds = new DataSet();
                string name_function = "zfv_spooltopdf";
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                var fecha = DateTime.Now.ToString("yyyyMMdd");

                Char delimitador = '/';

                string fecha_myDatepicker = "";
                string fecha_datePickerCondicionPago = "";
                string fecha_datePickerFechaAl = "";
                string fecha_contratoPicker = "";
                string fecha_utilizacionPicker = "";
                if (myDatepicker.Visible && myDatepicker.Value.ToString() != "")
                {
                    String fecha_original_myDatepicker = myDatepicker.Value.ToString();
                    String[] substrings_myDatepicker = fecha_original_myDatepicker.Split(delimitador);
                    fecha_myDatepicker = substrings_myDatepicker[2] + substrings_myDatepicker[1] + substrings_myDatepicker[0];
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("No lleno el datepiker");
                    return;
                }

                if (datePickerCondicionPago.Visible)
                {
                    String fecha_original_datePickerCondicionPago = datePickerCondicionPago.Value.ToString();
                    String[] substrings_datePickerCondicionPago = fecha_original_datePickerCondicionPago.Split(delimitador);
                    fecha_datePickerCondicionPago = substrings_datePickerCondicionPago[2] + substrings_datePickerCondicionPago[1] + substrings_datePickerCondicionPago[0];
                }

                if (datePickerFechaAl.Visible)
                {
                    String fecha_original_datePickerFechaAl = datePickerFechaAl.Value.ToString();
                    String[] substrings_datePickerFechaAl = fecha_original_datePickerFechaAl.Split(delimitador);
                    fecha_datePickerFechaAl = substrings_datePickerFechaAl[2] + substrings_datePickerFechaAl[1] + substrings_datePickerFechaAl[0];
                }
                if (contratoPicker.Visible)
                {
                    String fecha_original_contratoPicker = contratoPicker.Value.ToString();
                    String[] substrings_contratoPicker = fecha_original_contratoPicker.Split(delimitador);
                    fecha_contratoPicker = substrings_contratoPicker[2] + substrings_contratoPicker[1] + substrings_contratoPicker[0];
                }

                if (utilizacionPicker.Visible)
                {
                    String fecha_original_utilizacionPicker = utilizacionPicker.Value.ToString();
                    String[] substrings_utilizacionPicker = fecha_original_utilizacionPicker.Split(delimitador);
                    fecha_utilizacionPicker = substrings_utilizacionPicker[2] + substrings_utilizacionPicker[1] + substrings_utilizacionPicker[0];
                }

                rfcFunction_Company.SetValue("RUTA", (rutaControl.Visible ? cmbRuta.SelectedValue : ""));
                rfcFunction_Company.SetValue("CET", (cetControl.Visible ? cmbCet.SelectedValue: ""));
                rfcFunction_Company.SetValue("FECHA", (fechaDeControl.Visible ? fecha_myDatepicker : ""));//fecha
                rfcFunction_Company.SetValue("FECHA2", (datePickerFechaAl.Visible ? fecha_datePickerFechaAl : ""));//fecha
                rfcFunction_Company.SetValue("AGENCIA", (agenciaControl.Visible ? cmbAgency.SelectedValue : ""));
                rfcFunction_Company.SetValue("CLIENTE", (clienteSapControl.Visible ? txtClienteSap.Value : ""));
                rfcFunction_Company.SetValue("CONPAGO", (condicionPagoControll.Visible ? fecha_datePickerCondicionPago : ""));//fecha
                rfcFunction_Company.SetValue("CADENA", (cadenaControl.Visible ? cmbCadena.SelectedValue : ""));
                rfcFunction_Company.SetValue("CLISIO", clienteSioControl.Visible ? txtClienteSIO.Value : "");
                rfcFunction_Company.SetValue("NEGOCIO", (giroNegocioControl.Visible ? cmbGiroNegocio.SelectedValue : ""));
                rfcFunction_Company.SetValue("TMERC", (tipoMercadoControl.Visible ? cmbTipoMercado.SelectedValue : ""));
                rfcFunction_Company.SetValue("DOCF1", (documentoF1Control.Visible ? cmbDocumentoF1.SelectedValue : ""));
                rfcFunction_Company.SetValue("NOCONTRATO", (contratoControl.Visible ? fecha_contratoPicker : ""));//fecha
                rfcFunction_Company.SetValue("UTILIZACION", (utilizacionControl.Visible ? fecha_utilizacionPicker : ""));//fecha
                rfcFunction_Company.SetValue("CENTRO", (centroControl.Visible ? txtCentro.Value : ""));
                rfcFunction_Company.SetValue("NOM_PROGRAMA", cmbReporte.SelectedValue);
                rfcFunction_Company.SetValue("USUARIO", Session["usuario"].ToString().ToUpper());
                rfcFunction_Company.SetValue("NOMBRE_PC", Environment.MachineName);
                rfcFunction_Company.SetValue("PATH_DESTINO", "C");
                rfcFunction_Company.SetValue("NUMERO_SPOOL", "0000000000");
                rfcFunction_Company.SetValue("SOCIEDAD", obtenerCodigoSociedad());
                rfcFunction_Company.SetValue("MATERIAL", (cmbMaterial.Visible? cmbMaterial.SelectedValue : ""));

                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Company.GetTable("IT_PDF_HEXADECIMAL"), hex_data));

                string hex_SAP = "";
                foreach (GridViewRow rows in hex_data.Rows)
                {
                    hex_SAP += rows.Cells[0].Text;//concatena los hexadecimales de la funcion de sap y los hace un solo string
                }

                int NumChars = hex_SAP.Length / 2;
                byte[] pdfFile = new byte[NumChars];
                using (var sr = new StringReader(hex_SAP))
                {
                    for (int i = 0; i < NumChars; i++)
                        pdfFile[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
                }
                
                Byte[] FileBuffer = pdfFile;
                Session["pdf_array"] = FileBuffer;//sirve para Reprte_PDF, envia la data
                ClientScript.RegisterStartupScript(GetType(), "open_tab", "open_tab();", true);

            }
            catch (TimeoutException te)
            {
                advertencia_final.Visible = true;
                advertencia_final.InnerText = "El reporte es muy grande, revisar el spool de reportes.";
                System.Diagnostics.Debug.WriteLine("Error create_PDF() --> TIMEOUT" + te.Message + " -->");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error create_PDF() --> " + ex.Message);
            }
        }

        // Verifico que el reporte a generar no exista todavia
        public Boolean get_Spool_Reportes()
        {
            Boolean respuesta = true;
            Boolean existe = false;
            try
            {
                DataSet ds = new DataSet();
                string name_function = "Zfv_Spool_X_Usuario";
                IRfcFunction rfcFunction_Spool_Reportes = rfcRepository.CreateFunction(name_function.ToUpper());
                var fecha = "";
  
                fecha = DateTime.Now.ToString("yyyyMMdd");
                rfcFunction_Spool_Reportes.SetValue("I_USUARIO", Session["usuario"].ToString());//--1
                rfcFunction_Spool_Reportes.SetValue("I_FECHA", fecha);//--1
                rfcFunction_Spool_Reportes.Invoke(rfcDestination);
                ds.Tables.Add(convert.toDataTable(rfcFunction_Spool_Reportes.GetTable("R_SPOOL")));
                string fecha_myDatepicker = "";
                 if (myDatepicker.Visible)
                {
                    String fecha_original_myDatepicker = myDatepicker.Value.ToString();
                    String[] substrings_myDatepicker = fecha_original_myDatepicker.Split('/');
                    fecha_myDatepicker = substrings_myDatepicker[2] + substrings_myDatepicker[1] + substrings_myDatepicker[0];
                }

                foreach(DataRow row in ds.Tables[0].Rows){
                    String[] fechaRowSplit = row["FECHA"].ToString().Split('-');
                    string fechaRow = fechaRowSplit[0] + fechaRowSplit[1] + fechaRowSplit[2];
                    
                    if (fechaRow.Equals(fecha_myDatepicker)
                        && row["NOM_PROGRAMA"].ToString().Equals(cmbReporte.SelectedValue.ToString())
                        && row["RUTA"].ToString().Equals(cmbRuta.SelectedValue.ToString()))
                    {
                        existe = true;
                        advertencia_final.Visible = true;
                        advertencia_final.InnerText = "Ya existe un reporte igual";
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error get_Spool_Reportes() --> " + ex.Message);
            }
            if (existe)
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }

        //boton de crear pdf clickeado
        protected void pdf_clicked(object sender, EventArgs e)
        {
            if (get_Spool_Reportes())
            {
                create_PDF();
            }
        }

        // Ejecuto si se selecciona un CET
        protected void cmbCet_SelectedIndexChanged(object sender, EventArgs e)
        {
            obtengoRutas();           
        }

        // Ejecuto si se selecciona una Agencia
        protected void agendaChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Hola mundo: "+cmbAgency.SelectedValue);
            cmbCet.Items.Clear();
            cets.Clear();
            cargarCets(cmbAgency.SelectedValue);

        }

        // Ejecuto cuando se da click en Consulta Avanzada, Muestra y oculta
        protected void consult_icon_Click(object sender, ImageClickEventArgs e)
        {
            String var = visibleVar.Value;
            String script = @"<script type='text/javascript'>
                            ocultarAvanzado("+var+");</script>";
            if (algoVisibleVar.Value == "true")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "ocultarAvanzado", script, false);
            }

            if (visibleVar.Value == "true")
            {
                visibleVar.Value = "false";
            }
            else
            {
                visibleVar.Value = "true";
            }
            
        }

        // Mando a verificar el programa
        private void habilitarCampos(){
            rep = (DataTable)(Session["reportes"]);
            try
            {
                foreach (DataRow dr in rep.Rows)
                {
                    if (dr["Programa"].ToString().Equals(cmbReporte.SelectedValue))
                    {
                        verificarPrograma(dr);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error habilitarCampos() --> " + ex.Message);
            }
        
        }

        // Evento select del dropdownlist reporte
        protected void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            habilitarCampos();

            if (visibilidadAvanzados.Contains(true))
            {
                algoVisibleVar.Value = "true";

            }
            else
            {
                algoVisibleVar.Value = "false";

            }
        }

        // Obtengo la sociedad
        public string obtenerCodigoSociedad() {
            string codigo = "";
            DataTable dt = (DataTable)(Session["sociedades"]);
            foreach (DataRow dr in dt.Rows) {
                if (cmbAgency.SelectedValue.ToString() == dr[0].ToString())
                {
                    codigo = dr[2].ToString();
                }
            }
            return codigo;
        }

        // Verifico que tipo de reporte es para ocultar o mostrar avanzadas
        protected void verificarPrograma(DataRow data)
        {
            
            visibilidadAvanzados.Clear();
            string _nombre = data["Programa"].ToString();
            string _rutas = data["Param_Ruta"].ToString().Trim();
            string _cets = data["Param_CET"].ToString().Trim();
            string _fechaDe = data["Param_Fecha_Del"].ToString().Trim();
            string _fechaAl = data["Param_Fecha_Al"].ToString().Trim();
            string _clienteSAP = data["Param_Cliente_Sap"].ToString().Trim();
            string _condicionPago = data["Param_Condicion_Pago"].ToString().Trim();
            string _clienteSIO = data["Param_Cliente_SIO"].ToString().Trim();
            string _cadena = data["Param_Cadena"].ToString().Trim();
            string _tipoMercado = data["Param_Tipo_Mercado"].ToString().Trim();
            string _giroNegocio = data["Param_Giro_Negocio"].ToString().Trim();
            string _documentoF1 = data["Param_DocF1"].ToString().Trim();
            string _catProductp = data["Param_Cat_Producto"].ToString().Trim();
            string _archivo = data["Param_Path"].ToString().Trim();
            string _contrato = data["Param_Contrato"].ToString().Trim();
            string _utilizacion = data["Param_Utilizacion"].ToString().Trim();
            string _centro = data["Param_Centro"].ToString().Trim();
            string _grupoMateria = data["Param_Grupo_Material"].ToString().Trim();
            string _material = data["Param_Material"].ToString().Trim();

            

            // Habilitar o deshabilitar la fecha del
            if (_fechaDe == "X")
            {
                fechaDeControl.Visible = true;
            }
            else
            {
                fechaDeControl.Visible = false;

                visibleVar.Value = "false";
            }
            // Habilitar o deshabilitar la fecha al
            if (_fechaAl == "X")
            {
                datePickerFechaAl.Visible = true;

            }
            else
            {
                datePickerFechaAl.Visible = false;

            }
            // Habilitar o deshabilitar la fecha al
            if (data["Param_Fecha_Al"].ToString().Trim() == "X")
            {

                datePickerFechaAl.Visible = true;

            }
            else
            {
                datePickerFechaAl.Visible = false;

            }
            // Habilitar o deshabilitar laS rutas
            if (_rutas == "X")
            {
                rutaControl.Visible = true;

            }
            else
            {
                rutaControl.Visible = false;

            }
            // Habilitar o deshabilitar los cets
            if (_cets == "X")
            {
                cetControl.Visible = true;

            }
            else if (
                _nombre == "ZDSD_MATINAL_TMP" ||
                _nombre == "ZDSD_REPORTE5" ||
                _nombre == "ZDSD_REPORTE4" ||
                _nombre == "ZDSD_RP_RETENIDOS" ||
                _nombre == "ZDSD_CRITICA" ||
                _nombre == "ZDSD_TOP50_JR_FE" ||
                _nombre == "ZDSD_REP_LIBRORUTA"
                )
            {
                cetControl.Visible = true;
            }
            else
            {
                cetControl.Visible = false;
                algoVisible = false;
            }
            // Habilitar o deshabilitar Cliente SAP
            if (_clienteSAP == "X")
            {
                clienteSapControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else if (_nombre == "ZDSD_CONSIG_NET")
            {
                clienteSapControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                clienteSapControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Condicion Pago
            if (_condicionPago == "X")
            {
                condicionPagoControll.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                condicionPagoControll.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Cliente SIO
            if (_clienteSIO == "X")
            {
                clienteSioControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                clienteSioControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Cadena
            if (_cadena == "X")
            {
                cadenaControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                cadenaControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Tipo Mercado
            if (_tipoMercado == "X")
            {
                tipoMercadoControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                tipoMercadoControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Giro negocio
            if (_giroNegocio == "X")
            {
                giroNegocioControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                giroNegocioControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Documento F1
            if (_documentoF1 == "X")
            {
                documentoF1Control.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                documentoF1Control.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Cat Producto
            if (_catProductp == "X")
            {
                catProductoControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                catProductoControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Archivo
            if (_archivo == "X")
            {
                archivoControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                archivoControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Contrato
            if (_contrato == "X")
            {
                contratoControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                contratoControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Utilizacion
            if (_utilizacion == "X")
            {
                utilizacionControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                utilizacionControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            // Habilitar o deshabilitar Centro
            if (_centro == "X")
            {
                centroControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else if (_nombre == "ZDSD_CONSIG_NET")
            {
                centroControl.Visible = true;
                visibilidadAvanzados.Add(true);
            }
            else
            {
                centroControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            if (_grupoMateria == "X")
            {
                if (_nombre != "ZDSD_CONSIG_NET")
                {
                    grupoMaterialControl.Visible = true;
                    visibilidadAvanzados.Add(true);
                }
            }
            else
            {
                grupoMaterialControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
            if (_material == "X")
            {
                if (_nombre != "ZDSD_CONSIG_NET")
                {
                    materialControl.Visible = true;
                    visibilidadAvanzados.Add(true);
                }
                
            }
            else
            {
                materialControl.Visible = false;
                visibilidadAvanzados.Add(false);
            }
        }
    }
}