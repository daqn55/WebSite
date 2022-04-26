using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AreaBg.Web.GlobalsVariables
{
    public static class Globals
    {
        public readonly static decimal Ekont = 0.00M;

        public readonly static decimal Speedy = 3.99M;

        public readonly static decimal OfficeEkont = 0.00M;

        public readonly static decimal OfficeSpeedy = 2.99M;

        public readonly static string[] DeliveryPlaces = { "Home", "До адрес със Спиди", "До адрес с Еконт", "Офис на Еконт", "Офис на Спиди" };

        public readonly static string[] OrderStatus = { "Обработва се", "Изпратена", "Доставена", "Отказана" };

        public readonly static string FinishOrderBody = @"
<table cellspacing='0' cellpadding='0' border='0' width='90%' style='margin-left: auto; margin-right: auto;'>
    <thead>
        <tr>
            <th scope='col' style='background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; text-align: center;'>
                <img src='cid:logo' alt='AreaBG' class='img-responsive' style='width: 139px; height: 90px;' />
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope='row' style='display: inline-block; white-space: pre-line; text-align:justify; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'>
                Здравейте {0},<br /><br />
                Вашата поръчка № {1}SA{8} в <a href='http://area.bg'>Area.bg</a> е регистрирана успешно.<br /><br />
                Тук ще видите всички детайли за нея. Ако забележите несъответствие, свържете се с нас на тел. 0877 018 634 или на имейл: info@area.bg, за да го коригираме преди изпращане на поръчката. Можете да следите статуса на поръчката си и през своя профил в <a href='http://area.bg/account/my-orders'>Area.bg</a>, а когато поръчката ви е изпратена, ще получите и второ писмо с информация за нейната доставка.
            </th>
        </tr>
        <tr>
            <td style='background-color: lightgrey; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'><strong>Продукти</strong></td>
        </tr>
        <tr>
            <td style='padding-top: 10px; padding-bottom: 10px;'>
                <table width='100%'>
                    <thead>
                        <tr>
                            <th style='width: 5%' scope='col' colspan='5'></th>
                            <th style='width: 40%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                        </tr>
                    </thead>
                    <tbody>
                       {2}
                        <tr style='padding-top: 10px; padding-bottom: 10px;'>
                            <th colspan='5'></th>
                            <td>Цена за доставка:</td>
                            <td></td>
                            <td></td>
                            <td>{3}</td>
                        </tr>
                        <tr style='padding-top: 10px; padding-bottom: 10px;'>
                            <th colspan='5'></th>
                            <td><strong>Крайна сума:</strong></td>
                            <td></td>
                            <td></td>
                            <td><strong>{4}</strong></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td style='background-color: lightgrey; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'><strong>Информация за доставка</strong></td>
        </tr>
        <tr>
            <td style='padding-left: 5px;'><br />Адрес<br />
            {5}<br />
            тел: {6}<br />
            Начин на доставка: {7}<br /><br />
            </td>
        </tr>
        <tr>
            <th scope='row' style='text-align: justify; background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; padding-left: 5px;'>
                М.С.Д Голд ЕООД, София, 0877 018 634, <a href='mailto:info@area.bg'>info@area.bg</a>
            </th>
        </tr>
    </tbody>
</table>";

        public readonly static string FinishOrderSubject = "Потвърждение за поръчка номер:";

        public readonly static string RegisterNewAccSubject = "Добре дошли в Area.bg!";

        public static string RegisterNewAccBody = @"
<table cellspacing='0' cellpadding='0' border='0' width='90%' style='text-align:center; margin-left: auto; margin-right: auto;'>
    <thead>
        <tr>
            <th scope ='col' style='background-color: lightgrey; padding-top: 10px; padding-bottom: 10px;'>
                <img src ='cid:logo' alt='AreaBG' class='img-responsive' style='width: 139px; height: 90px;' />
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope = 'row' style='display: inline-block; white-space: pre-line; text-align:justify; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'>
                Честито! Ти вече имаш потребителска регистрация в онлайн магазина<a href= 'http://area.bg'>Area.bg</a>.<br />
                Можеш да влезеш в потребителския си профил от тук: <a href = 'http://area.bg/account'>Моят профил</a>.<br />
                Вече можеш да правиш поръчки от нашия сайт бързо и лесно. В своя профил можеш да редактираш и добавяш информация за адреси, да следиш избрани от теб продукти и да получаваш актуална информация за всички свои поръчки, както и да видиш информация за вече завършени.
                Искаш да се свържеш директно с нас и да зададеш въпрос? Използвай секцията Контакти: <a href='http://area.bg/contacts'>Връзка с нас</a>.<br />
                Екипът на Area.bg ти пожелава приятно пазаруване.
            </th>
        </tr>
        <tr>
            <th scope = 'row' style='text-align: justify; background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; padding-left: 5px;' >
                М.С.Д Голд ЕООД, София, 0877 018 634, <a href='mailto:info@area.bg'>info@area.bg</a>
            </th>
        </tr>
    </tbody>
</table>";

        public static string ChangeStatusSubject = "Вашата поръчка е изпратена";

        public static string ChangeStatusBody = @"
<table cellspacing='0' cellpadding='0' border='0' width='90%' style='margin-left: auto; margin-right: auto;'>
    <thead>
        <tr>
            <th scope='col' style='background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; text-align: center;'>
                <img src='cid:logo' alt='AreaBG' class='img-responsive' style='width: 139px; height: 90px;' />
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope='row' style='display: inline-block; white-space: pre-line; text-align:justify; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'>
                Здравейте {0},<br /><br />
                Вашата поръчка с № {1}SA{8} в <a href='http://area.bg'>Area.bg</a> е изпратена.<br /><br />
                Тук ще видите всички детайли за нея.<br />
                Ако забележите несъответствие, свържете се с нас на тел. 0877 018 634 или на имейл: info@area.bg.
            </th>
        </tr>
        <tr>
            <td style='background-color: lightgrey; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'><strong>Продукти</strong></td>
        </tr>
        <tr>
            <td style='padding-top: 10px; padding-bottom: 10px;'>
                <table width='100%'>
                    <thead>
                        <tr>
                            <th style='width: 5%' scope='col' colspan='5'></th>
                            <th style='width: 40%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                            <th style='width: 15%' scope='col'></th>
                        </tr>
                    </thead>
                    <tbody>
                       {2}
                        <tr style='padding-top: 10px; padding-bottom: 10px;'>
                            <th colspan='5'></th>
                            <td>Цена за доставка:</td>
                            <td></td>
                            <td></td>
                            <td>{3}</td>
                        </tr>
                        <tr style='padding-top: 10px; padding-bottom: 10px;'>
                            <th colspan='5'></th>
                            <td><strong>Крайна сума:</strong></td>
                            <td></td>
                            <td></td>
                            <td><strong>{4}</strong></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td style='background-color: lightgrey; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'><strong>Информация за доставка</strong></td>
        </tr>
        <tr>
            <td style='padding-left: 5px;'><br />Адрес<br />
            {5}<br />
            тел: {6}<br />
            Начин на доставка: {7}<br /><br />
            </td>
        </tr>
        <tr>
            <th scope='row' style='text-align: justify; background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; padding-left: 5px;'>
                М.С.Д Голд ЕООД, София, 0877 018 634, <a href='mailto:info@area.bg'>info@area.bg</a>
            </th>
        </tr>
    </tbody>
</table>";

        public static string RefusedSubject = "Поръчка номер: ";

        public static string RefusedBody = @"
<table cellspacing='0' cellpadding='0' border='0' width='90%' style='margin-left: auto; margin-right: auto;'>
    <thead>
        <tr>
            <th scope='col' style='background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; text-align: center;'>
                <img src='cid:logo' alt='AreaBG' class='img-responsive' style='width: 139px; height: 90px;' />
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope='row' style='display: inline-block; white-space: pre-line; text-align:justify; padding-left: 5px; padding-top: 10px; padding-bottom: 10px;'>
                Здравейте,<br /><br />
                За съжаление няма да можем да Ви изпълним поръчката, тъй като книгите са с изчерпан тираж и не се очакват допечатки в скоро време.<br /><br />
                Извиняваме се за причиненото неудобство.<br /><br />
                Поздрави,<br />
                Екипът на Area.bg
            </th>
        </tr>
        <tr>
            <th scope='row' style='text-align: justify; background-color: lightgrey; padding-top: 10px; padding-bottom: 10px; padding-left: 5px;'>
                М.С.Д Голд ЕООД, София, 0877 018 634, <a href='mailto:info@area.bg'>info@area.bg</a>
            </th>
        </tr>
    </tbody>
</table>";
    }
}