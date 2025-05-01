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
    public class OpencvAoiTest
    {
        public OpencvAoiTest()
        {

        }
        public void KeyFine(int index)
        {
            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;
            byte[] buffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], buffer);
            Mat srcImage = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(buffer, 0, srcImage.Data, dataSize);


            // 1. 블러 + 이진화
            Mat blurred = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(7, 7), 0);

            // 예: 중심에서 100x100 크기의 영역 평균을 구함
            int centerX = blurred.Cols / 2;
            int centerY = blurred.Rows / 2;
            int roiSize = 150;

            // ROI 영역 지정 (x, y, width, height)
            Rect roi = new Rect(centerX - roiSize / 2, centerY - roiSize / 2, roiSize, roiSize);
            Mat roiMat = new Mat(blurred, roi);

            // 평균 밝기 계산
            Scalar mean = Cv2.Mean(roiMat);

            // 결과 출력
            Console.WriteLine($"중앙 영역 평균 밝기: {mean.Val0:F2} / {255-mean.Val0:F2}");
            int blockSize = 31;// 21; // 반드시 홀수
            int C = 3;
            Mat binary = new Mat();
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (mean.Val0 / 2.5), 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (255 - mean.Val0), 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv
            Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, blockSize, C);
            ////Cv2.BitwiseNot(binary, binary);  // 밝은 부분만 검정으로 반전

            // 2. 커널 생성 (원형 커널 추천)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3)); 
            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(binary, binary, kernel);
            // 3. 모폴로지 클로징 → 선 연결
            //Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Open, kernel); 

            Cv2.NamedWindow("Detected binary", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected binary", binary);
            Cv2.WaitKey(0);
        }
        public void ContoursCircleFine(int index)
        {

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;
            byte[] buffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], buffer);
            Mat srcImage = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(buffer, 0, srcImage.Data, dataSize);


            // 1. 블러 + 이진화
            Mat blurred = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(3, 3), 0);

            // 예: 중심에서 100x100 크기의 영역 평균을 구함
            int centerX = blurred.Cols / 2;
            int centerY = blurred.Rows / 2;
            int roiSize = 150;

            // ROI 영역 지정 (x, y, width, height)
            Rect roi = new Rect(centerX - roiSize / 2, centerY - roiSize / 2, roiSize, roiSize);
            Mat roiMat = new Mat(blurred, roi);

            // 평균 밝기 계산
            Scalar mean = Cv2.Mean(roiMat);

            // 결과 출력
            Console.WriteLine($"중앙 영역 평균 밝기: {mean.Val0:F2}");

            Mat binary = new Mat();
            int blockSize = 51; // 반드시 홀수
            int C = 3;
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            Cv2.Threshold(blurred, binary, mean.Val0 / 1.1, 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
            //Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, blockSize, C);  // blockSize = 15, C = 4
                                                                                                                                    //MEMO: Inv가 더 잘 잡힘



            // 3. 모폴로지 팁: 얇은 선을 조금 두껍게
            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2, 2));

            

            // 2. 커널 생성 (원형 커널 추천)
            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2, 2)); 
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3)); 
            Cv2.Dilate(binary, binary, kernel);
            // 3. 모폴로지 클로징 → 선 연결
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);
            //Cv2.MorphologyEx(binary, binary, MorphTypes.Open, kernel);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Dilate, kernel); // 약한 선 확장

            Cv2.NamedWindow("Detected binary", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected binary", binary);
            Cv2.WaitKey(0);

            // 2. 윤곽선 찾기
            //RetrievalModes.External
            //RetrievalModes.List
            //RetrievalModes.External
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxSimple);//ApproxSimple

            // 3. 원형성 조건으로 필터링
            Mat result = new Mat();
            Cv2.CvtColor(srcImage, result, ColorConversionCodes.GRAY2BGR);
            int i = 0;
            Console.Write("------------FindContours\n");
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);

                if (perimeter == 0) continue; // 나누기 에러 방지

                double circularity = 4 * Math.PI * area / (perimeter * perimeter);
                // 외접 원 그리기
                Point2f center;
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);

                if (circularity > 0.05 && radius > 200 && area >100 && radius < 1200) // 원형이고 일정 크기 이상
                {
                    

                    Cv2.Circle(result, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 2);

                    i++;
                    Console.Write("[{0:0}] measured circle: x = {1:0.00}, y = {2:0.00}, radius = {3:0.00}\n", i, center.X, center.Y, radius);

                    Rectangle m_clRect = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));

                    if (contour.Length >= 5)
                    {
                        RotatedRect ellipse = Cv2.FitEllipse(contour);
                        double axisRatio = Math.Min(ellipse.Size.Width, ellipse.Size.Height) / Math.Max(ellipse.Size.Width, ellipse.Size.Height);
                        Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);
                        Console.WriteLine($"Axis ratio (타원 비율): {axisRatio:F2}");
                    }
                }
            }
            Cv2.NamedWindow("Detected Circles", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected Circles", result);
            Cv2.WaitKey(0);
        }

        public void houghCircleFine(int index)
        {

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;
            byte[] buffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], buffer);
            Mat srcImage = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(buffer, 0, srcImage.Data, dataSize);


            // 블러 처리 (노이즈 제거에 매우 중요!)
            Mat blurred = new Mat();
            Cv2.MedianBlur(srcImage, blurred, 5);

            // 원 검출
            CircleSegment[] circles = Cv2.HoughCircles(
                blurred,
                HoughModes.Gradient,
                dp: 1.5,           // 누적기 해상도 비율
                minDist: 200,       // 원 간 최소 거리
                param1: 35,       // Canny 상한값
                param2: 100,        // 투표 임계값 (작을수록 더 많이 잡힘)
                minRadius: 280,
                maxRadius: 1100
            );

            // 컬러로 그리기 위해 BGR 이미지 생성
            Mat colorImage = new Mat();
            Cv2.CvtColor(srcImage, colorImage, ColorConversionCodes.GRAY2BGR);

            // 원 그리기
            foreach (var circle in circles)
            {
                OpenCvSharp.Point center = new OpenCvSharp.Point((int)circle.Center.X, (int)circle.Center.Y);
                int radius = (int)circle.Radius;

                Cv2.Circle(colorImage, center, radius, Scalar.Red, 2);
                Cv2.Circle(colorImage, center, 2, Scalar.Blue, 3);  // 중심점
                
            }

            Cv2.ImWrite("d:\\srcImage.jpg", colorImage);

            Cv2.NamedWindow("Result", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성


            Cv2.ImShow("Result", colorImage);
            Cv2.WaitKey(0);
        }
    }
}
