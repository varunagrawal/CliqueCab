using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CliqueCab
{
	class User
	{	
		String Access_Token
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
	}
}
