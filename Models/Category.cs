﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
	public class Category
	{
		[Key]
		public int CategoryId { get; set; }

		[Column(TypeName = "nvarchar(50)")]
		public string Tittle { get; set; }

		[Column(TypeName = "nvarchar(5)")]
		public string Icon { get; set; } = "";

		[Column(TypeName = "nvarchar(10)")]
		public string Type { get; set; } = "Expense";
	}
}
