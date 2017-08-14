using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace myWalletXAML
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletsPage : ContentPage
	{
        int WalletsValue = 0;
        ObservableCollection<string> wallets = new ObservableCollection<string>();
        public WalletsPage ()
        {
            InitializeComponent();
            App.Current.Properties.Clear();

            if (App.Current.Properties.ContainsKey("Number of wallets"))
            {
                string tempWallets;
                tempWallets = App.Current.Properties["Number of wallets"].ToString();
                int.TryParse(tempWallets, out WalletsValue);
                DisplayAlert("Test", tempWallets, "OK");
                if (WalletsValue > 0)
                {
                    for (int i = 1; i <= WalletsValue; i++)
                    {
                        while (!NewWalletButton(App.Current.Properties[$"Wallet {i}"].ToString()))
                        {

                        }
                    }
                }

            }
            else
            {

                App.Current.Properties.Add("Number of wallets", WalletsValue);
                App.Current.SavePropertiesAsync();
            }

        }

        void OnEntryWalletNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (entryWalletName.Text != null)
            {
                btnNewWallet.IsEnabled = true;
            }
        }

        async void OnNewWalletButtonClicked(object sender, EventArgs e)
        {
            if (!wallets.Contains(entryWalletName.Text))
            {
                ++WalletsValue;
                NewWalletButton(entryWalletName.Text);
                await DisplayAlert($"Wallet {WalletsValue}", entryWalletName.Text, "OK");
                App.Current.Properties.Add($"Wallet {WalletsValue}", entryWalletName.Text);
                App.Current.Properties["Number of wallets"] = WalletsValue;
                await App.Current.SavePropertiesAsync();
            }
            else
            {
                await DisplayAlert("Wallet exists!", $"Wallet of name: {entryWalletName.Text} already exists.", "OK");
            }
            
        }

        bool NewWalletButton(string walletName)
        {
            var button = new Button
            {
                Text = $"Wallet {walletName}",
                ClassId = $"Wallet {WalletsValue}",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            button.Clicked += OnOpenWalletButtonClicked;
            walletsStack.Children.Add(button);
            wallets.Add(walletName);
            return true;
        }


        async void OnOpenWalletButtonClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if(btn.ClassId == "Wallet 1")
            {
                await DisplayAlert("Button 1 clicked!", "Page 1", "OK");
                
            }
            else if(btn.ClassId == "Wallet 2")
            {
                await DisplayAlert("Button 2 clicked!", "Page 2", "OK");
            }
            //await Navigation.PushAsync(new newWalletListPage(btn.ClassId));
            await Navigation.PushAsync(new newWalletListPage(btn.ClassId));
        }
		
    }
}