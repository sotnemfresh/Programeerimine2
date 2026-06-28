using KooliProjekt.WindowsForms.Api;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace KooliProjekt.WindowsForms.Api
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Tooted/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<Toode>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<Toode>>>(body);
                return result;
            }
            catch (Exception ex)
            {
                var result = new OperationResult<PagedResult<Toode>>();
                result.AddError("Ühendus serveriga ebaõnnestus: " + ex.Message);
                return result;
            }
        }

        public async Task<OperationResult> Save(Toode list)
        {
            var url = _baseUrl + "Save";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = JsonContent.Create(list)
                };            
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<OperationResult>(body);
                return result;
            }
            catch (Exception ex)
            {
                var result = new OperationResult();
                result.AddError("Ühendus serveriga ebaõnnestus: " + ex.Message);
                return result;
            }
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = JsonContent.Create(new { id = id })
                };
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<OperationResult>(body);
                return result;
            }
            catch (Exception ex)
            {
                var result = new OperationResult();
                result.AddError("Ühendus serveriga ebaõnnestus: " + ex.Message);
                return result;
            }
        }
    }
}