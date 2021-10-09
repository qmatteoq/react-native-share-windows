using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.DataTransfer;

namespace ReactNativeShareWindows
{
    [ReactModule("ReactNativeShareWindows")]
    internal sealed class ReactNativeModule
    {
        // See https://microsoft.github.io/react-native-windows/docs/native-modules for details on writing native modules

        private ReactContext _reactContext;

        [ReactInitializer]
        public void Initialize(ReactContext reactContext)
        {
            _reactContext = reactContext;
        }

        [ReactMethod("share")]
        public async Task Share(string title, string uri)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            DataTransferManager.GetForCurrentView().DataRequested += (obj, args) =>
            {
                args.Request.Data.Properties.Title = title;
                args.Request.Data.SetWebLink(new Uri(uri));

                tcs.SetResult(true);
            };

            await tcs.Task;

            DataTransferManager.ShowShareUI();
        }
    }
}
