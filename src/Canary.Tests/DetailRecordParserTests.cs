using Canary.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class DetailRecordParserTests
    {
        [Fact]
        public void ValidateParserWithGoodInputs()
        {
            // Arrange
            var sampleRecString = "1123-456157108231 530000001234A B Jones                       TEST BATCH        062-123 12223123MY ACCOUNT      00001450";
            var lineNumber = "2";

            // Act
            var rec = new DetailRecord(sampleRecString, lineNumber);

            // Assert
            Assert.Equal("123-456", rec.Bsb);
            Assert.Equal("157108231", rec.AccountNumber);
            Assert.Equal(" ", rec.WithholdingTaxIndicator);
            Assert.Equal("53", rec.TransactionCode);
            Assert.Equal("0000001234", rec.AmountInCents);
            Assert.Equal("A B Jones                       ", rec.AccountTitle);
            Assert.Equal("TEST BATCH        ", rec.LodgementReference);
            Assert.Equal("062-123", rec.TraceBsb);
            Assert.Equal(" 12223123", rec.TraceAccountNumber);
            Assert.Equal("MY ACCOUNT      ", rec.RemitterName);
            Assert.Equal("00001450", rec.WithholdingTaxAmountInCents);
        }

        [Fact]
        public void ValidateParserWithBadInputs()
        {
            // Arrange
            var sampleRecString = "";
            var lineNumber = "2";

            // Act
            var rec = new DetailRecord(sampleRecString, lineNumber);

            // Assert
            Assert.Equal(string.Empty, rec.Bsb);
            Assert.Equal(string.Empty, rec.AccountNumber);
            Assert.Equal(string.Empty, rec.WithholdingTaxIndicator);
            Assert.Equal(string.Empty, rec.TransactionCode);
            Assert.Equal(string.Empty, rec.AmountInCents);
            Assert.Equal(string.Empty, rec.AccountTitle);
            Assert.Equal(string.Empty, rec.LodgementReference);
            Assert.Equal(string.Empty, rec.TraceBsb);
            Assert.Equal(string.Empty, rec.TraceAccountNumber);
            Assert.Equal(string.Empty, rec.RemitterName);
            Assert.Equal(string.Empty, rec.WithholdingTaxAmountInCents);
        }
    }
}
