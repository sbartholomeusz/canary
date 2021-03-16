using Canary.Core.Model;
using System;
using Xunit;

namespace Canary.Tests
{
    public class DescriptiveRecordParserTests
    {
        [Fact]
        public void ValidateParserWithGoodInputs()
        {
            // Arrange
            var sampleRecString = "0                 01CBA       John Smith                088148Payroll     280220                                        ";
            var lineNumber = 1;

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber.ToString());

            // Assert
            Assert.Equal("                 ", rec.Blank1);
            Assert.Equal("01", rec.ReelSequenceNumber);
            Assert.Equal("CBA", rec.UserFinancialInstitutionName);
            Assert.Equal("       ", rec.Blank2);
            Assert.Equal("John Smith                ", rec.UserPreferredSpecification);
            Assert.Equal("088148", rec.UserIdentificationNumber);
            Assert.Equal("Payroll     ", rec.Description);
            Assert.Equal("280220", rec.DateToBeProcessed);
            Assert.Equal("                                        ", rec.Blank3);
        }

        [Fact]
        public void ValidateParserWithBadInputs()
        {
            // Arrange
            var sampleRecString = "";
            var lineNumber = 1;

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber.ToString());

            // Assert
            Assert.Equal(string.Empty, rec.Blank1);
            Assert.Equal(string.Empty, rec.ReelSequenceNumber);
            Assert.Equal(string.Empty, rec.UserFinancialInstitutionName);
            Assert.Equal(string.Empty, rec.Blank2);
            Assert.Equal(string.Empty, rec.UserPreferredSpecification);
            Assert.Equal(string.Empty, rec.UserIdentificationNumber);
            Assert.Equal(string.Empty, rec.Description);
            Assert.Equal(string.Empty, rec.DateToBeProcessed);
            Assert.Equal(string.Empty, rec.Blank3);
        }
    }
}
