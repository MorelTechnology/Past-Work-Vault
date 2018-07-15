using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            MessageBox.Show(e.Message, "Unhandled Exception");

            DataTable dtException = new DataTable();
            dtException.Columns.Add("Application");
            dtException.Columns.Add("User");
            dtException.Columns.Add("AddDate");
            dtException.Columns.Add("Severity");
            dtException.Columns.Add("Title");
            dtException.Columns.Add("Message");
            dtException.Columns.Add("ExtProp1");
            dtException.Columns.Add("ExtProp2");
            dtException.Columns.Add("ExtProp3");
            dtException.Columns.Add("ExtProp4");
            dtException.Columns.Add("ExtProp5");
            dtException.TableName = "Exceptions";

            dtException.Rows.Add("OPSCONSOLE", System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now.ToShortDateString(), "Error", "Unhandled OpsConsole Exception", e.Message + " " + e.StackTrace, "", "", "", "", "");

            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtException, "INSERT_EXCEPTION", "EXCEPTIONHANDLING");
        }


    }

}
