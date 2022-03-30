using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Infragistics.Win.UltraWinListView;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LeakDetector
{
    public partial class Form1 : Form
    {
        [DllImport("User32")]
        extern public static int GetGuiResources(IntPtr hProcess, int uiFlags);
        List<Type> Assemblies = new List<Type>();
        const Int32 INTANCE_ITERATIONS = 1000;
        public Form1()
        {
            InitializeComponent();
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            
            ListAssembly.Items.Clear();

            var q = from t in Assembly.LoadFrom(@"C:\TFSCode\11.2.3\Web\Works.NET\Applications.TestHarness\bin\Debug\Allscripts.TouchWorks.Common.dll").GetTypes()
                    where t.IsClass && t.IsPublic && !t.IsAbstract
                    select t;
            q.ToList().ForEach(t => Assemblies.Add(t));

            
            
            //var dups = Assemblies.Where(x => Assemblies.Any(y => y != x));
            //dups.ToList().ForEach(t => ListAssembly.Items.Add(t.));
            Assemblies = Assemblies.OrderBy(t => t.Name).ToList();
            Assemblies.ForEach(t => ListAssembly.Items.Add(t.FullName, t.Name));

            
           
            
            //MessageBox.Show("Finished");
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            foreach (UltraListViewItem Item in ListAssembly.CheckedItems)
            {
                if (Item.CheckState == CheckState.Checked)
                {
                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    Int32 GDIObject = Form1.GetGuiResourcesGDICount();
                    Int32 UserObject = Form1.GetGuiResourcesUserCount();
                    Int64 BeforeExecution = System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64;
                    label1.Text = string.Format("{0:0.00}kb", BeforeExecution / 1048576);
                    label4.Text = string.Format("{0}", GDIObject);
                    label6.Text = string.Format("{0}", UserObject);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    for (Int32 i = 0; i < Form1.INTANCE_ITERATIONS; i++)
                    {
                        Object obj = Activator.CreateInstance(Assemblies.Find(t => t.FullName == Item.Key));
                        if (obj is IDisposable)
                        {
                            ((IDisposable)obj).Dispose();
                        }
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    //BeforeExecution = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    BeforeExecution = System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64;
                    GDIObject = Form1.GetGuiResourcesGDICount();
                    UserObject = Form1.GetGuiResourcesUserCount();
                    label2.Text = string.Format("{0:0.00}kb", BeforeExecution / 1048576);
                    label3.Text = string.Format("{0}", GDIObject);
                    label5.Text = string.Format("{0}", UserObject);
                }
            }
        }


        public static int GetGuiResourcesGDICount()
        {
            return GetGuiResources(Process.GetCurrentProcess().Handle, 0);
        }

        public static int GetGuiResourcesUserCount()
        {
            return GetGuiResources(Process.GetCurrentProcess().Handle, 1);
        }

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            MessageBox.Show(string.Format("{0:0.00}k", System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64 / 1024));
        }
    }
}
