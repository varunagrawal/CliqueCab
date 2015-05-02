using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
	public sealed partial class Cabs : Page
	{
		Uber uber = new Uber();
		StatusBar statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

		public Cabs()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			statusbar.ProgressIndicator.Text = "Getting Cab Options...";
			statusbar.BackgroundColor = Windows.UI.Color.FromArgb(255, 135, 125, 119);
			statusbar.ProgressIndicator.ShowAsync();

			long passengers = 2;
			try
			{
				passengers = (long)e.Parameter;
			}
			catch(Exception ex)
			{
				MessageDialog md = new MessageDialog(ex.Message, "");
				md.ShowAsync();
			}

			var cabs = await GetCabOptions(passengers);
		}

		private async Task<List<CabOption>> GetCabOptions(long Passengers)
		{
			Geolocator locator = new Geolocator();
			locator.DesiredAccuracy = PositionAccuracy.High;

			if (locator.LocationStatus == PositionStatus.Disabled)
			{
				MessageDialog md = new MessageDialog("Please turn on location services and try again.", "Location Disabled");
				this.Frame.GoBack();
			}
			try
			{
				Geoposition pos = await locator.GetGeopositionAsync();
				UberProducts products = await uber.Products(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);

				List<CabOption> cabOptions = CabOption.CabOptionsFromUberProducts(products);
				statusbar.HideAsync();

				return products;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return null;
		}
	}
}
