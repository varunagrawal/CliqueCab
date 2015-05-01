using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

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
				double expiration_ticks = expiration.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).Ticks;
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				if (localSettings.Values.ContainsKey("expires_in"))
					localSettings.Values["expires_in"] = expiration_ticks.ToString();
				else localSettings.Values.Add("expires_in", expiration_ticks.ToString());
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
			
		public User(string JSON){
			JsonObject user = JsonObject.Parse(JSON);

			First_Name = user.GetNamedString("first_name");
			Last_Name = user.GetNamedString("last_name");
			Email = user.GetNamedString("email");
			Picture = user.GetNamedString("picture");
			Promo_Code = user.GetNamedString("promo_code");
			UUID = user.GetNamedString("uuid");
		}
	}
}
