using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BisleriumCafe.Models;
using System.Text.Json;


namespace BisleriumCafe.Services
{
    public class ItemServices
    {
        public static List<Items> GetAll()
        {
            string appItemsFilePath = Utils.GetAppItemsFilePath();
            if (!File.Exists(appItemsFilePath))
            {
                return new List<Items>();
            }

            var json = File.ReadAllText(appItemsFilePath);
            if (IsValidJson(json)) {
                return JsonSerializer.Deserialize<List<Items>>(json);
            }
            else { return new List<Items>(); }
        }

        private static bool IsValidJson(string strInput)
        {
            try
            {
                JsonSerializer.Deserialize<List<Items>>(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static List<Items> Create(string name, float price, ItemCategory itemCategory)
        {
            List<Items> items = GetAll();
            bool itemExists = items.Any(x => x.Name.ToLower() == name.ToLower());

            if (itemExists)
            {
                throw new Exception("Item already exists.");
            }

            items.Add(
                new Items
                {
                    Name = name,
                    Price = price,
                    ItemCategory = itemCategory
                }
            );
            SaveAll(items);
            return items;
        }
        private static void SaveAll(List<Items> items)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appItemsFilePath = Utils.GetAppItemsFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(appItemsFilePath, json);
        }

       
        public static List<Items> Delete(Guid id)
        {
            List<Items> items = GetAll();
            Items item = items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new Exception("User not found.");
            }

            items.Remove(item);
            SaveAll(items);

            return items;
        }
        public static List<Items> Update(Guid id, string name, float price,  ItemCategory itemCategory)
        {
            List<Items> items = GetAll();
            Items item = items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new Exception("User not found.");
            }

            item.Name = name;
            item.Price = price;
            item.ItemCategory = itemCategory;
            SaveAll(items);

            return items;
        }
    }
}
