using Impact.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Backend
{
    public static class Requests
    {
#if ANDROID
        public static string URL { get; set; } = "https://lmapkt.com"; // HTTPS hosting
#elif LINUX
        public static string URL { get; set; } = "https://lmapkt.com"; // HTTPS hosting
#else
        public static string URL { get; set; } = "http://localhost:8000";   // Local windows.
        // public static string URL { get; set; } = "https://lmapkt.com"; // HTTPS hosting
#endif

        /* SCANNER */
        public static string Scanner_Register_Link =>
            $"{URL}/scanner/register";

        public static async Task<(Resp_Scanner_Register?, bool, string?)> Post_Scanner_Register(Req_Scanner_Register req)
        {
            bool result = false;
            string? error_mes = string.Empty;
            Resp_Scanner_Register? resp = null;

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Scanner_Register_Link, req);
                if (response.IsSuccessStatusCode)
                {
                    resp = await response.Content.ReadFromJsonAsync<Resp_Scanner_Register>();
                    result = true;
                }
                else
                {
                    error_mes = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                error_mes = ex.Message;
            }
            finally
            {
                ;
            }
            return (resp, result, error_mes);
        }

        public static void Post_Scanner_CheckIn()
        {
            try
            {

            }
            catch (Exception e)
            {
            }
            finally
            {

            }
        }

        public static void Post_Scanner_MemberInfo()
        {
            try
            {

            }
            catch (Exception e)
            {
            }
            finally
            {

            }
        }
    }
}
