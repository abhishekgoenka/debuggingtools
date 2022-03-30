#region

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Fiddler;

#endregion

[assembly: RequiredVersion("2.3.5.0")]

namespace FiddlerEEHRRequestDecrypter
{
    public class DecryptResponse : IAutoTamper
    {
        private Boolean decryptAllEEEHRRequest;
        private const String CONTEXT_TEXT = "Decrypt EEHR request";
        private static readonly Object _lock = new Object();

        public void AutoTamperRequestAfter(Session oSession)
        {
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            if (decryptAllEEEHRRequest)
            {
                lock (_lock)
                {
                    oSession.oRequest.headers.RequestPath =
                        oSession.oRequest.headers.RequestPath.Replace("SerializationFormat=BINARY",
                            "SerializationFormat=XML");

                    oSession.oRequest.headers.RequestPath =
                        oSession.oRequest.headers.RequestPath.Replace("action=load,Compressed1", String.Empty);
                }
            }
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
        }

        public void OnBeforeReturningError(Session oSession)
        {
        }

        public void OnBeforeUnload()
        {
        }

        public void OnLoad()
        {
            MenuItem menuItem2 = new MenuItem(CONTEXT_TEXT);
            menuItem2.Click += oMI2_Click;
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, menuItem2);
        }

        private void oMI2_Click(object sender, EventArgs e)
        {
            decryptAllEEEHRRequest = !decryptAllEEEHRRequest;
            foreach (MenuItem item in FiddlerApplication.UI.mnuSessionContext.MenuItems)
            {
                if (item.Text == CONTEXT_TEXT)
                {
                    item.Checked = decryptAllEEEHRRequest;
                    break;
                }
            }
        }
    }
}