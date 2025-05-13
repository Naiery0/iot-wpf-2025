using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Models
{
    public class Book : ObservableObject
    {
        private int _idx;
        private string _names;
        private string _division;
        private string _dnames;
        private string _author;
        private DateTime _ReleaseDate;
        private string _isbn;
        private decimal _price;

        public int Idx { get => _idx; set => SetProperty(ref _idx, value); }
        public string Names { get => _names; set => SetProperty(ref _names, value); }
        public string Division { get => _division; set => SetProperty(ref _division, value); }
        public string Author { get => _author; set => SetProperty(ref _author, value); }
        public DateTime ReleaseDate { get => _ReleaseDate; set => SetProperty(ref _ReleaseDate, value); }
        public string Isbn { get => _isbn; set => SetProperty(ref _isbn, value); }
        public decimal Price { get => _price; set => SetProperty(ref _price, value); }
        public string DNames { get => _dnames; set => SetProperty(ref _dnames, value); }
    }
}
