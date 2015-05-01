using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.Data.Json;
using Newtonsoft.Json;

namespace CliqueCab
{
	public class Products
	{
		List<Product> List { get; set; }

		public Products(string JSON)
		{
			List = JsonConvert.DeserializeObject<List<Product>>(JSON);
			//JsonObject obj = JsonObject.Parse(JSON);
			//JsonArray array = obj.GetArray();

			//List = new List<Product>();
			
			//foreach(JsonObject product in array)
			//{
			//	var prod = ParseJSONProduct(product);
			//	List.Add(prod);
			//}

		}

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
}
