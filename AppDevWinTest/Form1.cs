using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace AppDevWinTest
{
    public partial class Form1 : Form
    {
        private int formHeight;
        private int formWidth;

        public Form1()
        {
            InitializeComponent();

            InitializeVariables();
            InitializeButton();

        }

        private void InitializeVariables()
        {
            formHeight = ActiveForm.Height;
            formWidth = ActiveForm.Width;
        }

        private void DrawPen()
        {
            //Graphics g = CreateGraphics();
            Graphics g = ActiveForm.CreateGraphics();
            Pen p = new Pen(Color.Red, 10);

            // Draw a line:
            g.DrawLine(p, 1, 1, 100, 100);

            // Draw a Pie:
            g.DrawPie(p, 200, 20, 100, 100,-30, 60);

            // Draw a polygon
            Point[] points = new Point[] { new Point(5, 5), new Point(50, 10), new Point(60, 20), new Point(100, 70), new Point(200, 100) };
            g.DrawPolygon(p, points);

            // Set pen style
            p.DashStyle = DashStyle.Dash;
            p.Color = Color.BlueViolet;
            g.DrawLine(p, 100, 100, 200, 200);

            // Set start/end cap
            p.StartCap = LineCap.ArrowAnchor;
            p.EndCap = LineCap.Triangle;
            p.Color = Color.ForestGreen;
            g.DrawLine(p, 200, 200, 300, 300);

        }

        private void DrawBrush()
        {
            Graphics g = ActiveForm.CreateGraphics();
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Point[] points = new Point[] { new Point(50, 50), new Point(100,125), new Point(50, 200), new Point(75, 250), new Point(200, 150) };

            // Solid brush
            g.FillPolygon(solidBrush, points);

            Brush linearBrush = new LinearGradientBrush(new Point(160, 160), new Point(200, 600), Color.Red, Color.Gray);
            Point[] points1 = new Point[] { new Point(160, 160), new Point(240, 350), new Point(370, 500), new Point(270, 550), new Point(200, 600) };

            // linear brush
            g.FillPolygon(linearBrush, points1);
        }

       // With a texture brush, you can fill a shape with a pattern stored in a bitmap
        private void DrawTextureBrush()
        {
            //C:\Users\Public\Pictures\Sample Pictures
            try
            {
                //Bitmap image1 = (Bitmap)Image.FromFile(@"C:\Users\Public\Pictures\Sample Pictures\tulips.jpg", true);
                //TextureBrush texture = new TextureBrush(image1);
                //texture.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
                //Graphics formGraphics = this.CreateGraphics();
                ////formGraphics.FillEllipse(texture, new RectangleF(90.0F, 110.0F, 100, 100));
                //formGraphics.FillEllipse(texture, 20, 20, 200, 400);
                //formGraphics.Dispose();

                // msdn example - http://msdn.microsoft.com/en-us/library/cwka53ef(v=vs.71).aspx
                Graphics formGraphics = this.CreateGraphics();
                Image myImage = Image.FromFile(@"C:\Users\Public\Pictures\Sample Pictures\MyTexture.gif");
                TextureBrush myTextureBrush = new TextureBrush(myImage);
                formGraphics.FillEllipse(myTextureBrush, 0, 0, 100, 50);
                formGraphics.Dispose();



            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error opening the bitmap. Please check the path.");
            }
        }

        //When you fill a shape with a hatch brush, you specify a foreground color, a background color, and a hatch style. The foreground color is the color of the hatching.
        private void DrawHatchBrush()
        {
            HatchBrush brush = new HatchBrush(HatchStyle.Plaid, Color.Red, Color.Blue);
            Graphics formGraphics = this.CreateGraphics();
            //formGraphics.FillEllipse(texture, new RectangleF(90.0F, 110.0F, 100, 100));
            formGraphics.FillEllipse(brush, 20, 20, 200, 400);
            formGraphics.Dispose();
        }

        private void DrawLinearGradientBrush()
        {
            Rectangle r = new Rectangle(10, 10, 450, 25);
            LinearGradientBrush brush = new LinearGradientBrush(r, Color.AliceBlue, Color.CornflowerBlue, LinearGradientMode.ForwardDiagonal);
            Graphics g = this.CreateGraphics();
            g.FillRectangle(brush, r);
        }

        private void InitializeButton()
        {
            // Specify the Size of the control
            int buttonWidth = 100;
            int buttonHeight = 50;
            int buttonX = formWidth/2 - buttonWidth/2;
            int buttonY = formHeight - 25; // 25 off the bottom

            button1.Size = new Size(buttonWidth, buttonHeight);

            // Specify the location of the control
            button1.Location = new Point(buttonX, buttonY);

            //Specfify the colour(s) of the control
            button1.ForeColor = Color.Cyan;
            button1.BackColor = Color.Black;
            //button1.ForeColor = Color.FromArgb(10, 200, 200);
            //button1.BackColor = Color.FromArgb(200, 5, 5);

            button1.Text = "Close";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawPen();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //DrawBrush();
            //DrawTextureBrush();
            //DrawHatchBrush();
            DrawLinearGradientBrush();

        }

        private void btnIcon_Click(object sender, EventArgs e)
        {
            DrawIcon();
        }

        private void DrawIcon()
        {
            Graphics g = ActiveForm.CreateGraphics();
            g.DrawIcon(SystemIcons.Information, 40, 40);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPEG Files (*.jpg)|*.jpg;*.jpeg|All Files (*.*)|*.*";
            Bitmap b = CreateBitMapImage();

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                // save out our image...
                b.Save(dialog.FileName, ImageFormat.Jpeg);
            }
        }

        // Screesshot current form - same as alt-prntscreen
        private Bitmap CreateBitMapImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(this.Bounds.Width, this.Bounds.Height, PixelFormat.Format32bppArgb);

            Graphics screenShot = Graphics.FromImage(bitmap);
            //Graphics screenShot = ActiveForm.CreateGraphics();
            // Take the screenshot from the upper left corner to the right bottom corner
              screenShot.CopyFromScreen(this.Bounds.X, this.Bounds.Y, 0, 0, this.Bounds.Size, CopyPixelOperation.SourceCopy);

              return bitmap;


            //// Create a graphics object for drawing.
            //Graphics g = Graphics.FromImage(bitmap);
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //Rectangle rect = new Rectangle(0, 0, this.width, this.height);

            //// Fill in the background.
            //HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            //g.FillRectangle(hatchBrush, rect);
        }

        private void btnText_Click(object sender, EventArgs e)
        {
            Graphics g = CreateGraphics();
            System.Drawing.Font font = new Font("Arial", 20, FontStyle.Bold);
            g.DrawString("Hello, World", font, Brushes.Azure, 300, 300);

        }
    }
}
