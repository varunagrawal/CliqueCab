using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace CliqueCab
{
	public class Products
	{
		List<Product> List;

		public Products(string JSON)
		{
			JsonObject obj = JsonObject.Parse(JSON);
			JsonArray array = obj.GetArray();

			List = new List<Product>();
			
			foreach(JsonObject product in array)
			{
				var prod = ParseJSONProduct(product);
				List.Add(prod);
			}

		}

		Product ParseJSONProduct(JsonObject obj)
		{
			return new Product(obj);
		}
	}

	public class Product
	{
		public string ProductId { get; set; }
		public string Description { get; set; }
		public string DisplayName { get; set; }
		public int Capacity { get; set; }
		public string ImageUrl { get; set; }

		public Product(JsonObject obj) 
		{
			ProductId = obj.GetNamedString("product_id");
			Description = obj.GetNamedString("description");
			DisplayName = obj.GetNamedString("display_name");
			Capacity = int.Parse(obj.GetNamedNumber("capacity").ToString());
			ImageUrl = obj.GetNamedString("image");
		}
	}
}
