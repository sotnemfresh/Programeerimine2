using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private Toode _selectedList;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(Toode selectedList)
        {
            _selectedList = selectedList;
            if (_selectedList == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentTitle = "";
                _mainView.CurrentFotoUrl = string.Empty;
                _mainView.CurrentPrice = 0;
                _mainView.CurrentStockQuantity = 0;
            }
            else
            {
                _mainView.CurrentId = _selectedList.Id;
                _mainView.CurrentTitle = _selectedList.Name;
                _mainView.CurrentFotoUrl = _selectedList.FotoURL;
                _mainView.CurrentPrice = _selectedList.Price;
                _mainView.CurrentStockQuantity = _selectedList.StockQuantity;
            }
        }

        public async Task Save()
        {
            var toode = new Toode();
            toode.Id = _mainView.CurrentId;
            toode.Name = _mainView.CurrentTitle;
            toode.FotoURL = _mainView.CurrentFotoUrl;
            toode.Price = _mainView.CurrentPrice;
            toode.StockQuantity = _mainView.CurrentStockQuantity;

            var result = await _apiClient.Save(toode);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
            }
            await LoadData();
        }

        public async Task Add()
        {
            // Create a new empty product immediately and refresh the list
            var toode = new Toode
            {
                Id = 0,
                Name = _mainView.CurrentTitle ?? string.Empty
            };

            var result = await _apiClient.Save(toode);
            if (result == null)
            {
                var err = new OperationResult();
                err.AddError("Server returned no response.");
                _mainView.ShowError("Viga lisamisel", err);
                return;
            }

            if (result.HasErrors)
            {
                _mainView.ShowError("Viga lisamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            var message = "Oled kindel, et soovid kustutada " + _mainView.CurrentTitle + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            var id = _mainView.CurrentId;
            var result = await _apiClient.Delete(id);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga kustutamisel", result);
            }

            await LoadData();
        }
    }
}
