using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class Sura
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set;}
        public string Meaning { get; set; }
        public string Description { get; set; }
        public int AyaCount { get; set; }
        public bool RevealedIn { get; set; }

    }
}
