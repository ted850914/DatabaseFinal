using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
	public class Users
	{
		public int user_id { get; set; }

		[Required(ErrorMessage = "請輸入帳號")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "請使用英數字組合")]
		public string account { get; set; }

		[Required(ErrorMessage = "請輸入密碼")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "請使用英數字組合")]
		[DataType(DataType.Password)]
		public string password { get; set; }

		[Required(ErrorMessage = "請輸入確認密碼")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "請使用英數字組合")]
		[DataType(DataType.Password)]
		[Compare("password", ErrorMessage = "密碼與確認密碼不相同")]
		public string passwordConfirm { get; set; }

		[Required(ErrorMessage = "請輸入姓")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string last_name { get; set; }

		[Required(ErrorMessage = "請輸入名")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string first_name { get; set; }

		[Required(ErrorMessage = "請輸入信箱")]
		[StringLength(100, ErrorMessage = "長度不得超過100")]
		[DataType(DataType.EmailAddress, ErrorMessage = "信箱格式不正確")]
		public string email { get; set; }

		[Required(ErrorMessage = "請輸入手機")]
		[StringLength(12, MinimumLength = 7, ErrorMessage = "長度必須再7~12之間")]
		[RegularExpression(@"^([0-9]+)$", ErrorMessage = "請使用數字")]
		public string phone { get; set; }

		public bool admin { get; set; }

		[Required(ErrorMessage = "請輸入城市")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string city { get; set; }

		[Required(ErrorMessage = "請輸入鄉鎮市區")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string town { get; set; }

		[Required(ErrorMessage = "請輸入路")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string road { get; set; }

		[Required(ErrorMessage = "請輸入號")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		public string address_number { get; set; }

		public System.Guid salt { get; set; }
	}
}