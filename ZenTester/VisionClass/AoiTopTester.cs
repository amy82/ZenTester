using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;


namespace ZenHandler.VisionClass
{
    public class AoiTopTester
    {
        public AoiTopTester()
        {

        }
        public OpenCvSharp.Point CenterFineTopCamera(int index)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            OpenCvSharp.Point centerPos = new OpenCvSharp.Point();
            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;
            byte[] buffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], buffer);
            Mat gray = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(buffer, 0, gray.Data, dataSize);

            //
            //
            //
            //
            //
            int view = 3;
            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            var edges = new Mat();
            //Cv2.Threshold(gray, binary, 140, 255, ThresholdTypes.Binary);
            Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 1.2);
            Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화
            if (view > 0)
            {
                Cv2.NamedWindow("Detected edges ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected edges ", edges);
                Cv2.WaitKey(0);
            }

            //Cv2.EqualizeHist(gray, gray);
            int blockSize = 13;// 21; // 반드시 홀수
            int C = 13;
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (mean.Val0 / 2.5), 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (255 - mean.Val0), 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(gray, binary, mean.Val0 / 1.1, 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv



            Cv2.AdaptiveThreshold(edges, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            if (view > 1)
            {
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 7));
                Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
                Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary ", binary);
                Cv2.WaitKey(0);
            }



            // 3. Contours 찾기
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 4. 이미지 중심 계산
            ///OpenCvSharp.Point center = new OpenCvSharp.Point(gray.Width / 2, gray.Height / 2);
            double minDist = double.MaxValue;
            OpenCvSharp.Point bestPoint = new OpenCvSharp.Point();
            OpenCvSharp.Point2d imageCenter = new OpenCvSharp.Point2d(gray.Width / 2, gray.Height / 2);

            // 3. 각 컨투어 중심을 평균
            List<Point2d> centers = new List<Point2d>();

            Mat colorView = new Mat();
            Cv2.CvtColor(gray, colorView, ColorConversionCodes.GRAY2BGR);


            foreach (var contour in contours)
            {
                Moments M = Cv2.Moments(contour);
                if (M.M00 == 0) continue;
                //centers.Add(new Point2d(M.M10 / M.M00, M.M01 / M.M00));

                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);

                //if (perimeter == 0) continue; // 나누기 에러 방지



                double circularity = 4 * Math.PI * area / (perimeter * perimeter);
                // 외접 원 그리기
                Point2f center;
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);

                if (circularity > 0.7 && radius > 300 && radius < 1500) // 원형이고 일정 크기 이상 && area > 1000
                {
                    //Cv2.Circle(gray, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 3);
                    // 컬러 이미지에 원을 그림
                    Cv2.Circle(colorView, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 5);
                    if (radius > 800)
                    {
                        //foreach (var point in contour)
                        for (int i = 0; i < contour.Length; i += 15)
                        {
                            // 중심으로부터 거리 계산
                            float dx = contour[i].X - center.X;
                            float dy = contour[i].Y - center.Y;
                            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                            Console.Write("[CenterFind] dist - radius: x = {0:0.00}\n", (dist - radius));
                            // 기준 반지름과 비교
                            if (Math.Abs(dist - radius) <= 10.0)
                            {
                                Cv2.Circle(colorView, new OpenCvSharp.Point(contour[i].X, contour[i].Y), 11, Scalar.Yellow, 3);
                                Rectangle m_clRect = new Rectangle((int)(contour[i].X - (10)), (int)(contour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                            }
                            else
                            {
                                Cv2.Circle(colorView, new OpenCvSharp.Point(contour[i].X, contour[i].Y), 11, Scalar.Red, 3);
                                Rectangle m_clRect = new Rectangle((int)(contour[i].X - (10)), (int)(contour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Red, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                            }

                        }
                    }
                    centers.Add(new Point2d(center.X, center.Y));
                    Console.Write("[CenterFind] measured circle: x = {0:0.00}, y = {1:0.00}, circularity = {2:0.00}, radius = {3:0.00}, area = {4:0.00}\n", center.X, center.Y, circularity, radius, area);
                }
            }
            if (view > 2)
            {
                Cv2.NamedWindow("Detected colorView ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorView ", colorView);
                Cv2.WaitKey(0);
            }

            // 평균 좌표 계산
            if (centers.Count > 0)
            {
                double sumX = centers.Sum(c => c.X);
                double sumY = centers.Sum(c => c.Y);
                Point2d avgCenter = new Point2d(sumX / centers.Count, sumY / centers.Count);







                Rectangle m_clRect = new Rectangle((int)(avgCenter.X - (50)), (int)(avgCenter.Y - (50)), (int)(50 * 2), (int)(50 * 2));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);
                //Cv2.Circle(gray, bestPoint, 5, new Scalar(0, 255, 0), 2);
                //Cv2.NamedWindow("Detected gray ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                //Cv2.ImShow("Detected gray ", gray);
                //Cv2.WaitKey(0);
            }
            //Rect keyRoi = new Rect((int)2190 + 250, (int)1429 + 250, 650, 650);
            //Mat roiKeyMat = gray[keyRoi];
            //Cv2.NamedWindow("Detected roiKeyMat ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("Detected roiKeyMat ", roiKeyMat);
            //Cv2.WaitKey(0);
            //bool keyRtn = MilKeyCheck(roiKeyMat);

            int centerX = gray.Cols / 2;
            int centerY = gray.Rows / 2;
            int roiSize = 150;
            // ROI 영역 지정 (x, y, width, height)
            Rect roi = new Rect(centerX - roiSize / 2, centerY - roiSize / 2, roiSize, roiSize);
            Mat roiMat = new Mat(gray, roi);

            // 평균 밝기 계산
            Scalar mean = Cv2.Mean(roiMat);

            // 결과 출력
            Console.WriteLine($"[CenterFind] 중앙 영역 평균 밝기: {mean.Val0:F2}");


            return centerPos;
        }
    }
}
