using Canary.Core;
using Canary.Core.Model;
using Canary.Logging;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Canary.Form
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly ILogger _logger = null;

        public MainWindow()
        {
            InitializeComponent();
            _logger = new Log4NetLogger();
        }

        #region Form Event Handlers

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("BtnBrowse_Click Start");
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().Value)
            {
                txtFilePath.Text = dialog.FileName;
                _logger.Info($"BtnBrowse_Click: Selected File = {txtFilePath.Text}");
            }
        }

        private void BtnValidateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.Info("BtnValidateFile_Click Start");

                var fileName = txtFilePath.Text;

                StartProgressBarAnimation();

                if (FileExists(fileName))
                {
                    ValidateFile(fileName, out var validationMsgs);
                    PopulateFormWithFileRecords(new StreamReader(fileName).BaseStream, validationMsgs);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BtnValidateFile_Click: An unexpected error occurred while processing file ...");
                _logger.Error(ex);
                throw;
            }

            StopProgressBarAnimation();
        }

        private void LvwResults_OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            _logger.Info("lvwResults_OnMouseLeftButtonDown Start");

            if (sender is ListViewItem item && item.IsSelected)
            {
                // Validation message may/may not pertain to a specific line
                var selectedLineNumber = int.TryParse((((FileValidationMessage)item.Content).LineNumber), out var outVal) ? (int?)outVal : null;
                _logger.Info($"lvwResults_OnMouseLeftButtonDown: selectedLineNumber='{selectedLineNumber}'");
                if (!selectedLineNumber.HasValue) return;

                // Select the matching line in the records list view
                for (int i = 0; i < lvwRecords.Items.Count; i++)
                {
                    if (lvwRecords.Items[i] is FileRecord rec && rec.LineNumber == selectedLineNumber)
                    {
                        lvwRecords.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        #endregion

        #region File Operations

        private Boolean FileExists(string fileName)
        {
            var exists = false;

            _logger.Info($"FileExists: Checking file '{fileName}' exists ...");

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                exists = File.Exists(fileName);
            }

            if (exists)
            {
                _logger.Info($"FileExists: File '{fileName}' exists");
            }
            else
            {
                _logger.Warning($"FileExists: File '{fileName}' does NOT exist");
            }

            return exists;
        }

        private Boolean ValidateFile(string fileName, out List<FileValidationMessage> fileValidationMessages)
        {
            const string NO_LINE_NUM = "N/A";
            fileValidationMessages = new List<FileValidationMessage>();

            try
            {
                _logger.Info("ValidateFile: Beginning file validation ...");

                // Validate file
                var tempValidationMessages = new AbaFileOperations(new Log4NetLogger()).ValidateFile(new StreamReader(fileName).BaseStream);
                _logger.Info($"ValidateFile: Found {tempValidationMessages.Count()} validation issues");

                // Display output messages on form
                foreach (var tempValidationMessage in tempValidationMessages)
                {
                    var lineNumString = tempValidationMessage.LineNumber.HasValue ? tempValidationMessage.LineNumber.ToString() : NO_LINE_NUM;

                    string type; 
                    switch (tempValidationMessage.Type)
                    {
                        case ValidationMessage.MessageTypes.Information:
                            type = "Info";
                            break;
                        case ValidationMessage.MessageTypes.Warning:
                            type = "Warning";
                            break;
                        case ValidationMessage.MessageTypes.Error:
                            type = "Error";
                            break;
                        default:
                            type = ""; // Should never happen
                            break;
                    }
                    fileValidationMessages.Add(new FileValidationMessage() { Type = type, LineNumber = lineNumString, Message = tempValidationMessage.Message });
                }

                lvwResults.ItemsSource = fileValidationMessages.OrderBy(i => i.LineNumber);
                //
            }
            catch (Exception e)
            {
                _logger.Error("ValidateFile: An unexpected error occurred while validating the file ...");
                _logger.Error(e);
                return false;
            }

            return true;
        }

        private Boolean PopulateFormWithFileRecords(Stream s, List<FileValidationMessage> validationMessages)
        {
            var fileContents = new List<FileRecord>();

            try
            {
                // TODO: Should consolidate with FileReaderService.cs
                _logger.Info("PopulateFormWithFileRecords: Reading file ...");
                using (StreamReader sr = new StreamReader(s))
                {
                    var lineNumber = 0;
                    while (!sr.EndOfStream)
                    {
                        // Read each line from file
                        lineNumber++;

                        // Highlight if validation errors logged for this record
                        var errorsForThisRec = validationMessages.Where(i => i.LineNumber == lineNumber.ToString()).ToList();
                        var highlightThisRec = errorsForThisRec.Count() > 0;

                        fileContents.Add(new FileRecord() { LineNumber=lineNumber, Record=sr.ReadLine(), HighlightThisRecord=highlightThisRec });
                    }
                }

                lvwRecords.ItemsSource = fileContents;
            }
            catch (Exception e)
            {
                _logger.Error("PopulateFormWithFileRecords: An unexpected error occurred while populating form with records ...");
                _logger.Error(e);
                return false;
            }

            return true;
        }

        #endregion

        #region Progress Bar Operations

        private void StartProgressBarAnimation()
        {
            pbProgress.IsIndeterminate = true;
            pbProgress.Visibility = Visibility.Visible;
        }

        private void StopProgressBarAnimation()
        {
            pbProgress.IsIndeterminate = false;
            pbProgress.Visibility = Visibility.Hidden;
        }

        #endregion

    }

    public class FileValidationMessage
    {

        public string Type { get; set; }
        public string LineNumber { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Line {LineNumber}: {Message}";
        }
    }

    public class FileRecord
    {
        public int LineNumber { get; set; }

        public string Record { get; set; }

        public Boolean HighlightThisRecord { get; set; }
    }
}
