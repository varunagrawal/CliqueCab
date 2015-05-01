using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

		public Cabs()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			StatusBar statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
			statusbar.ProgressIndicator.Text = "Getting Cab Options...";
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

			GetCabOptions(passengers);
		}

		private List<CabOption> GetCabOptions(long Passengers)
		{
			//var products = uber.Products();
			return null;
		}
	}
}
