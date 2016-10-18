using FrontEnd_Liquidaciones.functions;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace FrontEnd_Liquidaciones
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Convertir convert = new Convertir();

            string destinationConfigName = "SapQA";
            //string destinationConfigName = "MAP";
            IDestinationConfiguration destinationConfig = null;
            bool destinationIsInialised = false;
            if (!destinationIsInialised)
            {
                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);

                if (RfcDestinationManager.TryGetDestination(destinationConfigName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationIsInialised = true;
                }

            }
        }
    }
}