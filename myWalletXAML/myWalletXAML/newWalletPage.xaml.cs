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
	public partial class newWalletPage : ContentPage
	{
		public newWalletPage ()
		{
			InitializeComponent ();
		}
        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}