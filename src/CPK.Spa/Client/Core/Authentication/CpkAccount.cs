using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace CPK.Spa.Client.Core.Authentication
{
    public class CpkAccount : RemoteUserAccount
    {
        [JsonPropertyName("amr")]
        public string[] AuthenticationMethod { get; set; }

        [JsonPropertyName("roles")]
        public string Roles { get; set; }
    }
}