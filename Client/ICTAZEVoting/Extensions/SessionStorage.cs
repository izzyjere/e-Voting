﻿using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using ICTAZEVoting.BlockChain.IO;

using LevelDB;
namespace ICTAZEVoting.Extensions
{
    public static class SessionStorage
    {
        static readonly StorageContext SecureStorage = new(Path.Combine(Environment.CurrentDirectory, "data"));

        public static async Task SaveItemEncryptedAsync<T>(string key, T item)
        {
            var itemJson = JsonSerializer.Serialize(item);
            var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
            var base64Json = Convert.ToBase64String(itemJsonBytes);
            await SecureStorage.SetAsync(key, base64Json);
        }

        public static async Task<T> ReadEncryptedItemAsync<T>(string key)
        {
            var base64Json = await SecureStorage.GetAsync(key);
            if (string.IsNullOrEmpty(base64Json))
            {
                return default;
            }
            var itemJsonBytes = Convert.FromBase64String(base64Json);
            var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
            var item = JsonSerializer.Deserialize<T>(itemJson);
            return item;
        }

        internal static Task RemoveItemAsync(string key)
        {
            SecureStorage.Remove(key);
            return Task.CompletedTask;
        }
    }
}
