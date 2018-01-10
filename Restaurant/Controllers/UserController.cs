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

		public ActionResult Login(loginModel lm)
		{
			ViewBag.username = lm.lm_username;
			ViewBag.password = lm.lm_password;
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
				ViewBag.savedHPW = r.password;
				ViewBag.inputHPW = EncryptPassword;
				if (string.Compare(EncryptPassword, r.password, false) == 0)
				{
					ViewBag.message = "登入成功";
					GetTicket(lm.lm_username);
				}
				else
				{
					ViewBag.message = "帳號密碼錯誤";
				}
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "account,password,salt,last_name,first_name,phone,email")] User user)
		{
			if (ModelState.IsValid)
			{
				user.salt = Guid.NewGuid();
				string SaltAndPassword = String.Concat(user.salt, user.password);
				string EncryptPassword = Encrypt(SaltAndPassword);
				user.password = EncryptPassword;
				db.Users.Add(user);
				db.SaveChanges();
				return RedirectToAction("Login");
			}
			return View();
		}

		[HttpGet]
		public ActionResult Create() {
			return View();
		}

		// GET: User
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}

		[Authorize]
		public ActionResult Test()
		{
			return View();
		}

		[Authorize]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return View();
		}
	}
}