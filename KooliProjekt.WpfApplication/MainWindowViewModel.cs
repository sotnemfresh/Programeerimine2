using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private readonly ObservableCollection<Toode> _data;
        private readonly IDialogProvider _dialogProvider;

        public ICommand AddNewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private Toode _selectedItem;

        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider())
        {

        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _data = new ObservableCollection<Toode>();
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;

            AddNewCommand = new RelayCommand<Toode>(
                toode =>
                {
                    SelectedItem = new Toode();
                });
            SaveCommand = new RelayCommand<Toode>(
                async toode =>
                {
                    var result = await _apiClient.Save(toode);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot save data", result);
                        return;
                    }

                    SelectedItem = null;
                    await LoadData();
                },
                toode =>
                {
                    return SelectedItem != null;
                });
            DeleteCommand = new RelayCommand<Toode>(
                async toode =>
                {
                    var canDelete = _dialogProvider.Confirm("Are you sure you want to delete this item?");
                    if (!canDelete)
                    {
                        return;
                    }

                    var result = await _apiClient.Delete(toode.Id);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot delete data", result);
                        return;
                    }
                    SelectedItem = null;
                    await LoadData();
                },
                toode =>
                {
                    return SelectedItem != null && SelectedItem.Id != 0;
                });
        }

        public async Task LoadData()
        {
            var data = await _apiClient.List(1, 100);
            _data.Clear();

            if (data.HasErrors)
            {
                ShowError("Cannot load data", data);
                return;
            }

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
        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            _dialogProvider.ShowError(error);
        }
    }

    //public class AddNewCommand : ICommand
    //{
    //    public event EventHandler CanExecuteChanged;

    //    public bool CanExecute(object parameter)
    //    {
    //        return true;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        MessageBox.Show("Execute");
    //    }
    //}
}
