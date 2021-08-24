namespace ConformityCheck.Common.ValidationAttributes.Tests
{
    using System;

    using Xunit;

    public class DataAttributeTests
    {
        private const string MinDate = "01/01/2000";

        [Fact]
        public void IsValidReturnsFalseForDatesAfterCurrentDate()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.IsValid(DateTime.UtcNow.AddYears(1));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidReturnsFalseForDatesBeforeMinDate()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.IsValid("01/01/1950");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidReturnsFalseForInvalidDateTimeInput()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.IsValid("01.01.2001");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidReturnsTrueForPreviousDate()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.IsValid(DateTime.UtcNow.AddYears(-1));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidReturnsTrueForCurrentDate()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.IsValid(DateTime.UtcNow);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetsProperMinDate()
        {
            // Arange
            var attribute = new DateAttribute(MinDate);

            // Act
            var result = attribute.MinDate;

            // Assert
            Assert.Equal(result, DateTime.Parse(MinDate));
        }
    }
}
