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
    public class DescriptiveRecordValidatorTests
    {
        [Fact]
        public void Validate_GoodRec1()
        {
            // Arrange
            var sampleRecString = "0                 01CBA       John Smith                088148Payroll     280220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

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
            // Arrange
            var sampleRecString = "0";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 6);
        }

        [Fact]
        public void Validate_BadRec2()
        {
            // Arrange
            var sampleRecString = "0aaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbcccccccccccccccccccccccccccccccccccccccddddddddddddddddddddddd";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 6);
        }

        [Fact]
        public void Validate_BadRec3()
        {
            // Invalid chars in blank fields

            // Arrange
            var sampleRecString = "0XXXXXXXXXXXXXXXXX01CBAYYYYYYYJohn Smith                088148Payroll     280220ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 3);
        }

        [Fact]
        public void Validate_BadRec4()
        {
            // Illegal chars

            // Arrange
            var sampleRecString = "0                 01C#A       !ohn Smith                088148{ayro}l     280220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

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
        public void Validate_BadRec5()
        {
            // Invalid date

            // Arrange
            var sampleRecString = "0                 01CBA       John Smith                088148Payroll     400220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

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
        public void Validate_BadRec6()
        {
            // Check numeric only fields

            // Arrange
            var sampleRecString = "0                 0aCBA       John Smith                088b48Payroll     280220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 2);
        }

        [Fact]
        public void Validate_BadRec7()
        {
            // Check left alignment validation

            // Arrange
            var sampleRecString = "0                 01CBA                       John Smith088148     Payroll280220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 2);
        }

        [Fact]
        public void Validate_BadRec8()
        {
            // Empty 'Name of User Supplying File' and 'Description' field

            // Arrange
            var sampleRecString = "0                 01CBA                                 088148            280220                                        ";
            var lineNumber = "2";

            // Act
            var rec = new DescriptiveRecord(sampleRecString, lineNumber);
            var results = new DescriptiveRecordValidator(new ConsoleLogger()).Validate(rec);

            var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
            var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
            var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

            // Assert
            Assert.True(results.Count == (informationalCount + warningCount + errorCount));
            Assert.True(informationalCount == 0);
            Assert.True(warningCount == 0);
            Assert.True(errorCount == 2);
        }
    }
}
