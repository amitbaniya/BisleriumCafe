using BisleriumCafe.Models;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BisleriumCafe.Services
{
    public class PDFServices
    {
        public static void DailyPDF(List<DailyRevenue> dailyRevenue)
        {

            var appPath = Utils.GetAppDirectoryPath();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Text("Daily Revenue Report");

                    page.Content().Table(table =>
                    {
                        // Assuming your object has Date and TotalRevenue properties
                        table.ColumnsDefinition(column =>
                        {
                            column.RelativeColumn();
                            column.RelativeColumn();
                        });

                        // Header row
                        table.Header(header =>
                        {
                            header.Cell().Text("Date");
                            header.Cell().Text("Total Revenue");
                        });

                        // Data rows
                        foreach (var item in dailyRevenue)
                        {
                            table.Cell().Text(item.Date.ToString());
                            table.Cell().Text(item.TotalRevenue.ToString());
                        }
                    });
                    page.Footer().Text(text =>
                    {
                        text.Span("Page :");
                        text.CurrentPageNumber();
                    });
                });

            }).GeneratePdf(Path.Combine(appPath,"Daily.pdf"));
        }
        public static void MonthlyPDF(string selectedMonth, double monthlyRevenue, List<FiveItems> topFiveItems, List<FiveAddIns> topFiveAddIns)
        {
            var appPath = Utils.GetAppDirectoryPath();
            Debug.Print(monthlyRevenue.ToString());
            Debug.Print(topFiveItems[0].Name.ToString());
            Document.Create(container =>
            {
               
                container.Page(page =>
                {
                    
                    page.Header().Text($"The Monthly Revenue for {selectedMonth} month is {monthlyRevenue.ToString("F2")} and the top 5 coffee and addins are as follows:");

                page.Content().Table(table =>
                    {
                        
                        table.ColumnsDefinition(column =>
                        {
                            column.RelativeColumn();
                            column.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Items Name");
                            header.Cell().Text("Quantity Sold");
                        });

                        foreach (var addIn in topFiveAddIns)
                        {
                            table.Cell().Text(addIn.AddInName);
                            table.Cell().Text(addIn.Quantity.ToString());
                        }

                        
                        foreach (var item in topFiveItems)
                        {
                            table.Cell().Text(item.Name);
                            table.Cell().Text(item.Quantity.ToString());

                        }
                    });

                    page.Footer().Text(text =>
                    {
                        text.Span("Page :");
                        text.CurrentPageNumber();
                    });
                });

            }).GeneratePdf(Path.Combine(appPath, $"Monthly Report - {selectedMonth}.pdf"));
        }
    }
}

