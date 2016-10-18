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
    public partial class Descarga : System.Web.UI.Page
    {
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        //  ############################## Añadido
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        Convertir convert = new Convertir();
        protected void Page_Load(object sender, EventArgs e)
        {
            verificarUsuario();
            //System.Diagnostics.Debug.WriteLine(Session["bajada_Sal_Venta"]);
            //System.Diagnostics.Debug.WriteLine(Session["bajada_Cet"]);
            //System.Diagnostics.Debug.WriteLine(Session["bajada_ruta"]);
            asdf.Value = Session["bajada_ruta"].ToString();
            
            //Label2.Text = Convert.ToString(Session["rut"]);
        }

        // Verificar la sesion de usuario
        protected void verificarUsuario()
        {
            if (Session["usuario"] == null || (string)Session["usuario"] == "")
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void download(object sender, EventArgs e) {
            string name_function = "Zdsd_Funciones_Asp_Net";
            //string fecha = myDatepicker.Value;
            var fecha = DateTime.Now.ToString("yyyyMMdd");

            //System.Diagnostics.Debug.WriteLine("Se hizo click en el botón: " + fecha);
            //System.Diagnostics.Debug.WriteLine("Se hizo click en el botón CAMBIADA: " + fecha_original_datePicker);
            try
            {
                Char delimitador = '/';
                String fecha_original_datePicker = myDatepicker.Value.ToString();
                String[] substrings_datePickerCondicionPago = fecha_original_datePicker.Split(delimitador);
                fecha_original_datePicker = substrings_datePickerCondicionPago[2] + substrings_datePickerCondicionPago[1] + substrings_datePickerCondicionPago[0];

                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "BAJADA");
                IRfcTable i_tbl_refresh = rfcFunction_Company.GetTable("I_Tbl_Refresh");
                i_tbl_refresh.Append();
                i_tbl_refresh.SetValue("SALA_VENTA", Session["bajada_Sal_Venta"]);
                i_tbl_refresh.SetValue("CET", Session["bajada_Cet"]);
                i_tbl_refresh.SetValue("RUTA", Session["bajada_ruta"]);
                i_tbl_refresh.SetValue("FECHA_LIQUIDA", fecha_original_datePicker);
                     
                rfcFunction_Company.Invoke(rfcDestination);
                //ds.Tables.Add(ConvertToDonNet(rfcFunction_Company.GetTable("O_Tbl_Refresh"), GridView1));
                //System.Diagnostics.Debug.WriteLine(Session["bajada_Sal_Venta"]);
                //System.Diagnostics.Debug.WriteLine(Session["bajada_Cet"]);
                //System.Diagnostics.Debug.WriteLine(Session["bajada_ruta"]);

                Response.Redirect("Cockpit.aspx", false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("download Error: " + ex.Message + " -->");
            }
            
        }
    }
}