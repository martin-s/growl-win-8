using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Growl.Destinations;

namespace Toast_Plugin
{
    public partial class ToastInputs : DestinationSettingsPanel
    {
        public ToastInputs()
        {
            InitializeComponent();
        }

        public override void Initialize(bool isSubscription, DestinationListItem dli, DestinationBase db)
        {
            // set text box values
            this.highlightTextBoxAppId.Text = String.Empty;
            this.highlightTextBoxAppId.Enabled = true;

            ToastDestination td = db as ToastDestination;
            if (td != null)
            {
                this.highlightTextBoxAppId.Text = td.Description;

            }

            ValidateInputs();

            this.highlightTextBoxAppId.Focus();
        }

        public override DestinationBase Create()
        {
            return new ToastDestination(this.highlightTextBoxAppId.Text, true);
        }

        public override void Update(DestinationBase db)
        {
            ToastDestination td = db as ToastDestination;
            td.Description = this.highlightTextBoxAppId.Text;
        }

        private void ValidateInputs()
        {
            bool valid = true;

            // name
            if (String.IsNullOrEmpty(this.highlightTextBoxAppId.Text))
            {
                this.highlightTextBoxAppId.Highlight();
                valid = false;
            }
            else
            {
                this.highlightTextBoxAppId.Unhighlight();
            }

            OnValidChanged(valid);
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            ValidateInputs();
        }
    }
}
