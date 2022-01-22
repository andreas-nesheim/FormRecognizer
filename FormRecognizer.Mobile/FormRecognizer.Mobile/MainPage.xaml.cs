using Azure.AI.FormRecognizer.DocumentAnalysis;
using FormRecognizer.Core;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FormRecognizer.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var photoResult = await MediaPicker.PickPhotoAsync();
            var fullPath = photoResult.FullPath;
            var fileName = photoResult.FileName;

            ActivityIndicator.IsRunning = true;

            var uploaderClient = new BlobUploader();
            var blobUrl = await uploaderClient.UploadSync(fileName, fullPath);

            var client = new FormRecognizerClient();
            var result = await client.AnalyzeAsync(blobUrl);

            foreach (var document in result.Documents)
            {
                if (document.Fields.TryGetValue("VendorName", out DocumentField vendorNameField))
                {
                    if (vendorNameField.ValueType == DocumentFieldType.String)
                    {
                        string vendorName = vendorNameField.AsString();
                        Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}");
                        VendorNameLabel.Text = $"Vendor Name: {vendorName}";
                    }
                }

                if (document.Fields.TryGetValue("CustomerName", out DocumentField customerNameField))
                {
                    if (customerNameField.ValueType == DocumentFieldType.String)
                    {
                        string customerName = customerNameField.AsString();
                        Console.WriteLine($"Customer Name: '{customerName}', with confidence {customerNameField.Confidence}");
                    }
                }

                if (document.Fields.TryGetValue("Items", out DocumentField itemsField))
                {
                    if (itemsField.ValueType == DocumentFieldType.List)
                    {
                        foreach (DocumentField itemField in itemsField.AsList())
                        {
                            Console.WriteLine("Item:");

                            if (itemField.ValueType == DocumentFieldType.Dictionary)
                            {
                                IReadOnlyDictionary<string, DocumentField> itemFields = itemField.AsDictionary();

                                if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                                {
                                    if (itemDescriptionField.ValueType == DocumentFieldType.String)
                                    {
                                        string itemDescription = itemDescriptionField.AsString();

                                        Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                    }
                                }

                                if (itemFields.TryGetValue("Amount", out DocumentField itemAmountField))
                                {
                                    if (itemAmountField.ValueType == DocumentFieldType.Double)
                                    {
                                        double itemAmount = itemAmountField.AsDouble();

                                        Console.WriteLine($"  Amount: '{itemAmount}', with confidence {itemAmountField.Confidence}");
                                    }
                                }
                            }
                        }
                    }
                }

                if (document.Fields.TryGetValue("SubTotal", out DocumentField subTotalField))
                {
                    if (subTotalField.ValueType == DocumentFieldType.Double)
                    {
                        double subTotal = subTotalField.AsDouble();
                        Console.WriteLine($"Sub Total: '{subTotal}', with confidence {subTotalField.Confidence}");
                    }
                }

                if (document.Fields.TryGetValue("TotalTax", out DocumentField totalTaxField))
                {
                    if (totalTaxField.ValueType == DocumentFieldType.Double)
                    {
                        double totalTax = totalTaxField.AsDouble();
                        Console.WriteLine($"Total Tax: '{totalTax}', with confidence {totalTaxField.Confidence}");
                    }
                }

                if (document.Fields.TryGetValue("InvoiceTotal", out DocumentField invoiceTotalField))
                {
                    if (invoiceTotalField.ValueType == DocumentFieldType.Double)
                    {
                        double invoiceTotal = invoiceTotalField.AsDouble();
                        Console.WriteLine($"Invoice Total: '{invoiceTotal}', with confidence {invoiceTotalField.Confidence}");
                        InvoiceTotalLabel.Text = $"Invoice Total: {invoiceTotal}";
                    }
                }

                if (document.Fields.TryGetValue("DueDate", out DocumentField dueDateField))
                {
                    if (dueDateField.ValueType == DocumentFieldType.Date)
                    {
                        // This doesn't work, even though it is a date...
                        //var dueDate = dueDateField.AsDate();
                        var dueDate = dueDateField.Content;
                        Console.WriteLine($"Due Date: '{dueDate}', with confidence {invoiceTotalField.Confidence}");
                        DueDateLabel.Text = $"Due Date: {dueDate}";
                    }
                }
            }

            ActivityIndicator.IsRunning = false;

            await uploaderClient.DeleteBlobAsync();
        }
    }
}
