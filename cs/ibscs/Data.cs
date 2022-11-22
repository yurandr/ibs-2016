using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using StringTable = IbsCs.Properties.Resources;
using Microsoft.VisualBasic.FileIO;

namespace IbsCs
{
    public class Data : INotifyPropertyChanged
    {
        #region INotifyProperty
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        string _surname = string.Empty;
        public string Surname
        {
            get { return _surname; }
            set { _surname = value; OnPropertyChanged("Surname"); }
        }

        string _country = StringTable.CountryRu;
        public string Country
        {
            get { return _country; }
            set { _country = value; OnPropertyChanged("Country"); }
        }

        string _icq = string.Empty; // icq number often contain blanks, like "NNN NNN NNN" let's keep it as-is
        public string Icq
        {
            get { return _icq; }
            set { _icq = value; OnPropertyChanged("Icq"); }
        }

        ObservableCollection<FoodItem> _foodItems = new ObservableCollection<FoodItem>();
        public ObservableCollection<FoodItem> FoodItems
        {
            get { return _foodItems; }
        }

        List<string> _countries = new List<string>() { StringTable.CountryRu, StringTable.CountryZwe, StringTable.CountryOther };
        public List<string> Countries
        {
            get { return _countries; }
        }

        public Data()
        {
            // nothing to do here at the moment
        }
        public Data(string path)
        {   // this constructor is for CSV loading
            using (var parser = new TextFieldParser(path))  // hope I can use this .net component, in opposite case I can invent my own parser, or use 3rd side library
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = false;
                if (parser.PeekChars(1) != null)
                {
                    var fields = parser.ReadFields();
                    int index = 0;
                    if (fields.Count() > index)
                        Name = fields[index];
                    ++index;
                    if (fields.Count() > index)
                        Surname = fields[index];
                    ++index;
                    if (fields.Count() > index)
                    {   // some special handling for country item
                        var value = fields[index];
                        if (!Countries.Exists( o => 0 == string.Compare(o,value)))
                            value = StringTable.CountryOther;
                        Country = value;
                    }
                    ++index;
                    if (fields.Count() > index)
                        Icq = fields[index];
                    ++index;
                    for (int i = index; i < fields.Count(); ++i)
                    {
                        this.FoodItems.Add(new FoodItem(fields[i]));
                    }
                }
            }
        }
        string StringToCsvString(string src)
        {   // enfold special keys with quotes
            string result = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                result = src;
                if (-1 != result.IndexOfAny(new Char[] { '"', ',', ';', '\r', '\n' }))
                    result = string.Format("\"{0}\"", result.Replace("\"", "\"\""));
            }
            return result;
        }
        public string ToCsvString()
        {
            var parts = new List<string>();
            parts.Add(StringToCsvString(Name));
            parts.Add(StringToCsvString(Surname));
            parts.Add(StringToCsvString(Country));
            parts.Add(StringToCsvString(Icq));
            foreach (var fi in FoodItems)
                parts.Add(StringToCsvString(fi.Title));
            return string.Join(",",parts);
        }
        public string ToUrlString()
        {
            return System.Web.HttpUtility.UrlEncode(ToCsvString());
        }
    }

    public class FoodItem : INotifyPropertyChanged
    {
        #region INotifyProperty
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        public FoodItem() { }
        public FoodItem(string t) { Title = t; }
    }
}
