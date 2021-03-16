using Canary.Core.Model;
using Canary.Core.Validators;
using System;
using System.Collections.Generic;
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
            var errors = FileTotalRecordValidator.Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            // Assert
            Assert.True(errors.Count == 0);
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
            var errors = FileTotalRecordValidator.Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            // Assert
            Assert.True(errors.Count == 0);
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
            var errors = FileTotalRecordValidator.Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            // Assert
            Assert.True(errors.Count == 1);
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
            var errors = FileTotalRecordValidator.Validate(rec, actualNetTotalAmt, actualCreditTotalAmt, actualDebitTotalAmt, actualDetailRecordCount);

            // Assert
            Assert.True(errors.Count == 1);
        }
    }
}
