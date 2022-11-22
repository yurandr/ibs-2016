using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Navigation;
using StringTable = IbsCs.Properties.Resources;

namespace IbsCs
{
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
            string aboutInformation;
            try
            {
                Assembly app = Assembly.GetExecutingAssembly();
            	AssemblyTitleAttribute title = (AssemblyTitleAttribute)app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
                AssemblyProductAttribute product = (AssemblyProductAttribute)app.GetCustomAttributes(typeof(AssemblyProductAttribute),false)[0];
                AssemblyCopyrightAttribute copyright = (AssemblyCopyrightAttribute)app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
                AssemblyCompanyAttribute company = (AssemblyCompanyAttribute)app.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0];
                AssemblyDescriptionAttribute description = (AssemblyDescriptionAttribute)app.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];
                Version version = app.GetName().Version;

                aboutInformation = string.Format(StringTable.AboutBoxInfo,
                    copyright.Copyright,
                    title.Title,
                    version.ToString());

                if (!string.IsNullOrWhiteSpace(description.Description))
                    aboutInformation += string.Format(StringTable.AboutBoxDescription, description.Description);
            }
            catch (System.Exception ex)
            {
                aboutInformation = ex.Message;
            }
            textBoxAbout.Text = aboutInformation;
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
