using AreaBg.Web.ViewModels.Father;
using AreaBg.Web.ViewModels.Json;
using AreaBg.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AreaBg.Web.Controllers
{
    public class FatherController : Controller
    {
        private readonly IHttpClientFactory ClientFactory;
        private IConfiguration Configuration { get; }

        public FatherController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            this.ClientFactory = clientFactory;
            this.Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index(double amount, string orderId)
        {
            var amountForDsk = (int)(amount * 100);

            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(this.Configuration["DskConfigFather"], amountForDsk, orderId));

            var client = this.ClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var responseStream = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<BankReturnJson>(responseStream);
            var redirectLink = json.formUrl;

            return Redirect(redirectLink);
        }

        public async Task<IActionResult> Index(string orderId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(this.Configuration["DskStatus"], orderId));

            var client = this.ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<BankStatusJson>(responseStream);

            return Redirect($"http://bgbook.bg/retyrn_daian.php?orderID={json.OrderNumber}&orderStatus={json.OrderStatus}&amount={json.depositAmount}");
        }
    }
}
