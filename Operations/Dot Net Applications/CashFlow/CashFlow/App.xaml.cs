using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CashFlow
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
            MessageBox.Show(e.Message + " StackTrace: " + e.StackTrace.ToString());
            Data d = new Data();
            d.addError(CashFlow.MainWindow.uiCurrentUser.adid, CashFlow.MainWindow.uiCurrentUser.name, CashFlow.MainWindow.currentFunction, e.Message + " StackTrace: " + e.StackTrace.ToString());
        }
    }
}
