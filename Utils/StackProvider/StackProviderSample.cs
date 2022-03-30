using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Samples.Debugging.CorSymbolStore;

namespace StackProvider
{
    public partial class StackProviderSample : Form
    {
        public StackProviderSample()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK)
                txtAssembliesFolder.Text = d.SelectedPath;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            txtOutput.Text = ProcessErrorInfo(txtInput.Lines.ToList(), txtAssembliesFolder.Text);
        }

        private string ProcessErrorInfo(IList<string>lines, string assembliesFolder)
        {
            StackTraceSymbolProvider symbolProvider =
                new StackTraceSymbolProvider("", SymSearchPolicies.AllowReferencePathAccess);

            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    continue;
                }
                 
                string moduleName;
                int ilOffset;
                int methodMetatdataOffset;
                if (ExtractSymbolInfo(line, out moduleName, out ilOffset, out methodMetatdataOffset))
                {
                    string info = symbolProvider.GetSourceLoc(Path.Combine(assembliesFolder, moduleName), methodMetatdataOffset, ilOffset);
                    if (!string.IsNullOrEmpty(info))
                        sb.AppendLine(line.Substring(0, line.LastIndexOf("[")) + " in " + info);
                    else
                    {
                        sb.AppendLine(line.Substring(0, line.LastIndexOf("[")));
                    }
                }
            }
            return sb.ToString();
        }

        private bool ExtractSymbolInfo(string line, out string name, out int ilOffset, out int methodMetadataOffset)
        {
            name = string.Empty; ilOffset = 0;
            methodMetadataOffset = 0;

            line = line.Trim();
            string symbolinfo = line.Substring(line.LastIndexOf("[") + 1, line.Length - line.LastIndexOf("[")-2);
            string[] symbols = symbolinfo.Split(new[] { ',' });
            if (symbols.Length != 3)
                return false;
            else
            {
                name = symbols[0];
                ilOffset = int.Parse(symbols[1]);
                methodMetadataOffset = int.Parse(symbols[2]);
            }
            return true;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
        }
    }
}
