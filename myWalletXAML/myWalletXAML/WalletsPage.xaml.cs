using System;
using System.Collections.Generic;
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
        int i = 0;
		public WalletsPage ()
		{
			InitializeComponent ();
		}

        async void onNewWalletButtonClicked(object sender, EventArgs e)
        {
            var button = new Button
            {
                Text = $"Wallet {i++}",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            walletsStack.Children.Add(button);
            await Navigation.PushAsync(new newWalletPage());
        }
		
    }
}