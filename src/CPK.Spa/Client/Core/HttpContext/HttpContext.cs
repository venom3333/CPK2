using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CPK.Spa.Client.Core.HttpContext
{
    public sealed class HttpContext : IHttpContext
    {
        private static readonly JsonSerializerOptions OPTIONS = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationManager _navigation;

        public HttpContext(
            IServiceProvider serviceProvider,
            NavigationManager navigation
            )
        {
            _serviceProvider = serviceProvider;
            _navigation = navigation;
        }
        public Task<(T, string)> PostAsync<T>(string url, object data, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).PostAsync(
                url,
                new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            ), auth);

        public Task<(T, string)> GetAsync<T>(string url, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).GetAsync(url), auth);

        public Task<(T, string)> DeleteAsync<T>(string url, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).DeleteAsync(url), auth);

        public Task<(T, string)> PutAsync<T>(string url, object data, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).PutAsync(
                url,
                new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
                ), auth);

        public async Task<(T, string)> SendAsync<T>(Func<Task<HttpResponseMessage>> request, bool auth)
        {
            var response = await request();
            return await Handle<T>(response);
        }

        private async Task<(T, string)> Handle<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    _navigation.NavigateTo($"authentication/login?returnUrl={_navigation.Uri}");
                if (string.IsNullOrWhiteSpace(json))
                    return (default(T), response.StatusCode.ToString());
                return (default(T), json);
            }
            else
            {
                return (JsonSerializer.Deserialize<T>(json, OPTIONS), null);
            }
        }

        private HttpClient GetClient(bool auth = true) =>
            auth?
            _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient") :
            _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("PublicClient");
    }
}
