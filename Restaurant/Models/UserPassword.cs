using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
	public class UserPassword
	{
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
	}
}