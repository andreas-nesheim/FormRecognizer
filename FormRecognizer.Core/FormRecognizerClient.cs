using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class FormRecognizerClient
    {
        private readonly string endpoint = "formRecognizerEndpoint";
        private readonly string apiKey = "formRecognizerApiKey";
        private readonly AzureKeyCredential credential;
        private readonly DocumentAnalysisClient client;

        public FormRecognizerClient()
        {
            credential = new AzureKeyCredential(apiKey);
            client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }

        public async Task<AnalyzeResult> AnalyzeAsync(string path)
        {
            Uri invoiceUri = new Uri(path);

            AnalyzeDocumentOperation operation = await client.StartAnalyzeDocumentFromUriAsync("prebuilt-invoice", invoiceUri);

            await operation.WaitForCompletionAsync();

            AnalyzeResult result = operation.Value;

            return result;
        }
    }
}
