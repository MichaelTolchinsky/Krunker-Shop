using ConsoleAppDataBSela.Model;
using Krunker.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Krunker.DAL.DataAccess
{
    public class Data
    {
        string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "KrunkerFile.json");
        string path2 = Path.Combine(ApplicationData.Current.LocalFolder.Path, "KrunkerFileCarts.json");
        /// <summary>
        /// Create json file
        /// </summary>
        public void CreateJson()
        {
            if (!File.Exists(path))
                using (FileStream file = new FileStream(path, FileMode.Create)) { file.Close(); }
            if (!File.Exists(path2))
                using (FileStream file = new FileStream(path2, FileMode.Create)) { file.Close(); }
        }
        /// <summary>
        /// Writes to json file,and overrides previus json
        /// </summary>
        /// <param name="items"></param>
        /// <param name="cartitems"></param>
        /// <returns></returns>
        public bool WriteToJson(List<AbstractItem> items, List<ShoppingCartItems> cartitems)
        {
            bool flag = false;
            if (File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(items, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                File.WriteAllText(path, json);
                flag = true;
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
        /// <summary>
        /// Read from json file
        /// </summary>
        /// <returns></returns>
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
