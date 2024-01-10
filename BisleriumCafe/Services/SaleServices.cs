using BisleriumCafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class SaleServices
    {
        public static List<Sales> GetAll()
        {
            string appSalesFilePath = Utils.GetAppSalesFilePath();
            if (!File.Exists(appSalesFilePath))
            {
                return new List<Sales>();
            }

            var json = File.ReadAllText(appSalesFilePath);
            if (IsValidJson(json))
            {
                return JsonSerializer.Deserialize<List<Sales>>(json);
            }
            else { return new List<Sales>(); }
        }
        private static bool IsValidJson(string strInput)
        {
            try
            {
                JsonSerializer.Deserialize<List<Order>>(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void SaveAll(List<Sales> sales)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appSalesFilePath = Utils.GetAppSalesFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(sales);
            File.WriteAllText(appSalesFilePath, json);
        }

        public static List<DailyRevenue> GetDailyRevenue(List<Sales> sales)
        {
            List<DailyRevenue> dailyRevenue = sales
            .GroupBy(s => s.CreatedAt.Date)
            .Select(group => new DailyRevenue
            {
                Date = group.Key,
                TotalRevenue = group.Sum(s => s.TotalPrice)
            })
            .ToList();
            return dailyRevenue;
        }

      

        public static double GetMonthlyRevenue(List<Sales> sales, string selectedMonth)
        {
            double totalRevenue = 0;
            // Filter sales for the selected month
            var selectedMonthSales = sales
                .Where(s => s.CreatedAt.ToString("MMMM") == selectedMonth)
                .ToList();

            // Calculate total revenue
            totalRevenue = selectedMonthSales.Sum(s => s.TotalPrice);
            return totalRevenue;
        }

       

    }
    
}
