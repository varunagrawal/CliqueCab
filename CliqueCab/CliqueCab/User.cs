using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Popups;

namespace CliqueCab
{
	public class User
	{	
		public static String Access_Token
		{
			get
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("access_token"))
					return (string)localSettings.Values["access_token"];
				else return null;
			}

			set
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("access_token"))
					localSettings.Values["access_token"] = value;
				else localSettings.Values.Add("access_token", value);
			}
		}

		public static String Token_Type
		{
			get
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("token_type"))
					return (string)localSettings.Values["token_type"];
				else return null;
			}

			set
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("token_type"))
					localSettings.Values["token_type"] = value;
				else localSettings.Values.Add("token_type", value);
			}
		}

		public static String Expires_In
		{
			get
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("expires_in"))
					return (string)localSettings.Values["expires_in"];
				else return null;
			}

			set
			{
				
				DateTime expiration = DateTime.Now.AddSeconds(double.Parse(value));
				string expires_in = expiration.ToString();
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("expires_in"))
					localSettings.Values["expires_in"] = expires_in.ToString();
				else localSettings.Values.Add("expires_in", expires_in.ToString());
			}
		}

		public static String Scope
		{
			get
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("scope"))
					return (string)localSettings.Values["scope"];
				else return null;
			}

			set
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("scope"))
					localSettings.Values["scope"] = value;
				else localSettings.Values.Add("scope", value);
			}
		}

		public String First_Name { get; set; }
		public string Last_Name { get; set; }
		public string Email { get; set; }
		public string Picture { get; set; }
		public string Promo_Code { get; set; }
		public string UUID { get; set; }

		public static Geoposition Location { get; set; }

		public static async Task<Geoposition> GetLocation()
		{
			Geolocator locator = new Geolocator();
			//locator.DesiredAccuracy = PositionAccuracy.High;
			locator.DesiredAccuracyInMeters = 10;
			locator.MovementThreshold = 50;
			locator.PositionChanged += locator_PositionChanged;

			if (locator.LocationStatus == PositionStatus.Disabled || locator.LocationStatus == PositionStatus.NotAvailable)
			{
				MessageDialog md = new MessageDialog("Please turn on location services and try again.", "Location Disabled");
			}

			try
			{
				Location = await locator.GetGeopositionAsync();

				return Location;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}

		static void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			try
			{
				Location = args.Position;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		} 
	}
}
