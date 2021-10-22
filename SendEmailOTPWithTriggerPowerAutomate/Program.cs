using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SendEmailOTPWithTriggerPowerAutomate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.SendEmailForCodeVerificationAsync(174582, "ferddev21@gmail.com", "Ferdian Arjun", "https://prod-05.southeastasia.logic.azure.com:443/workflows/e4f6e041707942079ccb1d634aa937a8/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=28ZpJEHUiYcAQ6M8cgQgTs3qVO5kzVfd71vc0lZINNo");
        }

        public async Task SendEmailForCodeVerificationAsync(int verificationCode, string toAddress, string username, string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = $"{{\"Email\": \"{toAddress}\",\"Subject\":\"Email Verification Code\",\"OTP\":\"{verificationCode}\",\"Username\":\"{username}\"}}";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await MakeRequestAsync(request, client);
                Console.WriteLine(response);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception();
            }
        }

        public async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                // empty responseString
            }

            return responseString;
        }
    }

    public class AuthenticationModel
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}
