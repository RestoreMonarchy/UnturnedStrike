using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class File
    {
        public File() { }
        public File(string name, string mimeType, byte[] data, long size)
        {
            Name = name;
            MimeType = mimeType;
            Data = data;
            Size = size;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
