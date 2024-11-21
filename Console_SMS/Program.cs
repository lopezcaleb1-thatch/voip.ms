using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Console_SMS
{


    public class VoipMS
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _url = "https://voip.ms/api/v1/rest.php"; // REST API Endpoint

        public VoipMS(string username, string password)
        {
            _username = username;
            _password = password;
        }


        public async Task SendSMSAsync(string did, string destination, string message)
        {
            using (HttpClient client = new HttpClient())
            {
                // Build query parameters
                var queryParams = new Dictionary<string, string>
                {
                    { "api_username", _username },
                    { "api_password", _password },
                    { "method", "sendSMS" }, // Method for sending SMS
                    { "did", did },
                    { "dst", destination },
                    { "message", message }
                };

                // Convert parameters to query string
                var queryString = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;

                string requestUrl = $"{_url}?{queryString}";

                try
                {
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("SMS Sent Successfully!");
                        Console.WriteLine("Response:");
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        string errorBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error details: {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public async Task GetSMSAsync(string fromDate, string toDate, int limit = 50, string did = null, string contact = null, int timezone = 0, bool allMessages = true)
        {
            using (HttpClient client = new HttpClient())
            {
                // Build query parameters
                var queryParams = new Dictionary<string, string>
                {
                    { "api_username", _username },
                    { "api_password", _password },
                    { "method", "getSMS" }, // Explicitly specify the method
                    { "from", fromDate },
                    { "to", toDate },
                    { "limit", limit.ToString() },
                    { "timezone", timezone.ToString() },
                    { "all_messages", allMessages ? "1" : "0" }
                };

                // Optional parameters
                if (!string.IsNullOrEmpty(did))
                {
                    queryParams["did"] = did;
                }

                if (!string.IsNullOrEmpty(contact))
                {
                    queryParams["contact"] = contact;
                }

                // Convert parameters to query string
                var queryString = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;

                string requestUrl = $"{_url}?{queryString}";

                try
                {
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response:");
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                        string errorBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error details: {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            string username = "";
            string password = "";

            var voipMS = new VoipMS(username, password);

            // Fetch SMS messages
            await voipMS.SendSMSAsync(
                did: "5175166401",
                destination: "5175997902",
                message: "Hello from Thatch's automated SMS system! When technology fails, we won't!"
            );
        }
    }
}
