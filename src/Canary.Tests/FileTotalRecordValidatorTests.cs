using Canary.Core.Model;
using Canary.Core.Validators;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class FileTotalRecordValidatorTests
    {
        [Fact]
        public void Validate_GoodRec1()
        {
            // Arrange
            var sampleRecString = "7999-999            000312924700031292470000000000                        000004                                        ";
            var lineNo = 3;
            var rec = new FileTotalRecord(sampleRecString, lineNo.ToString());
            var actualNetTotalAmt = 3129247;
            var actualCreditTotalAmt = 3129247;
            var actualDebitTotalAmt = 0;
            var actualDetailRecordCount = 4;

            // Act
            var results = new FileTotalRecordValidator(new ConsoleLogger()).Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 0);
        }

        [Fact]
        public void Validate_GoodRec2()
        {
            // Arrange
            var sampleRecString = "7999-999            000312276500031292470000006482                        000004                                        ";
            var lineNo = 3;
            var rec = new FileTotalRecord(sampleRecString, lineNo.ToString());
            var actualNetTotalAmt = 3122765;
            var actualCreditTotalAmt = 3129247;
            var actualDebitTotalAmt = 6482;
            var actualDetailRecordCount = 4;

            // Act
            var results = new FileTotalRecordValidator(new ConsoleLogger()).Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 0);
        }

        [Fact]
        public void Validate_BadRec1()
        {
            //
            // Mismatching net total amount
            //

            // Arrange
            var sampleRecString = "7999-999            000312276500031292470000006482                        000004                                        ";
            var lineNo = 3;
            var rec = new FileTotalRecord(sampleRecString, lineNo.ToString());
            var actualNetTotalAmt = 0;
            var actualCreditTotalAmt = 3129247;
            var actualDebitTotalAmt = 6482;
            var actualDetailRecordCount = 4;

            // Act
            var results = new FileTotalRecordValidator(new ConsoleLogger()).Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 1);
        }

        [Fact]
        public void Validate_BadRec2()
        {
            //
            // Mismatching detail record count
            //

            // Arrange
            var sampleRecString = "7999-999            000312924700031292470000000000                        000004                                        ";
            var lineNo = 3;
            var rec = new FileTotalRecord(sampleRecString, lineNo.ToString());
            var actualNetTotalAmt = 3129247;
            var actualCreditTotalAmt = 3129247;
            var actualDebitTotalAmt = 0;
            var actualDetailRecordCount = 3;

            // Act
            var results = new FileTotalRecordValidator(new ConsoleLogger()).Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 1);
        }
    }
}
