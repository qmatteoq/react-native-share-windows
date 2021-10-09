using System;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.DataTransfer;

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

        [ReactMethod("share")]
        public void Share(string title, string uri)
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
    }
}
