﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		UberProducts products;
		StatusBar statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
		ObservableCollection<Product> selectedCabs = new ObservableCollection<Product>();

		long passengers = 1;

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
			BottomAppBar.Visibility = Visibility.Collapsed;
			CabsMainGrid.Visibility = Visibility.Collapsed;

			statusbar.ProgressIndicator.ShowAsync();

			try
			{
				passengers = (long)e.Parameter;
				User.Passengers_Left = passengers;
			}
			catch (Exception ex)
			{
				MessageDialog md = new MessageDialog(ex.Message, "");
				md.ShowAsync();
			}

			var cabs = await GetCabOptions(passengers);

			statusbar.ProgressIndicator.HideAsync();

			CabsListView.ItemsSource = cabs;
			SetCurrentPassengersRemaining();

			BottomAppBar.Visibility = Visibility.Visible;
			CabsMainGrid.Visibility = Visibility.Visible;
		}

		protected async override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			await statusbar.ProgressIndicator.HideAsync();
		}

		private async Task<List<Product>> GetCabOptions(long Passengers)
		{
			try
			{	
				statusbar.ProgressIndicator.Text = "Getting Location...";

				Geoposition pos = await User.GetLocation();
				if (pos == null) throw new NullReferenceException();

				statusbar.ProgressIndicator.Text = "Getting Cab Options...";

				products = await uber.Products(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);

				if(products == null)
				{
					MessageDialog md = new MessageDialog("Could not retrieve products. Please check internet connection.");
					var x = await md.ShowAsync();
				}

				return products.Products;

				//List<Product> bestCabOptions = CabOption.GetBestCabOption(products, Passengers);

				//return bestCabOptions;
			}
			catch (Exception ex)
			{
				MessageDialog md = new MessageDialog("Cannot access location data!");
				md.ShowAsync();
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return null;
		}

		
		private void CabsListView_ItemClick(object sender, ItemClickEventArgs e)
		{
			Product cab = e.ClickedItem as Product;
			selectedCabs.Add(cab);

			ListOfCabs.ItemsSource = selectedCabs;

			SetCurrentPassengersRemaining();
		}

		private void ListOfCabs_ItemClick(object sender, ItemClickEventArgs e)
		{
			Product cabToRemove = e.ClickedItem as Product;
			var selectedCabs = ListOfCabs.ItemsSource as ObservableCollection<Product>;

			selectedCabs.Remove(cabToRemove);

			SetCurrentPassengersRemaining();
		}

		private void SetCurrentPassengersRemaining()
		{
			long passengers_remaining = passengers;
			foreach (Product p in selectedCabs)
			{
				passengers_remaining -= p.Capacity;
			}

			if (passengers_remaining < 0) passengers_remaining = 0;

			PassengersLeft.Text = String.Format("Passengers Left: {0}", passengers_remaining);
		}

		private void BookCabsBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Requests), ListOfCabs.ItemsSource);
		}

		private void BestOptionsBtn_Click(object sender, RoutedEventArgs e)
		{
			var bestCabOptions = CabOption.GetBestCabOption(products, passengers);
			ListOfCabs.ItemsSource = new ObservableCollection<Product>(bestCabOptions);
		}



	}
}
