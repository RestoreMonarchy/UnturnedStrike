using KillFeedPlugin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnturnedStrike.Plugin.Models;
using UnturnedStrikeAPI;

namespace KillFeedGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync("https://www.unturnedstrike.com/api/weapons");

            var list = JsonConvert.DeserializeObject<List<Weapon>>(content);


            var jsonList = new List<JsonWeaponData>();
            var killFeed = new List<WeaponUnicode>();
            foreach (var weapon in list)
            {
                try
                {
                    killFeed.Add(new WeaponUnicode((ushort)weapon.ItemId, weapon.IconUnicode));
                    Enum.TryParse<EWeaponTeam>(weapon.Team, out var result);

                    var json = new JsonWeaponData()
                    {

                        Id = weapon.Id,
                        Category = weapon.Category,
                        Name = weapon.Name,
                        Price = weapon.Price,
                        Team = result,
                        KillRewardMultiplier = (float)weapon.KillRewardMultiplier,
                        Weapon = new JsonWeaponGunItem()
                        {
                            ItemId = (ushort)weapon.ItemId
                        },
                        Items = new JsonWeaponItem[]
                        {
                        new JsonWeaponItem(weapon.MagazineId == null ? (ushort)0 : (ushort)weapon.MagazineId, weapon.MagazineAmount)
                        }
                    };
                    jsonList.Add(json);
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }

            System.IO.File.Delete("WeaponsData.json");
            System.IO.File.AppendAllText("WeaponsData.json", JsonConvert.SerializeObject(jsonList, Newtonsoft.Json.Formatting.Indented));

            var ser = new XmlSerializer(typeof(List<WeaponUnicode>));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { Indent = true }))
                {
                    ser.Serialize(writer, killFeed);
                    xml = sww.ToString(); // Your XML
                }
            }

            System.IO.File.Delete("killfeed.xml");
            System.IO.File.AppendAllText("killfeed.xml", xml);
        }

    }
}
