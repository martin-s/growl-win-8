using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Growl.DisplayStyle;

namespace BlueFlower
{
    public partial class BlueFlowerWindow : NotificationWindow
    {
        Timer fadeOutTimer;
        int opacity = 254;
        Bitmap bitmap;
        BlueFlowerDisplay.Location location = BlueFlowerDisplay.Location.TopRight;
        private int leftXLocation = 0;
        private int rightXLocation = 0;
        private int topYLocation = 0;
        private int bottomYLocation = 0;

        public BlueFlowerWindow()
        {
            InitializeComponent();

            this.AfterLoad += new EventHandler(BlueFlowerWindow_AfterLoad);
            this.AutoClosing += new FormClosingEventHandler(BlueFlowerWindow_AutoClosing);

            HookUpClickEvents(this);

            SetAutoCloseInterval(4000);
        }

        void BlueFlowerWindow_AfterLoad(object sender, EventArgs e)
        {
            DoBeforeShow();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        public BlueFlowerDisplay.Location DisplayLocation
        {
            get
            {
                return this.location;
            }
        }

        public void SetDisplayLocation(BlueFlowerDisplay.Location location)
        {
            this.location = location;
        }

        public override void SetNotification(Notification n)
        {
            base.SetNotification(n);

            if (n.Duration > 0) SetAutoCloseInterval(n.Duration * 1000);

            this.pictureBox1.Image = n.Image;

            this.titleLabel.Text = n.Title;
            this.textLabel.Text = n.Description;
            this.Sticky = n.Sticky;

            Bitmap template = global::BlueFlower.Properties.Resources.blueflower;
            using (template)
            {
                this.bitmap = SizeImage(template, this.Width, this.Height);
            }
        }

        private void DoBeforeShow()
        {
            // multiple monitor support
            MultiMonitorVisualDisplay d = (MultiMonitorVisualDisplay)this.Tag;
            Screen screen = d.GetPreferredDisplay();

            // set initial location
            this.leftXLocation = screen.WorkingArea.Left;
            this.rightXLocation = screen.WorkingArea.Right - this.Width;
            this.topYLocation = screen.WorkingArea.Top;
            this.bottomYLocation = screen.WorkingArea.Bottom - this.Height;

            switch (location)
            {
                case BlueFlowerDisplay.Location.TopLeft:
                    this.Location = new Point(this.leftXLocation, this.topYLocation);
                    break;
                case BlueFlowerDisplay.Location.BottomLeft:
                    this.Location = new Point(this.leftXLocation, this.bottomYLocation);
                    break;
                case BlueFlowerDisplay.Location.BottomRight:
                    this.Location = new Point(this.rightXLocation, this.bottomYLocation);
                    break;
                default: // TopRight
                    this.Location = new Point(this.rightXLocation, this.topYLocation);
                    break;
            }
        }

        private System.Drawing.Bitmap SizeImage(System.Drawing.Bitmap originalImage, int newWidth, int newHeight)
        {
            int minHeight = 154;
            int topHeight = 152;
            int bottomHeight = 1;

            if (newHeight < minHeight) newHeight = minHeight;

            Size originalTopSize = new Size(originalImage.Width, topHeight);
            Size newTopSize = new Size(newWidth, topHeight);
            Size originalBottomSize = new Size(originalImage.Width, bottomHeight);
            Size newBottomSize = new Size(newWidth, bottomHeight);
            int originalBottomY = originalImage.Height - bottomHeight;
            int newBottomY = newHeight - bottomHeight;
            Point originalBottomLocation = new Point(0, originalBottomY);
            Point newBottomLocation = new Point(0, newBottomY);
            Point originalMiddleLocation = new Point(0, topHeight);
            Point newMiddleLocation = new Point(0, topHeight);
            Size originalMiddleSize = new Size(originalImage.Width, originalImage.Height - originalTopSize.Height - originalBottomSize.Height);
            Size newMiddleSize = new Size(newWidth, newHeight - newTopSize.Height - newBottomSize.Height);

            System.Drawing.Bitmap bmpResized = new System.Drawing.Bitmap(newWidth, newHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpResized);
            using (g)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // draw top
                g.DrawImage(
                    originalImage,
                    new System.Drawing.Rectangle(System.Drawing.Point.Empty, newTopSize),
                    new System.Drawing.Rectangle(System.Drawing.Point.Empty, originalTopSize),
                    System.Drawing.GraphicsUnit.Pixel);

                // draw middle
                g.DrawImage(
                    originalImage,
                    new System.Drawing.Rectangle(newMiddleLocation, newMiddleSize),
                    new System.Drawing.Rectangle(originalMiddleLocation, originalMiddleSize),
                    System.Drawing.GraphicsUnit.Pixel);

                // draw bottom
                g.DrawImage(
                    originalImage,
                    new System.Drawing.Rectangle(newBottomLocation, newBottomSize),
                    new System.Drawing.Rectangle(originalBottomLocation, originalBottomSize),
                    System.Drawing.GraphicsUnit.Pixel);
            }
            return bmpResized;
        }

        protected override void OnShown(EventArgs e)
        {
            Utility.UpdateLayeredWindow(this.bitmap, this, this.Left, this.Top, (byte)opacity);
            base.OnShown(e);
        }

        void BlueFlowerWindow_AutoClosing(object sender, FormClosingEventArgs e)
        {
            this.fadeOutTimer = new Timer();
            this.fadeOutTimer.Interval = 50;
            this.fadeOutTimer.Tick += new EventHandler(fadeOutTimer_Tick);
            this.fadeOutTimer.Start();
            e.Cancel = true;    // IMPORTANT!
        }

        void fadeOutTimer_Tick(object sender, EventArgs e)
        {
            this.opacity -= 10;
            if (this.opacity <= 0)
            {
                this.fadeOutTimer.Stop();
                this.Close();
            }
            else if(this.Visible)
            {
                Utility.UpdateLayeredWindow(this.bitmap, this, this.Left, this.Top, (byte)opacity);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style
                return cp;
            }
        }

        private void titleLabel_LabelHeightChanged(ExpandingLabel.LabelHeightChangedEventArgs args)
        {
            if (args.HeightChange != 0)
            {
                this.textLabel.Top += args.HeightChange;
                textLabel_LabelHeightChanged(args);
            }
        }

        private void textLabel_LabelHeightChanged(ExpandingLabel.LabelHeightChangedEventArgs args)
        {
            if (args.HeightChange != 0)
            {
                this.Height += args.HeightChange;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.fadeOutTimer != null) this.fadeOutTimer.Dispose();
                if (this.bitmap != null) this.bitmap.Dispose();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}