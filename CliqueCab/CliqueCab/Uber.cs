using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Windows.Data.Json;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;

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

		public async Task<Request> Request(string Product_Id, Geoposition start, Geoposition end, string Surge_Confirmation_Id = null)
		{
			UriBuilder builder = new UriBuilder(BaseAddress + "/v1/requests");
			List<KeyValuePair<string, string>> postParams = new List<KeyValuePair<string,string>>
			{
				new KeyValuePair<string, string>("product_id", Product_Id),
				new KeyValuePair<string, string>("start_latitude", start.Coordinate.Point.Position.Latitude.ToString()),
				new KeyValuePair<string, string>("start_longitude", start.Coordinate.Point.Position.Longitude.ToString()),
				new KeyValuePair<string, string>("end_latitude", end.Coordinate.Point.Position.Latitude.ToString()),
				new KeyValuePair<string, string>("end_longitude", end.Coordinate.Point.Position.Longitude.ToString())
			};

			if(Surge_Confirmation_Id != null)
			{
				postParams.Add(new KeyValuePair<string, string>("start_latitude", start.Coordinate.Point.Position.Latitude.ToString()));
			}

			HttpContent content = new FormUrlEncodedContent(postParams);

			try
			{
				HttpResponseMessage response = await client.PostAsync(builder.Uri, content);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					string result = await response.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<Request>(result);
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}
	}

	public class Request
	{
		public string Request_Id { get; set; }
		public string Status { get; set; }
		public Vehicle Vehicle { get; set; }
		public Driver Driver { get; set; }
		public Location DriverLocation { get; set; }
		public int eta { get; set; }
		public float Surge_Multiplier { get; set; }
		public Meta Meta { get; set; }

		[JsonProperty("errors")]
		public List<Error> Errors { get; set; }
	}

	public class Vehicle 
	{
	}

	public class Driver
	{

	}

	public class Location
	{

	}

	public class Meta 
	{
		public Surge_Confirmation Surge_Confirmation { get; set; }
	}

	public class Surge_Confirmation
	{
		public string href { get; set; }
		public string Surge_Confirmation_Id { get; set; }
	}

	public class Error
	{
		public int Status { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
	}
}
