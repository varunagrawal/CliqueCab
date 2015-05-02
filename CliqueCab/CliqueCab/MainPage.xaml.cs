using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CliqueCab
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
			if (User.Access_Token == null || DateTime.Now.Subtract(DateTime.Parse(User.Expires_In)).Ticks > 0)
			{
				LoginBtn.Visibility = Visibility.Visible;
				GetCabsPanel.Visibility = Visibility.Collapsed;
			}
			else
			{
				LoginBtn.Visibility = Visibility.Collapsed;
				GetCabsPanel.Visibility = Visibility.Visible;

				UserName.Visibility = Visibility.Collapsed;
				
				Uber uber = new Uber();
				User user = await uber.UserProfile();
				UserName.Text = string.Format("Hey there, {0}!", user.First_Name);

				UserName.Visibility = Visibility.Visible;
			}

        }

		private void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Login));
		}

		private void btnGetCabs_Click(object sender, RoutedEventArgs e)
		{
			long passengers = 1;
			try
			{
				passengers = Int64.Parse(txtPassengers.Text);
			}
			catch(Exception ex)
			{
				MessageDialog md = new MessageDialog("Invalid Passenger Count", "Passengers Error");
				md.ShowAsync();
			}
			this.Frame.Navigate(typeof(Cabs), passengers);
		}
    }
}
