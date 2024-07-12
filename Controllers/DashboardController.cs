﻿using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Implementation;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
	public class DashboardController : Controller
	{

		private readonly ApplicationDbContext _context;

		public DashboardController(ApplicationDbContext context) 
		{ 
			_context = context;
		}

		public async Task<ActionResult> Index()
		{
			//Last 7 days
			DateTime StartDate = DateTime.Today.AddDays(-6);
			DateTime EndDate = DateTime.Today;

			List<Transaction> SelectedTransaction = await _context.Transactions
				.Include(x=>x.Category)
				.Where(y => y.Date>= StartDate && y.Date<=EndDate)
				.ToListAsync();

			//Total Income
			int TotalIncome = SelectedTransaction
				.Where(i => i.Category.Type == "Income")
				.Sum(j => j.Amount);
			ViewBag.TotalIncome = TotalIncome.ToString("C0");

			//Total Expense
			int TotalExpense = SelectedTransaction
				.Where(i => i.Category.Type == "Expense")
				.Sum(j => j.Amount);
			ViewBag.TotalExpense = TotalExpense.ToString("C0");

			//Balance
			int Balance = TotalIncome - TotalExpense;
			CultureInfo culture = CultureInfo.CreateSpecificCulture("en-USD");
			culture.NumberFormat.CurrencyNegativePattern = 1;
			ViewBag.Balance = String.Format(culture,"{0:C0}",Balance);

			//Dougahnut Chart - Expense by Category
			ViewBag.DoughnutChartData = SelectedTransaction
				.Where(i => i.Category.Type == "Expense")
				.GroupBy(j => j.Category.CategoryId)
				.Select(k => new
				{
					categoryTittleWithIcon = k.First().Category.Icon+" "+ k.First().Category.Title,
					amount = k.Sum(j => j.Amount),
					formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
				})
				.ToList();


			return View();
		}
	}
}