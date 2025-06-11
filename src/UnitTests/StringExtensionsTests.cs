using System;
using Xunit;
using iStringExtensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests.Model;
using System.IO;

namespace UnitTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void WrapWithAList_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var expected = new List<string> { testString };

            // act
            var actual = testString.WrapWithAList();

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WrapWithAnArray_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var expected = new string[] { testString };

            // act
            var actual = testString.WrapWithAnArray();

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void LevenshteinDistance_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var textToCompare = "Some tset string";
            var expected = 2;

            // act
            var actual = testString.LevenshteinDistance(textToCompare);

            // assert
            actual.Should().Equals(expected);
        }

        [Fact]
        public void LevenshteinPercentage_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var textToCompare = "Some tset string";
            var expected = 12.5;

            // act
            var actual = testString.LevenshteinPercentage(textToCompare);

            // assert
            actual.Should().Equals(expected);
        }

        [Fact]
        public void SplitAsEnumerableOf_ReturnsCorrectResult()
        {
            // arrange
            var testString = "12,23,34";
            var expected = (new List<int>()
                {
                    12,
                    23,
                    34
                }).AsEnumerable();

            // act
            var actual = testString.SplitAsEnumerableOf<int>(",");

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SplitAsEnumerableOf_ThrowsExceptionIfTypeMismatch()
        {
            // arrange
            var testString = "asdf,fdsa,ert";

            // act && assert
            Action act = () => testString.SplitAsEnumerableOf<int>(",");
            act.Should().Throw<StringExtensionsException>();
        }

        [Theory]
        [InlineData("12/04/2019", true)]
        [InlineData("12.04.2019", false)]
        public void RegexMatch_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            const string pattern = @"\d{2}/\d{2}/\d{4}";

            // act
            var actual = testString.RegexMatch(pattern);

            // assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("12/04/2019", "12/04/2019")]
        [InlineData("sadf 12/04/2019", "12/04/2019")]
        [InlineData("12/04/2019 dsadf", "12/04/2019")]
        [InlineData("fdasdf 12/04/2019 dsadf", "12/04/2019")]
        public void RegexExtract_ReturnsCorrectResult(string testString, string expected)
        {
            // arrange
            const string pattern = @"\d{2}/\d{2}/\d{4}";

            // act
            var actual = testString.RegexExtract(pattern);

            // assert
            actual.Should().BeEquivalentTo(expected.WrapWithAnArray());
        }

        [Theory]
        [InlineData("{\"name\":\"some name\"}", true)]
        [InlineData("{\"name\":\"some name\"", false)]
        [InlineData("this is not a json", false)]
        public void IsJson_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsJson();

            // assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("3", true)]
        [InlineData("3.2", true)]
        [InlineData("-1", true)]
        [InlineData("999999999999999999", true)]
        [InlineData("Hello", false)]
        [InlineData("Hello x 2", false)]
        public void IsNumber_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsNumber();

            // assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("3", true)]
        [InlineData("3.2", false)]
        [InlineData("-1", true)]
        [InlineData("999999999999999999", false)]
        [InlineData("Hello", false)]
        [InlineData("Hello x 2", false)]
        public void IsInteger_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsInteger();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void DeserializeJson_ReturnsCorrectResult()
        {
            // arrange
            string json = "{\"Name\":\"John\", \"Age\":30}";
            var expected = new Person { Name = "John", Age = 30 };

            // act
            var actual = json.DeserializeJson<Person>();

            // assert
            actual.Name.Should().Be(expected.Name);
            actual.Age.Should().Be(expected.Age);
        }

        [Fact]
        public void GetMd5Hash_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Test String";
            var expected = "BD08BA3C982EAAD768602536FB8E1184";

            // act
            var actual = testString.GetMd5Hash();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void GetSha1Hash_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Test String";
            var expected = "A5103F9C0B7D5FF69DDC38607C74E53D4AC120F2";

            // act
            var actual = testString.GetSha1Hash();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ToMemoryStream_ReturnsCorrectResult()
        {
            // arrange
            string testString = "Hello, world!";
            byte[] expected = Encoding.UTF8.GetBytes(testString);

            // act
            MemoryStream result = testString.ToMemoryStream();
            byte[] actual = result.ToArray();

            // assert
            actual.Should().Equal(expected);
        }

        [Fact]
        public void ToException_ReturnsCorrectResult()
        {
            // arrange
            const string exceptionMessage = "Exception Message";
            var testString = exceptionMessage;
            var expected = new Exception(exceptionMessage);

            // act
            var actual = testString.ToException();

            // assert
            actual.Should().NotBeNull();
            actual.Message.Should().Be(exceptionMessage);
        }

        [Fact]
        public void ToStringBuilder_ReturnsCorrectResult()
        {
            // arrange
            const string message = "Exception Message";
            var testString = message;
            var expected = message;

            // act
            var actual = testString.ToStringBuilder();

            // assert
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void ReverseString_ReturnsCorrectResult()
        {
            // arrange
            var testString = "asdf";
            var expected = "fdsa";

            // act
            var actual = testString.ReverseString();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void AllWhiteSpacesTo_ReturnsCorrectResult()
        {
            // arrange
            var testString = "This text\ncontains\tsome white\r\nspaces";
            var expected = "This text contains some white spaces";

            // act
            var actual = testString.AllWhiteSpacesTo(WhiteSpaceType.Space);

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ConsolidateSpaces_ReturnsCorrectResult()
        {
            // arrange
            var testString = "This text    contains  some  spaces";
            var expected = "This text contains some spaces";

            // act
            var actual = testString.ConsolidateSpaces();

            // assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("10.0.0.12", true)]
        [InlineData("255.255.255.255", true)]
        [InlineData("asdf", false)]
        [InlineData("255.255.255.300", false)]
        public void IsIpV4_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsIpV4();

            // assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("0:0:0:0:0:0:0:1", true)]
        [InlineData("255.255.255.255", false)]
        [InlineData("asdf", false)]
        public void IsIpV6_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsIpV6();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ToBytes_ReturnsCorrectResult()
        {
            // arrange
            string testString = "Hello, World!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes(testString);

            // act
            byte[] actualBytes = testString.ToBytes();

            // assert
            actualBytes.Should().Equal(expectedBytes);
        }

        [Theory]
        [InlineData("asdf@asdf.com", true)]
        [InlineData("asdf@asdf.london", true)]
        [InlineData("asdf.fdsa@asdf.com", true)]
        [InlineData("asdf_fdsa@asdf.com", true)]
        [InlineData("asdf fdsa@asdf.com", false)]
        public void IsEmail_ReturnsCorrectResult(string testString, bool expected)
        {
            // arrange
            // act
            var actual = testString.IsEmail();

            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void QueryStringToDictionary_ReturnsCorrectResult()
        {
            // arrange
            string testQueryString = "key1=value1;key2=value2;key3=value3";
            var expectedDictionary = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                    { "key3", "value3" },
                };

            // act
            var actualDictionary = testQueryString.QueryStringToDictionary();

            // assert
            actualDictionary.Should().Equal(expectedDictionary);
        }

        [Theory]
        [InlineData("hello world", "l", 3)]
        [InlineData("hello world", "o", 2)]
        [InlineData("hello world", " ", 1)]
        [InlineData("hello world", "z", 0)]
        public void CountOccurences_ReturnsCorrectResult(string testString, string character, int expected)
        {
            // act
            var actual = testString.CountOccurences(character);

            // assert
            actual.Should().Be(expected);
        }

    }
}
