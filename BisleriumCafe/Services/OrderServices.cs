using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using BisleriumCafe.Models;
using Microsoft.Extensions.Options;


namespace BisleriumCafe.Services
{
    public class OrderServices
    {
        public static List<Order> GetAll()
        {
            string appOrdersFilePath = Utils.GetAppOrdersFilePath();
            if (!File.Exists(appOrdersFilePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(appOrdersFilePath);
            if (IsValidJson(json))
            {
                return JsonSerializer.Deserialize<List<Order>>(json);
            }
            else { return new List<Order>(); }
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
        public static List<Items> GetAllCartItems()
        {
            string appCartFilePath = Utils.GetAppCartFilePath();
            if (!File.Exists(appCartFilePath))
            {
                return new List<Items>();
            }

            var json = File.ReadAllText(appCartFilePath);
            if (IsValidJson(json))
            {
                return JsonSerializer.Deserialize<List<Items>>(json);
            }
            else { return new List<Items>(); }
        }
        private static void SaveToCart(List<Items> items)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appCartFilePath = Utils.GetAppCartFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(appCartFilePath, json);
        }
        public static Items AddToCart(Items item)
        {
            List<Items> items = GetAllCartItems();
            item.CartNumber = items.Count()+1;
            items.Add(item);
            SaveToCart(items); 

            return item;
        }
        public static AddIns AddAddInToCartItem(Items item, AddIns newAddIns)
        {
            List<Items> items = GetAllCartItems();
            int index = items.FindIndex(existingItem => existingItem.CartNumber == item.CartNumber);
            List<AddIns> addInsList = items[index].AddIns;
            var existingAddIns = addInsList.FirstOrDefault(addIns => addIns.Name.Equals(newAddIns.Name));
            if (existingAddIns != null)
            {
                existingAddIns.Quantity = existingAddIns.Quantity + 1;
            }
            else
            {
                newAddIns.Quantity = 1;
                addInsList.Add(newAddIns);
            }
            item.AddIns = addInsList;
            if (index != -1)
            {
                items[index] = item;
                SaveToCart(items);
            }

            return newAddIns;
        }

        public static List<Items> ItemRemove(Items item)
        {
            List<Items> items = GetAllCartItems();
            item = items.FirstOrDefault(existingItem => existingItem.CartNumber == item.CartNumber);
            items.Remove(item); 
                
            SaveToCart(items);
            

            return items;
        }

        public static List<AddIns> GetAddInsByCarNumber(Items item)
        {
            Debug.Print("Hello");
            List<Items> items = GetAllCartItems();
            List<AddIns> addIns = new List<AddIns>();
            int index = items.FindIndex(existingItem => existingItem.CartNumber == item.CartNumber);
            Debug.Print(index.ToString());
            if (index != -1)
            {
                Debug.Print("Hello");
                if (items[index].AddIns != null)
                {
                    Debug.Print("Hello");
                    addIns = items[index].AddIns;
                    return addIns;
                }
                else
                    return new List<AddIns>();
            }
            else return new List<AddIns>(); 
        }
        public static List<AddIns> AddInIncrease(Items item, AddIns newAddIns)
        {
            List<Items> items = GetAllCartItems();
            int index = items.FindIndex(existingItem => existingItem.CartNumber == item.CartNumber);
            List<AddIns> addInsList = items[index].AddIns;
            var existingAddIns = addInsList.FirstOrDefault(addIns => addIns.Id.Equals(newAddIns.Id));
            if (existingAddIns != null)
            {
                existingAddIns.Quantity = existingAddIns.Quantity + 1;
            }
            item.AddIns = addInsList;
            if (index != -1)
            {
                items[index] = item;
                SaveToCart(items);
            }

            return items[index].AddIns;
        }
        public static List<AddIns> AddInDecrease(Items item, AddIns newAddIns)
        {
            List<Items> items = GetAllCartItems();
            int index = items.FindIndex(existingItem => existingItem.CartNumber == item.CartNumber);
            List<AddIns> addInsList = items[index].AddIns;
            var existingAddIns = addInsList.FirstOrDefault(addIns => addIns.Id.Equals(newAddIns.Id));
            if (existingAddIns != null)
            {
                if(existingAddIns.Quantity > 1) { 
                    existingAddIns.Quantity = existingAddIns.Quantity - 1; 
                }
                else if(existingAddIns.Quantity == 1) 
                {
                     
                    addInsList.Remove(existingAddIns);
                }
                
            }

            item.AddIns = addInsList;
            if (index != -1)
            {
                items[index] = item;
                SaveToCart(items);
            }

            return items[index].AddIns;
        }

        public static List<AddIns> AddInRemove(Items item, AddIns newAddIns)
        {
            List<Items> items = GetAllCartItems();
            int index = items.FindIndex(existingItem => existingItem.CartNumber == item.CartNumber);
            List<AddIns> addInsList = items[index].AddIns;
            var existingAddIns = addInsList.FirstOrDefault(addIns => addIns.Id.Equals(newAddIns.Id));
            if (existingAddIns != null)
            {
                addInsList.Remove(existingAddIns);

            }
            item.AddIns = addInsList;
            if (index != -1)
            {
                items[index] = item;
                SaveToCart(items);
            }

            return items[index].AddIns;
        }

        public static float CalculateTotalCartPrice(List<Items> cartItems)
        {
            float totalPrice = 0;

            foreach (var item in cartItems)
            {
                float addInTotalPrice = 0;

                totalPrice = totalPrice + item.Price;
                if (item.AddIns != null)
                {
                    foreach (var addIn in item.AddIns)
                    {
                        float eachPrice = (float)(addIn.Price * addIn.Quantity);

                        addInTotalPrice = addInTotalPrice + eachPrice;
                    }
                }
                totalPrice = totalPrice + addInTotalPrice;
            }

            return totalPrice;
        }
        public static List<Order> Order(float totalPrice, List<Items> items,string description)
        {
            List<Order> orders = GetAll();

            orders.Add(
                new Order
                {
                        
                        TotalPrice = totalPrice,
                        Items = items,
                        Description = description
                }
            );
            SaveAll(orders);
            RenewCartAfterOrder(items);
            return orders;
        }
        public static List<Items> RenewCartAfterOrder(List<Items> removeItems)
        {
            List<Items> items = GetAllCartItems();
            items.Clear();  
            SaveToCart(items);

            return items;
        }
        private static void SaveAll(List<Order> orders)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appOrdersFilePath = Utils.GetAppOrdersFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(appOrdersFilePath, json);
        }
        public static List<Order> CancelOrder(Guid id)
        {
            List<Order> orders = GetAll();
            Order order = orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            orders.Remove(order);
            SaveAll(orders);

            return orders;
        }

        public static bool PayForOrder(Order order, double TotalPrice, String membercredentials)
        {
            List<Order> orders = GetAll();
            List<Sales> sales = SaleServices.GetAll();
            List<Members> members = MemberServices.GetAll();
            int indexOfOrder = orders.FindIndex(existingItem => existingItem.Id == order.Id);
            bool redemption = false;
            //For TotalPrice calculation if the customer has membership
            if (membercredentials != null)
            {
                Members memberExists = members.FirstOrDefault(x => x.Username == membercredentials);
               
                Members memberPhoneNumberExists = members.FirstOrDefault(x => x.PhoneNumber == membercredentials);
             
                if (memberExists != null || memberPhoneNumberExists != null)
                {
                    
                    if (memberExists != null)
                    {
                        List<Sales> memberSales = GetTenRecentPurchases(memberExists);
                        
                        if(memberSales.Count > 0 & memberSales.Count == 10 & memberSales[0].Member.Redemption == true)
                        {
                            redemption = true;
                            memberPhoneNumberExists.Redemption = true;
                            order.Items[0].Price = 0;
                            TotalPrice = CalculateTotalCartPrice(order.Items);
                        }

                        if (memberExists.DicountEndDate != null)
                        {
                            if (memberExists.DicountEndDate.Value.Date >= DateTime.Today)
                            {
                                if (IsMemberEligibleForDiscount(memberExists))
                                {
                                    memberExists.DicountStartDate = DateTime.Today;
                                    memberExists.DicountEndDate = DateTime.Today.AddDays(30);
                                    TotalPrice = TotalPrice + (TotalPrice * 0.1);
                                }
                                else
                                {
                                    memberExists.DicountStartDate = null;
                                    memberExists.DicountEndDate = null;
                                }

                            }
                            else
                            {
                                TotalPrice = TotalPrice + (TotalPrice * 0.1);
                            }
                        }
                        else
                        {
                            if (IsMemberEligibleForDiscount(memberExists))
                            {
                                memberExists.DicountStartDate = DateTime.Today;
                                memberExists.DicountEndDate = DateTime.Today.AddDays(30);
                                TotalPrice = TotalPrice + (TotalPrice * 0.1);
                            }
                            else
                            {
                                memberExists.DicountStartDate = null;
                                memberExists.DicountEndDate = null;
                            }
                        }
                        TotalPrice = TotalPrice - (TotalPrice * (memberExists.DiscountPercent/100));
                        
                        sales.Add(
                            new Sales
                        {
                            Items = order.Items,
                            Member = memberExists,
                            TotalPrice = TotalPrice

                        }
                         );
                        SaleServices.SaveAll(sales);
                        
                        
                    }
                    else if (memberPhoneNumberExists != null) {
                        List<Sales> memberSales = GetTenRecentPurchases(memberPhoneNumberExists);
                       
                        bool allRedemptionsFalse = memberSales.All(s => s.Member.Redemption == false);
                          
                        if (memberSales.Count > 0 & allRedemptionsFalse == true)
                        {
                            redemption = true;
                            memberPhoneNumberExists.Redemption = true;
                            order.Items[0].Price = 0;
                            TotalPrice = CalculateTotalCartPrice(order.Items);
                        }
                        if (memberPhoneNumberExists.DicountEndDate != null)
                        {
                            if (memberPhoneNumberExists.DicountEndDate.Value.Date >= DateTime.Today)
                            {
                                if (IsMemberEligibleForDiscount(memberPhoneNumberExists))
                                {
                                    memberPhoneNumberExists.DicountStartDate = DateTime.Today;
                                    memberPhoneNumberExists.DicountEndDate = DateTime.Today.AddDays(30);
                                    TotalPrice = TotalPrice + (TotalPrice * 0.1);
                                }
                                else
                                {
                                    memberPhoneNumberExists.DicountStartDate = null;
                                    memberPhoneNumberExists.DicountEndDate = null;
                                }

                            }
                            else
                            {
                                TotalPrice = TotalPrice + (TotalPrice * 0.1);
                            }
                        }
                        else
                        {
                            if (IsMemberEligibleForDiscount(memberPhoneNumberExists))
                            {
                                memberPhoneNumberExists.DicountStartDate = DateTime.Today;
                                memberPhoneNumberExists.DicountEndDate = DateTime.Today.AddDays(30);
                                TotalPrice = TotalPrice + (TotalPrice * 0.1);
                            }
                            else
                            {
                                memberPhoneNumberExists.DicountStartDate = null;
                                memberPhoneNumberExists.DicountEndDate = null;
                            }
                        }

                        TotalPrice = TotalPrice - (TotalPrice * (memberPhoneNumberExists.DiscountPercent / 100));
                        
                        sales.Add(
                          new Sales
                          {
                              Items = order.Items,
                              Member = memberPhoneNumberExists,
                              TotalPrice = TotalPrice

                          }
                      );
                        SaleServices.SaveAll(sales);
                        
                    }
                   
                    order.TotalPrice = TotalPrice;
                }
                else
                {
                    throw new Exception("Member doesnot exist.");
                }
            }
            else
            {
                
                sales.Add(
                          new Sales
                          {
                              Items = order.Items,
                              TotalPrice = order.TotalPrice

                          }
                      );
                SaleServices.SaveAll(sales);                
            }
            orders = CancelOrder(order.Id);
            return (redemption);

        }
        public static List<Sales> GetTenRecentPurchases(Members member)
        {

            List<Sales> sales = SaleServices.GetAll();

            //all sales for a single memeber
            var memberSales = sales.Where(s => s.Member?.Id == member.Id).ToList();
            
            //sorted by date in descending order 
            var sortedSales = memberSales.OrderByDescending(s => s.CreatedAt).ToList();

            
            var recentTenPurchases = sortedSales.Take(2).ToList();
            
            return recentTenPurchases;
        }

        public static bool IsMemberEligibleForDiscount(Members member)
        {
            List<Sales> sales = SaleServices.GetAll();

            // Filter sales for the specific member
            var memberSales = sales.Where(s => s.Member?.Id == member.Id).ToList();

            // Sort sales by date in descending order
            var sortedSales = memberSales.OrderByDescending(s => s.CreatedAt).ToList();

            // Take the latest purchase date
            var latestPurchaseDate = sortedSales.FirstOrDefault()?.CreatedAt;

            if (latestPurchaseDate != null)
            {
                // Calculate the start date as one day after the latest purchase date
                var startDate = latestPurchaseDate.Value.AddDays(1);

                // Calculate the end date as one month from the start date
                var endDate = startDate.AddMonths(1);

                // Check if the member has daily purchases (excluding weekends) for the following month
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // Skip weekends (Saturday and Sunday)
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        // Check if there is no purchase on this date
                        if (!sortedSales.Any(s => s.CreatedAt.Date == date.Date))
                        {
                            return false; // Member didn't purchase on this day
                        }
                    }
                }

                return true; // Member has daily purchases (excluding weekends) for the following month
            }

            return false; // No purchases found for the member
        }

        public static List<Items> GetOrderItems(Order order)
        {
            
            List<Items> item = order.Items;

            return item;
        }

        public static void ToBeUpdated(Order order)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appTempFilePath = Utils.GetAppTemporaryFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(order);
            File.WriteAllText(appTempFilePath, json);
        }

        public static Order GetAllFromTemp()
        {
            string appTempFilePath = Utils.GetAppTemporaryFilePath();
            if (!File.Exists(appTempFilePath))
            {
                return new Order();
            }

            var json = File.ReadAllText(appTempFilePath);
           
                return JsonSerializer.Deserialize<Order>(json);
            
           
        }

        public static Order TempClear(Order order)
        {
            Order temp = new Order();   

            ToBeUpdated(temp);


            return temp;
        }
    }
}
