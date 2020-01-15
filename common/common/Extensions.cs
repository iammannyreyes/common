using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace common
{
    public static class Extensions
    {
        public static readonly string UrlPattern =
    @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)";

        public static string ConvertUrlsToLinks(this string msg)
        {
            try
            {
                string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
                Regex r = new Regex(regex, RegexOptions.IgnoreCase);
                var convertUrlsToLinks = r.Replace(msg, "<a href=\"$1\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
                return convertUrlsToLinks;
            }
            catch (Exception ex)
            {
                return msg;
            }
        }

        public static string Linkify(this string text, bool omitHref, bool isTargetBlank)
        {
            try
            {
                var aHrefA = "<a href=\"$1\">$1</a>";
                if (isTargetBlank)
                {
                    aHrefA = "<a href=\"$1\" target='_blank'>$1</a>";
                }
                if (omitHref)
                {
                    aHrefA = "<a target='_blank'>$1</a>";
                }

                text = Regex.Replace(
                    text,
                    @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])",
                    aHrefA,
                    RegexOptions.IgnoreCase)
                    .Replace("href=\"www", "href=\"http://www");
                return text;
            }
            catch (Exception ex)
            {
                return text;
            }

        }

        public static List<string> GetLinkUrls(this string text)
        {
            var links = new List<string>();

            try
            {
                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var rawString = text;

                foreach (Match m in linkParser.Matches(rawString))
                {
                    links.Add(m.Value);
                }
                return links;
            }
            catch (Exception ex)
            {
                return links;
            }

        }

        public static string Linkify(this string text, bool omitHref)
        {
            try
            {
                if (omitHref)
                {
                    text = Regex.Replace(
                            text,
                            @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])",
                            "<a>$1</a>",
                            RegexOptions.IgnoreCase)
                            .Replace("href=\"www", "href=\"http://www");

                    return text;
                }

                // www|http|https|ftp|news|file
                text = Regex.Replace(
                    text,
                    @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])",
                    "<a href=\"$1\">$1</a>",
                    RegexOptions.IgnoreCase)
                    .Replace("href=\"www", "href=\"http://www");

                return text;

            }
            catch (Exception ex)
            {
                return text;
            }

        }

        public static string RemoveLinks(this string text)
        {
            var pattern = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";

            var cleaned = Regex.Replace(text, pattern, string.Empty, RegexOptions.IgnoreCase);
            return cleaned;
        }

        public static string UserAtfy(this string text)
        {
            try
            {
                text = Regex.Replace(text, @"(^|[^@\w])@(\w{1,20})", "$1<a class='text-primary' href='/profile-public/$2'>@$2</a>", RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
            }
            return text;
        }

        public static string UserAtfyNoLink(this string text)
        {
            try
            {
                text = Regex.Replace(text, @"(^|[^@\w])@(\w{1,20})", "$1<a>@$2</a>", RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
            }
            return text;
        }

        public static string CashBangfyNoLink(this string text)
        {
            try
            {
                var pattern = @"\$(?i)[A-Z](?i)[A-Z0-9]*(:[A-Z0-9]+)?";
                var r = new Regex(pattern);
                var matches = r.Matches(text);

                foreach (var element in matches)
                {
                    text = text.Replace(element.ToString(), $"<a>" + element.ToString() + "</a>");
                }
            }
            catch (Exception ex)
            {
            }
            return text;
        }
        public static string Hashtagfy(this string text)
        {
            try
            {
                var pattern = @"#(\w+)";
                var r = new Regex(pattern);
                var matches = r.Matches(text);

                ////        input = Regex.Replace(input, strTwitterTags,
                ////"<a href=\"http://search.twitter.com/search?q=$1\" target=\"_blank\">#$1</a>");

                foreach (var element in matches)
                {

                    var link = text.Replace(element.ToString(), $"<a href='/browse-now?query={element.ToString().Replace("#", String.Empty)}'>{element.ToString()}</a>");
                    text = link;
                }
                ////if (text.StartsWith("#"))
                ////{
                ////    var keyword = text.Substring(1);
                ////    text = $"<a href='/browse-now/query?={keyword}'></a>";
                ////    return text;
                ////}
            }
            catch (Exception ex)
            {
            }
            return text;
        }
        public static string CashBangfy(this string text)
        {
            try
            {
                var pattern = @"\$(?i)[A-Z](?i)[A-Z0-9]*(:[A-Z0-9]+)?";
                var r = new Regex(pattern);
                ////var test = "a $N225 b $2.25 c $TSE:BB d $TSE:B:::B: e $::";
                var matches = r.Matches(text);

                foreach (var element in matches)
                {
                    text = text.Replace(element.ToString(), $"<a class='text-primary' href='/symbol/{element.ToString().Substring(1)}' >" + element.ToString() + "</a>");
                }


            }
            catch (Exception ex)
            {
            }
            return text;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static List<string> GetCashBangs(this string text)
        {

            var cashBangSymbols = new List<string>();
            try
            {
                var pattern = @"\$(?i)[A-Z](?i)[A-Z0-9]*(:[A-Z0-9]+)?";
                var r = new Regex(pattern);
                var matches = r.Matches(text);

                foreach (var element in matches)
                {
                    cashBangSymbols.Add(element.ToString().Replace("$", string.Empty));
                }
            }
            catch (Exception ex)
            {
            }

            return cashBangSymbols;
        }
        public static List<string> GetMentionUserNames(this string text)
        {
            var userNames = new List<string>();
            try
            {
                var pattern = @"(^|[^@\w])@(\w{1,20})";
                var r = new Regex(pattern);
                var matches = r.Matches(text);

                foreach (var element in matches)
                {
                    userNames.Add(element.ToString().Replace("@", string.Empty).Trim());
                }
            }
            catch (Exception ex)
            {
            }
            return userNames;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static string ExtractDomainNameFromURL_Method1(string Url)
        {
            if (!Url.Contains("://"))
                Url = "https://" + Url;

            var uri = new Uri(Url);
            return uri.Host;
        }

        public static T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass =
                            (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return returnedXmlClass;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime UnixTimeStampMilliToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }

        public static DateTime UnixTimeStampMilliToDateTimeThis(this long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
        }

        public static string ReplaceSpecialCharactersForHashTags(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", " #", RegexOptions.Compiled);
        }

        public static string ToUrlSlug(this string value)
        {

            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
