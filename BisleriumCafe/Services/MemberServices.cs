using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Models;

namespace BisleriumCafe.Services
{
    public class MemberServices
    {
        public static List<Members> GetAll()
        {
            string appMembersFilePath = Utils.GetAppMembersFilePath();
            if (!File.Exists(appMembersFilePath))
            {
                return new List<Members>();
            }

            var json = File.ReadAllText(appMembersFilePath);
            if (IsValidJson(json))
            {
                return JsonSerializer.Deserialize<List<Members>>(json);
            }
            else { return new List<Members>(); }
        }

        private static bool IsValidJson(string strInput)
        {
            try
            {
                JsonSerializer.Deserialize<List<Members>>(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static List<Members> Create(string name, string username, string phoneNumber, float discount)
        {
            List<Members> members = GetAll();
            bool memberExists = members.Any(x => x.Username == username);
            bool memberPhoneNumberExists = members.Any(x => x.PhoneNumber == phoneNumber);
            if (memberExists || memberPhoneNumberExists)
            {
                throw new Exception("member already exists.");
            }

            members.Add(
                new Members
                {
                    Name = name,
                    Username = username,
                    PhoneNumber = phoneNumber,
                    DiscountPercent = discount   
                }
            );
            SaveAll(members);
            return members;
        }
        private static void SaveAll(List<Members> members)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appMembersFilePath = Utils.GetAppMembersFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(members);
            File.WriteAllText(appMembersFilePath, json);
        }


        public static List<Members> Delete(Guid id)
        {
            List<Members> members = GetAll();
            Members member = members.FirstOrDefault(x => x.Id == id);

            if (member == null)
            {
                throw new Exception("Member not found.");
            }

            members.Remove(member);
            SaveAll(members);

            return members;
        }
        public static List<Members> Update(Guid id, string name, string username, string phoneNumber, float discount)
        {
            List<Members> members = GetAll();
            Members member = members.FirstOrDefault(x => x.Id == id);

            if (member == null)
            {
                throw new Exception("Member not found.");
            }
            member.Username = username;
            member.Name = name;
            member.PhoneNumber = phoneNumber;
            member.DiscountPercent = discount;
            SaveAll(members);

            return members;
        }
    }
}
