using ConsoleAppDataBSela.Model;
using Krunker.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace Krunker.DAL.DataAccess
{
    public class Data
    {
        readonly string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "KrunkerFile.json");
        readonly string path2 = Path.Combine(ApplicationData.Current.LocalFolder.Path, "KrunkerFileCarts.json");

        // Create json file
        public void CreateJson()
        {
            if (!File.Exists(path))
                using (FileStream file = new FileStream(path, FileMode.Create)) { file.Close(); }
            if (!File.Exists(path2))
                using (FileStream file = new FileStream(path2, FileMode.Create)) { file.Close(); }
        }

        // Writes data to json file,and overrides previus json
        public bool WriteToJson(List<AbstractItem> items, List<ShoppingCartItems> cartitems)
        {
            bool flag;
            if (File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(items, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                File.WriteAllText(path, json);
            }
            if (File.Exists(path2))
            {
                string jsonCart = JsonConvert.SerializeObject(cartitems, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                File.WriteAllText(path2, jsonCart);
                flag = true;
            }
            else flag = false;

            return flag;
        }

        // Read Data from json file
        public Tuple<List<AbstractItem>, List<ShoppingCartItems>> ReadFromJson()
        {
            List<AbstractItem> ReturnList = null;
            List<ShoppingCartItems> cartList = null;
            if (File.Exists(path))
            {
                using (StreamReader jsonreader = new StreamReader(path))
                {
                    string json = jsonreader.ReadToEnd();
                    ReturnList = JsonConvert.DeserializeObject<List<AbstractItem>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                }
            }
            if (File.Exists(path2))
            {
                using (StreamReader jsonreader2 = new StreamReader(path2))
                {
                    string json2 = jsonreader2.ReadToEnd();
                    cartList = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(json2, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                }
            }
            return new Tuple<List<AbstractItem>, List<ShoppingCartItems>>(ReturnList, cartList);
        }
    }
}
