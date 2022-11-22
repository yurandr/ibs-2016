using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StringTable = IbsCs.Properties.Resources;

namespace IbsCs
{
    public partial class App : Application
    {
        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format(StringTable.UnhandledException, e.Exception.Message), StringTable.MessageBoxErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);            
        }
        public static void ReportError(System.Exception ex)
        {
            MessageBox.Show(ex.ToLongString(), StringTable.MessageBoxErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error );
        }
    }

    public class Commands
    {
        public static RoutedUICommand About = new RoutedUICommand(StringTable.CmdAboutText, "About", typeof(Commands));
        public static RoutedUICommand Save = new RoutedUICommand(StringTable.CmdSaveText, "Save", typeof(Commands));
        public static RoutedUICommand Load = new RoutedUICommand(StringTable.CmdLoadText, "Load", typeof(Commands));
        public static RoutedUICommand OpenBrowser = new RoutedUICommand(StringTable.CmdOpenBrowserText, "OpenBrowser", typeof(Commands));
        public static RoutedUICommand AddFoodItem = new RoutedUICommand(StringTable.CmdAddFoodItemText, "AddFoodItem", typeof(Commands));
        public static RoutedUICommand RemoveFoodItem = new RoutedUICommand(StringTable.CmdRemoveFoodItemText, "RemoveFoodItem", typeof(Commands));
    }

    static class Extensions
    {
        public static string ToLongString(this Exception ex)
        {
            List<string> items = new List<string>();
            items.Add(ex.Message);
            if (null != ex.InnerException)
                items.Add(ex.InnerException.ToString());
            return string.Join("\n", items);
        }
    }
}
