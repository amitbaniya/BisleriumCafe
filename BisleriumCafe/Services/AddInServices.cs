using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BisleriumCafe.Models;
using System.Text.Json;


namespace BisleriumCafe.Services
{
    public class AddInServices
    {
        public static List<AddIns> GetAll()
        {
            string appAddInsFilePath = Utils.GetAppAddInsFilePath();
            if (!File.Exists(appAddInsFilePath))
            {
                return new List<AddIns>();
            }

            var json = File.ReadAllText(appAddInsFilePath);
            if (IsValidJson(json)) {
                return JsonSerializer.Deserialize<List<AddIns>>(json);
            }
            else { return new List<AddIns>(); }
        }

        private static bool IsValidJson(string strInput)
        {
            try
            {
                JsonSerializer.Deserialize<List<AddIns>>(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static List<AddIns> Create(string name, float price)
        {
            List<AddIns> addIns = GetAll();
            bool addInExists = addIns.Any(x => x.Name.ToLower() == name.ToLower());

            if (addInExists)
            {
                throw new Exception("Add-In already exists.");
            }

            addIns.Add(
                new AddIns
                {
                    Name = name,
                    Price = price,
                }
            );
            SaveAll(addIns);
            return addIns;
        }
        private static void SaveAll(List<AddIns> addIns)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appAddInsFilePath = Utils.GetAppAddInsFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(addIns);
            File.WriteAllText(appAddInsFilePath, json);
        }

        
        public static List<AddIns> Delete(Guid id)
        {
            List<AddIns> addIns = GetAll();
            AddIns addIn = addIns.FirstOrDefault(x => x.Id == id);

            if (addIn == null)
            {
                throw new Exception("Add-In not found.");
            }

            addIns.Remove(addIn);
            SaveAll(addIns);

            return addIns;
        }
        public static List<AddIns> Update(Guid id, string name, float price)
        {
            List<AddIns> addIns = GetAll();
            AddIns addIn = addIns.FirstOrDefault(x => x.Id == id);

            if (addIn == null)
            {
                throw new Exception("Add-In not found.");
            }

            addIn.Name = name;
            addIn.Price = price;
            SaveAll(addIns);

            return addIns;
        }
    }
}
