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
	public class UberProducts
	{
		[JsonProperty("products")]
		public List<Product> Products { get; set; }
	}

	public class Product
	{
		public string Product_Id { get; set; }
		public string Description { get; set; }
		public string Display_Name { get; set; }
		public int Capacity { get; set; }
		public string Image { get; set; }

		public Price_Details Price_Details { get; set; }
		
	}

	public class Price_Details
	{
		public string Distance_Unit { get; set; }
		public float Cost_Per_Minute { get; set; }
		public float Minimum { get; set; }
		public float Cost_Per_Distance { get; set; }
		public float Base { get; set; }
		public float Cancellation_Fee { get; set; }
		public string Currency_Code { get; set; }

		[JsonProperty("service_fees")]
		public List<ServiceFees> Service_Fees { get; set; }

		public float Cost
		{
			get
			{
				return Base + Cost_Per_Distance + Cost_Per_Minute;
			}
		}

		public override string ToString()
		{
			return String.Format("{0} + {1}/d + {2}/min", Base, Cost_Per_Distance, Cost_Per_Minute);
		}
	}

	public class ServiceFees
	{
		public string Name { get; set; }
		public float Fee { get; set; }
	}

	public class Uber
	{
		HttpClient client = new HttpClient();
		string BaseAddress = "https://sandbox-api.uber.com";

		public Uber()
		{
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.Access_Token);
		}

		public async Task<UberProducts> Products(double latitude, double longitude)
		{
			UriBuilder builder = new UriBuilder(BaseAddress + "/v1/products");
			builder.Query = string.Format("latitude={0}&longitude={1}", latitude, longitude);

			HttpResponseMessage response = await client.GetAsync(builder.Uri);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<UberProducts>(result);
			}

			return null;
		}

		public async Task<User> UserProfile()
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync(new Uri(BaseAddress + "/v1/me"));

				if (response.StatusCode == HttpStatusCode.OK)
				{
					string result = await response.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<User>(result);
				}
			}
			catch (Exception ex)
			{
				User u = new User();
				u.First_Name = "User";
				u.Last_Name = "";

				return u;
			}

			return null;

		}
	}

}
