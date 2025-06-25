using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;


namespace ZenTester.VisionClass
{
    public class AoiTopTester
    {
        private byte[] ImageBuffer; // = new byte[dataSize];
        private Stopwatch TeststopWatch = new Stopwatch();
        public AoiTopTester()
        {

        }
        public bool Run(int index)
        {
            bool rtn = true;

            Globalo.visionManager.milLibrary.ClearOverlay_Manual(index);
            
            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[index];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[index];
            int dataSize = sizeX * sizeY;


            ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);



            //OpenCvSharp.Point centerPos = Housing_Dent_Test(index , src); //Con1,2(동심도)  / Dent (찌그러짐) 검사 


            //GasketTest(index, src, centerPos);     //가스켓 검사
            //
            //Keytest(index, src, centerPos, 0);        //키검사


            return rtn;
        }
        public bool FindCircleCenter(int index, Mat srcImage, ref OpenCvSharp.Point centerPos, bool autoRun = false)
        {
            bool IMG_VIEW = true;
            if (autoRun)
            {
                IMG_VIEW = false;
            }
            int startTime = Environment.TickCount;
            bool bRtn = false;
            //
            //int radiusOuter = 800;      //이미지 중심에서 pogoPin을 찾을 원 크기

            OpenCvSharp.Point ImageCenter = new OpenCvSharp.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / 2, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] / 2);


            centerPos.X = ImageCenter.X;
            centerPos.Y = ImageCenter.Y;
            // 이미지 크기에 맞는 빈 마스크
            //Mat mask = Mat.Zeros(srcImage.Size(), MatType.CV_8UC1);

            // 바깥쪽 원 그리기 (하얀색)
            //Cv2.Circle(mask, ImageCenter, radiusOuter, Scalar.White, -1);

            //Mat masked = new Mat();
            // 원과 원 사이의 영역만 남긴 결과
            //Cv2.BitwiseAnd(srcImage, srcImage, masked, mask);       //원본에서 마스크 영역만 남기기


            Mat gray = new Mat();
            //if (srcImage.Channels() == 3)
            //    Cv2.CvtColor(srcImage, gray, ColorConversionCodes.BGR2GRAY);
            //else
            //    gray = srcImage.Clone();  // 이미 흑백이면 그대로 복사
            //Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(7, 7), 0.3);


            var blurred = new Mat();
            //var edges = new Mat();
            //Cv2.GaussianBlur(srcImage, gray, new OpenCvSharp.Size(3, 3), 0.7);
            Cv2.MedianBlur(srcImage, gray, 5);

            Mat thresh = new Mat();
            int CircleblockSize = 77;   //77  // 반드시 홀수
            int Circle_C = 11;// 26;   //26;
            Cv2.AdaptiveThreshold(gray, thresh, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, CircleblockSize, Circle_C);//11);

            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));//(5, 5));
            //Cv2.MorphologyEx(thresh, thresh, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            //Cv2.Dilate(thresh, thresh, kernel);
            
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));//(5, 5));
            Cv2.MorphologyEx(thresh, thresh, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            Cv2.Dilate(thresh, thresh, kernel);
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected thresh ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected thresh ", thresh);
                Cv2.WaitKey(0);

            }
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(thresh, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);


            Mat colorImage = new Mat();
            Cv2.CvtColor(srcImage, colorImage, ColorConversionCodes.GRAY2BGR);  // 1채널 → 3채널 변환

            int imageCenterX = thresh.Width / 2;
            int imageCenterY = thresh.Height / 2;

            // 가장 큰 원을 찾기
            foreach (var contour in contours)
            {
                Moments M = Cv2.Moments(contour);
                if (M.M00 == 0) continue;
                //centers.Add(new Point2d(M.M10 / M.M00, M.M01 / M.M00));
                //
                float contourCenterX = (float)(M.M10 / M.M00);
                float contourCenterY = (float)(M.M01 / M.M00);

                float dx = contourCenterX - imageCenterX;
                float dy = contourCenterY - imageCenterY;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                // 거리 임계값, 예: 중심에서 200픽셀 이상 벗어나면 제외
                if (distance > 600)//200)
                {
                    //Console.WriteLine($"del distance:{distance}");
                    continue; // contour 무시
                }
                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);

                //if (perimeter == 0) continue; // 나누기 에러 방지
                double circularity = 4 * Math.PI * area / (perimeter * perimeter);
                if (circularity < 0.005)
                {
                    continue;
                }

                Point2f center;
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);

                //if (radius > 600 && radius < 1100)  //큰원- 실제 원 반지름 조건에 맞게 큰원
                if (radius > 700 && radius < 1000)  //큰원- 실제 원 반지름 조건에 맞게 큰원
                //if (radius > 400 && radius < 850)     //작은원 - 실제 원 반지름 조건에 맞게 
                {
                    centerPos.X = (int)center.X;
                    centerPos.Y = (int)center.Y;

                    //가장 바깥원  = 983
                    //Cv2.Circle(srcImage, new OpenCvSharp.Point((int)center.X, (int)center.Y), (int)radius, Scalar.Red, 2);
                    //Cv2.Circle(srcImage, new OpenCvSharp.Point((int)center.X, (int)center.Y), 3, Scalar.Yellow, -1); // 중심점

                    bRtn = true;
                    Cv2.Circle(colorImage, new OpenCvSharp.Point((int)center.X, (int)center.Y), (int)radius, Scalar.Yellow, 2);

                    System.Drawing.Point clPoint;
                    clPoint = new System.Drawing.Point((int)(center.X - radius), (int)(center.Y - radius));
                    if (autoRun == false)
                    {
                        Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(radius * 2), Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);
                        Globalo.visionManager.milLibrary.DrawOverlayCross(index, (int)(center.X), (int)(center.Y), 100, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                    }
                    else
                    {
                        Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(clPoint.X, clPoint.Y, (int)(radius * 2), 3, System.Drawing.Drawing2D.DashStyle.Solid, Color.Yellow);
                        Globalo.visionManager.milLibrary.m_clMilDrawCross[index].AddList((int)center.X, (int)center.Y, 100, 1, Color.Yellow);
                    }
                    break;
                }
            }

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected colorImage ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorImage ", colorImage);
                Cv2.WaitKey(0);

            }

            return bRtn;
        }
        public bool FindPogoPinCenter(int index, Mat srcImage)
        {
            bool IMG_VIEW = true;
            int startTime = Environment.TickCount;

            //
            int radiusOuter = 250;      //이미지 중심에서 pogoPin을 찾을 원 크기

            OpenCvSharp.Point ImageCenter = new OpenCvSharp.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / 2, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] / 2);

            // 이미지 크기에 맞는 빈 마스크
            Mat mask = Mat.Zeros(srcImage.Size(), MatType.CV_8UC1);

            // 바깥쪽 원 그리기 (하얀색)
            Cv2.Circle(mask, ImageCenter, radiusOuter, Scalar.White, -1);

            Mat masked = new Mat();
            // 원과 원 사이의 영역만 남긴 결과
            Cv2.BitwiseAnd(srcImage, srcImage, masked, mask);       //원본에서 마스크 영역만 남기기
            
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected masked ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected masked ", masked);
                Cv2.WaitKey(0);

            }

            int minThresh = 201;        //센터 찾을때 조명 밝기를 포고핀 외에 다른 영역은 201보다 낮게 조명을 설정해야된다.
            Mat binary = new Mat();
            Cv2.Threshold(masked, binary, minThresh, 255, ThresholdTypes.Binary);     //

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary ", binary);
                Cv2.WaitKey(0);
            }

            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            OpenCvSharp.Point Circlecenter = new OpenCvSharp.Point();
            // 3. 원형성 조건으로 필터링
            Mat result = new Mat();
            Cv2.CvtColor(srcImage, result, ColorConversionCodes.GRAY2BGR);
            int i = 0;
            Console.Write("------------FindContours\n");
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(index);

            Console.Write($"PogoFind Count: {contours.Length}");
            if (contours.Length < 1)
            {
                return false;     //찾은 개수 포고핀 1개 일때만 ok
            }


            bool pogoFindFlag = false;
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

                double dx = binary.Width / 2 - center.X;
                double dy = binary.Height / 2 - center.Y;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (circularity > 0.1 && radius > 1.0 && radius < 50.0)// && distance < 500) // 원형이고 일정 크기 이상
                {
                    int crossLength = 100;// (int)radius; // 또는 원하는 길이로 설정

                    Cv2.Line(result,new OpenCvSharp.Point(center.X - crossLength, center.Y),new OpenCvSharp.Point(center.X + crossLength, center.Y),Scalar.Blue, 2);
                    Cv2.Line(result,new OpenCvSharp.Point(center.X, center.Y - crossLength),new OpenCvSharp.Point(center.X, center.Y + crossLength),Scalar.Blue, 2);
                    //Cv2.Circle(result, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 3);


                    Console.Write("[{0:0}] measured circle: x = {0:0.00}, y = {1:0.00}, radius = {2:0.00}\n", center.X, center.Y, radius);

                    Rectangle m_clRect = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));

                    if (contour.Length >= 5)
                    {
                        pogoFindFlag = true;
                        RotatedRect ellipse = Cv2.FitEllipse(contour);
                        double axisRatio = Math.Min(ellipse.Size.Width, ellipse.Size.Height) / Math.Max(ellipse.Size.Width, ellipse.Size.Height);
                        //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);

                        Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(center.X - crossLength), (int)center.Y, (int)(center.X + crossLength), (int)center.Y, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                        Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)center.X, (int)(center.Y - crossLength), (int)center.X, (int)(center.Y + crossLength), Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                        Console.WriteLine($"Axis ratio (타원 비율): {axisRatio:F2} {circularity}");

                        Circlecenter = new OpenCvSharp.Point(center.X, center.Y);
                    }
                    

                }
            }

            if (IMG_VIEW) 
            {
                Cv2.NamedWindow("Detected result ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected result ", result);
                Cv2.WaitKey(0);
            }





            //
            long elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;

            string str = "";
            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3} (s)";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
            textPoint = new System.Drawing.Point(100, 100);
            str = $"Center : ({Circlecenter.X},{Circlecenter.Y})";
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 25);
            return pogoFindFlag;
        }
        public double OpencvKeytest(MIL_ID tempMilImage,  bool bAutorun = false)
        {
            bool IMG_VIEW = true;
            MIL_ID MilImage = MIL.M_NULL;


            MIL_INT ImageSizeX = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageSizeY = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_Y, MIL.M_NULL);
            int dataSize = (int)ImageSizeX * (int)ImageSizeY;
            if (dataSize < 1)
            {
                return 0.0;
            }
            byte[] ImageBuffer = new byte[dataSize];
            MIL.MbufGet(tempMilImage, ImageBuffer);
            Mat srcImage = new Mat((int)ImageSizeY, (int)ImageSizeX, MatType.CV_8UC1);

            Marshal.Copy(ImageBuffer, 0, srcImage.Data, dataSize);


            Mat gray = new Mat();
            //if (srcImage.Channels() == 3)
            //    Cv2.CvtColor(srcImage, gray, ColorConversionCodes.BGR2GRAY);
            //else
            //    gray = srcImage.Clone();  // 이미 흑백이면 그대로 복사
            //Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(7, 7), 0.3);


            var blurred = new Mat();
            //var edges = new Mat();
            //Cv2.GaussianBlur(srcImage, gray, new OpenCvSharp.Size(3, 3), 0.7);
            Cv2.MedianBlur(srcImage, gray, 3);

            Mat binary = new Mat();
            int CircleblockSize = 65;   //77  // 반드시 홀수
            int Circle_C = 7;// 26;   //26;
            Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, CircleblockSize, Circle_C);//11);



            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));//(5, 5));
            //Cv2.MorphologyEx(thresh, thresh, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            //Cv2.Dilate(thresh, thresh, kernel);
            //Cv2.MorphologyEx로 내부 구멍 제거:

            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5)));

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary ", binary);
                Cv2.WaitKey(0);
            }
            // 2. Contour 찾기
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 2. 가장 큰 외곽선 찾기
            OpenCvSharp.Point[] maxContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
            double area = Cv2.ContourArea(maxContour);
            //double area = Cv2.ContourArea(contour);
            //if (area < 100) continue;  // 노이즈 제거

            // 3. 외곽선 근사 (꼭짓점 수)
            var approx = Cv2.ApproxPolyDP(maxContour, 0.02 * Cv2.ArcLength(maxContour, true), true);

            // 4. 회전된 사각형 (기울기, 폭/높이 비율 등)
            var rotatedRect = Cv2.MinAreaRect(maxContour);
            double angle = (double)rotatedRect.Angle;
            float aspectRatio = Math.Max(rotatedRect.Size.Width, rotatedRect.Size.Height) / Math.Min(rotatedRect.Size.Width, rotatedRect.Size.Height);

            // A/B 분류 기준 예시
            string type;
            //if (approx.Length == 3 && angle > -20 && angle < 20)
            //    type = "A 타입 (정삼각형 형태)";
            //else if (approx.Length == 3 && (angle < -30 || angle > 30))
            //    type = "B 타입 (눕힌 삼각형)";
            //else
            //    type = "Unknown";

            Console.WriteLine($"면적: {area:F2}, 꼭짓점: {approx.Length}, 각도: {angle:F2}");


            Mat colorView = new Mat();
            Cv2.CvtColor(srcImage, colorView, ColorConversionCodes.GRAY2BGR); // 컬러 이미지로 변환
            Cv2.DrawContours(
                image: colorView,
                contours: new[] { maxContour },  // Point[][] 형식
                contourIdx: -1,                  // 전체 그리기
                color: new Scalar(0, 0, 255),    // 빨간색
                thickness: 2
            );
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected colorView ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorView ", colorView);
                Cv2.WaitKey(0);
            }
            return angle;
        }
        public int MilEdgeKeytest(int index, int roiIndex, string keyType , double offsetX = 0.0 , double offsetY = 0.0, bool bAutorun = false)
        {
            bool IMG_VIEW = false;
            int startTime = Environment.TickCount;
            int nRtn = 0;

            const int CONTOUR_MAX_RESULTS = 300;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilEdgeResult = MIL.M_NULL;                              // Edge result identifier.
            MIL_ID MilEdgeContext = MIL.M_NULL;                             // Edge context.

            double[] EdgeCircleFitCx = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeCircleFitErr = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeConvex = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeFastLength = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeLength = new double[CONTOUR_MAX_RESULTS];
            double[] EdgePositionY = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeSize = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeStrength = new double[CONTOUR_MAX_RESULTS];


            int startX = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].X + (int)offsetX;
            int startY = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Y + (int)offsetY;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            //MIL_INT ImageSizeX = MIL.MbufInquire(Globalo.visionManager.milLibrary.MilProcImageChild[index], MIL.M_SIZE_X, MIL.M_NULL);
            //MIL_INT ImageSizeY = MIL.MbufInquire(Globalo.visionManager.milLibrary.MilProcImageChild[index], MIL.M_SIZE_Y, MIL.M_NULL);
            
            //MIL.MgraArcFill(MIL.M_DEFAULT, Globalo.visionManager.milLibrary.MilProcImageChild[index], ImageSizeX/2, ImageSizeY/2, 1200, 800, 0, 360);


            //MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], startX, startY, OffsetWidth, OffsetHeight, ref tempMilImage);
            //MIL.MbufChild2d(tempMilImage, OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);

            if (keyType == "A" || keyType == "B")
            {
                if (roiIndex == 0)
                {
                    double keyAngle = Globalo.visionManager.aoiTopTester.OpencvKeytest(tempMilImage);
                    Console.WriteLine($"keyAngle ----- {keyAngle} ");
                }
            }

            MIL.MbufExport($"d:\\org_Key{roiIndex}.BMP", MIL.M_BMP, tempMilImage);
            MIL.MgraColor(MIL.M_DEFAULT, 0);
            if (keyType == "A")
            {
                if (roiIndex == 0)
                {
                    //A타입의 왼쪽 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);
                    //MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 0, OffsetWidth / 1.8, OffsetWidth / 1.8, 180, 270);
                }
                else
                {
                    //A타입의 우측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 0, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }
            if (keyType == "B")
            {
                if (roiIndex == 0)
                {
                    //좌측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);
                }
                else
                {
                    //우측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 0, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }
            if (keyType == "C")
            {
                if (roiIndex == 0)
                {
                    //좌측 위
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, OffsetHeight - 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 90, 180); //C의 왼쪽 위(1/2)
                }
                else
                {
                    //우측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 0, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }
            if (keyType == "D")
            {
                if (roiIndex == 0) 
                {
                    //우측 위
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 0, OffsetHeight - 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 0, 90);
                }
                else
                {
                    //우측아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 0, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }

            if (keyType == "E")
            {
                //Key 1개
                MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 0, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);//E타입의 왼쪽 아래 (1개)
            }
            
            
            MIL.MbufExport($"d:\\OrgKey{roiIndex}.BMP", MIL.M_BMP, tempMilImage);

            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_FIXED + MIL.M_GREATER, 40, MIL.M_NULL);
            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_PERCENTILE_VALUE + MIL.M_GREATER, 50, MIL.M_NULL); 
            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_TRIANGLE_BISECTION_DARK, MIL.M_NULL, MIL.M_NULL); 
            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_PERCENTILE_VALUE, MIL.M_NULL, MIL.M_NULL); 
            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_FIXED + MIL.M_GREATER, 50, MIL.M_NULL);


            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_BIMODAL + MIL.M_LESS, MIL.M_NULL, MIL.M_NULL); 
            MIL.MbufExport($"d:\\Key{roiIndex}.BMP", MIL.M_BMP, tempMilImage);


            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);
            MilImage = tempMilImage;
            if (true)//bAutorun == false)
            {
                MIL.MdispSelect(MilDisplay, MilImage);
            }
            

            /* Allocate a graphic list to hold the subpixel annotations to draw. */
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);
            /* Associate the graphic list to the display for annotations. */
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);
            // Allocate a Edge Finder context.
            MIL.MedgeAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_CONTOUR, MIL.M_DEFAULT, ref MilEdgeContext);

            // Allocate a result buffer.
            MIL.MedgeAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilEdgeResult);

            // Enable features to compute.
            MIL.MedgeControl(MilEdgeContext, MIL.M_CIRCLE_FIT_CENTER_X, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_CIRCLE_FIT_ERROR, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_CONVEX_PERIMETER, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_FAST_LENGTH, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_LENGTH, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_POSITION_Y, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_SIZE, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_STRENGTH, MIL.M_ENABLE);


            MIL.MedgeControl(MilEdgeContext, MIL.M_FILTER_TYPE, MIL.M_DERICHE);//MIL.M_DERICHE); M_SOBEL
            MIL.MedgeControl(MilEdgeContext, MIL.M_FILTER_SMOOTHNESS, 65.0);
            // Calculate edges and features.
            MIL.MedgeCalculate(MilEdgeContext, MilImage, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MilEdgeResult, MIL.M_DEFAULT);





            MIL_INT NumEdgeFound = 0;// Number of edges found.

            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumEdgeFound);


            //-----------------------------------------------------------------------------------------------------------------------------
            //
            //불필요한 edge 제거하기
            //
            //
            //-----------------------------------------------------------------------------------------------------------------------------
            //
            //double CONTOUR_MAXIMUM_ELONGATION = 0.8;
            // Exclude elongated edges.
            //MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_MOMENT_ELONGATION, MIL.M_LESS, CONTOUR_MAXIMUM_ELONGATION, MIL.M_NULL);
            // Exclude inner chains.
            //MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_INCLUDED_EDGES, MIL.M_INSIDE_BOX, MIL.M_NULL, MIL.M_NULL);
            //MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_BOX_Y_MIN, MIL.M_LESS, 580.0, MIL.M_NULL);
            //MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_LESS, 100.0, MIL.M_NULL);


            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_LESS, 3.0, MIL.M_NULL);     //250615 less Size 5.0
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_GREATER, 40.0, MIL.M_NULL); //250615 greater Size 30.0

            // Draw edges in the source image to show the result.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MedgeControl(MilEdgeResult, 319L, (double)-startX);
            MIL.MedgeControl(MilEdgeResult, 320L, (double)-startY);


            MIL.MedgeControl(MilEdgeResult, 3203L, Globalo.visionManager.milLibrary.xReduce[index]);
            MIL.MedgeControl(MilEdgeResult, 3204L, Globalo.visionManager.milLibrary.yReduce[index]);
            if (bAutorun)
            {
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, Globalo.visionManager.milLibrary.MilSetCamOverlay, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            }
            
            //M_DRAW_BOX + M_DRAW_POSITION + M_DRAW_EDGES + M_DRAW_AXIS

            MIL_INT NumResults = 0;                                         // Number of results found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumResults);

            int minIndex = 0;
            int maxIndex = 0;
            double CircleCx = 0.0;
            double CircleErr = 0.0;
            string str = "";
            Rectangle m_clRect = new Rectangle((int)(startX), (int)(startY), OffsetWidth, OffsetHeight);
            // If the right number of edges were found.
            
            Color ConeColor;
            System.Drawing.Point textPoint;
            Console.Write($"Key Edge Count:{NumResults}\n");
            if (NumResults <= CONTOUR_MAX_RESULTS)
            {
                // Draw the index of each edge.
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_INDEX, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Get the mean Feret diameters.
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_CENTER_X, EdgeCircleFitCx);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_ERROR, EdgeCircleFitErr);

                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CONVEX_PERIMETER, EdgeConvex);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_FAST_LENGTH, EdgeFastLength);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_LENGTH, EdgeLength);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, EdgePositionY);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_SIZE, EdgeSize);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_STRENGTH, EdgeStrength);

                // Print the results.
                

                //Console.WriteLine($"M_CIRCLE_FIT_CENTER_X : {EdgeCircleFitCx[0]}");
                //Console.WriteLine($"M_CIRCLE_FIT_ERROR : {EdgeCircleFitErr[0]}");
                //Console.WriteLine($"M_CONVEX_PERIMETER : {EdgeConvex[0]}");
                //Console.WriteLine($"M_FAST_LENGTH : {EdgeFastLength[0]}");
                //Console.WriteLine($"M_LENGTH : {EdgeLength[0]}");
                //Console.WriteLine($"M_POSITION_Y : {EdgePositionY[0]}");
                //Console.WriteLine($"M_SIZE : {EdgeSize[0]}");
                //Console.WriteLine($"M_STRENGTH : {EdgeStrength[0]}");



                CircleCx = EdgeCircleFitCx[0];
                CircleErr = EdgeCircleFitErr[0];
                str = $"#{roiIndex + 1}";
                textPoint = new System.Drawing.Point(m_clRect.X, m_clRect.Y);


                

                int KeyEdgeSpec = Globalo.yamlManager.configData.CamSettings.KeyEdgeSpecCount;

                
                if (NumResults < KeyEdgeSpec)
                {
                    nRtn = 0;
                    Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Red, 2);
                    Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 13);
                } 
                else
                {
                    nRtn = 1;
                    Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Yellow, 2);
                    Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 13);
                }
                
               
                

                str = $"[CONE] Edge Count:{NumResults}/{KeyEdgeSpec}";

                //textPoint = new System.Drawing.Point(10, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 17);

                if (nRtn == 1)
                {
                    str = $"{keyType} Key #{roiIndex+1} Detected!";
                    ConeColor = Color.Green;
                }
                else
                {
                    str = $"{keyType} Key #{roiIndex + 1} Not Detected!";
                    ConeColor = Color.Red;
                }


                //int leng = str.Length;
                //textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / (leng - 10)), 200 + (250* roiIndex));

                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, ConeColor, 35);
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");


                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Red, 2);
                nRtn = 0;
            }

            //--------------------------------------------------------------------------------------------------------Test
            //
            //
            //
            
            //
            //
            //
            //
            //--------------------------------------------------------------------------------------------------------


            int elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;


            str = $"Test Time: {elapsedMs} ms";
            //Console.WriteLine(str);
            //Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
            //Console.WriteLine(str);
            //Globalo.LogPrint("", str);

            if (bAutorun == false)
            {
                System.Drawing.Point timetextPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150 - (100* roiIndex));
                Globalo.visionManager.milLibrary.DrawOverlayText(index, timetextPoint, str, Color.Blue, 15);
            }
            return nRtn;
        }

        public void Keytest(int index, Mat srcImage, OpenCvSharp.Point circle1, int roiIndex)
        {
            bool IMG_VIEW = true;
            int startTime = Environment.TickCount;
            int radius = 700;

            int OffsetX = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].X;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Height;


            //Rect keyRoi = new Rect((int)circle1.X - 920, (int)circle1.Y + 160, 650, 650);
            Rect keyRoi = new Rect((int)OffsetX, (int)OffsetY, OffsetWidth, OffsetHeight);
            Mat roiKeyMat = srcImage[keyRoi];

            // 이미지 크기에 맞는 빈 마스크
            Mat mask = Mat.Zeros(roiKeyMat.Size(), MatType.CV_8UC1);

            // 중심 (0, 0)에서 반지름만큼 하얀 원 그리기
            //Cv2.Circle(mask, new OpenCvSharp.Point(720, -140), radius, Scalar.White, -1);
            // 원의 중심은 모서리(0,0)
             
            OpenCvSharp.Point center = new OpenCvSharp.Point(roiKeyMat.Width, 0);
            OpenCvSharp.Size axes = new OpenCvSharp.Size(450, 450); // 원형이면 x=y
            // 0도에서 90도까지의 부채꼴 (시계방향)

            // 타원 회전 각도
            // 시작 각
            // 끝 각
            Cv2.Ellipse(mask, center, axes, 0, 180, 90,Scalar.White, -1 );
            Mat invertedMask = new Mat();

            //Cv2.BitwiseXor(mask, mask, invertedMask);
            Cv2.BitwiseNot(mask, invertedMask);
            

            Mat result = new Mat();
            roiKeyMat.CopyTo(result, invertedMask);  // 반전된 마스크를 이용해 src의 특정 영역 복사
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Keytest result ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Keytest result ", result);
                Cv2.WaitKey(0);
            }
            

            var binary = new Mat();
            int blockSize = 21;// 21; // 반드시 홀수
            int C = 13;
            Cv2.AdaptiveThreshold(result, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            int i = 0;
            //List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            for (i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                double perimeter = Cv2.ArcLength(contours[i], true);

                // if (hierarchy[i].Parent != -1 && area > 0.01 && area < 10000) // 조건: 면적이 100 이상
                if (area > 150 && area < 1100) // 조건: 면적이 100 이상
                {
                    Cv2.DrawContours(binary, contours, i, new Scalar(0, 255, 255), 2);

                    // 윤곽선 그리기 (노란색)

                    Console.WriteLine($"Contour with area {area} detected.");

                    // contour는 List<Point> 형식이므로, 이를 Graphics.DrawPolygon에 맞게 사용
                    //points = contours[i].Select(p => new System.Drawing.Point(p.X, p.Y)).ToList();
                    List<System.Drawing.Point> points = new List<System.Drawing.Point>();
                    foreach (var p in contours[i])
                    {
                        points.Add(new System.Drawing.Point((int)((p.X + circle1.X - 920) * Globalo.visionManager.milLibrary.xReduce[index]), (int)((p.Y + circle1.Y + 200) * Globalo.visionManager.milLibrary.yReduce[index])));
                    }
                    if (points.Count >= 3) // 다각형이 되려면 꼭지점 3개 이상
                    {
                        Globalo.visionManager.milLibrary.DrawOverlayPolygon(index, points, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);
                    }
                }
                
            }

            //
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected result2 ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected result2 ", binary);
                Cv2.WaitKey(0);
            }
                


            long elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;

            string str = "";
            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
        }

        public int GasketTest(int index, Mat srcImage, OpenCvSharp.Point circle1, bool bAutorun = false)
        {
            //가스켓 밝기 계산
            bool IMG_VIEW = false;
            int startTime = Environment.TickCount;

            //Mat binary = new Mat();

            int blockSize = 13; // 반드시 홀수
            int C = 13;

            //Cv2.AdaptiveThreshold(srcImage, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            //Cv2.NamedWindow("GasketTest binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("GasketTest binary ", binary);
            //Cv2.WaitKey(0);
            int radiusOuter = 830;// 700;
            int radiusInner = 580;// 430;
                

            // 이미지 크기에 맞는 빈 마스크
            Mat mask = Mat.Zeros(srcImage.Size(), MatType.CV_8UC1);

            // 바깥쪽 원 그리기 (하얀색)
            Cv2.Circle(mask, circle1, radiusOuter, Scalar.White, -1);

            // 안쪽 원을 검정으로 덮기 (원 내부 제거)
            Cv2.Circle(mask, circle1, radiusInner, Scalar.Black, -1);
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected mask ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected mask ", mask);
                Cv2.WaitKey(0);
            }
           
            Mat masked = new Mat();
            Cv2.BitwiseAnd(srcImage, srcImage, masked, mask);

            // 원과 원 사이의 영역만 남긴 결과
            //Mat ringRegion = new Mat();
            //srcImage.CopyTo(ringRegion, mask);  // src는 그레이스케일 이미지

            // 평균 밝기 계산
            Scalar avg = Cv2.Mean(srcImage, mask);  // mask로 지정된 영역만

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected masked ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected masked ", masked);
                Cv2.WaitKey(0);

            }
            string str = "";
            int gaskerLight = (int)Math.Round(avg.Val0);

            Console.WriteLine($"가스켓 영역 평균 밝기: {avg.Val0:F2}");

            System.Drawing.Point clPoint;


            //안쪽 Mask roi
            clPoint = new System.Drawing.Point((int)(circle1.X - radiusInner), (int)(circle1.Y - radiusInner));
            if (bAutorun == false)
            {
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(radiusInner * 2), Color.DarkOliveGreen, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            }
            else
            {
                Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(clPoint.X, clPoint.Y, (int)(radiusInner * 2), 2, System.Drawing.Drawing2D.DashStyle.Solid, Color.DarkOliveGreen);
            }
            
            //바깥쪽 Mask roi
            clPoint = new System.Drawing.Point((int)(circle1.X - radiusOuter), (int)(circle1.Y - radiusOuter));
            if (bAutorun == false)
            {
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(radiusOuter * 2), Color.DarkOliveGreen, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            }
            else
            {
                Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(clPoint.X, clPoint.Y, (int)(radiusOuter * 2), 2, System.Drawing.Drawing2D.DashStyle.Solid, Color.DarkOliveGreen);
            }
            int specMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MIN"].value);
            int specMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MAX"].value);

            //Average brightness of gasket area
            //밝기 표시
            clPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 370);// Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
            str = $"Gasket Light: {gaskerLight} [{specMin}~{specMax}]";


            if (bAutorun == false)
            {
                if (gaskerLight < specMin || gaskerLight > specMax)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, clPoint, str, Color.OrangeRed, 20);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(clPoint, str, "나눔고딕", Color.OrangeRed, 13);
                }
                else
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, clPoint, str, Color.GreenYellow, 20);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(clPoint, str, "나눔고딕", Color.GreenYellow, 13);
                }
            }
            
            
            //////////////////////////////////////////////////////////////////////////////////////////////
            long elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;

            
            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3} s";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            if (bAutorun == false)
            {
                System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 400);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", Color.Blue, 15);
            }

            return gaskerLight;

        }
        //public OpenCvSharp.Point Housing_Fakra_Test(int index, Mat srcImage)
        public List<OpenCvSharp.Point> Housing_Fakra_Test(int index, Mat srcImage, bool bAutorun = false)
        {
            // 측정 시작
            bool IMG_VIEW = true;
            int startTime = Environment.TickCount;
            Console.WriteLine($"Housing_Fakra_Test Test Start");
            OpenCvSharp.Point centerPos = new OpenCvSharp.Point();
            string str = "";
            List<OpenCvSharp.Point> FakraPoints = new List<OpenCvSharp.Point>();
            //
            //
            //
            

            


            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            var edges = new Mat();
            //Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(5, 5), 0.5);// 0.7);
            Cv2.MedianBlur(srcImage, blurred, 3);
            //Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화

            //Mat lap = new Mat();
            //Cv2.Laplacian(blurred, lap, MatType.CV_8U, ksize: 5);

            ////// 4. 절대값으로 변환 (음수 엣지를 양수로)
            //Mat absLap = new Mat();
            //Cv2.ConvertScaleAbs(lap, absLap);


            //Cv2.NamedWindow("Detected absLap ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("Detected absLap ", absLap);
            //Cv2.WaitKey(0);

            //int weakedge = 65;// 65;//40;      //<-- 이값보다 작으면 무시
            //int strongedge = 100;//170;// 150;   //<---이값보다 크면 엣지 강화

            //Cv2.Canny(blurred, edges, weakedge, strongedge);  // 윤곽 강화
            //if (IMG_VIEW)
            //{
            //    Cv2.NamedWindow("Detected srcImage ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //    Cv2.ImShow("Detected srcImage ", srcImage);
            //    Cv2.WaitKey(0);
            //}

            ///Cv2.EqualizeHist(srcImage, srcImage);
            int blockSize = 55;// 77;// 19; // 반드시 홀수
            //픽셀마다 기준 밝기를 계산할 때, 주변 영역 크기를 의미해요.
            //작을수록 세밀한 기준 밝기 계산 → 노이즈에 민감
            //클수록 넓은 영역 기준 → 밝기 변화 큰 영역에 안정적
            int C = 54; //c가 크면 검은 영역 강화, 작으면 흰색 영역 강화
            //큰원 26
            //작은원 30
            //int minThresh = 70;
            //Cv2.Threshold(edges, binary, minThresh, 255, ThresholdTypes.Binary);     //
            Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            //Cv2.AdaptiveThreshold(absLap, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            //Cv2.AdaptiveThreshold(edges, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);

            // 5. (선택) 이진화로 엣지 강화
            //Cv2.Threshold(blurred, binary, 80, 255, ThresholdTypes.Binary);
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected binary1 ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary1 ", binary);
                Cv2.WaitKey(0);
            }

            // 2. 커널 생성 (원형 커널 추천)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            Cv2.Dilate(binary, binary, kernel);
            // 3. Contours 찾기
            int imageCenterX = 1172;// binary.Width / 2;
            int imageCenterY = 1427;// binary.Height / 2;

            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //결과
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------

            // 4. 이미지 중심 계산
            double minDist = double.MaxValue;
            OpenCvSharp.Point bestPoint = new OpenCvSharp.Point();
            OpenCvSharp.Point2d imageCenter = new OpenCvSharp.Point2d(srcImage.Width / 2, srcImage.Height / 2);

            // 3. 각 컨투어 중심을 평균
            ///List<Point2d> centers = new List<Point2d>();

            Mat colorView = new Mat();
            Cv2.CvtColor(srcImage, colorView, ColorConversionCodes.GRAY2BGR);

            List<(OpenCvSharp.Point2f center, float radius)> circles = new List<(Point2f center, float radius)>();

            OpenCvSharp.Point2f maxCenter = new Point2f();
            OpenCvSharp.Point[] maxContour = null;
            float maxRadius = 0f;

            foreach (var contour in contours)
            {
                Moments M = Cv2.Moments(contour);
                if (M.M00 == 0) continue;
                //centers.Add(new Point2d(M.M10 / M.M00, M.M01 / M.M00));

                float contourCenterX = (float)(M.M10 / M.M00);
                float contourCenterY = (float)(M.M01 / M.M00);

                float dx = contourCenterX - imageCenterX;
                float dy = contourCenterY - imageCenterY;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                // 거리 임계값, 예: 중심에서 200픽셀 이상 벗어나면 제외
                if (distance > 300)//350)
                {
                    //Console.WriteLine($"del distance:{distance}");
                    continue; // contour 무시
                }
                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);

                //if (perimeter == 0) continue; // 나누기 에러 방지

                double minArea = 1841053.5;
                double maxArea = 2136083.5;

                if (area > minArea && area < maxArea)
                {
                    //continue;
                }


                double circularity = 4 * Math.PI * area / (perimeter * perimeter);
                // 외접 원 그리기
                Point2f center;
                float radius = 0.0f;

                Cv2.MinEnclosingCircle(contour, out center, out radius);

                if (radius > 100 && radius < 600)
                {
                    Console.WriteLine($"Fakra radius:{radius}");
                }


                //if (radius < 300 || radius > 600)   //안쪽원 377정도나옴
                if (radius < 120 || radius > 280)
                {
                    continue;
                }
                Console.Write($"[Housing] radius: {radius}, area: {area}, circularity: {circularity}\n");
                if (circularity < 0.01)//0.01)
                {
                    continue;
                }

                //여기서 가스켓 검사위한 동심도 중심 x,y 축 찾기

               // centerPos[0].x
                if (radius > maxRadius)
                {
                    maxRadius = radius;
                    maxCenter = center;
                    maxContour = contour;
                }

                
                circles.Add((center, radius));
                ///centers.Add(new Point2d(center.X, center.Y));
                //Console.Write("[CenterFind] measured circle: x = {0:0.00}, y = {1:0.00}, circularity = {2:0.00}, radius = {3:0.00}, area = {4:0.00}\n", center.X, center.Y, circularity, radius, area);
            }
            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            // Result Display
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------

            double CamResolX = 0.0;
            double CamResolY = 0.0;
            CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   //0.0186f;
            if (circles.Count > 0)
            {
                // 반지름 순으로 정렬
                //var sortedCircles = circles.OrderBy(c => c.radius).ToList();

                var minCircle = circles.OrderBy(c => c.radius).First();
                var maxCircle = circles.OrderByDescending(c => c.radius).First();

                centerPos.X = (int)maxCircle.center.X;
                centerPos.Y = (int)maxCircle.center.Y;

                // 그리기 예시
                Cv2.Circle(colorView, (OpenCvSharp.Point)minCircle.center, (int)minCircle.radius, Scalar.Red, 2);   // 내경
                Cv2.Circle(colorView, (OpenCvSharp.Point)maxCircle.center, (int)maxCircle.radius, Scalar.Blue, 2);  // 외경

                Console.Write($"[minCircle] {minCircle.center.X},{minCircle.center.Y}, radius: {minCircle.radius}\n");
                Console.Write($"[maxCircle] {maxCircle.center.X},{maxCircle.center.Y}, radius: {maxCircle.radius}\n");
                System.Drawing.Point clPoint;

                //Rectangle m_clRect2 = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));
                clPoint = new System.Drawing.Point((int)(minCircle.center.X - minCircle.radius), (int)(minCircle.center.Y - minCircle.radius));
                if (bAutorun == false)
                {
                    Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(minCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);
                }
                else
                {
                    Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(clPoint.X, clPoint.Y, (int)(minCircle.radius * 2), 2, System.Drawing.Drawing2D.DashStyle.Solid, Color.Blue);
                }
                
                


                clPoint = new System.Drawing.Point((int)(maxCircle.center.X - maxCircle.radius), (int)(maxCircle.center.Y - maxCircle.radius));
                if (bAutorun == false)
                {
                    Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(maxCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);
                }
                else
                {
                    Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(clPoint.X, clPoint.Y, (int)(maxCircle.radius * 2), 2, System.Drawing.Drawing2D.DashStyle.Solid, Color.Blue);
                }
                
                    

                System.Drawing.Point HousingPoint = new System.Drawing.Point();


                HousingPoint = new System.Drawing.Point(850, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 410);
                str = $"Fakra In  X:{(minCircle.center.X * CamResolX).ToString("0.00#")},Y:{(minCircle.center.Y * CamResolY).ToString("0.00#")},R:{(minCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.LogPrint("ManualControl", str);
                if (bAutorun == false)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(HousingPoint, str, "나눔고딕", Color.GreenYellow, 13);
                }
                


                HousingPoint = new System.Drawing.Point(850, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 340);
                str = $"Fakra Out X:{(maxCircle.center.X * CamResolX).ToString("0.00#")},Y:{(maxCircle.center.Y * CamResolY).ToString("0.00#")},R:{(maxCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.LogPrint("ManualControl", str);
                if (bAutorun == false)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(HousingPoint, str, "나눔고딕", Color.GreenYellow, 13);

                }
                



                FakraPoints.Add((OpenCvSharp.Point)minCircle.center);
                FakraPoints.Add((OpenCvSharp.Point)maxCircle.center);

            }


            

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected colorView ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorView ", colorView);
                Cv2.WaitKey(0);
            }

            


            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            // 측정 시간 출력
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------
            long elapsedMs = Environment.TickCount - startTime;
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;

            str = $"In Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            str = $"In Test Time: {elapsedMs / 1000.0:F3}(s)";
            Console.WriteLine(str);
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
            //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
            if (bAutorun == false)
            {
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", Color.Blue, 13);
            }
                

            return FakraPoints;
        }
        public List<OpenCvSharp.Point> Housing_Dent_Test(int index, Mat srcImage, bool bDentTest = false, bool bAutorun = false)
        {
            //roiIndex = 0 (외경)
            //roiIndex = 1 (내경)
            // 측정 시작
            bool IMG_VIEW = true;
            int startTime = Environment.TickCount;
            Console.WriteLine($"Housing_Dent_Test Test Start");
            OpenCvSharp.Point centerPos = new OpenCvSharp.Point();
            string str = "";
            List<OpenCvSharp.Point> HousingPoints = new List<OpenCvSharp.Point>();
            //
            //안쪽원 마스크 처리하기

            OpenCvSharp.Point circleCenter = new OpenCvSharp.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index]/2, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index]/2);
            // 이미지 크기에 맞는 빈 마스크
            //Mat mask = Mat.Zeros(srcImage.Size(), MatType.CV_8UC1);
            //// 1. 원형 마스크 만들기
            //Mat outerMask = new Mat(srcImage.Size(), MatType.CV_8UC1, Scalar.Black);
            //Mat innerMask = new Mat(srcImage.Size(), MatType.CV_8UC1, Scalar.Black);

            //Cv2.Circle(outerMask, circleCenter, 1300, Scalar.White, -1);//, LineTypes.AntiAlias);
            //Cv2.Circle(innerMask, circleCenter, 580, Scalar.White, -1);//, LineTypes.AntiAlias);
            // 안쪽 원을 검정으로 덮기 (원 내부 제거)
            //Cv2.Circle(mask, circleCenter, 580, Scalar.White, -1);


            // 바깥쪽 사각형 그리기 (하얀색)
            //OpenCvSharp.Rect maskRect = new Rect(circleCenter.X - 1400, 0, 2800, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index]);
            //Cv2.Rectangle(mask, maskRect, Scalar.White, -1, LineTypes.AntiAlias);

            // 안쪽 원을 검정으로 덮기 (원 내부 제거)
           // Cv2.Circle(mask, circleCenter, 580, Scalar.Black, -1, LineTypes.AntiAlias);

            // 2. 테두리만 추출
            //Cv2.Subtract(outerMask, innerMask, mask);

            // 3. 필요하다면 약간 부드럽게
            //Cv2.GaussianBlur(mask, mask, new OpenCvSharp.Size(7, 7), 0);

            //Mat masked = new Mat();
            //Cv2.BitwiseAnd(srcImage, srcImage, masked, mask);

            //Cv2.NamedWindow("Detected masked ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("Detected masked ", masked);
            //Cv2.WaitKey(0);
            //
            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            //var edges = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(3, 3), 0);
            //Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화

            //int weakedge = 65;//40;      //<-- 이값보다 작으면 무시
            //int strongedge = 170;// 150;   //<---이값보다 크면 엣지 강화

            //Cv2.Canny(blurred, edges, weakedge, strongedge);  // 윤곽 강화
            //if (IMG_VIEW)
            //{
            //    Cv2.NamedWindow("Detected edges ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //    Cv2.ImShow("Detected edges ", edges);
            //    Cv2.WaitKey(0);
            //}

            ///Cv2.EqualizeHist(srcImage, srcImage);
            int blockSize = 77;// 77; // 반드시 홀수
            //픽셀마다 기준 밝기를 계산할 때, 주변 영역 크기를 의미해요.
            //작을수록 세밀한 기준 밝기 계산 → 노이즈에 민감
            //클수록 넓은 영역 기준 → 밝기 변화 큰 영역에 안정적
            int C = 35;// 18; //30//c가 크면 검은 영역 강화, 작으면 흰색 영역 강화
            //작은원 30
            //큰원 18
            //int minThresh = 70;
            //Cv2.Threshold(edges, binary, minThresh, 255, ThresholdTypes.Binary);     //
            //Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);

            // 2. 커널 생성 (원형 커널 추천)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));//(5, 5));
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            Cv2.Dilate(binary, binary, kernel);

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary ", binary);
                Cv2.WaitKey(0);
            }
            // 3. Contours 찾기
            int imageCenterX = 1172;// binary.Width / 2;
            int imageCenterY = 1427;// binary.Height / 2;
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //결과
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------

            // 4. 이미지 중심 계산
            double minDist = double.MaxValue;
            OpenCvSharp.Point bestPoint = new OpenCvSharp.Point();
            OpenCvSharp.Point2d imageCenter = new OpenCvSharp.Point2d(srcImage.Width / 2, srcImage.Height / 2);

            // 3. 각 컨투어 중심을 평균
            ///List<Point2d> centers = new List<Point2d>();

            Mat colorView = new Mat();
            Cv2.CvtColor(srcImage, colorView, ColorConversionCodes.GRAY2BGR);

            List<(OpenCvSharp.Point2f center, float radius)> circles = new List<(Point2f center, float radius)>();

            OpenCvSharp.Point2f maxCenter = new Point2f();
            OpenCvSharp.Point[] maxContour = null;
            float maxRadius = 0f;

            foreach (var contour in contours)
            {
                Moments M = Cv2.Moments(contour);
                if (M.M00 == 0) continue;
                //centers.Add(new Point2d(M.M10 / M.M00, M.M01 / M.M00));

                float contourCenterX = (float)(M.M10 / M.M00);
                float contourCenterY = (float)(M.M01 / M.M00);

                float dx = contourCenterX - imageCenterX;
                float dy = contourCenterY - imageCenterY;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                // 거리 임계값, 예: 중심에서 200픽셀 이상 벗어나면 제외
                if (distance > 300)//200)
                {
                    //Console.WriteLine($"del distance:{distance}");
                    continue; // contour 무시
                }
                double area = Cv2.ContourArea(contour);
                double perimeter = Cv2.ArcLength(contour, true);

                //if (perimeter == 0) continue; // 나누기 에러 방지

                double minArea = 1841053.5;
                double maxArea = 2136083.5;

                if (area > minArea && area < maxArea)
                {
                    //continue;
                }
                

                double circularity = 4 * Math.PI * area / (perimeter * perimeter); 
                // 외접 원 그리기
                Point2f center;
                float radius = 0.0f;

                Cv2.MinEnclosingCircle(contour, out center, out radius);


                //if (radius < 600 || radius > 1000)//890)
                if (radius < 280 || radius > 550)//890)
                {
                    continue;
                }
                Console.Write($"[Housing] radius: {radius}, area: {area}, circularity: {circularity}\n");
                if (circularity < 0.01)
                {
                    continue;
                }

                if (radius > maxRadius)
                {
                    maxRadius = radius;
                    maxCenter = center;
                    maxContour = contour;
                }
                circles.Add((center, radius));
                ///centers.Add(new Point2d(center.X, center.Y));
                //Console.Write("[CenterFind] measured circle: x = {0:0.00}, y = {1:0.00}, circularity = {2:0.00}, radius = {3:0.00}, area = {4:0.00}\n", center.X, center.Y, circularity, radius, area);
            }
            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            // 내경,외경 원찾기
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------
            double CamResolX = 0.0;
            double CamResolY = 0.0;
            CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   //0.0186f;
            if (circles.Count > 0 && bDentTest == false)
            {
                var minCircle = circles.OrderBy(c => c.radius).First();
                var maxCircle = circles.OrderByDescending(c => c.radius).First();

                centerPos.X = (int)maxCircle.center.X;
                centerPos.Y = (int)maxCircle.center.Y;

                // 그리기 예시
                Cv2.Circle(colorView, (OpenCvSharp.Point)minCircle.center, (int)minCircle.radius, Scalar.Red, 2);   // 내경
                Cv2.Circle(colorView, (OpenCvSharp.Point)maxCircle.center, (int)maxCircle.radius, Scalar.Blue, 2);  // 외경

                Console.Write($"[minCircle] {minCircle.center.X},{minCircle.center.Y}, radius: {minCircle.radius}\n");
                Console.Write($"[maxCircle] {maxCircle.center.X},{maxCircle.center.Y}, radius: {maxCircle.radius}\n");
                System.Drawing.Point clPoint;

                //Rectangle m_clRect2 = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));
                
                clPoint = new System.Drawing.Point((int)(minCircle.center.X - minCircle.radius), (int)(minCircle.center.Y - minCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(minCircle.radius * 2), Color.Blue, 3, System.Drawing.Drawing2D.DashStyle.Solid);

                clPoint = new System.Drawing.Point((int)(maxCircle.center.X - maxCircle.radius), (int)(maxCircle.center.Y - maxCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(maxCircle.radius * 2), Color.Blue, 3, System.Drawing.Drawing2D.DashStyle.Solid);
                

                System.Drawing.Point HousingPoint = new System.Drawing.Point();
                
                HousingPoint = new System.Drawing.Point(850, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 270);
                str = $"Housing In  X:{(minCircle.center.X * CamResolX).ToString("0.00#")},Y:{(minCircle.center.Y * CamResolY).ToString("0.00#")},R:{(minCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.LogPrint("ManualControl", str);
                if (bAutorun == false)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(HousingPoint, str, "나눔고딕", Color.GreenYellow, 13);
                }
                
                

                HousingPoint = new System.Drawing.Point(850, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 200);
                str = $"Housing Out X:{(maxCircle.center.X * CamResolX).ToString("0.00#")},Y:{(maxCircle.center.Y * CamResolY).ToString("0.00#")},R:{(maxCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.LogPrint("ManualControl", str);
                if (bAutorun == false)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(HousingPoint, str, "나눔고딕", Color.GreenYellow, 13);
                }
                
                    


                HousingPoints.Add((OpenCvSharp.Point)minCircle.center);
                HousingPoints.Add((OpenCvSharp.Point)maxCircle.center);
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //Dent 찌그러짐 검사 결과
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------
            int DentUnderCount = 0;
            if (maxRadius > 1 && bDentTest == true)
            {
                int sampleCount = Globalo.yamlManager.configData.CamSettings.DentTotalCount; // 원하는 샘플 개수 (조절 가능)


                List<Point2f> idealCirclePoints = new List<Point2f>();
                for (int i = 0; i < sampleCount; i++)
                {
                    double angle = 2 * Math.PI * i / sampleCount;

                    float x = maxCenter.X + (float)(maxRadius * Math.Cos(angle));
                    float y = maxCenter.Y + (float)(maxRadius * Math.Sin(angle));

                    idealCirclePoints.Add(new Point2f(x, y));
                }

                OpenCvSharp.Point[] sampledContour = ResampleContour(maxContour, sampleCount); // 아까 만든 함수


                double area = Cv2.ContourArea(maxContour);
                double perimeter = Cv2.ArcLength(maxContour, true);
                double circularity = 4 * Math.PI * area / (perimeter * perimeter);
                Console.WriteLine($"Dentest circularity: {circularity}");

                int drawRadius = 35;

                double dentSpec = Globalo.yamlManager.configData.CamSettings.DentLimit;
                //for (int i = 0; i < maxContour.Length; i += 17)
                for (int i = 0; i < sampleCount; i++)
                {
                    // 이상적인 원 점
                    var idealPt = idealCirclePoints[i];

                    // 실제 contour 점 (리샘플링된 거여야 함!)
                    var actualPt = sampledContour[i]; // <- maxContour가 아님

                    float dx = actualPt.X - maxCenter.X;
                    float dy = actualPt.Y - maxCenter.Y;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                    float deviation = dist - maxRadius;  // 중심 기준 거리 - 기준 반지름

                    //// 중심으로부터 거리 계산
                    //float dx = maxContour[i].X - maxCenter.X;
                    //float dy = maxContour[i].Y - maxCenter.Y;
                    //float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                    //Console.Write("[CenterFind] dist - radius: x = {0:0.00}\n", (dist - radius));
                    // 기준 반지름과 비교

                    if (Math.Abs(deviation) < dentSpec)//13.0) //if (Math.Abs(dist - maxRadius) <= 10.0)//10.0)
                    {
                        Cv2.Circle(colorView, new OpenCvSharp.Point(actualPt.X, actualPt.Y), 12, Scalar.Yellow, 3);
                        Rectangle m_clRect = new Rectangle((int)(actualPt.X - (10)), (int)(actualPt.Y - (10)), (int)(10 * 2), (int)(10 * 2));
                        if (bAutorun == false)
                        {
                            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                            //Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(actualPt.X - drawRadius / 2, actualPt.Y - drawRadius / 2, drawRadius, 1, System.Drawing.Drawing2D.DashStyle.Solid, Color.Yellow);
                        }
                        Globalo.visionManager.milLibrary.m_clMilDrawCross[index].AddList(actualPt.X, actualPt.Y, drawRadius, 1, Color.Yellow);
                    }
                    else
                    {
                        DentUnderCount++;
                        Cv2.Circle(colorView, new OpenCvSharp.Point(actualPt.X, actualPt.Y), 12, Scalar.Red, 3);
                        Rectangle m_clRect = new Rectangle((int)(actualPt.X - (10)), (int)(actualPt.Y - (10)), (int)(10 * 2), (int)(10 * 2));
                        if (bAutorun == false)
                        {
                            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Red, 1, System.Drawing.Drawing2D.DashStyle.Solid); 
                            //Globalo.visionManager.milLibrary.m_clMilDrawCircle[index].AddList(actualPt.X- drawRadius/2, actualPt.Y - drawRadius / 2, drawRadius, 1, System.Drawing.Drawing2D.DashStyle.Solid, Color.Red);
                        }
                        Globalo.visionManager.milLibrary.m_clMilDrawCross[index].AddList(actualPt.X, actualPt.Y, drawRadius, 1, Color.Red);
                    }
                }

                //ConePoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex] - 520);
                System.Drawing.Point clPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 450);// Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                str = $"Dent Test: {DentUnderCount} / {sampleCount}";

                if (bAutorun == false)
                {
                    if (DentUnderCount > 15)
                    {
                        //Globalo.visionManager.milLibrary.DrawOverlayText(index, clPoint, str, Color.Red, 20);
                        Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(clPoint, str, "나눔고딕", Color.Red, 13);
                    }
                    else
                    {
                        //Globalo.visionManager.milLibrary.DrawOverlayText(index, clPoint, str, Color.GreenYellow, 20);
                        Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(clPoint, str, "나눔고딕", Color.GreenYellow, 13);
                    }
                }
                    
                Console.WriteLine($"Dentest DentUnderCount: {DentUnderCount}");

                HousingPoints.Add(new OpenCvSharp.Point(DentUnderCount, sampleCount));
            }
            
            if (IMG_VIEW) 
            {
                Cv2.NamedWindow("Detected colorView ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorView ", colorView);
                Cv2.WaitKey(0);
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            // Result Display
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------



            //--------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            // 측정 시간 출력
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------
            long elapsedMs = Environment.TickCount - startTime;
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;
            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            if (bDentTest == false)
            {
                str = $"Out Test Time: {elapsedMs / 1000.0:F3}(s)";
            }
            else
            {
                str = $"Dent Test Time: {elapsedMs / 1000.0:F3}(s)";
            }
            
            
            Console.WriteLine(str);
            if (bAutorun == false)
            {
                System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint,  str, Color.Blue, 15);
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", Color.Blue, 13);
            }

            return HousingPoints;
        }

        private OpenCvSharp.Point[] ResampleContour(OpenCvSharp.Point[] contour, int targetCount)
        {
            // 누적 거리 구하기
            double[] distances = new double[contour.Length];
            distances[0] = 0;
            for (int i = 1; i < contour.Length; i++)
            {
                double dx = contour[i].X - contour[i - 1].X;
                double dy = contour[i].Y - contour[i - 1].Y;
                distances[i] = distances[i - 1] + Math.Sqrt(dx * dx + dy * dy);
            }

            double totalLength = distances[distances.Length - 1];
            double step = totalLength / targetCount;

            List<OpenCvSharp.Point> resampled = new List<OpenCvSharp.Point>();
            int currentIndex = 0;

            for (int i = 0; i < targetCount; i++)
            {
                double targetDist = i * step;

                // 해당 위치가 어디 구간에 속하는지 찾기
                while (currentIndex < distances.Length - 1 && distances[currentIndex + 1] < targetDist)
                {
                    currentIndex++;
                }

                // 선형 보간
                double ratio = (targetDist - distances[currentIndex]) /
                               (distances[currentIndex + 1] - distances[currentIndex]);

                int x = (int)(contour[currentIndex].X * (1 - ratio) + contour[currentIndex + 1].X * ratio);
                int y = (int)(contour[currentIndex].Y * (1 - ratio) + contour[currentIndex + 1].Y * ratio);
                resampled.Add(new OpenCvSharp.Point(x, y));
            }

            return resampled.ToArray();
        }
    }
}
