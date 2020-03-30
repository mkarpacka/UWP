using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Zadanie1
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new DataStoreViewModel();
        }
        public DataStoreViewModel ViewModel
        {
            get; set;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ClearHistory();
        }
    }
    public class DataStoreViewModel : INotifyPropertyChanged
    {
        private string fname { get; set; }
        private string lname { get; set; }

        private string lifeHistory;

        Windows.Storage.ApplicationDataContainer localSettings;
        Windows.Storage.ApplicationDataCompositeValue composite;

        private static DataStoreViewModel myInstance;
        public static DataStoreViewModel getInstance()
        {
            return myInstance;
        }
        public DataStoreViewModel()
        {
            myInstance = this;
            fname = "UWP";
            lname = "User";
            lifeHistory = "";
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            composite = (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values["DataBindingViewModel"];
            if (composite == null)
            {
                composite = new Windows.Storage.ApplicationDataCompositeValue();
                StoreLocalSettings();
            }
            else
            {
                fname = (String)composite["fname"];
                lname = (String)composite["lname"];
                lifeHistory = (String)composite["lifeHistory"];
            }
        }

        public void StoreLocalSettings()
        {
            composite["fname"] = fname;
            composite["lname"] = lname;
            composite["lifeHistory"] = lifeHistory;
            localSettings.Values["DataBindingViewModel"] = composite;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FName
        {
            get { return this.fname; }
            set
            {
                this.fname = value;
                Summary = "";
                StoreLocalSettings();
                this.OnPropertyChanged();
            }
        }

        public string LName
        {
            get { return this.lname; }
            set
            {
                this.lname = value;
                Summary = "";
                StoreLocalSettings();
                this.OnPropertyChanged();
            }
        }

        /*IDictionary<string, int> dict = new Dictionary<string, int>(){
            {"launched", 1},
            {"restored", 2},
            {"suspended", 3},
            {"resumed", 4}
        };

        public string LifeHistory
        {
            get { return $"Life History: { this.lifeHistory}"; }
            set
            {
                if (!dict.ContainsKey(value)){
                  //  System.Diagnostics.Debug.WriteLine("debug");
                    this.lifeHistory = "";
                }
                else
                {
                    this.lifeHistory += " " + dict[value];
                }

                StoreLocalSettings();
                this.OnPropertyChanged();

            }
        }*/

        public string LifeHistory
        {
            get
            {
                return $"LifeHistory: {this.lifeHistory}";
            }
            set
            {
                if (value.CompareTo("launched") == 0)
                {
                    this.lifeHistory += " 1";
                }
                else if (value.CompareTo("restored") == 0)
                {
                    this.lifeHistory += " 2";
                }
                else if (value.CompareTo("suspended") == 0)
                {
                    this.lifeHistory += " 3";
                }
                else if (value.CompareTo("resumed") == 0)
                {
                    this.lifeHistory += " 4";
                }
                if (value.CompareTo("Clear") == 0)
                {
                    this.lifeHistory = "";
                }
                StoreLocalSettings();
                this.OnPropertyChanged();
            }
        }

        public void ClearHistory()
        {
            LifeHistory = "Clear";
        }

        public string Summary
        {
            get { return $"Hello, {this.FName} {this.LName}"; }
            set { this.OnPropertyChanged(); }
        }
    }
}
