using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
	[Serializable]
	public class CartModel : IEnumerable<CartItem>
	{
		public CartModel()
		{
			this.cartItems = new List<CartItem>();
		}

		public List<CartItem> cartItems;


		public int count
		{
			get
			{
				return this.cartItems.Count;
			}
		}
		public int TotalAmont
		{
			get {
				int totalAmont = 0;
				foreach (var cartItem in this.cartItems)
				{
					totalAmont = totalAmont + cartItem.amount;
				}
				return totalAmont;
			}
		}

		public bool AddFood(int id)
		{
			var findItem = this.cartItems.Where(x => x.id == id)
										 .Select(x => x)
										 .FirstOrDefault();
			if (findItem == default(Models.CartItem))
			{
				using (Models.DatabaseEntities1 db = new DatabaseEntities1())
				{
					var food = (from x in db.Meals
								where x.meal_id == id
								select x).FirstOrDefault();
					if (food != default(Models.Meal))
					{
						this.cartItems.Add(
							new Models.CartItem()
							{
								id = food.meal_id,
								name = food.name,
								quantity = 1,
								money = food.money
							});
					}
					else
					{
						return false;
					}
				}
			}
			else
			{
				this.cartItems.Find(x => x.id == id).quantity += 1;
			}
			return true;
		}

		public bool RemoveFood(int id)
		{
			var findItem = this.cartItems.Where(x => x.id == id)
										 .Select(x => x)
										 .FirstOrDefault();

			if (findItem == default(Models.CartItem))
			{
				return false;
			}
			else
			{
				this.cartItems.Find(x => x.id == id).quantity -= 1;
				if (this.cartItems.Find(x => x.id == id).quantity == 0)
				{
					this.cartItems.Remove(findItem);
				}
			}
			return true;
		}

		public bool RemoveAll()
		{
			this.cartItems.Clear();
			return true;
		}

		#region IEnumerator

		IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
		{
			return this.cartItems.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.cartItems.GetEnumerator();
		}

		#endregion
	}
}