using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace iStringExtensions
{
    public static class StringExtensions
    {
        public static List<string> WrapWithAList(this string input) => new List<string>() { input };

        public static string[] WrapWithAnArray(this string input) => new [] { input };

        public static int LevenshteinDistance(this string input, string textToCalculate)
        {
            return Fastenshtein.Levenshtein.Distance(input, textToCalculate);
        }

        public static double LevenshteinPercentage(this string input, string textToCalculate)
        {
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
            where T: struct
        {
            Guard.AgainstNull(input);

            var result = new List<T>();
            var parts = input.Split(input.WrapWithAnArray(), StringSplitOptions.None);

            foreach (var part in parts)
            {
                try
                {
                    result.Add((T)Convert.ChangeType(part, typeof(T)));
                }
                catch (Exception ex)
                {
                    if (!ignoreNonMatching)
                    {
                        throw new StringExtensionsException($"All elements in the array must be in type { typeof(T).ToString() }");
                    }
                }
            }

            return result;
        }


        public static bool RegexMatch(this string input, string regexPattern)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(regexPattern);

            return (new Regex(regexPattern)).IsMatch(input);
        }

        public static string[] RegexExtract(this string input, string regexPattern)
        {
            Guard.AgainstNull(input);
            Guard.AgainstNull(regexPattern);

            var regex = new Regex(regexPattern).Match(input);
            var results = new List<string>();

            for (int i = 0; i < regex.Groups.Count; i++)
            {
                results.Add(regex.Groups[i].Value);
            }

            return results.ToArray();
        }

        public static bool IsJson(this string input)
        {
            Guard.AgainstNull(input);

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

        public static bool IsNumber(this string input) => double.TryParse(input, out var output);

        public static bool IsInteger(this string input) => int.TryParse(input, out var output);

        public static T DeserializeJson<T>(this string input) => JsonConvert.DeserializeObject<T>(input);

        public static string GetMd5Hash(this string input)
        {
            throw new NotImplementedException();
        }

        public static string GetSha1Hash(this string input)
        {
            throw new NotImplementedException();
        }

        public static MemoryStream ToMemoryStream(this string input)
        {
            Guard.AgainstNull(input);

            return new MemoryStream(input.ToBytes());
        }

        public static Exception ToException(this string input) => new Exception(input);

        public static Exception ToException<TException>(this string input)
            where TException : Exception
        {
            return (TException)input.ToException();
        }

        public static StringBuilder ToStringBuilder(this string input) => new StringBuilder(input);

        public static string ReverseString(this string input)
        {
            Guard.AgainstNull(input);

            var result = new StringBuilder();

            for (int i = input.Length - 1; i > 0; i--)
            {
                result.Append(input[i]);
            }

            return result.ToString();
        }

        public static string ReverseWords(this string input)
        {
            Guard.AgainstNull(input);


            throw new NotImplementedException();
        }

        public static string AllWhiteSpacesTo(this string input, WhiteSpaceType whiteSpaceType)
        {
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

        public static string ConsolidateSpaces(this string input)
        {
            Guard.AgainstNull(input);

            var result = input.Clone().ToString();

            while (result.Contains("  "))
                result = result.Replace("  ", " ");

            return result;
        }

        public static bool IsIpV4(this string input)
        {
            Guard.AgainstNull(input);

            const string IpV4Regex = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            return input.RegexMatch(IpV4Regex);
        }

        public static bool IsIpV6(this string input)
        {
            Guard.AgainstNull(input);

            const string IpV6Regex = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";

            return input.RegexMatch(IpV6Regex);
        }

        public static byte[] ToBytes(this string input) => Encoding.UTF8.GetBytes(input);

        public static bool IsEmail(this string input)
        {
            Guard.AgainstNull(input);

            const string EmailRegex = @"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*";

            return input.RegexMatch(EmailRegex);
        }

        public static Dictionary<string, string> QueryStringToDictionary(this string input)
        {
            Guard.AgainstNull(input);

            if (!input.Contains("="))
            {
                throw new StringExtensionsException("Not a valid querystring");
            }

            var parts = new string[1];

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

        public static int CountOccurences(this string input, string character)
        {
            var count = 0;
            var i = 0;

            while ((i = input.IndexOf(character, i)) != -1)
            {
                i += character.Length;
                count++;
            }

            return count;
        }
    }
}
