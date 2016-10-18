using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontEnd_Liquidaciones
{
    public partial class Reporte_PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Byte[] FileBuffer = (Byte[])Session["pdf_array"];
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }
    }
}