using Growl.Destinations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toast_Plugin
{
    public class ToastForwardHandler : IForwardDestinationHandler
    {
        public string Name
        {
            get
            {
                return "Toast";
            }
        }

        public List<DestinationListItem> GetListItems()
        {
            ForwardDestinationListItem item = new ForwardDestinationListItem("Forward to Windows Action-Center", GetIcon(), this);
            List<DestinationListItem> list = new List<DestinationListItem>(1);
            list.Add(item);
            return list;
        }

        public DestinationSettingsPanel GetSettingsPanel(DestinationListItem dbli)
        {
            return new ToastInputs();
        }

        public DestinationSettingsPanel GetSettingsPanel(DestinationBase db)
        {
            return new ToastInputs();
        }

        public List<Type> Register()
        {
            List<Type> list = new List<Type>(1);
            list.Add(typeof(ToastDestination));
            return list;
        }

        /// <summary>
        /// Gets the icon associated with this forwarder.
        /// </summary>
        /// <returns><see cref="System.Drawing.Image"/></returns>
        internal static System.Drawing.Image GetIcon()
        {
            return new System.Drawing.Bitmap(Properties.Resources.pc);
        }
    }
}
