using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;

namespace UnturnedStrike.Plugin.Providers
{
    public class JsonHostagesProvider : IHostagesProvider
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private DataStorage<List<Hostage>> HostagesDataStorage { get; set; }

        public IEnumerable<Hostage> Hostages => hostages;
        private List<Hostage> hostages;

        public JsonHostagesProvider()
        {
            HostagesDataStorage = new DataStorage<List<Hostage>>(pluginInstance.Directory, $"HostagesData.{pluginInstance.MapName}.json");
        }

        private int NewID
        {
            get
            {
                int lastId = hostages.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
                return ++lastId;
            }
        }

        public void ReloadHostages()
        {
            hostages = HostagesDataStorage.Read();
            if (hostages == null)
                hostages = new List<Hostage>();
        }

        public void AddHostage(Hostage hostage)
        {
            hostage.Id = NewID;
            hostages.Add(hostage);
            HostagesDataStorage.Save(hostages);
        }

        public bool RemoveHostage(int hostageId)
        {
            int count = hostages.RemoveAll(x => x.Id == hostageId);
            if (count == 0)
                return false;

            HostagesDataStorage.Save(hostages);
            return true;
        }
    }
}
