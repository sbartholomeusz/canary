using Canary.Core;
using Canary.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count == 0);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();
                
                // Assert
                Assert.True(errors.Count == 0);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count > 0);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count > 0);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count == 1);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count == 4);
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
                var abaValidator = new AbaFileOperations();

                // Act
                var s = new StreamReader(filePath);
                var errors = abaValidator.ValidateFile(s.BaseStream);
                s.Close();

                // Assert
                Assert.True(errors.Count == 10);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
