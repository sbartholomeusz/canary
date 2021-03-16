using Canary.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class FileTotalRecordParserTests
    {
        [Fact]
        public void ValidateParserWithGoodInputs()
        {
            // Arrange
            var sampleRecString = "7999-999            000312924700031292470000000000                        000004                                        ";
            var lineNumber = 3;

            // Act
            var rec = new FileTotalRecord(sampleRecString, lineNumber.ToString());

            // Assert
            Assert.True(rec.Type == Core.Interfaces.IRecord.RecordType.FileTotal);
            Assert.Equal("999-999", rec.Bsb);
            Assert.Equal("            ", rec.Blank1);
            Assert.Equal("0003129247", rec.NetTotalInCents);
            Assert.Equal("0003129247", rec.CreditTotalInCents);
            Assert.Equal("0000000000", rec.DebitTotalInCents);
            Assert.Equal("                        ", rec.Blank2);
            Assert.Equal("000004", rec.DetailRecordCount);
            Assert.Equal("                                        ", rec.Blank3);
        }

        [Fact]
        public void ValidateParserWithBadInputs()
        {
            // Arrange
            var sampleRecString = "";
            var lineNumber = 1;

            // Act
            var rec = new FileTotalRecord(sampleRecString, lineNumber.ToString());

            // Assert
            Assert.Equal(string.Empty, rec.Bsb);
            Assert.Equal(string.Empty, rec.Blank1);
            Assert.Equal(string.Empty, rec.NetTotalInCents);
            Assert.Equal(string.Empty, rec.CreditTotalInCents);
            Assert.Equal(string.Empty, rec.DebitTotalInCents);
            Assert.Equal(string.Empty, rec.Blank2);
            Assert.Equal(string.Empty, rec.DetailRecordCount);
            Assert.Equal(string.Empty, rec.Blank3);
        }
    }
}
