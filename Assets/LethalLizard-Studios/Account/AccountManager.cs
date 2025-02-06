using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class AccountManager
{
    private static string accountsFolderPath = Path.Combine(Application.dataPath, "Accounts");

    static AccountManager()
    {
        if (!Directory.Exists(accountsFolderPath))
        {
            Directory.CreateDirectory(accountsFolderPath);
        }
    }

    public static string GenerateUniqueIdentification()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        HashSet<string> existingIds = new HashSet<string>(LoadAllAccounts().Select(a => a.identification));
        string newId;

        do
        {
            newId = new string(Enumerable.Range(0, 6).Select(_ => chars[UnityEngine.Random.Range(0, chars.Length)]).ToArray());
        } while (existingIds.Contains(newId));

        return newId;
    }

    public static void SaveAccount(Account account)
    {
        string filePath = Path.Combine(accountsFolderPath, $"{account.identification}.json");
        string jsonData = JsonUtility.ToJson(account, true);

        File.WriteAllText(filePath, jsonData);
        Debug.Log($"Account saved: {filePath}");
    }

    public static Account LoadAccount(string identification)
    {
        string filePath = Path.Combine(accountsFolderPath, $"{identification}.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Account>(jsonData);
        }
        else
        {
            Debug.LogWarning($"Account not found: {filePath}");
            return null;
        }
    }

    public static List<Account> LoadAllAccounts()
    {
        List<Account> accounts = new List<Account>();

        if (Directory.Exists(accountsFolderPath))
        {
            string[] files = Directory.GetFiles(accountsFolderPath, "*.json");

            foreach (string file in files)
            {
                string jsonData = File.ReadAllText(file);
                Account account = JsonUtility.FromJson<Account>(jsonData);
                accounts.Add(account);
            }
        }

        return accounts;
    }
}
