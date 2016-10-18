using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAP.Middleware.Connector;
using System.Data;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public partial class Log_De_Errores : System.Web.UI.Page
    {
        Convertir convertir = new Convertir();
        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;

        protected void Page_Load(object sender, EventArgs e)
        {
            show_Errors();
            //System.Diagnostics.Debug.WriteLine(Session["detalle_sala_venta"]);
            //System.Diagnostics.Debug.WriteLine(Session["detalle_cet"]);
            //System.Diagnostics.Debug.WriteLine(Session["detalle_ruta"]);
        }

        public void show_Errors()
        {
            input_agencia.Value = Session["detalle_sala_venta"].ToString();
            input_cet.Value = Session["detalle_cet"].ToString();
            input_ruta.Value = Session["detalle_ruta"].ToString();

            DataSet ds = new DataSet();
            string name_function = "Zdsd_Funciones_Asp_Net";
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            //System.Diagnostics.Debug.WriteLine("tiempo" + fecha);
            try
            {
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                rfcFunction_Company.SetValue("I_Proceso", "ERROR");
                IRfcTable i_tbl_refresh = rfcFunction_Company.GetTable("I_Tbl_Refresh");

                i_tbl_refresh.Append();
                i_tbl_refresh.SetValue("SALA_VENTA", Session["detalle_sala_venta"]);
                i_tbl_refresh.SetValue("CET", Session["detalle_cet"]);
                i_tbl_refresh.SetValue("RUTA", Session["detalle_ruta"]);
                i_tbl_refresh.SetValue("FECHA_LIQUIDA", fecha);

                rfcFunction_Company.Invoke(rfcDestination);
                ds.Tables.Add(convertir.toGridView(rfcFunction_Company.GetTable("O_Tbl_Lerror"), errors));

                foreach (GridViewRow links in errors.Rows)
                {
                    if (links.Cells[0].Text == "S")
                    {
                        links.Cells[0].Text = "Subida";
                    }
                    else if (links.Cells[0].Text == "B")
                    {
                        links.Cells[0].Text = "Bajada";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("<-- show_Errors Error: " + ex.Message + " -->");
            }
        }

        //boton que regresa a Cockpit
        protected void Out_Clicked(object sender, EventArgs e)
        {
            Response.Redirect("Cockpit.aspx", false);
        }
    }
}