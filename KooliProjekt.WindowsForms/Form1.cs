using System.Collections;
using System.Net.Http.Json;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadTooded();
        }

        private void LoadTooded()
        {
            var url = "http://localhost:5086/api/Tooted/List";
            url += "?page=1&pageSize=10";

            using var client = new HttpClient();
            var response = client.GetFromJsonAsync<OperationResult<PagedResult<Toode>>>(url).Result;
            dataGridView1.DataSource = response.Value.Results;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
