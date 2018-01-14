using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
	public class History
	{
		public List<Order> order { get; set; }
		public List< List<Order_Detail> > order_detail { get; set; }
		public List<Meal> meal { get; set; }
	}
}