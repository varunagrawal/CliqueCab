using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Windows.Data.Json;
using Newtonsoft.Json;

namespace CliqueCab
{
	public class Uber
	{
		HttpClient client = new HttpClient();
		string BaseAddress = "https://sandbox-api.uber.com";

		public Uber()
		{
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.Access_Token);
		}

		public async Task<Products> Products(double latitude, double longitude)
		{
			UriBuilder builder = new UriBuilder(BaseAddress + "/v1/products");
			builder.Query = string.Format("latitude={0}&longitude={1}", latitude, longitude);

			HttpResponseMessage response = await client.GetAsync(builder.Uri);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();

				return new Products(result);
			}

			return null;
		}

		public async Task<User> UserProfile()
		{ 
			HttpResponseMessage response = await client.GetAsync(new Uri(BaseAddress + "/v1/me"));

			if(response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<User>(result);
			}

			return null;
		}
	}
}
