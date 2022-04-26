using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Data.Models.enums;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.GlobalsVariables;
using AreaBg.Web.Interfaces.Services;
using AreaBg.Web.Services;
using AreaBg.Web.ViewModels;
using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Json;
using AreaBg.Web.ViewModels.Order;
using AreaBg.Web.ViewModels.Products;
using AreaBg.Web.ViewModels.UserInfoViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AreaBg.Web.Controllers
{
    //[Route("[controller]/[action]")]
    //[ApiController]
    public class CheckoutController : BaseController
    {
        private readonly IHttpClientFactory ClientFactory;


        private readonly MyDbContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;
        public IConfiguration Configuration { get; }

        public CheckoutController(IProductAdminService productAdminService, IProductService productService, MyDbContext db, UserManager<MyIdentityUser> userManager, IHttpClientFactory clientFactory, IConfiguration configuration)
            : base(productAdminService, productService)
        {
            this.Configuration = configuration;
            _db = db;
            _userManager = userManager;
            this.ClientFactory = clientFactory;
        }

        [HttpGet] 
        public async Task<IActionResult> Cart()
        {
            var cookieCart = HttpContext.Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cookieCart))
            {

                return View();
            }
            else
            {
                var splitProducts = cookieCart.Split(':');

                var cartViewModel = new CartViewModel();

                foreach (var p in splitProducts)
                {
                    if (string.IsNullOrEmpty(p))
                    {
                        continue;
                    }

                    var productAndCount = p.Split('-');

                    var productId = int.Parse(productAndCount[0]);
                    var productCount = int.Parse(productAndCount[1]);
                    var book = await _db.Books.FindAsync(productId);
                    var bookDisc = book.Discount;
                    var bookPrice = book.Price;
                    decimal priceWithDisc = ((decimal)(100 - bookDisc) / 100) * bookPrice;
                    var details = new ProductCartDetails
                    {
                        ProductId = productId,
                        CountOfProduct = productCount,
                        ImageName = book.ImageNameWithExtension != "" ? $"{book.ReleaseDate.Year}/" + book.ImageNameWithExtension : "",
                        NameOfProduct = book.Title,
                        Author = book.Author,
                        Discount = book.Discount,
                        Price = book.Price,
                        PriceWithDisc = priceWithDisc,
                        QuantityPriceWithDisc = productCount * priceWithDisc,
                        TitleLatin = ConvertCharsService.CyrillicToLatinChars(book.Title)
                    };

                    cartViewModel.ProductCartDetails.Add(details);
                }

                cartViewModel.TotalPrice = cartViewModel.ProductCartDetails.Sum(x => x.QuantityPriceWithDisc);

                return View(cartViewModel);
            }
        }

        [HttpGet("Checkout/LastStep")]
        public async Task<IActionResult> LastStep()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var orderVM = await GetDataFromCart();

                if (orderVM == null)
                {
                    return View();
                }

                return View(orderVM);
            }

            return Redirect("/Identity/Account/Login?returnUrl=" + HttpContext.Request.Path);
        }

        [HttpPost]
        public async Task<IActionResult> LastStep(OrderFinishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.AddressId == null || model.AddressId == "newAddress" || !model.AcceptTerms)
                {
                    if (model.Name == null || model.City == null || model.PhoneNumber == null || model.DeliveryAddress == null || !model.AcceptTerms)
                    {
                        return View(model);
                    }

                }
            }

            var deliveryPlace = DeliveryPlace.OfficeSpeedy;
            var deliveryPrice = Globals.OfficeSpeedy;
            var deliveryPlaceMail = "Офис на Спиди";
            switch (model.ChooseAddress)
            {
                case "OfficeEkont":
                    deliveryPlace = DeliveryPlace.OfficeEkont;
                    deliveryPrice = Globals.OfficeEkont;
                    deliveryPlaceMail = "Офис на Еконт";
                    break;
                case "Ekont":
                    deliveryPlace = DeliveryPlace.Ekont;
                    deliveryPrice = Globals.Ekont;
                    deliveryPlaceMail = "До адрес с Еконт";
                    break;
                case "Speedy":
                    deliveryPlace = DeliveryPlace.Speedy;
                    deliveryPrice = Globals.Speedy;
                    deliveryPlaceMail = "До адрес с Спиди";
                    break;
            }

            var newAddressId = 0;
            var userId = _userManager.GetUserId(User);
            var user = _db.Users.Find(userId);

            if (model.AddressId == "newAddress" || model.AddressId == null)
            {
                var address = new Address
                {
                    Name = model.Name,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    Street = model.DeliveryAddress,
                    DeliveryPlace = deliveryPlace
                };

                user.UserAddresses.Add(new UserAddress
                {
                    Address = address,
                    User = user
                });

                _db.SaveChanges();
                newAddressId = address.Id;
            }
            else
            {
                var isIdValid = int.TryParse(model.AddressId, out int addressId);
                if (!isIdValid)
                {
                    //TODO: return error
                }

                newAddressId = addressId;

            }

            var userAddress = _db.Addresses.Find(newAddressId);

            var order = new Order
            {
                City = userAddress.City,
                DeliveryPlace = userAddress.DeliveryPlace,
                Name = userAddress.Name,
                Address = userAddress.Street,
                PhoneNumber = userAddress.PhoneNumber,
                Note = model.OrderNote,
                User = user,
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.processes
            };

            //get books from cookie
            var cookieCart = HttpContext.Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cookieCart))
            {
                return View();
            }

            var booksToMail = string.Empty;
            var splitProducts = cookieCart.Split(':');
            decimal totalPriceWithDisc = 0;
            foreach (var p in splitProducts)
            {
                if (string.IsNullOrEmpty(p))
                {
                    continue;
                }

                var productAndCount = p.Split('-');

                var productId = int.Parse(productAndCount[0]);
                var productCount = int.Parse(productAndCount[1]);
                var book = _db.Books.Find(productId);
                var bookDisc = book.Discount;
                var bookPrice = book.Price;
                decimal priceWithDisc = ((decimal)(100 - bookDisc) / 100) * bookPrice;
                totalPriceWithDisc += (priceWithDisc * productCount);
                var orderBook = new OrderBook
                {
                    Count = productCount,
                    BookId = book.BookId,
                    Book = book,
                    Discount = bookDisc,
                    Price = bookPrice,
                    PriceWithDisc = priceWithDisc
                };
                order.OrderBooks.Add(orderBook);

                _db.Books.Where(x => x.BookId == productId).FirstOrDefault().CountOfOrders += (1 * productCount);

                booksToMail += @$"<tr style='padding-top: 10px; padding-bottom: 10px;'>
                                    <th colspan='5'></th>
                                    <td>{orderBook.Book.Title}</td>
                                    <td>{priceWithDisc.ToString("f2")}лв.</td>
                                    <td>{productCount}бр.</td>
                                    <td>{(priceWithDisc * productCount).ToString("f2")}лв.</td>
                                  </tr>";
            }

            order.TotalPriceWithoutDelivery = totalPriceWithDisc;

            if (totalPriceWithDisc >= 30 && deliveryPlace == DeliveryPlace.OfficeSpeedy)
            {
                deliveryPrice = 0;
            }

            order.DeliveryPrice = deliveryPrice;
            order.TotalPrice = totalPriceWithDisc + deliveryPrice;

            var userOrder = new UserOrder
            {
                Order = order,
                User = user
            };

            user.UserOrders.Add(userOrder);
            _db.SaveChanges();

            var orderNumber = order.Id + "SA" + DateTime.Today.ToString("MMdd");
            order.OrderNumber = orderNumber;

            _db.Orders.Update(order);
            _db.SaveChanges();

            Response.Cookies.Delete("cart");
            var mail = new MailService();
            var email = user.Email;

            var dateFormat = DateTime.Today.ToString("MMdd");
            var addr = $"{order.City}, {order.Address}";
            var completeBody = string.Format(Globals.FinishOrderBody, user.UserName, order.Id, booksToMail, deliveryPrice.ToString("f2"), order.TotalPrice.ToString("f2"), addr, order.PhoneNumber, deliveryPlaceMail, dateFormat);

            var mailSender = new Task(() => mail.SendMail(email, completeBody, Globals.FinishOrderSubject + " " + order.Id + "SA" + DateTime.Today.ToString("MMdd")));
            mailSender.Start();

            if (model.Paying == "viaCard")
            {
                var amount = (int)(order.TotalPrice * 100);

                var request = new HttpRequestMessage(HttpMethod.Get, string.Format(this.Configuration["DskConfig"], amount, orderNumber));

                var client = this.ClientFactory.CreateClient();

                var response = await client.SendAsync(request);

                var responseStream = await response.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<BankReturnJson>(responseStream);
                var redirectLink = json.formUrl;

                return Redirect(redirectLink);
            }
            else
            {
                return Redirect("/Checkout/Finish");
            }
        }

        [HttpGet("Checkout/Finish")]
        public async Task<IActionResult> Finish(string orderId, string orderNum)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format(this.Configuration["DskStatus"], orderId));

                var client = this.ClientFactory.CreateClient();
                var response = await client.SendAsync(request);
                var responseStream = await response.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<BankStatusJson>(responseStream);

                var finishViewModel = new FinishViewModel();

                finishViewModel.OrderId = json.OrderNumber;
                finishViewModel.OrderStatus = json.OrderStatus;

                return View(finishViewModel);
            }
            else if (!string.IsNullOrEmpty(orderNum))
            {
                var finishViewModel = new FinishViewModel();

                finishViewModel.OrderNum = orderNum;

                return View(finishViewModel);
            }

            return View();
        }

        [HttpGet("Checkout/GetAddress")]
        public IActionResult GetAddress(string a)
        {
            if (User.Identity.IsAuthenticated)
            {
                var place = Enum.Parse<DeliveryPlace>(a);
                var userId = _userManager.GetUserId(User);

                var user = _db.Users.Include(x => x.UserAddresses)
                                    .ThenInclude(x => x.Address)
                                    .Where(x => x.Id == userId)
                                    .First();

                var address = user.UserAddresses.Where(x => x.Address.DeliveryPlace == place)
                                                .Select(x => new SimpleAddress
                                                {
                                                    Address = x.Address.Street,
                                                    City = x.Address.City,
                                                    Id = x.Id
                                                }).ToArray();
                if (address.Length <= 0)
                {
                    return new JsonResult(null);
                }

                return new JsonResult(address);
            }

            return new JsonResult(a);
        }

        [HttpGet("Checkout/LastStepNoReg")]
        public async Task<IActionResult> LastStepNoReg()
        {
            var orderVM = await GetDataFromCart();

            if (orderVM == null)
            {
                return View();
            }

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> LastStepNoReg(OrderFinishViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.AddressId == null || model.AddressId == "newAddress" || !model.AcceptTerms)
                {
                    if (model.Name == null || model.City == null || model.PhoneNumber == null || model.DeliveryAddress == null || !model.AcceptTerms || model.Email == null)
                    {
                        return View(model);
                    }

                }
            }

            var deliveryPlace = DeliveryPlace.OfficeSpeedy;
            var deliveryPrice = Globals.OfficeSpeedy;
            var deliveryPlaceMail = "Офис на Спиди";
            switch (model.DeliveryOption)
            {
                case "OfficeEkont":
                    deliveryPlace = DeliveryPlace.OfficeEkont;
                    deliveryPrice = Globals.OfficeEkont;
                    deliveryPlaceMail = "Офис на Еконт";
                    break;
                case "Ekont":
                    deliveryPlace = DeliveryPlace.Ekont;
                    deliveryPrice = Globals.Ekont;
                    deliveryPlaceMail = "До адрес с Еконт";
                    break;
                case "Speedy":
                    deliveryPlace = DeliveryPlace.Speedy;
                    deliveryPrice = Globals.Speedy;
                    deliveryPlaceMail = "До адрес с Спиди";
                    break;
            }

            var order = new Order
            {
                City = model.City,
                DeliveryPlace = deliveryPlace,
                Name = model.Name,
                Address = model.DeliveryAddress,
                PhoneNumber = model.PhoneNumber,
                Note = model.OrderNote,
                Email = model.Email,
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.processes
            };

            //get books from cookie
            var cookieCart = HttpContext.Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cookieCart))
            {
                return View();
            }

            var booksToMail = string.Empty;
            var splitProducts = cookieCart.Split(':');
            decimal totalPriceWithDisc = 0;
            foreach (var p in splitProducts)
            {
                if (string.IsNullOrEmpty(p))
                {
                    continue;
                }

                var productAndCount = p.Split('-');

                var productId = int.Parse(productAndCount[0]);
                var productCount = int.Parse(productAndCount[1]);
                var book = _db.Books.Find(productId);
                var bookDisc = book.Discount;
                var bookPrice = book.Price;
                decimal priceWithDisc = ((decimal)(100 - bookDisc) / 100) * bookPrice;
                totalPriceWithDisc += (priceWithDisc * productCount);
                var orderBook = new OrderBook
                {
                    Count = productCount,
                    BookId = book.BookId,
                    Book = book,
                    Discount = bookDisc,
                    Price = bookPrice,
                    PriceWithDisc = priceWithDisc
                };
                order.OrderBooks.Add(orderBook);

                _db.Books.Where(x => x.BookId == productId).FirstOrDefault().CountOfOrders += (1 * productCount);

                booksToMail += @$"<tr style='padding-top: 10px; padding-bottom: 10px;'>
                                    <th colspan='5'></th>
                                    <td>{orderBook.Book.Title}</td>
                                    <td>{priceWithDisc.ToString("f2")}лв.</td>
                                    <td>{productCount}бр.</td>
                                    <td>{(priceWithDisc * productCount).ToString("f2")}лв.</td>
                                  </tr>";
            }

            order.TotalPriceWithoutDelivery = totalPriceWithDisc;

            if (totalPriceWithDisc >= 30 && deliveryPlace == DeliveryPlace.OfficeSpeedy)
            {
                deliveryPrice = 0;
            }

            order.DeliveryPrice = deliveryPrice;
            order.TotalPrice = totalPriceWithDisc + deliveryPrice;

            _db.Orders.Add(order);
            _db.SaveChanges();

            var orderNumber = order.Id + "SA" + DateTime.Today.ToString("MMdd");
            order.OrderNumber = orderNumber;

            _db.Orders.Update(order);
            _db.SaveChanges();

            Response.Cookies.Delete("cart");
            var mail = new MailService();
            var email = order.Email;

            var dateFormat = DateTime.Today.ToString("MMdd");
            var addr = $"{order.City}, {order.Address}";
            var completeBody = string.Format(Globals.FinishOrderBody, order.Name, order.Id, booksToMail, deliveryPrice.ToString("f2"), order.TotalPrice.ToString("f2"), addr, order.PhoneNumber, deliveryPlaceMail, dateFormat);
            mail.SendMail(email, completeBody, Globals.FinishOrderSubject + " " + order.Id + "SA" + DateTime.Today.ToString("MMdd"));


            if (model.Paying == "viaCard")
            {
                var amount = (int)(order.TotalPrice * 100);

                var request = new HttpRequestMessage(HttpMethod.Get, string.Format(this.Configuration["DskConfig"], amount, orderNumber));

                var client = this.ClientFactory.CreateClient();

                var response = await client.SendAsync(request);

                var responseStream = await response.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<BankReturnJson>(responseStream);
                var redirectLink = json.formUrl;

                return Redirect(redirectLink);
            }
            else
            {
                return Redirect($"/Checkout/Finish?orderNum={order.OrderNumber}");
            }
        }

        [HttpGet("Checkout/GetDataFromCart")]
        private async Task<OrderFinishViewModel> GetDataFromCart()
        {
            var cookieCart = HttpContext.Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cookieCart))
            {
                return null;
            }

            var splitProducts = cookieCart.Split(':');
            decimal totalPriceWithDisc = 0;
            foreach (var p in splitProducts)
            {
                if (string.IsNullOrEmpty(p))
                {
                    continue;
                }

                var productAndCount = p.Split('-');

                var productId = int.Parse(productAndCount[0]);
                var productCount = int.Parse(productAndCount[1]);
                var book = await _db.Books.FindAsync(productId);
                var bookDisc = book.Discount;
                var bookPrice = book.Price;
                decimal priceWithDisc = ((decimal)(100 - bookDisc) / 100) * bookPrice;
                totalPriceWithDisc += (priceWithDisc * productCount);
            }

            var deliveryPrice = 2.99;
            if (totalPriceWithDisc >= 100)
            {
                deliveryPrice = 0;
            }

            var orderVM = new OrderFinishViewModel { Price = totalPriceWithDisc.ToString("f2"), DeliveryPrice = deliveryPrice };

            return orderVM;
        }
    }
}