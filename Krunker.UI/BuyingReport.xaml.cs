using ConsoleAppDataBSela.Model;
using Krunker.BL.Service;
using Krunker.Common;
using Krunker.DAL.Repository;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using static Krunker.BL.Service.Service;


namespace Krunker.UI
{

    public sealed partial class BuyingReport : Page
    {
        public BuyingReport()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataGrid.ItemsSource = e.Parameter as List<ShoppingCartItems>;
            base.OnNavigatedTo(e);
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void DataGridGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.PropertyName == "items") e.Cancel = true;
        }
    }
}
