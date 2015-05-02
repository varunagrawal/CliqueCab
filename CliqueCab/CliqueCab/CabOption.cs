using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliqueCab
{
	public class CabOption
	{
		public long NumberOfCabs { get; set; }

		/// <summary>
		/// Knapsack Problem to get best set of Cabs for least cost. Knapsack weight ~ Number of passengers
		/// </summary>
		/// <returns>List<CliqueCab.CabOption></returns>
		public static List<Product> GetBestCabOption(UberProducts uber, long passengers)
		{
			long max_capacity = 0;
			float min_cost_product = float.MaxValue;
			Product min_cost_product_index = null;

			foreach(Product p in uber.Products)
			{
				if(max_capacity < p.Capacity)
				{
					max_capacity = p.Capacity;
				}

				if(p.Price_Details.Cost < min_cost_product)
				{
					min_cost_product = p.Price_Details.Cost;
					min_cost_product_index = p;
				}
			}

			List<Product>[] BestCabOptions = new List<Product>[passengers + 1];
			BestCabOptions[0] = null; // new List<Product>();
			//BestCabOptions[0].Add(min_cost_product_index);

			float[] knapsack = new float[passengers + 1];
			knapsack[0] = 0;

			for(int i=1; i<knapsack.Length; i++)
			{
				float min_cost = float.MaxValue;
				List<Product> CabOptions = new List<Product>();
				Product bestCab = null;

				foreach(Product p in uber.Products)
				{
					if(i - p.Capacity < 0)
					{
						if(min_cost > p.Price_Details.Cost)
						{
							min_cost = p.Price_Details.Cost;
							
							CabOptions.Clear();
							CabOptions.Add(p);
						}
					}
					else
					{
						if (min_cost > knapsack[i - p.Capacity] + p.Price_Details.Cost)
						{
							min_cost = knapsack[i - p.Capacity] + p.Price_Details.Cost;
							//bestCab = p;
							CabOptions.Clear();
							
							if(BestCabOptions[i-p.Capacity] != null)
								CabOptions.AddRange(BestCabOptions[i - p.Capacity]);

							CabOptions.Add(p);
						}
					}
				}

				knapsack[i] = min_cost;
				BestCabOptions[i] = CabOptions;
			}

			return BestCabOptions[passengers];
		}

		public static List<CabOption> CabOptionsFromUberProducts(UberProducts uber, long passengers)
		{
			List<CabOption> options = new List<CabOption>();

			foreach(Product p in uber.Products)
			{
				CabOption option = new CabOption();
				option.NumberOfCabs = (long)Math.Ceiling((double)passengers / p.Capacity);
				
			}

			return options;
		}

	}
}
