using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace myWalletXAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class newWalletListPage : ContentPage
    {
        ObservableCollection<MoneyModel> Items { get; set; }

        int testID = 0;
        float testSum = 0;
        string localWalletID;
        


        public newWalletListPage(string walletID)
        {
            InitializeComponent();
            
            localWalletID = walletID;
            this.Title = App.Current.Properties[walletID].ToString();
            Items = new ObservableCollection<MoneyModel>();
            
            BindingContext = this;
            
            if (App.Current.Properties.ContainsKey($"{walletID} Number of items"))
            {
                string tempItems;
                tempItems = App.Current.Properties[$"{walletID} Number of items"].ToString();
                int.TryParse(tempItems, out testID);

                

                if (testID > 0)
                {
                    //DisplayAlert("Test", App.Current.Properties[$"Wallet {localWalletID} {testID} Name"].ToString(), "OK");
                    for (int i = 1; i <= testID; i++)
                    {
                        //DisplayAlert("Test", i.ToString(), "OK");
                        //while(!AddItem(i, "500", "Test", "Income"))
                        //{

                        //}
                        if(App.Current.Properties.ContainsKey($"{localWalletID} {i} Name"))
                        {
                            while (!AddItem(i, App.Current.Properties[$"{localWalletID} {i} Value"].ToString(), App.Current.Properties[$"{localWalletID} {testID} Name"].ToString(), App.Current.Properties[$"{localWalletID} {i} Type"].ToString()))
                            {

                            }
                        }
                        
                    }
                }
            }
            else
            {
                App.Current.Properties.Add($"{walletID} Number of items", testID);
                App.Current.SavePropertiesAsync();
            }
            lstEntries.ItemsSource = Items;
        }

        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (entryName.Text != null && entryValue.Text != null && picker.SelectedIndex != -1)
            {
                AddItem(++testID, entryValue.Text, entryName.Text, picker.Items[picker.SelectedIndex]);
                App.Current.Properties.Add($"{localWalletID} {testID} Name", entryName.Text);
                App.Current.Properties.Add($"{localWalletID} {testID} Value", entryValue.Text);
                App.Current.Properties.Add($"{localWalletID} {testID} Type", picker.Items[picker.SelectedIndex]);
                App.Current.Properties[$"{localWalletID} Number of items"] = testID;
                await App.Current.SavePropertiesAsync();
                entryName.Text = null;
                entryValue.Text = null;
            }
            else
            {
                await DisplayAlert("Empty values!", "Enter all required data to add field!", "Ok");
            }

        }

        bool AddItem(int id, string value, string name, string type)
        {
            float tempValue;
            Color tempColor;
            float.TryParse(value, out tempValue);
            if (type == "Income")
            {
                tempColor = Color.Green;
            }
            else
            {
                tempColor = Color.Red;
            }
            Items.Add(new MoneyModel { ID = id, Value = tempValue, Name = name, wType = type, wColor = tempColor });
            AddValueToSum(Items.Last().Value, Items.Last().wType);
            return true;
        }

        async void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void AddValueToSum(float newValue, string type)
        {
            if (type == "Income")
            {
                testSum += newValue;
            }
            else
            {
                testSum -= newValue;
            }

            if (testSum > 0)
            {
                lblSum.TextColor = Color.Green;
            }
            else
            {
                lblSum.TextColor = Color.Red;
            }

            lblSum.Text = $"Sum: {testSum}";
        }

        void OnMore(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            DisplayAlert("More Context Action", item.CommandParameter.ToString() + " more context action", "OK");
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            int tempID = 0;
            string answer;
            int.TryParse(item.CommandParameter.ToString(), out tempID);
            
            for(int i = 0; i < Items.Count(); i++)
            {
                if(Items[i].ID == tempID)
                {
                    answer = await DisplayActionSheet("Are you sure you want to delete?", null, null, "Yes", "No");
                    if (answer == "Yes")
                    {
                        if(Items[i].wType == "Income")
                        {
                            AddValueToSum(Items[i].Value, "Expense");
                        }
                        else
                        {
                            AddValueToSum(Items[i].Value, "Income");
                        }
                        Items.RemoveAt(i);
                        App.Current.Properties.Remove($"{localWalletID} {tempID} Name");
                        App.Current.Properties.Remove($"{localWalletID} {tempID} Value");
                        App.Current.Properties.Remove($"{localWalletID} {tempID} Type");
                        await App.Current.SavePropertiesAsync();
                    }
                    
                    break;
                }
            }
            //DisplayAlert("test", Items.Count().ToString(), "OK");
        }
    }
}