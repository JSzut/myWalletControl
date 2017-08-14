using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace myWalletXAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletsListPage : ContentPage
    {
        int WalletsValue = 0;
        public ObservableCollection<WalletsModel> WalletsItems { get; set; }

        public WalletsListPage()
        {
            InitializeComponent();
            //App.Current.Properties.Clear();
            WalletsItems = new ObservableCollection<WalletsModel>();

            if (App.Current.Properties.ContainsKey("Number of wallets"))
            {
                string tempWallets;
                tempWallets = App.Current.Properties["Number of wallets"].ToString();
                int.TryParse(tempWallets, out WalletsValue);
                if (WalletsValue > 0)
                {
                    for (int i = 1; i <= WalletsValue; i++)
                    {
                        while (!NewWallet(i,App.Current.Properties[$"Wallet {i}"].ToString()))
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

            lsWallets.ItemsSource = WalletsItems;
        }
        
        async void OnWalletOpenClicked(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            int tempID = -1;
            for(int i = 0; i < WalletsItems.Count; i++)
            {
                if(WalletsItems[i].Name == btn.Text)
                {
                    tempID = i;
                }
            }
            if(tempID != -1)
            {
                await Navigation.PushAsync(new newWalletListPage($"Wallet {WalletsItems[tempID].ID}"));
            }
            
        }

        bool NewWallet(int id, string name)
        {
            WalletsItems.Add(new WalletsModel { ID = id, Name = name});
            return true;
        }

        async void OnNewWalletButtonClicked(object sender, EventArgs e)
        {
            bool walletExists = false;
            if(WalletsValue > 0)
            {
                for (int i = 0; i < WalletsItems.Count; i++)
                {
                    if (WalletsItems[i].Name == entryName.Text)
                    {
                        walletExists = true;
                        await DisplayAlert("Wallet exists!", $"Wallet of name: {entryName.Text} already exists.", "OK");
                        break;
                    }
                }
            }

            if (!walletExists)
            {
                ++WalletsValue;
                NewWallet(WalletsValue, entryName.Text);
                await DisplayAlert($"Wallet {WalletsValue}", $"Wallet {entryName.Text} added", "OK");
                if(!App.Current.Properties.ContainsKey($"Wallet {WalletsValue}"))
                {
                    App.Current.Properties.Add($"Wallet {WalletsValue}", entryName.Text);
                }
                else
                {
                    App.Current.Properties[$"Wallet {WalletsValue}"] = entryName.Text;
                }
                App.Current.Properties["Number of wallets"] = WalletsValue;
                await App.Current.SavePropertiesAsync();
            }
            entryName.Text = null;
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var item = (Button)sender;
            int tempID = 0;
            string answer;
            int.TryParse(item.CommandParameter.ToString(), out tempID);
            answer = await DisplayActionSheet("Are you sure you want to delete?", null, null, "Yes", "No");
            if (answer == "Yes")
            {
                //await DisplayAlert("Test", $"Wallet {tempID}", "OK");
                WalletsItems.RemoveAt(tempID-1);
                App.Current.Properties.Remove($"Wallet {tempID-1}");
                --WalletsValue;
                App.Current.Properties["Number of wallets"] = WalletsValue;
                await App.Current.SavePropertiesAsync();
            }
        }
    }
}