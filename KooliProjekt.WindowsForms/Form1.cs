using System.Collections;
using System.ComponentModel;
using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form, IMainView
    {
        private readonly IApiClient _apiClient;
        private MainViewPresenter _mainViewPresenter;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Toode> DataSource
        {
            get { return (IList<Toode>)dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Toode? SelectedItem
        {
            get { return dataGridView1.CurrentRow?.DataBoundItem as Toode; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentId
        {
            get { return idField?.Text != null && int.TryParse(idField.Text, out int id) ? id : -1; }
            set { if (idField != null) idField.Text = value.ToString(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentTitle
        {
            get { return titleField?.Text ?? string.Empty; }
            set { if (titleField != null) titleField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentFotoUrl
        {
            get { return fotoUrlField?.Text ?? string.Empty; }
            set { if (fotoUrlField != null) fotoUrlField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal CurrentPrice
        {
            get { return decimal.TryParse(priceField?.Text, out decimal price) ? price : 0; }
            set { if (priceField != null) priceField.Text = value.ToString("0.00"); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal CurrentStockQuantity
        {
            get { return decimal.TryParse(stockQuantityField?.Text, out decimal result) ? result : 0; }
            set { if (stockQuantityField != null) stockQuantityField.Text = value.ToString("0.00"); }
        }


        public void SetPresenter(MainViewPresenter presenter)
        {
            _mainViewPresenter = presenter;
        }

        public Form1(IApiClient apiClient)
        {
            _apiClient = apiClient;

            InitializeComponent();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            saveCommand.Click += SaveCommand_Click;
            addCommand.Click += AddCommand_Click;
            deleteCommand.Click += DeleteCommand_Click;
        }

        private async void DeleteCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Delete();
        }

        private async void AddCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Add();
        }

        private async void SaveCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Save();
        }

        public bool ConfirmDelete()
        {
            var message = "Oled kindel, et soovid kustutada " + titleField.Text + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (answer == DialogResult.Yes);
        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest
        // veateade ja näita seda kasutajale
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

            MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                _mainViewPresenter.SetSelection(null);
                return;
            }

            var selectedList = (Toode)dataGridView1.CurrentRow.DataBoundItem;
            _mainViewPresenter.SetSelection(selectedList);
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            await _mainViewPresenter.LoadData();
        }
    }
}
