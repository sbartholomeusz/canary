using Canary.Core;
using Canary.Core.Helpers;
using Canary.Core.Model;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Canary.Tests
{
    public class AbaFileOperationTests
    {
        [Fact]
        public void Validate_ValidFile1()
        {
            try
            {
                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-valid-01.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 1);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_ValidFile2()
        {
            try
            {
                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-valid-02.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 1);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_BadFile1()
        {
            try
            {
                //
                // Empty file
                //

                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-empty.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 0);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_BadFile2()
        {
            try
            {
                //
                // File contains missing totals
                //

                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-missingtotals.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 0);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 3);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_BadFile3()
        {
            try
            {
                //
                // File contains missing descriptive record
                //

                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-missingdescriptive.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 0);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_BadFile4()
        {
            try
            {
                //
                // File contains missing detail records
                //

                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-missingdetail.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 0);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 4);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Fact]
        public void Validate_BadFile5()
        {
            try
            {
                //
                // File contains bad characters
                //

                // Arrange
                var filePath = RuntimeEnvironment.GetCurrentExecutionFolderPath() + "\\SampleFiles\\sample-aba-file-badchars.aba";
                var abaValidator = new AbaFileOperations(new ConsoleLogger());

                // Act
                List<ValidationMessage> results;
                using (var s = new StreamReader(filePath))
                {
                    results = abaValidator.ValidateFile(s.BaseStream);
                    s.Close();
                }

                var informationalCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorCount = results.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                // Assert
                Assert.True(results.Count == (informationalCount + warningCount + errorCount));
                Assert.True(informationalCount == 0);
                Assert.True(warningCount == 0);
                Assert.True(errorCount == 10);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
