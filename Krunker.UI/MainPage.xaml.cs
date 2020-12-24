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
        Service service;
        Data data;
        public MainPage()
        {
            this.InitializeComponent();
            service = Service.Instance;
            data = new Data();
            InitializeItems();
            service.OutOfStockEventHandler += Service_OutOfStockEventHandler;
            NavigationCacheMode = NavigationCacheMode.Enabled;
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
                ComboBox b = new ComboBox();
                b.Width = 300;
                b.Header = typeof(T).Name;
                b.FontSize = 33;
                b.ItemsSource = Service.Instance.GetItems<T>();
                b.PointerEntered += B_PointerEntered;
                b.SelectionChanged += B_SelectionChanged;
                return b;
            }
        }
        /// <summary>
        /// Adds chosen item to cart,dislpay cart and price
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
                Service.Instance.AddToCart((sender as ComboBox).SelectedItem as AbstractItem);
            ItemsTbl.Text = Service.Instance.GetCartItems();
            ToPayTbl.Text = $"{Service.Instance.CalculateCart():C}";
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
            InventoryLst.ItemsSource = Service.Instance.GetItems<HeadItem>();
        }
        /// <summary>
        ///  Admin sees only items from backitem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackCategoty_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = Service.Instance.GetItems<BackItem>();
        }
        /// <summary>
        ///  Admin sees only items from primaryweapon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrimaryWeaponCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = Service.Instance.GetItems<PrimaryWeapon>();
        }
        /// <summary>
        ///  Admin sees only items from secondaryweapon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondaryWeaponCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = Service.Instance.GetItems<SecondaryWeapon>();
        }
        /// <summary>
        ///  Admin sees all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllCategory_Checked(object sender, RoutedEventArgs e)
        {
            InventoryLst.ItemsSource = Service.Instance.GetAllItems();
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
                Content = $"Amount To Pay {ToPayTbl.Text:C}\nThe Selected Items:\n{Service.Instance.GetCartItems()}",
                PrimaryButtonText = "Pay For Real",
                CloseButtonText = "Regret"
            };

            ContentDialogResult result = await payDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

                Service.Instance.CartCheckout();
                ItemsTbl.Text = "";
                foreach (var item in ShopListLv.Items)
                    (item as ComboBox).SelectedItem = null;

                InventoryLst.ItemsSource = null;
                InventoryLst.ItemsSource = Service.Instance.GetAllItems();

                data.CreateJson();
                data.WriteToJson(Service.Instance.GetAllItems(), Service.Instance.cartItems);
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
                            for (int i = 0; i < Service.Instance.GetAllItems().Count; i++)
                            {
                                Service.Instance.GetAllItems()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "PrimaryWeaponCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<PrimaryWeapon>();
                            for (int i = 0; i < Service.Instance.GetItems<PrimaryWeapon>().Count; i++)
                            {
                                Service.Instance.GetItems<PrimaryWeapon>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "SecondaryWeaponCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<SecondaryWeapon>();
                            for (int i = 0; i < Service.Instance.GetItems<SecondaryWeapon>().Count; i++)
                            {
                                Service.Instance.GetItems<SecondaryWeapon>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "HeadCategory":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<HeadItem>();
                            for (int i = 0; i < Service.Instance.GetItems<HeadItem>().Count; i++)
                            {
                                Service.Instance.GetItems<HeadItem>()[i].Discount = double.Parse(DiscountNumTbx.Text);
                            }
                        }
                        break;
                    case "BackCategoty":
                        {
                            //List<AbstractItem> items = Service.Instance.GetItems<BackItem>();
                            for (int i = 0; i < Service.Instance.GetItems<BackItem>().Count; i++)
                            {
                                Service.Instance.GetItems<BackItem>()[i].Discount = double.Parse(DiscountNumTbx.Text);
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

            (ShopListLv.Items[0] as ComboBox).ItemsSource = Service.Instance.GetItems<PrimaryWeapon>();
            (ShopListLv.Items[1] as ComboBox).ItemsSource = Service.Instance.GetItems<SecondaryWeapon>();
            (ShopListLv.Items[2] as ComboBox).ItemsSource = Service.Instance.GetItems<BackItem>();
            (ShopListLv.Items[3] as ComboBox).ItemsSource = Service.Instance.GetItems<HeadItem>();

        }

        private void GoTOReport_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BuyingReport), Service.Instance.cartItems);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            (List<AbstractItem> inventory, List<ShoppingCartItems> shoppingCartItems) = data.ReadFromJson();

            if (shoppingCartItems != null)
            {
                Service.Instance.cartItems.Clear();
                Service.Instance.cartItems.AddRange(shoppingCartItems);
            }

            if (inventory != null)
            {
                Service.Instance.ResetInventory(inventory);
                //for (int i = 0; i < inventory.Count; i++)
                //{
                //    inventory[i].Discount = 0;
                //}
            }


        }
    }
}