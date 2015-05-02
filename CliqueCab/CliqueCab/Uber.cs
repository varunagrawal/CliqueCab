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
		List<Product> Products { get; set; }

		//public Products(string JSON)
		//{
		//List = JsonConvert.DeserializeObject<List<Product>>(JSON);
		//JsonObject obj = JsonObject.Parse(JSON);
		//JsonArray array = obj.GetArray();

		//List = new List<Product>();

		//foreach(JsonObject product in array)
		//{
		//	var prod = ParseJSONProduct(product);
		//	List.Add(prod);
		//}

		//}

		//Product ParseJSONProduct(JsonObject obj)
		//{
		//	return new Product(obj);
		//}
	}

	public class Product
	{
		public string Product_Id { get; set; }
		public string Description { get; set; }
		public string Display_Name { get; set; }
		public int Capacity { get; set; }
		public string Image { get; set; }

		//public Product(JsonObject obj) 
		//{
		//	ProductId = obj.GetNamedString("product_id");
		//	Description = obj.GetNamedString("description");
		//	DisplayName = obj.GetNamedString("display_name");
		//	Capacity = int.Parse(obj.GetNamedNumber("capacity").ToString());
		//	ImageUrl = obj.GetNamedString("image");
		//}
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
