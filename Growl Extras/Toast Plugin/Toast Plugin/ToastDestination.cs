using Growl.Destinations;
using System;
using System.Collections.Generic;
using Growl.Connector;
using DesktopToast;
using System.IO;

namespace Toast_Plugin
{
    [Serializable]
    public class ToastDestination : ForwardDestination
    {
        public ToastDestination(string description, bool enabled)
            : base(description, enabled)
        {
        }

        public override string AddressDisplay
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool Available
        {
            get
            {
                return true;
            }

            protected set
            {
                //throw new NotImplementedException();
            }
        }

        public override DestinationBase Clone()
        {
            return new ToastDestination(this.Description, this.Enabled);
        }

        public override void ForwardNotification(Notification notification, CallbackContext callbackContext, RequestInfo requestInfo, bool isIdle, ForwardedNotificationCallbackHandler callbackFunction)
        {
            var shortcutFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles),
                "Growl for Windows",
                "Growl.exe");

            var request = new ToastRequest
            {
                ToastTitle = notification.Title,
                ToastBody = notification.Text,
                ShortcutFileName = "Growl.lnk",
                ShortcutTargetFilePath = shortcutFilePath,
                AppId = this.Description
            };

            try
            {
                ToastManager.ShowAsync(request)
                    .Wait();

            } catch(Exception ex)
            {
                Growl.CoreLibrary.DebugInfo.WriteLine(String.Format("Toast forwarding failed: {0}", ex.Message));
            }
        }

        public override void ForwardRegistration(Application application, List<NotificationType> notificationTypes, RequestInfo requestInfo, bool isIdle)
        {
            // Do nothing for now.
        }
    }
}
