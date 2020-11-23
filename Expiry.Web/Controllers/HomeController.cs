using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Expiry.Web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Expiry.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            var token = GetTokenFromSesssion();

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client
                .GetAsync("https://localhost:44356/api/reminders")
                .ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var reminders = JsonConvert.DeserializeObject<List<ReminderViewModel>>(content);

                return View(reminders);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Edit(Guid id)
        {
            var token = GetTokenFromSesssion();

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            if (id == null || id == Guid.Empty)
            {
                return View(new ReminderViewModel());
            }

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client
                .GetAsync($"https://localhost:44356/api/reminders/{id}")
                .ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var reminder = JsonConvert.DeserializeObject<ReminderViewModel>(content);

                return View(reminder);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public IActionResult Edit(Guid id, ReminderViewModel reminder)
        {

            var token = GetTokenFromSesssion();

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpContent = new StringContent(JsonConvert.SerializeObject(reminder, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (id != null && id != Guid.Empty)
            {
                response = client
                    .PutAsync($"https://localhost:44356/api/reminders/{id}", httpContent)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                response = client
                    .PostAsync($"https://localhost:44356/api/reminders", httpContent)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var token = GetTokenFromSesssion();

            if (string.IsNullOrEmpty(token))
            {
                return new JsonResult(new { StatusCode = 401 });
            }

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client
                .DeleteAsync($"https://localhost:44356/api/reminders/{id}")
                .ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(null);
            }
            else
            {
                return new JsonResult(new { StatusCode = response.StatusCode, Value = response.ReasonPhrase });
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var client = _clientFactory.CreateClient();
                var content = $"{{\"Username\":\"{username}\",\"password\":\"{password}\"}}";
                var response = client
                    .PostAsync("https://localhost:44356/api/authenticate", new StringContent(content, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var token = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                    HttpContext.Session.SetString("ExpiryToken", token);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetTokenFromSesssion()
        {
            return HttpContext.Session.GetString("ExpiryToken");
        }
    }
}
