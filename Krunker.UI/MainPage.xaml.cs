using ConsoleAppDataBSela.Model;
using Krunker.BL.Service;
using Krunker.Common;
using Krunker.DAL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Krunker.UI
{
    public sealed partial class MainPage : Page
    {
        readonly DataService service;
        readonly Data data;
        public MainPage()
        {
            this.InitializeComponent();
            service = DataService.Instance;
            data = new Data();
            InitializeItems();
            service.OutOfStockEventHandler += Service_OutOfStockEventHandler;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Create all Comboxes for user to choose items
        /// </summary>
        public void InitializeItems()
        {
            ShopListLv.Items.Add(BuildComboBox<PrimaryWeapon>());
            ShopListLv.Items.Add(BuildComboBox<SecondaryWeapon>());
            ShopListLv.Items.Add(BuildComboBox<BackItem>());
            ShopListLv.Items.Add(BuildComboBox<HeadItem>());

            ComboBox BuildComboBox<T>() where T : AbstractItem
            {
                ComboBox b = new ComboBox
                {
                    Width = 300,
                    Header = typeof(T).Name,
                    FontSize = 33,
                    ItemsSource = DataService.Instance.GetItems<T>()
                };
                b.PointerEntered += B_PointerEntered;
                b.SelectionChanged += B_SelectionChanged;
                return b;
            }
        }
        /// <summary>
        /// When occurs one of the items is out of stock,User can't see this item anymore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Service_OutOfStockEventHandler(object sender, AbstractItem e)
        {
            var FuncByType = new Dictionary<Type, Action<AbstractItem>> {
              { typeof(HeadItem), ai => (ShopListLv.Items[3] as ComboBox).ItemsSource = service.GetItems<HeadItem>().Where(x=> x.CurrentAmout>0) },
              { typeof(BackItem), ai => (ShopListLv.Items[2] as ComboBox).ItemsSource = service.GetItems<BackItem>().Where(x=> x.CurrentAmout>0) },
              { typeof(PrimaryWeapon), ai => (ShopListLv.Items[0] as ComboBox).ItemsSource = service.GetItems<PrimaryWeapon>().Where(x=> x.CurrentAmout>0) },
              { typeof(SecondaryWeapon),ai => (ShopListLv.Items[1] as ComboBox).ItemsSource = service.GetItems<SecondaryWeapon>().Where(x=> x.CurrentAmout>0) }
             };
            FuncByType[e.GetType()]?.Invoke(e);
        }
        /// <summary>
        /// Adds chosen item to cart,dislpay cart and price
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
                DataService.Instance.AddToCart((sender as ComboBox).SelectedItem as AbstractItem);
            ItemsTbl.Text = DataService.Instance.GetCartItems();
            ToPayTbl.Text = $"{DataService.Instance.CalculateCart():C}";
        }
        /// <summary>
        /// User sees the combobox that the mouse is on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ShopListItems.ItemsSource = (sender as ComboBox).ItemsSource;
        }
        /// <summary>
        /// Admin sees only items from headitem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeadCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = DataService.Instance.GetItems<HeadItem>();
        }
        /// <summary>
        ///  Admin sees only items from backitem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackCategoty_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = DataService.Instance.GetItems<BackItem>();
        }
        /// <summary>
        ///  Admin sees only items from primaryweapon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrimaryWeaponCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = DataService.Instance.GetItems<PrimaryWeapon>();
        }
        /// <summary>
        ///  Admin sees only items from secondaryweapon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondaryWeaponCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = DataService.Instance.GetItems<SecondaryWeapon>();
        }
        /// <summary>
        ///  Admin sees all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = DataService.Instance.GetAllItems();
        }
        /// <summary>
        /// Check if cart is empty when user try to pay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPayClick(object sender, RoutedEventArgs e)
        {
            if (ItemsTbl.Text == "")
                NoItemsInCartDialog();
            else DisplayPayDialog();

        }
        /// <summary>
        /// ContentDialog for the customer to confirm purchase
        /// </summary>
        private async void DisplayPayDialog()
        {
            ContentDialog payDialog = new ContentDialog
            {
                Title = "Thanks for buying at Michael's Krunker Store",
                Content = $"Amount To Pay {ToPayTbl.Text:C}\nThe Selected Items:\n{DataService.Instance.GetCartItems()}",
                PrimaryButtonText = "Pay For Real",
                CloseButtonText = "Regret"
            };

            ContentDialogResult result = await payDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

                DataService.Instance.CartCheckout();
                ItemsTbl.Text = "";
                foreach (var item in ShopListLv.Items)
                    (item as ComboBox).SelectedItem = null;

                InventoryLst.ItemsSource = null;
                InventoryLst.ItemsSource = DataService.Instance.GetAllItems();

                data.CreateJson();
                data.WriteToJson(DataService.Instance.GetAllItems(), DataService.Instance.CartItems);
            }
            else DisplayPayDialog();
        }
        /// <summary>
        /// ContentDialog for empty cart
        /// </summary>
        private async void NoItemsInCartDialog()
        {
            ContentDialog payDialog = new ContentDialog
            {
                Title = "No Items In cart",
                CloseButtonText = "Close Me"
            };
            await payDialog.ShowAsync();
        }
        /// <summary>
        /// Admin can choose a discount to give for any category of items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiscountClick(object sender, RoutedEventArgs e)
        {
            if ((RadioBurronLSV.SelectedItem as TextBlock) != null)
                switch ((RadioBurronLSV.SelectedItem as TextBlock).Name)
                {
                    case "AllCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetAllItems();
                            for (int i = 0; i < DataService.Instance.GetAllItems().Count; i++)
                            {
                                DataService.Instance.GetAllItems()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "PrimaryWeaponCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<PrimaryWeapon>();
                            for (int i = 0; i < DataService.Instance.GetItems<PrimaryWeapon>().Count; i++)
                            {
                                DataService.Instance.GetItems<PrimaryWeapon>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "SecondaryWeaponCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<SecondaryWeapon>();
                            for (int i = 0; i < DataService.Instance.GetItems<SecondaryWeapon>().Count; i++)
                            {
                                DataService.Instance.GetItems<SecondaryWeapon>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "HeadCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<HeadItem>();
                            for (int i = 0; i < DataService.Instance.GetItems<HeadItem>().Count; i++)
                            {
                                DataService.Instance.GetItems<HeadItem>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "BackCategoty":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<BackItem>();
                            for (int i = 0; i < DataService.Instance.GetItems<BackItem>().Count; i++)
                            {
                                DataService.Instance.GetItems<BackItem>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                }
            ShopListLv.Items.Clear();
            InitializeItems();
        }
        /// <summary>
        /// When item out stock,Admin can renew the stock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RenewStockBtn(object sender, RoutedEventArgs e)
        {
            if ((InventoryLst.SelectedItem as AbstractItem).CurrentAmout == 0)
                (InventoryLst.SelectedItem as AbstractItem).CurrentAmout = 5;
            else
            {
                ContentDialog payDialog = new ContentDialog
                {
                    Title = "Item stock Isn't Finished, Can't Restock",
                    CloseButtonText = "Close Me"
                };
                await payDialog.ShowAsync();
            }

            (ShopListLv.Items[0] as ComboBox).ItemsSource = DataService.Instance.GetItems<PrimaryWeapon>();
            (ShopListLv.Items[1] as ComboBox).ItemsSource = DataService.Instance.GetItems<SecondaryWeapon>();
            (ShopListLv.Items[2] as ComboBox).ItemsSource = DataService.Instance.GetItems<BackItem>();
            (ShopListLv.Items[3] as ComboBox).ItemsSource = DataService.Instance.GetItems<HeadItem>();

        }
        /// <summary>
        /// Navigates to report page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToReport_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BuyingReport), DataService.Instance.CartItems);
        }
        /// <summary>
        /// initialization function for the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (List<AbstractItem> inventory, List<ShoppingCartItems> shoppingCartItems) = data.ReadFromJson();

            if (shoppingCartItems != null)
            {
                DataService.Instance.CartItems.Clear();
                DataService.Instance.CartItems.AddRange(shoppingCartItems);
            }

            if (inventory != null)
            {
                DataService.Instance.ResetInventory(inventory);
            }


        }
    }
}