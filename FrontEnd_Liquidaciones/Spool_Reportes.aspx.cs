using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FrontEnd_Liquidaciones.functions;

namespace FrontEnd_Liquidaciones
{
    public partial class Spool_Reportes : System.Web.UI.Page
    {

        static RfcDestination rfcDestination = RfcDestinationManager.GetDestination("SapQA");
        static RfcRepository rfcRepository = rfcDestination.Repository;
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        Convertir convert = new Convertir();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarUsuario();
                get_Spool_Reportes();
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
                System.Diagnostics.Debug.WriteLine("Error Conexion()--> " + ex.Message);
            }
        }

        //crea el spool reportes
        public void get_Spool_Reportes()
        {
            try
            {
                DataSet ds = new DataSet();
                string name_function = "Zfv_Spool_X_Usuario";
                IRfcFunction rfcFunction_Spool_Reportes = rfcRepository.CreateFunction(name_function.ToUpper());
                var fecha = "";
                if (myDatepicker.Value.Trim() != "")
                {
                    String fecha_original = myDatepicker.Value.ToString();
                    Char delimitador = '/';
                    String[] substrings = fecha_original.Split(delimitador);
                    fecha = substrings[2] + substrings[1] + substrings[0];
                }
                else
                {
                    fecha = DateTime.Now.ToString("yyyyMMdd");
                }
                rfcFunction_Spool_Reportes.SetValue("I_USUARIO", Session["usuario"].ToString());//--1
                rfcFunction_Spool_Reportes.SetValue("I_FECHA", fecha);//--1
                rfcFunction_Spool_Reportes.Invoke(rfcDestination);
                ds.Tables.Add(convert.toGridView(rfcFunction_Spool_Reportes.GetTable("R_SPOOL"), GridView_Spool_Reportes));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error get_Spool_Reportes() --> " + ex.Message + " -->");
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
                    TableCell statusCell = e.Row.Cells[8];
                    if (statusCell.Text == "R")
                    {
                        div.Attributes["class"] = "circle_red";
                    }
                    else if (statusCell.Text == "A")
                    {
                        div.Attributes["class"] = "circle_yellow";
                    }
                    else if (statusCell.Text == "V")
                    {
                        div.Attributes["class"] = "circle_green";
                    }

                    e.Row.Cells[8].Controls.Add(div);
                }
            }
             catch (Exception ex)
             {
                 System.Diagnostics.Debug.WriteLine("Error OnRowDataBound() --> " + ex.Message + " -->");
             }
        }

        //ve que numero de spool es y manda a llamar create_PDF
        protected void OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView_Spool_Reportes.Rows[index];
                string estatus = row.Cells[8].Text;
                if (e.CommandName == "num_spool" && estatus == "V")
                {
                    string fecha_reporte = row.Cells[2].Text;
                    string nombre_programa = row.Cells[3].Text;
                    string no_spool = row.Cells[5].Text;
                    create_PDF(nombre_programa, fecha_reporte, no_spool);
                }

                foreach (GridViewRow row_colors in GridView_Spool_Reportes.Rows)
                {
                    if (row_colors.RowType == DataControlRowType.DataRow)
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        TableCell statusCell = row_colors.Cells[8];
                        if (statusCell.Text == "R")
                        {
                            div.Attributes["class"] = "circle_red";
                        }
                        else if (statusCell.Text == "A")
                        {
                            div.Attributes["class"] = "circle_yellow";
                        }
                        else if (statusCell.Text == "V")
                        {
                            div.Attributes["class"] = "circle_green";
                        }
                        row_colors.Cells[8].Controls.Add(div);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error OnRowCommand() --> " + ex.Message + " -->");
            }
        }  

        //crea el pdf
        protected void create_PDF(String nombre_programa, String fecha_reporte, String no_spool)
        {
            try
            {
                DataSet ds = new DataSet();
                string name_function = "zfv_spooltopdf";
                IRfcFunction rfcFunction_Company = rfcRepository.CreateFunction(name_function.ToUpper());
                String fecha_original = fecha_reporte;
                Char delimitador = '-';
                String[] substrings = fecha_original.Split(delimitador);
                string fecha = substrings[0] + substrings[1] + substrings[2];
                
                rfcFunction_Company.SetValue("NOM_PROGRAMA", nombre_programa);
                if (!fecha.Equals("00000000"))
                {
                    rfcFunction_Company.SetValue("FECHA", fecha);
                }
                rfcFunction_Company.SetValue("USUARIO", Session["usuario"].ToString());
                rfcFunction_Company.SetValue("NOMBRE_PC", Environment.MachineName);
                rfcFunction_Company.SetValue("PATH_DESTINO", "C");
                rfcFunction_Company.SetValue("NUMERO_SPOOL", no_spool);
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error create_PDF() --> " + ex.Message + " -->");
            }
        }

        //Borra los spool que esten con el checkbox seleccionado
        protected void Delete_Spool()
        {
            try
            {
                string name_function = "Zfv_Borra_Spool";
                IRfcFunction rfcFunction_Spool_Reportes = rfcRepository.CreateFunction(name_function.ToUpper());
                IRfcTable i_tbl_delete = rfcFunction_Spool_Reportes.GetTable("I_JOBS");
                foreach (GridViewRow row in GridView_Spool_Reportes.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string No_Job = row.Cells[1].Text;
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                        if (chkRow.Checked)
                        {
                            i_tbl_delete.Append();
                            i_tbl_delete.SetValue("NO_JOB", No_Job);
                            //System.Diagnostics.Debug.WriteLine(No_Job);
                        }
                    }
                }
                rfcFunction_Spool_Reportes.Invoke(rfcDestination);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error Delete_Spool() --> " + ex.Message + " -->");
            }
        }

        //Refrescar Clickeado
        protected void Refresh_Clicked(object sender, EventArgs e)
        {
            get_Spool_Reportes();
        }

        //Borrar Clickeado
        protected void Delete_Clicked(object sender, EventArgs e)
        {
            Delete_Spool();
            get_Spool_Reportes();
        }
    }
}