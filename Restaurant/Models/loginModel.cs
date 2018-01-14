using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
	public class loginModel
	{
		[Required(ErrorMessage = "請輸入帳號")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "請使用英數字組合")]
		public string lm_username { get; set; }

		[Required(ErrorMessage = "請輸入密碼")]
		[StringLength(50, ErrorMessage = "長度不得超過50")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "請使用英數字組合")]
		[DataType(DataType.Password)]
		public string lm_password { get; set; }
	}
}