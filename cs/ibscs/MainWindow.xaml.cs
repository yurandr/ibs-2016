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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StringTable = IbsCs.Properties.Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IbsCs
{
    public partial class MainWindow : Window
    {
        #region DataProperty
        public static readonly DependencyProperty DataProperty
            = DependencyProperty.Register("Data", typeof(Data), typeof(MainWindow)
            , new PropertyMetadata(new Data()));
        public Data Data
        {
            get { return GetValue(DataProperty) as Data; }
            set { SetValue(DataProperty, value); }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Owner = this;
            about.ShowDialog();
        }
        private void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog())
                {
                    dlg.Filter = StringTable.CsvFilesFilter;
                    if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
                        Data = new Data(dlg.FileName);
                }
            }
            catch (System.Exception ex)
            {
                App.ReportError(ex);
            }
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (null != Data)
                {
                    System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
                    dlg.DefaultExt = ".csv";
                    dlg.Filter = StringTable.CsvFilesFilter;
                    if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
                        System.IO.File.WriteAllText(dlg.FileName, Data.ToCsvString());
                }
            }
            catch (System.Exception ex)
            {
                App.ReportError(ex);
            }
        }
        private void OpenBrowser_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (null != Data)
                    System.Diagnostics.Process.Start("https://translate.google.com/#en/uk/" + Data.ToUrlString());
            }
            catch (System.Exception ex)
            {
                App.ReportError(ex);
            }
        }
        private void AddFoodItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != Data)
                Data.FoodItems.Add(new FoodItem());
        }
        private void RemoveFoodItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != Data)
            {
                var fi = e.Parameter as FoodItem;
                if (null != fi)
                    Data.FoodItems.Remove(fi);
            }
        }
    }

    public class CountryToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (0 == string.Compare(value as string, StringTable.CountryZwe))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IcqNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex = new Regex("[^0-9 ]+"); //regex that matches disallowed text
                if (regex.IsMatch(str))
                    return new ValidationResult(false, StringTable.ErrorIcqIncorrectCharacters);
            }
            return new ValidationResult(true, null);
        }
    }
}
