using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using ServiceStack.Text;

namespace DowJones.SyntaxHost.Modules
{
    public class QrCodeModule : NancyModule
    {
        public QrCodeModule()
        {
            Get["/QrCode"] = parameters => View["QrCode"];

            Post["/QrCode", true] = async (x, ct) =>
            {
                var link = await GetQrCode(ct);
                var model = new { QrPath = link };
                return View["QrCode", model];
            };
        }

        private static async Task<string> GetQrCode(CancellationToken ct)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Mashape-Authorization", "qUxvw1qOWI0FSwSWmaOV1oKDL67mwvmG");
            var response = await client.GetAsync(
                    "https://mutationevent-qr-code-generator.p.mashape.com/generate.php?content=http%3A%2F%2Fwww.nancyfx.org&quality=%3Cquality%3E&size=%3Csize%3E&type=url"
                    , ct);

            var stringContent = await response.Content.ReadAsStringAsync();
            ct.ThrowIfCancellationRequested();
            dynamic model = JsonObject.Parse(stringContent);

            return model["image_url"];
        }
    }
}