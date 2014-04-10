using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BetterCMS.Module.FAQ.Helpers
{
    public static class Utility
    {
        public static string GenerateSlug(this string phrase, int maxLength = 50)
        {
            string str = phrase.ToLower();
            str = ConvertVN(str);
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public static string ConvertVN(string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }
        public static string ShortGuid(this string phrase)
        {
            Guid guid = Guid.Empty;
            string str = "";
            if (Guid.TryParse(phrase, out guid))
            {
                str = guid.ToString().Split('-')[0];
            }
            return str;
        }

        public static string AppendQueryString(List<KeyValuePair<string, string>> lst)
        {
            var uriBuilder = new UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var item in lst)
            {
                query[item.Key] = item.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public static string CurrentLanguageCode
        {
            get
            {
                CultureInfo ci = (CultureInfo)HttpContext.Current.Session["Culture"];
                return ci.Name;
            }
        }
    }
}