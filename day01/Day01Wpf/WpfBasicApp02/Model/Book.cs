using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBasicApp02.Model
{
    public class Book : INotifyCollectionChanged
    {
        public int Idx { get; set; }
        public string Division { get; set; }
        public string DNames { get; set; }
        public string Names { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Price { get; set; }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}
