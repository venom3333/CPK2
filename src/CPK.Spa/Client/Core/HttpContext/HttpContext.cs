using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using CPK.Spa.Client.Core.Models;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace CPK.Spa.Client.Core.HttpContext
{
    public sealed class HttpContext : IHttpContext
    {
        //private static readonly JsonSerializerOptions OPTIONS = new JsonSerializerOptions()
        //{
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    PropertyNameCaseInsensitive = true
        //};

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
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            ), auth);

        public Task<(T, string)> PostFormAsync<T>(string url, MultipartFormDataContent data, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).PostAsync(
                url,
                data
            ), auth);

        public async Task<(T, string)> PostFileAsync<T>(string url, FileModel file, bool auth = true)
        {
            var client = GetClient(auth);
            //file.Content.Position = 0;
            using (var form = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                form.Add(new StringContent(file.ContentType), nameof(file.ContentType));
                form.Add(new StringContent(file.FileName), nameof(file.FileName));
                form.Add(new StringContent(file.Size.ToString()), nameof(file.Size));
                form.Add(new StreamContent(file.Content), "file", file.FileName);

                //var fileParameter = new FileParameter
                //{
                //    ContentLength = file.Size,
                //    ContentType = file.Type,
                //    FileName = file.Name,
                //    Name = "file",
                //    Writer = (s) =>
                //    {
                //        file.WriteToStreamAsync(s);
                //    }
                //};

                var result = await SendAsync<T>(() => client.PostAsync(url, form), auth);
                return result;
            }
        }

        public Task<(T, string)> GetAsync<T>(string url, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).GetAsync(url), auth);

        public Task<(T, string)> DeleteAsync<T>(string url, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).DeleteAsync(url), auth);

        public Task<(T, string)> PutAsync<T>(string url, object data, bool auth = true) =>
            SendAsync<T>(() => GetClient(auth).PutAsync(
                url,
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
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
                return (JsonConvert.DeserializeObject<T>(json), null);
            }
        }

        private HttpClient GetClient(bool auth = true) =>
            auth ?
            _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient") :
            _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("PublicClient");

    }
}
