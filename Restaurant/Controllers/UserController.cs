using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant.Controllers
{
	public class UserController : Controller
	{
		private DatabaseEntities1 db = new DatabaseEntities1();

		public static string Encrypt(string plainText)
		{
			byte[] data = Encoding.Default.GetBytes(plainText);
			SHA256 sha256 = new SHA256CryptoServiceProvider();
			byte[] result = sha256.ComputeHash(data);
			return Convert.ToBase64String(result);
		}

		public void GetTicket(string username)
		{
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
						username, DateTime.Now, DateTime.Now.AddMinutes(30),
						false, "user data", FormsAuthentication.FormsCookiePath);
			string EncryptTicket = FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptTicket);
			cookie.HttpOnly = true;
			Response.Cookies.Add(cookie);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(loginModel lm)
		{
			if (lm.lm_username == null || lm.lm_password == null)
			{
				return View();
			}

			var r = (from user in db.Users
					 where user.account == lm.lm_username
					 select user).FirstOrDefault();
			if (r == null)
			{
				ViewBag.message = "帳號密碼錯誤";
			}
			else
			{
				string SaltAndPassword = String.Concat(r.salt, lm.lm_password);
				string EncryptPassword = Encrypt(SaltAndPassword);
				if (string.Compare(EncryptPassword, r.password, false) == 0)
				{
					ViewBag.message = "登入成功";
					GetTicket(lm.lm_username);
					return RedirectToAction("Menu", "Restaurant");
				}
				else
				{
					ViewBag.message = "帳號密碼錯誤";
				}
			}

			return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "account,password,passwordConfirm,salt,last_name,first_name,phone,email,city,town,road,address_number")] Users user)
		{
			if (ModelState.IsValid)
			{
				User add = new User();
				user.salt = Guid.NewGuid();
				string SaltAndPassword = String.Concat(user.salt, user.password);
				string EncryptPassword = Encrypt(SaltAndPassword);
				user.password = EncryptPassword;
				add.account = user.account;
				add.address_number = user.address_number;
				add.city = user.city;
				add.email = user.email;
				add.first_name = user.first_name;
				add.last_name = user.last_name;
				add.password = user.password;
				add.phone = user.phone;
				add.road = user.road;
				add.salt = user.salt;
				add.town = user.town;
				db.Users.Add(add);
				db.SaveChanges();
				return RedirectToAction("Login");
			}
			return View();
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		// GET: User
		[Authorize]
		public ActionResult Index()
		{
			return RedirectToAction("Login");
		}

		[Authorize]
		public ActionResult History()
		{
			History history = new History();
			var ur = (from u in db.Users
					  where u.account == User.Identity.Name
					  select u).FirstOrDefault();
			history.order = (from o in db.Orders
							 where o.user_id == ur.user_id
							 select o).OrderByDescending(o => o.receive_time).ToList();

			history.order_detail = new List<List<Order_Detail>>();

			for (int i = 0; i < history.order.Count(); i++)
			{
				int order_id = history.order.ElementAt(i).order_id;
				history.order_detail.Add((from od in db.Order_Detail
										  where od.order_id == order_id
										  select od).ToList());
			}
			history.meal = (from m in db.Meals select m).ToList();

			return View(history);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Modify([Bind(Include = "last_name,first_name,phone,email,city,town,road,address_number")] PersonInformation pi)
		{
			if (ModelState.IsValid)
			{
				var ur = (from u in db.Users
						  where u.account == User.Identity.Name
						  select u).FirstOrDefault();
				ur.last_name = pi.last_name;
				ur.first_name = pi.first_name;
				ur.phone = pi.phone;
				ur.email = pi.email;
				ur.city = pi.city;
				ur.town = pi.town;
				ur.road = pi.road;
				ur.address_number = pi.address_number;
				db.SaveChanges();
				Response.Write("<script>alert('已儲存修改')</script>");
			}
			return View();
		}

		[Authorize]
		[HttpGet]
		public ActionResult Modify()
		{
			PersonInformation pi = new PersonInformation();
			var ur = (from u in db.Users
					  where u.account == User.Identity.Name
					  select u).FirstOrDefault();
			if (ur == null)
			{
				return RedirectToAction("Login");
			}
			else
			{
				pi.last_name = ur.last_name;
				pi.first_name = ur.first_name;
				pi.email = ur.email;
				pi.phone = ur.phone;
				pi.city = ur.city;
				pi.town = ur.town;
				pi.road = ur.road;
				pi.address_number = ur.address_number;
			}
			return View(pi);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ModifyPassword([Bind(Include = "password,passwordConfirm")] UserPassword up)
		{
			if (ModelState.IsValid)
			{
				var ur = (from u in db.Users
						  where u.account == User.Identity.Name
						  select u).FirstOrDefault();

				string SaltAndPassword = String.Concat(ur.salt, up.password);
				string EncryptPassword = Encrypt(SaltAndPassword);
				ur.password = EncryptPassword;

				db.SaveChanges();
				Response.Write("<script>alert('已儲存修改')</script>");
			}
			return View();
		}

		[Authorize]
		[HttpGet]
		public ActionResult ModifyPassword()
		{
			return View();
		}

		[Authorize]
		public ActionResult Order_Confirm()
		{
			var currentCart = Restaurant.Models.OperationModel.GetCurrentCart();
			if (currentCart.TotalAmont == 0)
			{
				Response.Write("<script>alert('你沒有點餐！')</script>");
				return RedirectToAction("Menu", "Restaurant");
			}
			return View();
		}

		[Authorize]
		public void SendOrder(string remark)
		{
			var currentCart = Restaurant.Models.OperationModel.GetCurrentCart();
			var ur = (from u in db.Users
					  where u.account == User.Identity.Name
					  select u).FirstOrDefault();
			Order order = new Order();

			order.user_id = ur.user_id;
			order.money = currentCart.TotalAmont;
			order.receive_time = System.DateTime.Now;
			order.arrive_time = System.DateTime.Now.AddMinutes(30);
			order.remark = remark;
			order.status = 0;
			db.Orders.Add(order);
			db.SaveChanges();

			var send_order = (from u in db.Orders
							  where u.user_id == ur.user_id
							  select u).OrderByDescending(u => u.receive_time).FirstOrDefault();

			foreach (var meal in currentCart.ToList())
			{
				var meal_popularity = (from m in db.Meals
									   where m.meal_id == meal.id
									   select m).FirstOrDefault();
				meal_popularity.popularity += meal.quantity;
				Order_Detail od = new Order_Detail();
				od.order_id = send_order.order_id;
				od.meal_id = meal.id;
				od.quantity = meal.quantity;
				db.Order_Detail.Add(od);
				db.SaveChanges();
			}

			currentCart.RemoveAll();
		}

		[Authorize]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Login");
		}
	}
}