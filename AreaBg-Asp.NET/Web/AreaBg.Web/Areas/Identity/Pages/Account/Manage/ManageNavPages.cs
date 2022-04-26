using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string FavoriteProducts => "FavoriteProducts";

        public static string MyOrders => "MyOrders";

        public static string MyAddresses => "MyAddresses";

        public static string AddAddress => "AddAddress";

        public static string OrderDetails => "OrderDetails";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
        ///
        public static string FavoriteProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, FavoriteProducts);

        public static string MyOrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyOrders);

        public static string MyAddressesNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyAddresses);

        public static string AddAddressNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddAddress);

        public static string OrderDetailsNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderDetails);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}