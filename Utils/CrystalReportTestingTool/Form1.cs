using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace CrystalReportTestingTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                TxtMsg.Text = String.Empty;

                ReportDocument oDoc = new ReportDocument();
                oDoc.Load("Lab Order Requisition.rpt");

                PrepareReport(ref oDoc);
                Log("PrepareReport");

                oDoc.PrintOptions.NoPrinter = false;
                oDoc.PrintOptions.PrinterName = TxtPrinterName.Text;

                PaperOrientation oOrientation = oDoc.PrintOptions.PaperOrientation;
                PaperSize oPaperSize = oDoc.PrintOptions.PaperSize;
                oDoc.PrintOptions.PaperOrientation = oOrientation;
                oDoc.PrintOptions.PaperSize = oPaperSize;
                //oDoc.PrintOptions.CustomPaperSource = new System.Drawing.Printing.PaperSource() ;

                oDoc.PrintToPrinter(1, true, 1, 9999);

                TxtMsg.Text += Environment.NewLine + "Print Successfully Completed…";
            }
            catch(Exception ex)
            {
                TxtMsg.Text += Environment.NewLine + ex;
            }

        }

        protected bool PrepareReport(ref ReportDocument oDoc)
        {
            Log("Setting Database Info");
            foreach (IConnectionInfo ci in oDoc.DataSourceConnections)
            {
                ci.SetConnection(TxtDBServer.Text, TxtDatabase.Text, TxtUserName.Text, TxtPwd.Text);
            }
            Log(String.Format("DBServer : {0}, Database : {1}, User : {2}", TxtDBServer.Text, TxtDatabase.Text, TxtUserName.Text));

            // Need to validate this on server before we remove it
            /* Doesn't appear we need to do this with the .NET component
            // Fixing up the table location stored in the report
            foreach (Table oTable in oDoc.Database.Tables)
            {
                string sOrigLocation = oTable.Location;
                oTable.Location = this.m_DBDatabase + ".dbo." + sOrigLocation;
            }
            */

            Log("Setting Report Parameters");
            PassReportParams(oDoc);
            return true;
        }

        private void Log(String msg)
        {
            TxtMsg.Text += Environment.NewLine + msg;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TxtPrinterName.Text = ConfigurationManager.AppSettings["PrinterName"];
            TxtDBServer.Text = ConfigurationManager.AppSettings["DBServer"];
            TxtDatabase.Text = ConfigurationManager.AppSettings["Database"];
            TxtUserName.Text = ConfigurationManager.AppSettings["Username"];
            TxtPwd.Text = ConfigurationManager.AppSettings["Password"];
            TxtOrgId.Text = ConfigurationManager.AppSettings["OrgId"];
            TxtSiteId.Text = ConfigurationManager.AppSettings["SiteId"];
        }

        public void PassReportParams(ReportDocument oDoc)
        {
            for (int i = 0; i < oDoc.DataDefinition.ParameterFields.Count; i++)
            {
                ParameterFieldDefinition oParam = oDoc.DataDefinition.ParameterFields[i];

                var sPromptText = oParam.PromptText ?? string.Empty;

                // There is a bug in the ParameterFieldDefinition object
                // It is not telling us accurately that a parameter is linked
                // So for now, we are going to embed prompting text = LINKED inside
                // each report that has linked parameters
                if (oParam.HasCurrentValue ||
                    oParam.IsLinked() ||
                    oParam.Name.IndexOf(".") >= 0 ||
                    oParam.Name.IndexOf("-?") >= 0 ||
                    sPromptText.ToUpper() == "LINKED")
                {
                    continue;
                }

                string sVal = GetParamValue(oParam.Name);

                try
                {
                    if (oParam.ValueType == FieldValueType.BooleanField)
                        oDoc.SetParameterValue(i, ConvertToBool(sVal));
                    else if (oParam.ValueType == FieldValueType.StringField)
                        oDoc.SetParameterValue(i, sVal);
                    else if (oParam.ValueType == FieldValueType.DateField || oParam.ValueType == FieldValueType.DateTimeField)
                        oDoc.SetParameterValue(i, ConvertToDateTime(sVal));
                    else
                        oDoc.SetParameterValue(i, ConvertToNumeric(sVal));
                }
                catch (Exception e)
                {
                    //string s = e.Message;
                    //string t = s;  /* Removed Not used - TFS# 404744 - Ger */
                    throw e;
                }
            }
        }

        public bool ConvertToBool(string sVal)
        {
            bool ret = false;
            sVal = sVal.ToUpper();
            if (sVal == "TRUE" || sVal == "Y" || sVal == "1")
                ret = true;

            return ret;
        }

        public decimal ConvertToNumeric(string sVal)
        {
            decimal ret = 0;
            decimal.TryParse(sVal, out ret);
            return ret;
        }

        protected DateTime ConvertToDateTime(string sValue)
        {
            DateTime oRet = DateTime.Now;
            DateTime.TryParse(sValue, out oRet);
            return oRet;
        }

        public string GetParamValue(string sParam)
        {
            if (sParam.Substring(0, 1) == "?")
                sParam = sParam.Substring(1);
            if (sParam.Substring(0, 1) == "@")
                sParam = sParam.Substring(1);

            sParam = sParam.ToUpper();
            sParam.Replace(" ", string.Empty);

            string sRet;

            // Some of these values are 'known' and we hard code for them
            if (sParam == "SHOWWATERMARK")
                sRet = "N";

            else if (sParam == "TESTFLAG")
                sRet = "N";

            else if (sParam == "CLIENTSTDTZ")
                sRet = "Central Standard Time";

            else if (sParam == "SESSIONID")
                sRet = "0";

            else if (sParam == "ORGANIZATIONID" ||
                sParam == "ORGID" ||
                sParam == "ORGANIZATIONNAME" ||
                sParam == "ORGANIZATION")
                sRet = TxtOrgId.Text;

            else if (sParam == "SITEID" ||
                sParam == "SITE_ID" ||
                sParam == "SITEDE" ||
                sParam == "SITE_DE")
                sRet = TxtSiteId.Text;

            else if (sParam == "USERNAME" ||
                sParam == "PRINTEDBY")
                sRet = ConfigurationManager.AppSettings["applicationusername"];

            else if (sParam == "USERID" ||
                sParam == "TASKOWNERID")
                sRet = ConfigurationManager.AppSettings["applicatoinUID"];

            else if (sParam == "PATIENTID")
                sRet = ConfigurationManager.AppSettings["patientID"];

            else if (sParam == "ITEMID" ||
                sParam == "PATIENTLISTID" ||
                sParam == "ACTIVITYHEADERID" ||
                sParam == "PRTHOLDKEY" ||
                sParam == "ENCOUNTERID" ||
                sParam == "ID" ||
                sParam == "GROUPDE" ||
                sParam == "DOCUMENTID")
                sRet = ConfigurationManager.AppSettings["ItemId"];

            else if (sParam == "FILTER")
            {
                var sFilter = "";
                sRet = " " + sFilter + " ";
            }

            else if (sParam == "SORTORDER" ||
                sParam == "SORT" ||
                sParam == "PRIMARYSORT")
            {
                var csSortOrder = "";
                sRet = csSortOrder.ToUpper() != "SORTORDER NOT FOUND" ? csSortOrder : string.Empty;
            }

            else if (sParam == "SORTDIRECTION" ||
                sParam == "TASKVIEW" ||
                sParam == "PATLISTNAME" ||
                sParam == "SECONDARYSORT")
                sRet = "";
            else if (sParam == "MODE" ||
                sParam == "EXCLUDEHMPROBLEM" ||
                sParam == "DUMMY" ||
                sParam == "BWIP" || sParam == "ITEMTYPE")
                sRet = "";

            else // Everything else should be found in the XML within the job request
                throw new Exception("param missing : " + sParam);


            // We need to plug in something for these dates or many reports will bomb
            if (sRet.Length == 0 &&
                (sParam == "BEGINDATE" ||
                sParam == "ENDDATE"))
            {
                sRet = DateTime.Now.ToShortDateString();
            }

            return sRet;
        }

        private void BtnValidatePrinter_Click(object sender, EventArgs e)
        {
            GetPrinterInformation(TxtPrinterName.Text);
        }

        private void GetPrinterInformation(String printerName)
        {
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection coll = searcher.Get();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Printer diagnosis information");
            foreach (ManagementObject printer in coll)
            {
                foreach (PropertyData property in printer.Properties)
                {
                    Object propvalue = property.Value;
                    if (property.Value != null)
                    {
                        Type valueType = property.Value.GetType();
                        if (valueType.IsArray)
                        {
                            propvalue = String.Empty;
                            if (valueType.Name == "UInt16[]")
                            {
                                UInt16[] val = (UInt16[])property.Value;
                                foreach(var value in val)
                                {
                                    propvalue += value + ", ";
                                }
                            }
                            else if(valueType.Name == "String[]")
                            {
                                String[] val = (String[])property.Value;
                                foreach (var value in val)
                                {
                                    propvalue += value + ", ";
                                }
                            }
                            //foreach(var a in property.Value.)
                            //sb.AppendLine(string.Format("{0}: {1}", property.Name, string.Join(", ", property.Value.Select(v => v.ToString()))));
                        }
                    }
                    sb.AppendLine(string.Format("{0}: {1}", property.Name, propvalue));
                }
            }
            Log(sb.ToString());
        }
    }
}
