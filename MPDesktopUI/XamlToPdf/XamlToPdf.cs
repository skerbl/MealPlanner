﻿using MPData;
using System.IO;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using PdfSharp.Xps;
using System;
using System.Windows.Threading;
using System.Windows;

namespace MPDesktopUI
{
    public class XamlToPdf
    {
        public event EventHandler<MessageEventArgs> OnErrorMessageRaised;

        private MainViewModel _mainViewModel;

        #region Constructor

        public XamlToPdf(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Saves the generated data from the MainViewModel in the template as a PDF file
        /// </summary>
        public void SaveAsPdf()
        {
            if (string.IsNullOrEmpty(_mainViewModel.FileName))
            {
                RaiseMessage("Bitte geben Sie einen Dateinamen an.");
                return;
            }

            if (!Utils.IsValidFilename(_mainViewModel.FileName))
            {
                RaiseMessage("Der Dateiname ist ungültig.");
                //_mainViewModel.FileName = "";
                return;
            }

            string tempFileName = "tempFile.xps";
            string combinedPath = Path.Combine(_mainViewModel.Settings.ExportPath, Path.GetFileName(_mainViewModel.FileName));
            FileInfo file = new FileInfo(combinedPath);

            SaveAsXps(tempFileName);
            ConvertXpsToPdf(tempFileName, Path.ChangeExtension(file.FullName, ".pdf"));

            File.Delete(tempFileName);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes the XAML template page to an XPS file, which can be converted to PDF
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveAsXps(string fileName)
        {
            //Set up the WPF Control to be printed
            XamlToPdfTest controlToPrint;
            controlToPrint = new XamlToPdfTest
            {
                DataContext = _mainViewModel
            };

            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            //Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));
            
            //Create first page of document
            fixedPage.Width = 793.92;
            fixedPage.Height = 1122.24;
            fixedPage.Children.Add(controlToPrint);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            XpsDocument xpsd = new XpsDocument(fileName, FileAccess.ReadWrite);
            System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDoc);
            xpsd.Close();
        }

        /// <summary>
        /// Converts an XPS file to PDF
        /// </summary>
        /// <param name="xpsFileName">The XPS file name</param>
        /// <param name="pdfFileName">The PDF file name</param>
        private void ConvertXpsToPdf(string xpsFileName, string pdfFileName)
        {
            using (PdfSharp.Xps.XpsModel.XpsDocument pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(xpsFileName))
            {
                XpsConverter.Convert(pdfXpsDoc, pdfFileName, 0);
            }
        }

        private void RaiseMessage(string message)
        {
            OnErrorMessageRaised?.Invoke(this, new MessageEventArgs(message));
        } 

        #endregion
    }
}
