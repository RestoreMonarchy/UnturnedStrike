using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillFeedPlugin.Models
{
    public class KillFeedItem
    {
        public KillFeedItem(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
