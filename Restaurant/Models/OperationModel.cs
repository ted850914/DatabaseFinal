using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Restaurant.Models
{
	public static class OperationModel
	{
		[WebMethod(EnableSession = true)]
		public static Models.CartModel GetCurrentCart()
		{
			if (HttpContext.Current != null)
			{
				if (HttpContext.Current.Session["Cart"] == null)
				{
					var order = new CartModel();
					HttpContext.Current.Session["Cart"] = order;
				}
				return (CartModel)HttpContext.Current.Session["Cart"];
			}
			else
			{
				throw new InvalidOperationException("System.Web.HttpContext.Current為空");
			}
		}
	}
}