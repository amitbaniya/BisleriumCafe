﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class Utils
    {
        private const char _segmentDelimiter = ':';
        public static string GetAppDirectoryPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Bislerium-Cafe"
            );
        }

        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        public static string GetAppItemsFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "items.json");
        }

        public static string GetAppAddInsFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "addIns.json");
        }
        public static string GetAppOrdersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "orders.json");
        }
        public static string GetAppCartFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "cart.json");
        }
        public static string GetAppMembersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "members.json");
        }
        public static string GetAppSalesFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "sales.json");
        }

        public static string GetAppTemporaryFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "temp.json");
        }

       

        public static string HashSecret(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            return string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }
        public static bool VerifyHash(string input, string hashString)
        {
            string[] segments = hashString.Split(_segmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}
