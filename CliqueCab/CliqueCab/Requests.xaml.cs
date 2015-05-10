using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CliqueCab
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Requests : Page
	{
		ObservableCollection<Product> requestedCabs = null;
		StatusBar statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

		public Requests()
		{
			this.InitializeComponent();
		}
 
		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			statusbar.ProgressIndicator.ShowAsync();
			statusbar.ProgressIndicator.Text = "Requesting cabs...";

			requestedCabs = e.Parameter as ObservableCollection<Product>;
			Uber uber = new Uber();
			Geoposition start = await User.GetLocation();
			Geoposition end = start;

			foreach(Product cab in requestedCabs)
			{
				Request r = await uber.Request(cab.Product_Id, start, end);
				
				if(r.Errors != null)
				{
					foreach(Error err in r.Errors)
					{
						if(err.Code == "no_drivers_available")
						{
							MessageDialog md = new MessageDialog("Not enough drivers available. Please try again in a while.");
							await md.ShowAsync();
							Frame.GoBack();
						}
						else if(err.Code == "surge")
						{
							//Confirm Surge Pricing
						}
					}
				}
			}

			List<Request> requests = new List<Request>(User.Requests);
			long cabs_booked = 0;

			while(requests.Count != 0)
			{
				foreach(var req in requests)
				{
					var status = await uber.GetRequestStatus(req);
					if(status.Status == "accepted" || status.Status == "arriving")
					{
						requests.Remove(req);
						cabs_booked += 1;
					}
					else if(status.Status == "no_drivers_available" || status.Status == "driver_canceled")
					{
						requests.Remove(req);
					}
				}
				
			}

			statusbar.ProgressIndicator.HideAsync();
		}
	}
}
