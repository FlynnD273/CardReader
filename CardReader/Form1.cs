﻿using System;
using System.Collections.Generic;
using System.Data;
using Drawing = System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Detail;

namespace CardReader
{
    public partial class Form1 : Form
    {
        private Mat img;
        private VideoCapture capture;

        private double imgScale = 1;
        private double cardRatio = 63.0 / 88;

        private Scalar lowerB = new Scalar();
        private Scalar upperB = new Scalar();
        private Scalar hsvRadius = new Scalar(20, 40, 80);
        private Scalar avg = new Scalar();

        private Drawing.Size imgSize;
        private Drawing.Point mouseClick;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            img = new Mat();

            VideoDeviceDialog vd = new VideoDeviceDialog();
            vd.ShowDialog();

            capture = new VideoCapture();
            capture.Open(vd.DeviceIndex);
            Application.Idle += OnCameraFrame;

            WindowState = FormWindowState.Maximized;
        }

        private void OnCameraFrame(object sender, EventArgs e)
        {
            img = capture.RetrieveMat();
            ////Cv2.Flip(img, img, FlipMode.Y);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (img.Cols == 0)
                return;

            Mat topDown = new Mat();
            img.CopyTo(topDown);
            Mat binImage = new Mat();
            Cv2.InRange(img, lowerB, upperB, binImage);//Cv2.Canny(preprocessed, binImage, edges, 0, 7);

            OpenCvSharp.Point[][] contours;
            Cv2.FindContours(binImage, out contours, out _, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);

            Rect bounds = new Rect(new OpenCvSharp.Point(0, 0), new OpenCvSharp.Size(0, 0));
            for (int i = 0; i < contours.Length; i++)
            {
                Rect r = Cv2.BoundingRect(contours[i]);
                if (r.Width * r.Height > bounds.Width * bounds.Height)
                {
                    bounds = r;
                }
            }

            if (contours.Length > 0)
            {
                Mat mask = new Mat(new OpenCvSharp.Size(img.Width, img.Height), MatType.CV_8UC1);
                mask.SetTo(new Scalar(0, 0, 0));
                Cv2.Rectangle(mask, bounds, new Scalar(255), -1);
                Mat temp = new Mat(new OpenCvSharp.Size(img.Width, img.Height), MatType.CV_8UC1);
                temp.SetTo(new Scalar(255, 255, 255));
                overlayImage(temp, binImage.SubMat(bounds), binImage, bounds.TopLeft);
                //binImage.CopyTo(temp, mask);
                //temp.CopyTo(binImage);
            }

            Cv2.BitwiseNot(binImage, binImage);

            Bitmap screen = new Bitmap(img.Width, img.Height);
            Graphics g = Graphics.FromImage(screen);
            g.DrawImage(img.ToBitmap(), 0, 0);

            g.DrawRectangle(Pens.Red, bounds.Left, bounds.Top, bounds.Width, bounds.Height);

            #region Code the image and drawing to Graphics g goes here

            Cv2.FindContours(binImage, out contours, out _, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);

            Drawing.Point[] poly = null;
            double maxArea = -1;

            for (int i = 0; i < contours.Length; i++)
            {
                OpenCvSharp.Point[] contour = contours[i];
                OpenCvSharp.Point[] approx = Cv2.ApproxPolyDP(contour, Cv2.ArcLength(contour, true) * 0.02, true);
                Drawing.Point[] points = ToDrawingPointArray(approx);

                if (points.Length == 4)
                {
                    double area = Cv2.ContourArea(contour);
                    if (area > maxArea)
                    {
                        maxArea = area;
                        poly = points;
                    }
                }
            }

            if (poly != null)
            {
                g.DrawPolygon(new Pen(Brushes.White, 5), poly);

                topDown = TransformTopDown(topDown, poly);
            }

            #endregion

            Drawing.Size winSize = e.Graphics.VisibleClipBounds.Size.ToSize();
            Drawing.Size s = screen.Size;
            double ratio = (double)s.Width / s.Height;
            if ((double)winSize.Width / 2 / s.Width < (double)winSize.Height / 2 / s.Height)
            {
                imgSize = new Drawing.Size(winSize.Width / 2, (int)(winSize.Width / 2 / ratio));
            }
            else
            {
                imgSize = new Drawing.Size((int)(winSize.Height / 2 * ratio), winSize.Height / 2);
            }

            imgScale = (double)imgSize.Width / img.Width;

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, (int)avg[2], (int)avg[1], (int)avg[0])), 0, 0, 20, 20);

            e.Graphics.DrawImage(screen, new Rectangle(new Drawing.Point(0, 0), imgSize));
            e.Graphics.DrawImage(binImage.ToBitmap(), new Rectangle(new Drawing.Point(imgSize.Width, 0), imgSize));
            e.Graphics.DrawImage(topDown.ToBitmap(), new Drawing.Point(0, imgSize.Height));
            //e.Graphics.DrawImage(mask.ToBitmap(), new Rectangle(imgSize.Width, imgSize.Height, imgSize.Width, imgSize.Height));
        }
        
        public static void overlayImage(Mat background, Mat foreground, Mat output, OpenCvSharp.Point location)
        {

            background.CopyTo(output);

            for (int y = (int)Math.Max(location.Y, 0); y < background.Rows; ++y)
            {

                int fY = (int)(y - location.Y);

                if (fY >= foreground.Rows)
                    break;

                for (int x = (int)Math.Max(location.X, 0); x < background.Cols; ++x)
                {
                    int fX = x - location.X;
                    if (fX >= foreground.Cols)
                    {
                        break;
                    }

                    double opacity;
                    double[] finalPixelValue = new double[4];

                    opacity = foreground.Get<Scalar>(fY, fX)[3];

                    finalPixelValue[0] = background.Get<Scalar>(y, x)[0];
                    finalPixelValue[1] = background.Get<Scalar>(y, x)[1];
                    finalPixelValue[2] = background.Get<Scalar>(y, x)[2];
                    finalPixelValue[3] = background.Get<Scalar>(y, x)[3];

                    for (int c = 0; c < output.Channels(); ++c)
                    {
                        if (opacity > 0)
                        {
                            double foregroundPx = foreground.Get<Scalar>(fY, fX)[c];
                            double backgroundPx = background.Get<Scalar>(y, x)[c];

                            float fOpacity = (float)(opacity / 255);
                            finalPixelValue[c] = ((backgroundPx * (1.0 - fOpacity)) + (foregroundPx * fOpacity));
                            if (c == 3)
                            {
                                finalPixelValue[c] = foreground.Get<Scalar>(fY, fX)[3];
                            }
                        }
                    }
                    output.At<Scalar>(y, x) = new Scalar(finalPixelValue[0], finalPixelValue[1], finalPixelValue[2], finalPixelValue[3]);
                }
            }
        }

        private Mat TransformTopDown(Mat baseImg, Drawing.Point[] poly)
        {
            Drawing.Point center = new Drawing.Point(0, 0);
            int x = 0;
            int y = 0;
            foreach (Drawing.Point p in poly)
            {
                x += p.X;
                y += p.Y;
            }
            x /= poly.Length;
            y /= poly.Length;

            poly.ToList().Sort((a, b) => Less(a, b, center) ? 0 : 1);

            Drawing.Point[] temp = new Drawing.Point[4];
            temp[0] = poly[3];
            temp[1] = poly[0];
            temp[2] = poly[1];
            temp[3] = poly[2];

            temp.CopyTo(poly, 0);

            double widthA  = Dist(poly[0], poly[1]);
            double widthB  = Dist(poly[2], poly[3]);
            double heightA = Dist(poly[0], poly[3]);
            double heightB = Dist(poly[1], poly[2]);

            int width = (int)Math.Max(widthA, widthB);
            int height = (int)Math.Max(heightA, heightB);

            if (width > height)
            {
                temp = new Drawing.Point[4];
                temp[0] = poly[1];
                temp[1] = poly[2];
                temp[2] = poly[3];
                temp[3] = poly[0];

                temp.CopyTo(poly, 0);

                int tempDim = width;
                width = height;
                height = tempDim;
            }

            Point2f[] dst = new Point2f[4];
            dst[0] = new Point2f(0        , 0         );
            dst[1] = new Point2f(width - 1, 0         );
            dst[2] = new Point2f(width - 1, height - 1);
            dst[3] = new Point2f(0        , height - 1);

            Mat transform = Cv2.GetPerspectiveTransform(ToCvPointArray(poly), dst);

            Mat r = new Mat();
            baseImg.CopyTo(r);
            Cv2.WarpPerspective(r, r, transform, img.Size());

            try
            {
                r = r.SubMat(0, Math.Min(height - 1, r.Cols - 2), 0, Math.Min(width - 1, r.Rows - 2));
            }
            catch (Exception exc)
            {
                r = baseImg;
            }

            Cv2.Resize(r, r, new OpenCvSharp.Size(img.Height * cardRatio * imgScale, img.Height * imgScale));
            Cv2.Flip(r, r, FlipMode.Y);

            return r;
        }

        private Point2f[] ToCvPointArray (Drawing.Point[] points)
        {
            Point2f[] cv = new Point2f[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                Drawing.Point p = points[i];
                cv[i] = new OpenCvSharp.Point(p.X, p.Y);
            }

            return cv;
        }

        private Drawing.Point[] ToDrawingPointArray(OpenCvSharp.Point[] points)
        {
            Drawing.Point[] cv = new Drawing.Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                OpenCvSharp.Point p = points[i];
                cv[i] = new Drawing.Point(p.X, p.Y);
            }

            return cv;
        }

        private bool Less(Drawing.Point a, Drawing.Point b, Drawing.Point center)
        {
            if (a.Y > 0)
            { //a between 0 and 180
                if (b.Y < 0)  //b between 180 and 360
                    return false;
                return a.X < b.X;
            }
            else
            { // a between 180 and 360
                if (b.Y > 0) //b between 0 and 180
                    return true;
                return a.X > b.X;
            }
        }




        private double Angle(Drawing.Point start, Drawing.Point end)
        {
            return ((Math.PI / 2) - Math.Atan2(start.Y - end.Y, end.X - start.X)) * 180 / Math.PI;
        }

        private Bitmap RotateImage(Bitmap b, float angle)
        {
            double scale = Dist(new Drawing.Point(0, 0), new Drawing.Point(b.Width, b.Height));
            Bitmap returnBitmap = new Bitmap((int)(scale), (int)(scale));
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)returnBitmap.Width / 2, -(float)returnBitmap.Height / 2);
                g.DrawImage(b, new Drawing.Point((returnBitmap.Width - b.Width) / 2, (returnBitmap.Height - b.Height) / 2));
            }
            return returnBitmap;
        }

        private double Dist(Drawing.Point a, Drawing.Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClick = new Drawing.Point((int)(e.Location.X / imgScale), (int)(e.Location.Y / imgScale));
            int r = img.Width / 128;

            if (mouseClick.X >= img.Width || mouseClick.Y >= img.Height || mouseClick.X < 0 || mouseClick.Y < 0)
            {
                return;
            }
            Mat mouseArea;
            try
            {
                mouseArea = img.SubMat(Math.Max(mouseClick.Y - r, 0), Math.Min(mouseClick.Y + r, img.Width - 1), Math.Max(mouseClick.X - r, 0), Math.Min(mouseClick.X + r, img.Height - 1));
            }
            catch (Exception exception)
            {
                return;
            }

            //mouseArea = mouseArea.CvtColor(ColorConversionCodes.BGR2HSV);

            avg = Cv2.Sum(mouseArea);
            int area = mouseArea.Rows * mouseArea.Cols;
            avg.Val0 /= area;
            avg.Val1 /= area;
            avg.Val2 /= area;
            avg.Val3 = 0;

            avg = CvtColor(avg, ColorConversionCodes.BGR2HSV);

            lowerB = new Scalar(avg.Val0 - hsvRadius.Val0, avg.Val1 - hsvRadius.Val1, avg.Val2 - hsvRadius.Val2);
            upperB = new Scalar(avg.Val0 + hsvRadius.Val0, avg.Val1 + hsvRadius.Val1, avg.Val2 + hsvRadius.Val2);

            avg = CvtColor(avg, ColorConversionCodes.HSV2BGR);

            lowerB = CvtColor(lowerB, ColorConversionCodes.HSV2BGR);
            upperB = CvtColor(upperB, ColorConversionCodes.HSV2BGR);
            lowerB.Val3 = 0;
            upperB.Val3 = 255;

        }

        private Scalar CvtColor (Scalar input, ColorConversionCodes cvt)
        {
            Mat outputMat = new Mat();
            Mat inputMat = new Mat(1, 1, MatType.CV_8UC3, input);

            Cv2.CvtColor(inputMat, outputMat, cvt);

            byte[] c = new byte[3];
            Marshal.Copy(outputMat.Data, c, 0, c.Length);
            //bgr.GetArray(out c);
            return new Scalar(c[0], c[1], c[2]);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form1_MouseDown(sender, e);
            }
        }
    }
}
