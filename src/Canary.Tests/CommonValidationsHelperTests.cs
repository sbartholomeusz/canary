using Canary.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class CommonValidationsHelperTests
    {
        [Fact]
        public void CheckIsLeftJustifiedValidation()
        {
            // Arrange
            var stringToTest = "";
            var stringToTest2 = " ";
            var stringToTest3 = " ABC";
            var stringToTest4 = "ABC ";
            var stringToTest5 = " ABC ";

            // Act
            var result1 = CommonValidationRoutines.IsLeftJustified(stringToTest);
            var result2 = CommonValidationRoutines.IsLeftJustified(stringToTest2);
            var result3 = CommonValidationRoutines.IsLeftJustified(stringToTest3);
            var result4 = CommonValidationRoutines.IsLeftJustified(stringToTest4);
            var result5 = CommonValidationRoutines.IsLeftJustified(stringToTest5);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
            Assert.True(result4);
            Assert.False(result5);
        }

        [Fact]
        public void CheckIsRightJustifiedValidation()
        {
            // Arrange
            var stringToTest = "";
            var stringToTest2 = " ";
            var stringToTest3 = " ABC";
            var stringToTest4 = "ABC ";
            var stringToTest5 = " ABC ";

            // Act
            var result1 = CommonValidationRoutines.IsRightJustified(stringToTest);
            var result2 = CommonValidationRoutines.IsRightJustified(stringToTest2);
            var result3 = CommonValidationRoutines.IsRightJustified(stringToTest3);
            var result4 = CommonValidationRoutines.IsRightJustified(stringToTest4);
            var result5 = CommonValidationRoutines.IsRightJustified(stringToTest5);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.False(result4);
            Assert.False(result5);
        }

        [Fact]
        public void CheckIsBlankFilledValidation()
        {
            // Arrange
            var stringToTest = "";
            var stringToTest2 = " ";
            var stringToTest3 = " ABC ";

            // Act
            var result1 = CommonValidationRoutines.IsBlankFilled(stringToTest);
            var result2 = CommonValidationRoutines.IsBlankFilled(stringToTest2);
            var result3 = CommonValidationRoutines.IsBlankFilled(stringToTest3);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
        }

        [Fact]
        public void CheckIsValidDateValidation()
        {
            // Arrange
            var stringToTest = "";
            var stringToTest2 = " ";
            var stringToTest3 = "200120";
            var stringToTest4 = "990120";

            // Act
            var result1 = CommonValidationRoutines.IsValidDateString(stringToTest);
            var result2 = CommonValidationRoutines.IsValidDateString(stringToTest2);
            var result3 = CommonValidationRoutines.IsValidDateString(stringToTest3);
            var result4 = CommonValidationRoutines.IsValidDateString(stringToTest4);

            // Assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.True(result3);
            Assert.False(result4);
        }

        [Fact]
        public void CheckIsCentsStringValidation()
        {
            // Arrange
            var stringToTest = "";
            var stringToTest2 = " ";
            var stringToTest3 = "0000";
            var stringToTest4 = "00.00";
            var stringToTest5 = "a000";
            var stringToTest6 = "000$";

            // Act
            var result1 = CommonValidationRoutines.IsCentsString(stringToTest);
            var result2 = CommonValidationRoutines.IsCentsString(stringToTest2);
            var result3 = CommonValidationRoutines.IsCentsString(stringToTest3);
            var result4 = CommonValidationRoutines.IsCentsString(stringToTest4);
            var result5 = CommonValidationRoutines.IsCentsString(stringToTest5);
            var result6 = CommonValidationRoutines.IsCentsString(stringToTest6);

            // Assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.True(result3);
            Assert.False(result4);
            Assert.False(result5);
            Assert.False(result6);
        }
    }
}
