using Krunker.Common;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Krunker.UI
{

    public sealed partial class BuyingReport : Page
    {
        public BuyingReport()
        {
            this.InitializeComponent();
        }
        // Recieves data when navagated to
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataGrid.ItemsSource = e.Parameter as List<ShoppingCartItems>;
            base.OnNavigatedTo(e);
        }
        // naviga back to main page
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
