using System.Collections.ObjectModel;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private readonly ObservableCollection<Toode> _data;

        private Toode _selectedItem;        

        public MainWindowViewModel() : this(new ApiClient())
        {
            
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _data = new ObservableCollection<Toode>();
            _apiClient = apiClient;
        }

        public async Task LoadDataAsync()
        {
            var data = await _apiClient.List(1, 100);
            
            _data.Clear();
            foreach (var item in data.Value.Results)
            {
                _data.Add(item);
            }
        }

        public ObservableCollection<Toode> Data
        {
            get
            {
                return _data;
            }
        }

        public Toode SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }
    }
}