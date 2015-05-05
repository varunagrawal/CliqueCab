using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
			requestedCabs = e.Parameter as ObservableCollection<Product>;
			Uber uber = new Uber();
			Geoposition start = await User.GetLocation();
			Geoposition end = null;

			foreach(Product cab in requestedCabs)
			{
				Request r = await uber.Request(cab.Product_Id, start, start);

				if(r.Errors != null)
				{
					foreach(Error err in r.Errors)
					{
						if(err.Code == "no_drivers_available")
						{

						}
						else if(err.Code == "surge")
						{
							//Confirm Surge Pricing
						}
					}
				}
			}
		}
	}
}
