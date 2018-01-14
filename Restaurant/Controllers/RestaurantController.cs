using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
	public class RestaurantController : Controller
	{
		private DatabaseEntities1 db = new DatabaseEntities1();
		// GET: Restaurant
		public ActionResult Index()
		{
			return RedirectToAction("Menu");
		}

		public ActionResult Menu(string type, int? id, string keyword)
		{
			mealModel viewModel = new mealModel();
			if (id.HasValue)
			{
				var idMeals = db.Meals.Where(x => x.meal_id == id);
				if (idMeals.Count() != 0)
				{
					viewModel.MealCollection = idMeals.ToList();
					return View(viewModel);
				}
			}

			if (type == "combo")
			{
				type = "set";
			}

			var typeMeals = db.Meals.Where(x => x.type == type);

			if (type == "hot")
			{
				typeMeals = db.Meals.OrderByDescending(x => x.popularity);
			}

			if (keyword != null)
			{
				typeMeals = from m in db.Meals
							where m.name.Contains(keyword)
							select m;
				if (typeMeals.Count() == 0)
				{
					Response.Write("<script>alert('查無結果！')</script>");
				}
			}

			if (typeMeals.Count() == 0)
			{
				typeMeals = db.Meals.Where(x => x.type == "set");
			}
			var Meals = typeMeals.ToList();

			viewModel.MealCollection = Meals;
			return View(viewModel);
		}

		public void AddFood(int id)
		{
			var cart = Restaurant.Models.OperationModel.GetCurrentCart();
			cart.AddFood(id);
		}

		public void RemoveFood(int id)
		{
			var cart = Restaurant.Models.OperationModel.GetCurrentCart();
			cart.RemoveFood(id);
		}

		public void RemoveAll()
		{
			var cart = Restaurant.Models.OperationModel.GetCurrentCart();
			cart.RemoveAll();
		}
	}
}