using Canary.Core.Interfaces;
using Canary.Core.Model;
using Canary.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class DetailRecordValidatorTests
    {
        [Fact]
        public void Validate_GoodRec1()
        {
            // Arrange
            var detailString = "1123-456157108231 530000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void Validate_GoodRec2()
        {
            // Variation of Withholding Tax Indicator and Transaction Code

            // Arrange
            var detailString = "1123-456157108231X500000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 0);
        }

        [Fact]
        public void Validate_BadRec1()
        {
            // Arrange
            var detailString = "1";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count > 0);
        }

        [Fact]
        public void Validate_BadRec2()
        {
            // Arrange
            var detailString = "1aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaad";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count > 0);
        }

        [Fact]
        public void Validate_BadRec3()
        {
            //
            // Invalid Withholding Tax Indicator code
            //

            // Arrange
            var detailString = "1123-456157108231A530000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 1);
        }

        [Fact]
        public void Validate_BadRec4()
        {
            //
            // Invalid Withholding Tax Indicator code
            //

            // Arrange
            var detailString = "1123-456157108231}530000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 2);
        }

        [Fact]
        public void Validate_BadRec5()
        {
            //
            // Invalid Transaction Code Test
            //

            // Arrange
            var detailString = "1123-456157108231 030000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 1);
        }

        [Fact]
        public void Validate_BadRec6()
        {
            //
            // Invalid Transaction Code Test
            //

            // Arrange
            var detailString = "1123-456157108231 {30000001234S R SMITH                       TEST BATCH        062-000 12223123MY ACCOUNT      00001200";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 1);
        }

        [Fact]
        public void Validate_BadRec7()
        {
            //
            // Alpha chars in numeric fields
            //

            // Arrange
            var detailString = "1x23-456x57108231 530000x01234S R SMITH                       TEST BATCH        062-x00 12223123MY ACCOUNT      000012x0";
            var lineNum = "2";
            var rec = new DetailRecord(detailString, lineNum);

            // Act
            var errors = DetailRecordValidator.Validate(rec);

            // Assert
            Assert.True(errors.Count == 5);
        }
    }
}
