using System;
using Xunit;
using iStringExtensions;
using FluentAssertions;
using System.Collections.Generic;

namespace UnitTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void WrapWithAList_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var expected = new List<string>() { testString };

            // act
            var actual = testString.WrapWithAList();

            // assert
            actual.Should().Equals(expected);
            Assert.Equal(actual, expected);
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
            actual.Should().Equals(expected);
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void LevenshteinDistance_ReturnsCorrectResult()
        {
            // arrange
            var testString = "Some test string";
            var textToCompare = "Some tset strign";
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
            var textToCompare = "Some tset strign";
            var expected = 2;

            // act
            var actual = testString.LevenshteinPercentage(textToCompare);

            // assert
            actual.Should().Equals(expected);
        }
    }
}
