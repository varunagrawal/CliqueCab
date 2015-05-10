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

	public class CabRequest
	{
		public string product_id { get; set; }
		public string start_latitude { get; set; }
		public string start_longitude { get; set; }
		public string end_latitude { get; set; }
		public string end_longitude { get; set; }
		public string surge_confirmation_id { get; set; }

		public CabRequest(string Product_Id, Geoposition start, Geoposition end, string Surge_Confirmation = null)
		{
			product_id = Product_Id;
			start_latitude = start.Coordinate.Point.Position.Latitude.ToString();
			start_longitude = start.Coordinate.Point.Position.Longitude.ToString();
			end_latitude = (end.Coordinate.Point.Position.Latitude+1).ToString();
			end_longitude = (end.Coordinate.Point.Position.Longitude+1).ToString();
			surge_confirmation_id = Surge_Confirmation;
		}
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

			try
			{
				HttpResponseMessage response = await client.GetAsync(builder.Uri);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					string result = await response.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<UberProducts>(result);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
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
			
			CabRequest req = new CabRequest(Product_Id, start, end);
			string body = JsonConvert.SerializeObject(req);
			HttpContent content = new StringContent(body);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			try
			{
				client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");

				HttpResponseMessage response = await client.PostAsync(builder.Uri, content);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					string result = await response.Content.ReadAsStringAsync();

					return JsonConvert.DeserializeObject<Request>(result);
				}
				else if(response.StatusCode == HttpStatusCode.Accepted)
				{

					string result = await response.Content.ReadAsStringAsync();
					var res = JsonConvert.DeserializeObject<Request>(result);
					if(res.Status != "accepted")
					{
						User.Requests.Add(res);
						
						//builder = new UriBuilder(BaseAddress + string.Format("/v1/requests/{0}", res.Request_Id));
						//var resp = await client.GetAsync(builder.Uri);
						//result = await resp.Content.ReadAsStringAsync();
						//return JsonConvert.DeserializeObject<Request>(result);
					}
					return res;
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}

		public async Task<Request> GetRequestStatus(Request r)
		{
			UriBuilder builder = new UriBuilder(BaseAddress + string.Format("/v1/requests/{0}", r.Request_Id));
			var resp = await client.GetAsync(builder.Uri);
			string result = await resp.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Request>(result);
		}
	}

	public class Request
	{
		public string Request_Id { get; set; }
		public string Status { get; set; }
		public Vehicle Vehicle { get; set; }
		public Driver Driver { get; set; }
		public Location DriverLocation { get; set; }
		public int ETA { get; set; }
		public float Surge_Multiplier { get; set; }
		public Meta Meta { get; set; }

		[JsonProperty("errors")]
		public List<Error> Errors { get; set; }
	}

	public class Vehicle 
	{
		public string Make { get; set; }
		public string Model { get; set; }
		public string License_Plate { get; set; }
		public string Picture_Url { get; set; }

	}

	public class Driver
	{
		public string Phone_Number { get; set; }
		public int Rating { get; set; }
		public string Picture_Url { get; set; }
		public string Name { get; set; }
	}

	public class Location
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Bearing { get; set; }
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
