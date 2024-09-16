using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PythonCSharpProj
{
    class PictureBoxEX : PictureBox
    {
        private int LURow = 0;
        private int LUCol = 0;
        public int RealWidth = 0;
        public int RealHeight = 0;
        public Mat srcImage = new Mat();
        public Mat showImage = new Mat();

        public int srcWidth, srcHeight = 0;
        public OpenCvSharp.Point CurrentPoint = new OpenCvSharp.Point(0, 0);
        public OpenCvSharp.Point EndPoint = new OpenCvSharp.Point(0, 0);
        System.Drawing.Point p = new System.Drawing.Point(0, 0);
        public int w_empty = 0;
        public int h_empty = 0;


        public void pictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (Image == null) return;

            if (e.Button == MouseButtons.Left)
            {
                EndPoint = GetImagePoint(e.Location);

                int offsetX = EndPoint.X - CurrentPoint.X;
                int offsetY = EndPoint.Y - CurrentPoint.Y;
                LURow -= offsetY;
                LUCol -= offsetX;
                judgeBounds();
                showImage = new Mat(srcImage, new Rect(LUCol, LURow, RealWidth, RealHeight));

            }
            else
            {
                CurrentPoint = GetImagePoint(e.Location);
            }
        }

        public void pictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (Image == null) return;
            CurrentPoint = GetImagePoint(e.Location);

        }


        public void pictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Image == null) return;
            if (e.Delta > 0)
            {
                RealWidth /= 2;
                RealHeight /= 2;
                LUCol = CurrentPoint.X - (int)RealWidth / 2;
                LURow = CurrentPoint.Y - (int)RealHeight / 2;
            }
            else
            {
                RealWidth *= 2;
                RealHeight *= 2;
                LUCol = CurrentPoint.X - (int)RealWidth / 2;
                LURow = CurrentPoint.Y - (int)RealHeight / 2;

            }

            judgeBounds();
            showImage = new Mat(srcImage, new Rect(LUCol, LURow, RealWidth, RealHeight));

        }

        private void judgeBounds()
        {
            RealWidth = RealWidth <= 1 ? 1 : (RealWidth > srcWidth ? srcWidth : RealWidth);
            RealHeight = RealHeight <= 1 ? 1 : (RealHeight > srcHeight ? srcHeight : RealHeight);
            LUCol = LUCol <= 0 ? 0 : (LUCol > srcImage.Width - RealWidth ? srcImage.Width - RealWidth : LUCol);
            LURow = LURow <= 0 ? 0 : (LURow > srcImage.Height - RealHeight ? srcImage.Height - RealHeight : LURow);
        }

        private OpenCvSharp.Point GetImagePoint(System.Drawing.Point p)
        {
            OpenCvSharp.Point imagePoint;
            int width = showImage.Width;
            int height = showImage.Height;
            int w = Width; int h = Height;
            double ratio;
            if (w_empty > 0)
            {
                ratio = h * 1.0 / height;

                if (p.X < w_empty || p.X > w - w_empty)
                    imagePoint.X = -1;
                else
                    imagePoint.X = LUCol + (int)((p.X - w_empty) * 1.0 / ratio);
                imagePoint.Y = LURow + (int)(p.Y * 1.0 / ratio);
            }
            else
            {
                ratio = w * 1.0 / width;

                imagePoint.X = LUCol + (int)(p.X * 1.0 / ratio);
                if (p.Y < h_empty || p.Y > h - h_empty)
                    imagePoint.Y = -1;
                else
                    imagePoint.Y = LURow + (int)((p.Y - h_empty) * 1.0 / ratio);
            }
            return imagePoint;
        }

        public void pictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (Image == null)
                return;
            var state = e.Graphics.Save();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.Clear(BackColor);
            if (h_empty != 0)
                e.Graphics.DrawImage(Image, 0, h_empty, Width, Height - 2 * h_empty);
            else
                e.Graphics.DrawImage(Image, w_empty, 0, Width - 2 * w_empty, Height);
            e.Graphics.Restore(state);
        }
    }
}

