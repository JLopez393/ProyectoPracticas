using System;
using System.Net;
using System.Configuration;
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

namespace menu
{
    public partial class login : System.Web.UI.Page
    {
        static string connectionString_seguridad = ConfigurationManager.ConnectionStrings["seguridad"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString_seguridad);
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;
        protected void Page_Load(object sender, EventArgs e){}

        //Obtiene usuarios
        protected void Usuarios(string user) {

            try{
                DataTable dt = new DataTable();
                string sql = "SELECT usuario FROM usuario WHERE usuario = '" + user_lg.Value.Trim() + "'";

                dataAdapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dt);//contar rows

                if (dt.Rows.Count > 0) {
                    System.Diagnostics.Debug.WriteLine("Login exitoso");
                    Session["usuario"] = user.Trim().ToUpper();
                    Response.Redirect("Cockpit.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "user_not_found", "user_not_found();", true);
                }
            }catch(Exception ex){
                System.Diagnostics.Debug.WriteLine("Error Usuarios() --> " + ex);
            }
        }

        protected void login_succes(object sender, EventArgs e)
        {
            string user_go = user_lg.Value.ToString().Trim();
            string pass_go = pass.Value.ToString().Trim();
            if (user_go == null || user_go == "") { 
                
            }
            else
            {
                Usuarios(user_go);
            }
        }
    }
}