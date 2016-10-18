using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Threading;

namespace FrontEnd_Liquidaciones
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Request["lang"]))
            {
                Session["lang"] = Request["lang"];
            }
            string lang = Convert.ToString(Session["lang"]);
            string culture = string.Empty;
            System.Diagnostics.Debug.WriteLine("BasePage.cs --> " + Session["lang"]);
            if (lang.ToLower().CompareTo("es") == 0 || string.IsNullOrEmpty(culture))
            {
                culture = "es";
            }
            if (lang.ToLower().CompareTo("en") == 0)
            {
                culture = "en-US";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            base.InitializeCulture();
        }
    }
}