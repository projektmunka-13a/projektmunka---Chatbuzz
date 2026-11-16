using ChatBuzz_admin_wpf.DTOs;
using System;
using System.Net.Http;

namespace ChatBuzz_admin_wpf
{
    public static class ApiClient
    {
        // A portot az API projekt Properties/launchSettings.json fájljából vedd
        public static readonly HttpClient Http = new HttpClient
        {
            //for development, use the localhost address of your API project
            BaseAddress = new Uri("https://localhost:7104/")
            //for production, use the deployed API address
            //BaseAddress = new Uri("http://localhost:5000/")
        };

        public static UserDTO CurrentUser { get; set; }
    }
}