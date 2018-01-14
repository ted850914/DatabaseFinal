using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
	[Serializable]
	public class CartItem
	{
		public int id { get; set; }
		public string name { get; set; }
		public int money { get; set; }
		public int quantity { get; set; }
		public int amount
		{
			get
			{
				return this.money * this.quantity;
			}
		}
	}
}