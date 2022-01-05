using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class FormRecognizerClient
    {
        private readonly string endpoint = "Endpoint";
        private readonly string apiKey = "ApiKey";
        private readonly AzureKeyCredential credential;
        private readonly DocumentAnalysisClient client;

        public FormRecognizerClient()
        {
            credential = new AzureKeyCredential(apiKey);
            client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }

        public async Task<AnalyzeResult> AnalyzeAsync(string path)
        {
            // Doesn't seem to work with just a path. Needs to be a hosted URL to work...
            // sample invoice document
            Uri invoiceUri = new Uri(path);

            AnalyzeDocumentOperation operation = await client.StartAnalyzeDocumentFromUriAsync("prebuilt-invoice", invoiceUri);

            await operation.WaitForCompletionAsync();

            AnalyzeResult result = operation.Value;

            for (int i = 0; i < result.Documents.Count; i++)
            {
                Console.WriteLine($"Document {i}:");

                AnalyzedDocument document = result.Documents[i];

                if (document.Fields.TryGetValue("VendorName", out DocumentField vendorNameField))
                {
                    if (vendorNameField.ValueType == DocumentFieldType.String)
                    {
                        string vendorName = vendorNameField.AsString();
                        Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}");
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
                    }
                }
            }

            return result;
        }
    }
}
