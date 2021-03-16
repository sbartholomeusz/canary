using Canary.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class UnknownRecordParserTests
    {
        [Fact]
        public void ValidateParserWithGoodInputs()
        {
            // Arrange
            var sampleRecString = "6999-999            000312924700031292470000000000                        000004                                        ";
            var lineNumber = 1;

            // Act
            var rec = new UnknownRecord(sampleRecString, lineNumber.ToString());

            // Assert
            Assert.True(rec.Type == Core.Interfaces.IRecord.RecordType.Unknown);
        }

        [Fact]
        public void ValidateParserWithBadInputs()
        {
            // Arrange
            var sampleRecString = "";
            var lineNumber = "2";

            // Act
            var rec = new UnknownRecord(sampleRecString, lineNumber);

            // Assert
            Assert.True(rec.Type == Core.Interfaces.IRecord.RecordType.Unknown);
        }
    }
}
