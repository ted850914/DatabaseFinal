using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
	public class PersonInformation
	{
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
	}
}