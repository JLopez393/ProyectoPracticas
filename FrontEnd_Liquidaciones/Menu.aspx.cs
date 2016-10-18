using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace FrontEnd_Liquidaciones
{
    public partial class Menu : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Menu.aspx.cs --> " + Session["lang"]);
        }
    }
}