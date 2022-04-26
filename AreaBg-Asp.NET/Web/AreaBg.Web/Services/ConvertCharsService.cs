using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AreaBg.Web.Services
{
    /// <summary>
    /// static - no. ne se skalira. polzwai singleton.
    /// </summary>
    public static class ConvertCharsService
    {
        private static int _someNumber = 32;

        public static string CyrillicToLatinChars(string title)
        {
            _someNumber++;
            if (title != null)
            {
                Regex pattern = new Regex("[~`!@#$%^&*()_\\+\\-={}\\[\\]:;\"'\\|<>,.?/№§€—\\t\\r\\n]");
                var titleWithoutSymbols = pattern.Replace(title, "");
                var last = titleWithoutSymbols.Replace(' ', '-').Replace("\u00A0", "-");
                var final = Regex.Replace(last, @"\s+", "").ToLower();

                var map = new Dictionary<char, string>
                      {
                          { 'а', "a" }, { 'б', "b" }, { 'в', "v" }, { 'г', "g" }, { 'д', "d" },
                          { 'е', "e" }, { 'ж', "zh" }, { 'з', "z" }, { 'и', "i" }, { 'й', "i" },
                          { 'к', "k" }, { 'л', "l" }, { 'м', "m" }, { 'н', "n" }, { 'о', "o" },
                          { 'п', "p" }, { 'р', "r" }, { 'с', "s" }, { 'т', "t" }, { 'у', "u" },
                          { 'ф', "f" }, { 'х', "h" }, { 'ц', "c" }, { 'ч', "ch" }, { 'ш', "sh" },
                          { 'щ', "sht" }, { 'ъ', "a" }, { 'ь', "i" }, { 'ю', "yu" }, { 'я', "ya" },
                          { '-', "-" }, {',', ","}, {'.', "."}, {'(', "("}, {')', ")"}, {'/', "/"},
                          { '\\', "\\" }, {'*', "*"}, {'–', "-"}, {'„', "\""}, {'”', "\""}, { 'і', "i" },
                          { 'ы', "ы" }, {'’', "’"}, {'“', "“"}, {'…', "…"}, {'°', "°"}, {'ѝ', "i"},{'∙', "∙"},
                          {'½', "½"}, {'¼', "¼"}, {'a', "a"}, { 'b', "b" }, {'c', "c"}, {'d', "d"}, {'e', "e"},
                          {'f', "f"}, {'g',"g"},{'h',"h"},{'i',"i"},{'j',"j"},{'k',"k"},{'l',"l"},{'m',"m"},
                          {'n',"n"},{'o',"o"},{'p',"p"},{'q',"q"},{'r',"r"},{'s',"s"},{'t',"t"},{'u',"u"},
                          {'v',"v"},{'w',"w"},{'x',"x"},{'y',"y"},{'z',"z"},{'é', "e"},{'™',"™"}, {'ç', "c"},
                          {'à',"a"},{'í',"i"},{'­', "-"},{'ù',"u"},{'ß',"b"},{'ö',"o"},{'ü',"u"},{'¾',"¾"},
                          {'э',"e"},{ '0', "0" },{ '1', "1" },{ '2', "2" },{ '3', "3" },{ '4', "4" },{ '5', "5" },
                          { '6', "6" },{ '7', "7" },{ '8', "8" },{ '9', "9" }
                      };

                var sb = new StringBuilder();
                foreach (var i in final)
                {
                    sb.Append(map[i]);
                }

                //final = string.Join("", final.Select(c => map[c]));

                return sb.ToString();
            }

            return null;
        }
    }
}
