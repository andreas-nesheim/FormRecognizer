using FormRecognizer.Core;
using System;
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
            var name = photoResult.FileName;

            var uploaderClient = new BlobUploader();
            var blobUrl = await uploaderClient.UploadSync(name, fullPath);

            var client = new FormRecognizerClient();
            var result = await client.AnalyzeAsync(blobUrl);
        }
    }
}
