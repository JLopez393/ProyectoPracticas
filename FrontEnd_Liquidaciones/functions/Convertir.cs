using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SAP.Middleware.Connector;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;


namespace FrontEnd_Liquidaciones.functions
{
    public class Convertir
    {
        //Convierte a dropdownlist 
        public DataTable toDropdownList(IRfcTable RFCTable, DropDownList ID_DropdownList, string TextField, string ValueField)
        {
            DataTable dtTable = new DataTable();

            for (int item = 0; item < RFCTable.ElementCount; item++)
            {
                RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                dtTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in RFCTable)
            {
                DataRow dr = dtTable.NewRow();
                for (int item = 0; item < RFCTable.ElementCount; item++)
                {
                    RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                    if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                    {
                        dr[item] = row.GetInt(metadata.Name);
                    }
                    else
                    {
                        dr[item] = row.GetString(metadata.Name);
                    }
                }
                dtTable.Rows.Add(dr);
            }
            ID_DropdownList.DataSource = dtTable;
            ID_DropdownList.DataTextField = TextField;
            ID_DropdownList.DataValueField = ValueField;
            ID_DropdownList.DataBind();
            return dtTable;
        }

        //Convierte a DataTable
        public DataTable toDataTable(IRfcTable RFCTable)
        {
            DataTable dtTable = new DataTable();

            for (int item = 0; item < RFCTable.ElementCount; item++)
            {
                RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                dtTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in RFCTable)
            {
                DataRow dr = dtTable.NewRow();
                for (int item = 0; item < RFCTable.ElementCount; item++)
                {
                    RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                    if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                    {
                        dr[item] = row.GetInt(metadata.Name);
                    }
                    else
                    {
                        dr[item] = row.GetString(metadata.Name);
                    }
                }
                dtTable.Rows.Add(dr);
            }

            return dtTable;
        }

        //Convierte gridview
        public DataTable toGridView(IRfcTable RFCTable, GridView ID_GridView)
        {
            DataTable dtTable = new DataTable();

            for (int item = 0; item < RFCTable.ElementCount; item++)
            {
                RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                dtTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in RFCTable)
            {
                DataRow dr = dtTable.NewRow();
                for (int item = 0; item < RFCTable.ElementCount; item++)
                {
                    RfcElementMetadata metadata = RFCTable.GetElementMetadata(item);
                    if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                    {
                        dr[item] = row.GetInt(metadata.Name);
                    }
                    else
                    {
                        dr[item] = row.GetString(metadata.Name);
                    }
                }
                dtTable.Rows.Add(dr);
            }

            ID_GridView.DataSource = dtTable;
            ID_GridView.DataBind();
            return dtTable;
        }
    }

    public class SAPDestinationConfig : IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, ConfigurationManager.AppSettings["NAME"]);
            parms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"]);
            parms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"]);
            parms.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            parms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"]);
            parms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]);
            parms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            parms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]);
            parms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]);
            return parms;
        }
    }
}