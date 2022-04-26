using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.ViewModels.Orders;
using AreaBg.Web.Areas.Identity.ViewModels;
using AreaBg.Web.GlobalsVariables;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        protected MyDbContext Db { get; }

        public OrderController(MyDbContext db)
        {
            this.Db = db;
        }

        [HttpGet]
        public IActionResult Orders(string fromDate, string toDate)
        {
            if (fromDate != null && toDate != null)
            {
                var startDate = DateTime.Parse(fromDate);
                var endDate = DateTime.Parse(toDate).AddDays(1);

                var model = new List<OrdersViewModel>();

                model = this.Db.Orders
                                .Include(x => x.User)
                                .AsEnumerable()
                                .Where(x => x.OrderDate.Date >= startDate && x.OrderDate.Date < endDate)
                                .Select(x => new OrdersViewModel
                                {
                                    StartDate = fromDate,
                                    EndDate = toDate,
                                    Date = x.OrderDate.ToString("yyyy-MM-dd"),
                                    Id = x.Id,
                                    Name = x.Name,
                                    Note = x.Note,
                                    City = x.City,
                                    OrderStatus = (int)x.OrderStatus,
                                    Registred = x.User != null
                                })
                                .ToList();

                return View(model);
            }

            return View();
        }

        public IActionResult OrderSend(int id)
        {
            var order = this.Db.Orders.Include(x => x.User)
                                      .Include(x => x.OrderBooks)
                                      .ThenInclude(x => x.Book)
                                      .Where(x => x.Id == id)
                                      .FirstOrDefault();

            order.OrderStatus = Data.Models.enums.OrderStatus.sent;

            this.Db.Orders.Update(order);
            this.Db.SaveChanges();

            var deliveryPlaceMail = "Офис на Спиди";
            switch (order.DeliveryPlace.ToString())
            {
                case "OfficeEkont":
                    deliveryPlaceMail = "Офис на Еконт";
                    break;
                case "Ekont":
                    deliveryPlaceMail = "До адрес с Еконт";
                    break;
                case "Speedy":
                    deliveryPlaceMail = "До адрес с Спиди";
                    break;
            }

            var booksToMail = string.Empty;
            foreach (var b in order.OrderBooks)
            {
                decimal priceWithDisc = ((decimal)(100 - b.Book.Discount) / 100) * b.Book.Price;

                booksToMail += @$"<tr style='padding-top: 10px; padding-bottom: 10px;'>
                                    <th colspan='5'></th>
                                    <td>{b.Book.Title}</td>
                                    <td>{priceWithDisc.ToString("f2")}лв.</td>
                                    <td>{b.Count}бр.</td>
                                    <td>{(priceWithDisc * b.Count).ToString("f2")}лв.</td>
                                  </tr>";
            }

            var mail = new MailService();

            var email = string.Empty;
            if (order.User != null)
            {
                email = order.User.Email;
            }
            else
            {
                email = order.Email;
            }
            

            var addr = $"{order.City}, {order.Address}";
            var dateFormat = order.OrderDate.ToString("MMdd");
            var username = order.User != null ? order.User.UserName : order.Name;
            var completeBody = string.Format(Globals.ChangeStatusBody, username , order.Id, booksToMail, order.DeliveryPrice, order.TotalPrice.ToString("f2"), addr, order.PhoneNumber, deliveryPlaceMail, dateFormat);
            mail.SendMail(email, completeBody, Globals.ChangeStatusSubject);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult OrderDetails(int id)
        {
            var order = new OrderDetailsViewModel();

            var deliveryPlace = Globals.DeliveryPlaces[(int)this.Db.Orders.Find(id).DeliveryPlace];
            var orderStatus = Globals.OrderStatus[(int)this.Db.Orders.Find(id).OrderStatus];

            order = this.Db
                        .Orders
                        .Where(x => x.Id == id)
                        .Select(x => new OrderDetailsViewModel
                        {
                            Address = x.Address,
                            City = x.City,
                            DelieryPrice = x.DeliveryPrice, 
                            DeliveryPlace = deliveryPlace,
                            Id = x.Id, 
                            Name = x.Name,
                            Note = x.Note,
                            OrderDate = x.OrderDate,
                            PhoneNumber = x.PhoneNumber, 
                            TotalPrice = x.TotalPrice,
                            TotalPriceWithoutDelivery = x.TotalPriceWithoutDelivery,
                            OrderStatus = orderStatus,
                            Email = x.User != null ? x.User.Email : x.Email,
                            OrderBooks = x.OrderBooks
                                          .Select(b => new Web.ViewModels.Products.ProductCartDetails
                                          {
                                              Publisher = b.Book.Publisher,
                                              CountOfProduct = b.Count,
                                              ImageName = b.Book.ImageNameWithExtension,
                                              Discount = b.Book.Discount,
                                              NameOfProduct = b.Book.Title,
                                              ProductId = b.Book.BookId,
                                              Price = b.Price,
                                              PriceWithDisc = b.PriceWithDisc,
                                              QuantityPriceWithDisc = b.Count * b.PriceWithDisc
                                          }).ToArray()
                        })
                        .FirstOrDefault();

            return View(order);
        }

        public IActionResult PrintOrders(params int[] orderCheck)
        {
            if (orderCheck == null || orderCheck.Length == 0)
            {
                return Redirect("/Admin/Order/Orders");
            }

            var orders = this.Db.Orders
                                .Include(x => x.OrderBooks)
                                .ThenInclude(x => x.Book)
                                .Include(x => x.User)
                                .AsEnumerable()
                                .Where(x => orderCheck.Contains(x.Id))
                                .ToList();

            var ordersModel = orders.Select(x => new OrderDetailsViewModel
                                {
                                    Address = x.Address,
                                    City = x.City,
                                    DelieryPrice = x.DeliveryPrice,
                                    DeliveryPlace = Globals.DeliveryPlaces[(int)this.Db.Orders.Find(x.Id).DeliveryPlace],
                                    Id = x.Id,
                                    Name = x.Name,
                                    Note = x.Note,
                                    OrderDate = x.OrderDate,
                                    PhoneNumber = x.PhoneNumber,
                                    TotalPrice = x.TotalPrice,
                                    TotalPriceWithoutDelivery = x.TotalPriceWithoutDelivery,
                                    Email = x.User != null ? x.User.Email : x.Email,
                                    OrderStatus = Globals.OrderStatus[(int)this.Db.Orders.Find(x.Id).OrderStatus],
                                    OrderBooks = x.OrderBooks
                                          .Select(b => new Web.ViewModels.Products.ProductCartDetails
                                          {
                                              Publisher = b.Book.Publisher,
                                              CountOfProduct = b.Count,
                                              ImageName = b.Book.ImageNameWithExtension,
                                              Discount = b.Book.Discount,
                                              NameOfProduct = b.Book.Title,
                                              ProductId = b.Book.BookId,
                                              Price = b.Price,
                                              PriceWithDisc = b.PriceWithDisc,
                                              QuantityPriceWithDisc = b.Count * b.PriceWithDisc,
                                              Author = b.Book.Author
                                          }).ToArray()
                                })
                                .ToList();

            var result = new PrintOrdersViewModel
            {
                OrderDetails = ordersModel,
                OrderDeliveryPrices = ordersModel.Sum(x => x.DelieryPrice),
                OrdersCount = ordersModel.Count,
                OrdersTotalPrice = ordersModel.Sum(x => x.TotalPriceWithoutDelivery)
            };

            return View(result);
        }

        [HttpGet]
        public IActionResult SearchClients(string clientToSearch, string searchOption)
        {
            if (clientToSearch != null)
            {
                var order = new List<Order>();

                var terms = clientToSearch.ToLower().Split(" ");

                switch (searchOption)
                {
                    case "byId":
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.Id == int.Parse(clientToSearch)).ToList();
                        break;
                    case "byName":
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.Name != null).Where(x => terms.Any(t => x.Name.ToLower().Contains(t))).ToList();
                        break;
                    case "byAddress":
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.Address != null).Where(x => terms.Any(t => x.Address.ToLower().Contains(t))).ToList();
                        break;
                    case "byEmail":
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.User.Email != null).Where(x => terms.Any(t => x.User.Email.ToLower().Contains(t))).ToList();
                        break;
                    case "byCity":
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.City != null).Where(x => terms.Any(t => x.City.ToLower().Contains(t))).ToList();
                        break;
                    case "byPhone":
                        var phone = Regex.Split(clientToSearch.Trim(), @" -/\\");
                        order = this.Db.Orders.Include(x => x.User).AsEnumerable().Where(x => x.PhoneNumber != null).Where(x => x.PhoneNumber.Contains(string.Join("", phone))).ToList();
                        break;
                }

                var model = order.OrderByDescending(x => x.Id).Select(x => new OrdersViewModel
                                 {
                                     Date = x.OrderDate.ToString("yyyy-MM-dd"),
                                     Id = x.Id,
                                     Name = x.Name,
                                     Note = x.Note,
                                     City = x.City,
                                     OrderStatus = (int)x.OrderStatus,
                                     Email = x.User != null ? x.User.Email : x.Email
                                 })
                                .ToList();

                return View(model);
            }

            return View();
        }

        public IActionResult OrderBookChangeQuantity(OrderChangeQuantity model)
        {
            var orderId = model.OrderId;
            var books = this.Db.OrderBooks.Where(x => x.OrderId == orderId);
            var order = this.Db.Orders.Find(orderId);
            order.TotalPriceWithoutDelivery = 0;
            foreach (var i in model.Quantities)
            {
                var bookId = i.BookId;
                var quantity = i.Quantity;

                var book = books.Where(x => x.BookId == bookId).FirstOrDefault();
                var price = book.PriceWithDisc;

                book.Count = quantity;

                order.TotalPriceWithoutDelivery += book.PriceWithDisc * quantity;

                this.Db.OrderBooks.Update(book);
            }

            order.TotalPrice = order.TotalPriceWithoutDelivery + order.DeliveryPrice;

            this.Db.Orders.Update(order);
            this.Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult OrderRefuse(int id) 
        {
            var order = this.Db.Orders.Where(x => x.Id == id)
                                      .FirstOrDefault();

            order.OrderStatus = Data.Models.enums.OrderStatus.refuse;

            this.Db.Orders.Update(order);
            this.Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult OrderRefuseMail(int id)
        {
            var order = this.Db.Orders.Include(x => x.User)
                                      .Where(x => x.Id == id)
                                      .FirstOrDefault();

            order.OrderStatus = Data.Models.enums.OrderStatus.refuse;

            this.Db.Orders.Update(order);
            this.Db.SaveChanges();

            var mail = new MailService();

            var email = string.Empty;
            if (order.User != null)
            {
                email = order.User.Email;
            }
            else
            {
                email = order.Email;
            }

            mail.SendMail(email, Globals.RefusedBody, Globals.RefusedSubject + order.OrderNumber);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
