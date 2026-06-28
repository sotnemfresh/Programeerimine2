using System.Collections;
using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;

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
            var message = "Oled kindel, et soovid kustutada " + titleField.Text + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            var id = int.Parse(idField.Text);
            var result = await _apiClient.Delete(id);
            if (result.HasErrors)
            {
                ShowError("Viga kustutamisel", result);
            }

            await LoadTooted();
        }

        private async void AddCommand_Click(object sender, EventArgs e)
        {
            // Create a new empty product immediately and refresh the list
            var toode = new Toode
            {
                Id = 0,
                Name = titleField.Text ?? string.Empty
            };

            var result = await _apiClient.Save(toode);
            if (result == null)
            {
                var err = new OperationResult();
                err.AddError("Server returned no response.");
                ShowError("Viga lisamisel", err);
                return;
            }

            if (result.HasErrors)
            {
                ShowError("Viga lisamisel", result);
                return;
            }

            await LoadTooted();
        }

        private async void SaveCommand_Click(object sender, EventArgs e)
        {
            var toode = new Toode();
            toode.Id = int.Parse(idField.Text);
            toode.Name = titleField.Text;

            var result = await _apiClient.Save(toode);
            if (result.HasErrors)
            {
                ShowError("Viga salvestamisel", result);
            }
            await LoadTooted();
        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest
        // veateade ja näita seda kasutajale
        private void ShowError(string message, OperationResult result)
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
                return;
            }

            var selectedList = (Toode)dataGridView1.CurrentRow.DataBoundItem;
            if (selectedList == null)
            {
                return;
            }

            idField.Text = selectedList.Id.ToString();
            titleField.Text = selectedList.Name.ToString();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadTooted();
        }

        private async Task LoadTooted()
        {
            var response = await _apiClient.List(1, 100);

            if (response == null)
            {
                var err = new OperationResult<PagedResult<Toode>>();
                err.AddError("Server returned no response.");
                ShowError("Viga andmete laadimisel", err);
                dataGridView1.DataSource = null;
                return;
            }

            if (response.HasErrors)
            {
                ShowError("Viga andmete laadimisel", response);
                dataGridView1.DataSource = null;
                return;
            }

            var results = response.Value?.Results;
            if (results == null)
            {
                var err = new OperationResult<PagedResult<Toode>>();
                err.AddError("Server returned no data.");
                ShowError("Viga andmete laadimisel", err);
                dataGridView1.DataSource = null;
                return;
            }

            dataGridView1.DataSource = results;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void titleField_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
