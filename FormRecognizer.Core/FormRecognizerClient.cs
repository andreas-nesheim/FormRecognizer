using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class FormRecognizerClient
    {
        private readonly string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
        private readonly string apiKey = "ceba6a2b2af14e1497ddb4abdaa618cf";
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
