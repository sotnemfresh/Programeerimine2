namespace KooliProjekt.WpfApplication
{
    public class Toode : NotifyPropertyChangedBase
    {
        private int _id;
        private string _name;
        private string _fotoURL;
        private decimal _price;
        private decimal _stockQuantity;

        public int Id
        {
            get => _id;
            set { _id = value; NotifyPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(); }
        }

        public string FotoURL
        {
            get => _fotoURL;
            set { _fotoURL = value; NotifyPropertyChanged(); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; NotifyPropertyChanged(); }
        }

        public decimal StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; NotifyPropertyChanged(); }
        }
    }
}