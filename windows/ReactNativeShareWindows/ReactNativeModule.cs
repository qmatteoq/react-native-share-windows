using System;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Web.Http;

namespace ReactNativeShareWindows
{
    [ReactModule("ReactNativeShareWindows")]
    internal sealed class ReactNativeModule
    {
        private ReactContext _reactContext;

        [ReactInitializer]
        public void Initialize(ReactContext reactContext)
        {
            _reactContext = reactContext;
        }

        [ReactMethod("shareText")]
        public void ShareText(string title, string uri)
        {
            _reactContext.Handle.UIDispatcher.Post(() =>
            {
                DataTransferManager.GetForCurrentView().DataRequested += (obj, args) =>
                {
                    args.Request.Data.Properties.Title = title;
                    args.Request.Data.SetWebLink(new Uri(uri));
                };
            });

            _reactContext.Handle.UIDispatcher.Post(() =>
            {
                DataTransferManager.ShowShareUI();
            });
        }

        [ReactMethod("shareImage")]
        public void ShareImage(int id, string title, string description, string uri)
        {
            _reactContext.Handle.UIDispatcher.Post(() =>
            {
                DataTransferManager.GetForCurrentView().DataRequested += async (obj, args) =>
                {
                    args.Request.Data.Properties.Title = title;
                    args.Request.Data.Properties.Description = description;
                    DataRequestDeferral deferral = args.Request.GetDeferral();

                    HttpClient client = new HttpClient();
                    var stream = await client.GetBufferAsync(new Uri(uri));

                    var localFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"WithersCollection-{id}.jpg", CreationCollisionOption.ReplaceExisting);

                    await FileIO.WriteBufferAsync(localFile, stream);

                    args.Request.Data.SetStorageItems(new[] { localFile });
                    deferral.Complete();
                };
            });


            _reactContext.Handle.UIDispatcher.Post(() =>
            {
                DataTransferManager.ShowShareUI();
            });
        }
    }
}
