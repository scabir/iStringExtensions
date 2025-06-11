using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace iStringExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Wraps a string in a list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> WrapWithAList(this string input)
        {
            Guard.AgainstNull(input);

            return new List<string>() { input };
        }

        /// <summary>
        /// Wraps a string in an array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] WrapWithAnArray(this string input)
        {
            Guard.AgainstNull(input);

            return new[] { input };
        }

        /// <summary>
        /// Calculate the Levenshtein distance and percentage between two strings.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="textToCalculate"></param>
        /// <returns></returns>
        public static int LevenshteinDistance(this string input, string textToCalculate)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(textToCalculate);

            return Fastenshtein.Levenshtein.Distance(input, textToCalculate);
        }

        /// <summary>
        /// Calculate the Levenshtein distance and percentage between two strings.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="textToCalculate"></param>
        /// <returns></returns>
        public static double LevenshteinPercentage(this string input, string textToCalculate)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(textToCalculate);

            var percentageBase = Math.Max(input.Length, textToCalculate.Length);
            var distance = input.LevenshteinDistance(textToCalculate);

            return (double)100 * ((double)distance / (double)percentageBase);
        }

        /// <summary>
        /// Splits a string into an Enumerable of a primitive type. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <param name="ignoreNonMatching"></param>
        /// <returns></returns>
        public static IEnumerable<T> SplitAsEnumerableOf<T>(this string input, string separator, bool ignoreNonMatching = false)
            where T : struct
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(separator);

            var result = new List<T>();
            var parts = input.Split(separator.WrapWithAnArray(), StringSplitOptions.None);

            foreach (var part in parts)
            {
                try
                {
                    result.Add((T)Convert.ChangeType(part, typeof(T)));
                }
                catch (Exception)
                {
                    if (!ignoreNonMatching)
                    {
                        throw new StringExtensionsException($"All elements in the array must be in type { typeof(T).ToString() }");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Perform Regex matching operations.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static bool RegexMatch(this string input, string regexPattern)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(regexPattern);

            return (new Regex(regexPattern)).IsMatch(input);
        }

        /// <summary>
        /// Perform Regex matching and extraction operations.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static string[] RegexExtract(this string input, string regexPattern)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(regexPattern);

            var regex = new Regex(regexPattern).Match(input);
            var results = new List<string>();

            if (regex?.Groups != null)
            {
                for(int i = 0; i < regex.Groups.Count; i++) {
                    results.Add(regex.Groups[i].Value);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Check if a string is valid JSON
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static bool IsJson(this string input)
        {
            Guard.AgainstNull(input);

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            try
            {
                JsonConvert.DeserializeObject(input);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if a string is a valid number
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static bool IsNumber(this string input)
        {
            Guard.AgainstNull(input);

            return double.TryParse(input, out var output);
        }

        /// <summary>
        /// Check if a string is a valid integer.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static bool IsInteger(this string input)
        {
            Guard.AgainstNull(input);

            return int.TryParse(input, out var output);
        }

        /// <summary>
        /// Deserialize a JSON string to a specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string input)
        {
            Guard.AgainstNull(input);

            return JsonConvert.DeserializeObject<T>(input);
        }

        /// <summary>
        /// Compute MD5 hash of a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(this string input)
        {
            Guard.AgainstNull(input);

            using (MD5 md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Compute SHA1 hash of a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha1Hash(this string input)
        {
            Guard.AgainstNull(input);

            using (SHA1 sha1 = SHA1.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha1.ComputeHash(inputBytes);
                var sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Convert a string to a MemoryStream.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this string input)
        {
            Guard.AgainstNull(input);

            return new MemoryStream(input.ToBytes());
        }

        /// <summary>
        /// Convert a string to an Exception object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Exception ToException(this string input)
        {
            Guard.AgainstNull(input);

            return new Exception(input);
        }

        /// <summary>
        /// Convert a string to an Exception object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Exception ToException<TException>(this string input)
            where TException : Exception
        {
            Guard.AgainstNull(input);

            return (TException)input.ToException();
        }

        /// <summary>
        /// Convert a string to a StringBuilder object.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static StringBuilder ToStringBuilder(this string input)
        {
            Guard.AgainstNull(input);

            return new StringBuilder(input);
        }

        /// <summary>
        /// Reverse a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReverseString(this string input)
        {
            Guard.AgainstNull(input);

            return new string(input.Reverse().ToArray());
        }

        /// <summary>
        /// Replace all whitespace characters with a specified whitespace character.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="whiteSpaceType"></param>
        /// <returns></returns>
        public static string AllWhiteSpacesTo(this string input, WhiteSpaceType whiteSpaceType)
        {
            Guard.AgainstNull(input);
            
            if (whiteSpaceType == null)
            {
                throw new ArgumentNullException(nameof(whiteSpaceType));
            }

            string whiteSpace = " ";

            switch (whiteSpaceType)
            {
                case WhiteSpaceType.Space:
                    whiteSpace = " ";
                    break;
                case WhiteSpaceType.Newline:
                    whiteSpace = "\r\n";
                    break;
                case WhiteSpaceType.Tab:
                    whiteSpace = '\t'.ToString();
                    break;
            }

            return input
                .Replace(" ", whiteSpace)
                .Replace("\r\n", whiteSpace)
                .Replace("\n", whiteSpace)
                .Replace("\t", whiteSpace);
        }

        /// <summary>
        /// Replace multiple consecutive spaces with a single space.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConsolidateSpaces(this string input)
        {
            Guard.AgainstNull(input);

            return Regex.Replace(input, @"\s+", " ");
        }

        /// <summary>
        /// Check if a string is a valid IPv4 address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIpV4(this string input)
        {
            Guard.AgainstNull(input);

            const string IpV4Regex = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            return input.RegexMatch(IpV4Regex);
        }

        /// <summary>
        /// Check if a string is a valid IPv6 address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIpV6(this string input)
        {
            Guard.AgainstNull(input);

            const string IpV6Regex = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";

            return input.RegexMatch(IpV6Regex);
        }

        /// <summary>
        /// Convert a string to a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string input)
        {
            Guard.AgainstNull(input);

            return Encoding.UTF8.GetBytes(input);
        }

        /// <summary>
        /// Check if a string formed as a valid email address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            Guard.AgainstNull(input);

            const string EmailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            return input.RegexMatch(EmailRegex);
        }

        /// <summary>
        ///  Convert a query string to a dictionary.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="StringExtensionsException"></exception>
        public static Dictionary<string, string> QueryStringToDictionary(this string input)
        {
            Guard.AgainstNull(input);

            if (!input.Contains("="))
            {
                throw new StringExtensionsException("Not a valid querystring");
            }

            string[] parts;

            if (!input.Contains(";"))
            {
                parts = input.WrapWithAnArray();
            }
            else
            {
                parts = input.Split(";".WrapWithAnArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            var result = new Dictionary<string, string>();

            foreach (var part in parts)
            {
                var keyValue = part.Split('=');

                if (keyValue.Length != 2)
                {
                    throw new StringExtensionsException($"QueryString format error. Not matching element is {part} ");
                }

                if (result.ContainsKey(keyValue[0]))
                {
                    throw new StringExtensionsException($"Duplicate key {keyValue[0]}");
                }

                result.Add(keyValue[0], keyValue[1]);
            }

            return result;
        }

        //Count the occurrences of a specified character in a string.
        public static int CountOccurences(this string input, string character)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(character);

            return input.Split(new string[] { character }, StringSplitOptions.None).Length - 1;
        }
    }
}
