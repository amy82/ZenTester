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


namespace ZenHandler.VisionClass
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

            Globalo.visionManager.milLibrary.ClearOverlay(index);
            
            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;


            ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);



            OpenCvSharp.Point centerPos = Housing_Dent_Test(index , src); //Con1,2(동심도)  / Dent (찌그러짐) 검사 


            GasketTest(index, src, centerPos);     //가스켓 검사
            //
            Keytest(index, src, centerPos);        //키검사


            return rtn;
        }
        public bool FindPogoPinCenter(int index, Mat srcImage)
        {
            bool IMG_VIEW = false;
            int startTime = Environment.TickCount;

            //
            int radiusOuter = 250;      //이미지 중심에서 pogoPin을 찾을 원 크기

            OpenCvSharp.Point ImageCenter = new OpenCvSharp.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X / 2, Globalo.visionManager.milLibrary.CAM_SIZE_Y / 2);

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
            Globalo.visionManager.milLibrary.ClearOverlay(index);

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
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y -150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
            textPoint = new System.Drawing.Point(100, 100);
            str = $"Center : ({Circlecenter.X},{Circlecenter.Y})";
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 25);
            return pogoFindFlag;
        }
        public void Keytest(int index, Mat srcImage, OpenCvSharp.Point circle1)
        {
            bool IMG_VIEW = false;
            int startTime = Environment.TickCount;
            int radius = 700;

            Rect keyRoi = new Rect((int)circle1.X - 920, (int)circle1.Y + 160, 650, 650);
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
                        points.Add(new System.Drawing.Point((int)((p.X + circle1.X - 920) * Globalo.visionManager.milLibrary.xReduce), (int)((p.Y + circle1.Y + 200) * Globalo.visionManager.milLibrary.yReduce)));
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

            str = $"Test Time: {elapsedMs / 1000.0:F3} (s)";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
        }
        public void GasketTest(int index, Mat srcImage, OpenCvSharp.Point circle1)
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
            int radiusOuter = 700;
            int radiusInner = 410;
                

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
                


            Console.WriteLine($"가스켓 영역 평균 밝기: {avg.Val0:F2}");

            long elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;

            string str = "";
            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3} s";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
        }
        public OpenCvSharp.Point Housing_Dent_Test(int index, Mat srcImage)
        {
            // 측정 시작
            bool IMG_VIEW = false;
            //TeststopWatch.Start();
            int startTime = Environment.TickCount;
            Console.WriteLine($"Housing_Dent_Test Test Start");
            OpenCvSharp.Point centerPos = new OpenCvSharp.Point();
            //
            //
            //
            //
            //
            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            var edges = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(5, 5), 0);//1.2);
            //Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화
            int weakedge = 10;      //<-- 이값보다 작으면 무시
            int strongedge = 170;   //<---이값보다 크면 엣지 강화
            Cv2.Canny(blurred, edges, weakedge, strongedge);  // 윤곽 강화
            if (true)
            {
                Cv2.NamedWindow("Detected edges ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected edges ", edges);
                Cv2.WaitKey(0);
            }

            ///Cv2.EqualizeHist(srcImage, srcImage);
            int blockSize = 13;// 21; // 반드시 홀수
            int C = 13;

            int minThresh = 70;
            Cv2.AdaptiveThreshold(edges, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            //Cv2.Threshold(edges, binary, minThresh, 255, ThresholdTypes.Binary);     //

            // 2. 커널 생성 (원형 커널 추천)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);     //끊어졌거나 희미한 외곽선을 연결
            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(binary, binary, kernel);
            if (true)//IMG_VIEW)
            {
                //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 7));
                
                Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected binary ", binary);
                Cv2.WaitKey(0);
            }



            // 3. Contours 찾기
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 4. 이미지 중심 계산
            ///OpenCvSharp.Point center = new OpenCvSharp.Point(srcImage.Width / 2, srcImage.Height / 2);
            double minDist = double.MaxValue;
            OpenCvSharp.Point bestPoint = new OpenCvSharp.Point();
            OpenCvSharp.Point2d imageCenter = new OpenCvSharp.Point2d(srcImage.Width / 2, srcImage.Height / 2);

            // 3. 각 컨투어 중심을 평균
            List<Point2d> centers = new List<Point2d>();

            Mat colorView = new Mat();
            Cv2.CvtColor(srcImage, colorView, ColorConversionCodes.GRAY2BGR);

            List<(OpenCvSharp.Point2f center, float radius)> circles = new List<(Point2f center, float radius)>();

            foreach (var contour in contours)
            {
                Moments M = Cv2.Moments(contour);
                if (M.M00 == 0) continue;
                //centers.Add(new Point2d(M.M10 / M.M00, M.M01 / M.M00));

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
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);
                if (radius < 600)
                {
                    continue;
                }
                Console.Write($"[Housing] radius: {radius}, area: {area}, circularity: {circularity}\n");
                if (circularity < 0.01)
                {
                    continue;
                }
                
                if (radius > 760 && radius < 848)       //외경 in , out 안의 원 제거
                {
                    //continue;
                }
                circles.Add((center, radius));
                //circularity > 0.0 && 
                // if (((radius > 650 && radius < 801) || radius > 848)) // 원형이고 일정 크기 이상 && area > 1000 


                //Cv2.Circle(srcImage, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 3);
                // 컬러 이미지에 원을 그림
                //Cv2.Circle(colorView, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 5);
                
                if (radius > 800)
                {
                    //foreach (var point in contour)
                    for (int i = 0; i < contour.Length; i += 15)
                    {
                        // 중심으로부터 거리 계산
                        float dx = contour[i].X - center.X;
                        float dy = contour[i].Y - center.Y;
                        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                        //Console.Write("[CenterFind] dist - radius: x = {0:0.00}\n", (dist - radius));
                        // 기준 반지름과 비교
                        if (Math.Abs(dist - radius) <= 10.0) 
                        {
                            Cv2.Circle(colorView, new OpenCvSharp.Point(contour[i].X, contour[i].Y), 11, Scalar.Yellow, 3);
                            Rectangle m_clRect = new Rectangle((int)(contour[i].X - (10)), (int)(contour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                            if (IMG_VIEW)
                            {
                                //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                            }
                        }
                        else
                        {
                            Cv2.Circle(colorView, new OpenCvSharp.Point(contour[i].X, contour[i].Y), 11, Scalar.Red, 3);
                            Rectangle m_clRect = new Rectangle((int)(contour[i].X - (10)), (int)(contour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                            if (IMG_VIEW)
                            {
                                //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Red, 1, System.Drawing.Drawing2D.DashStyle.Solid); 
                            }
                        }
                    }
                }
                centers.Add(new Point2d(center.X, center.Y));
                //Console.Write("[CenterFind] measured circle: x = {0:0.00}, y = {1:0.00}, circularity = {2:0.00}, radius = {3:0.00}, area = {4:0.00}\n", center.X, center.Y, circularity, radius, area);
                
            }
            
            // 가장 작은 원과 큰 원 찾기
            if (circles.Count > 0)
            {
                var minCircle = circles.OrderBy(c => c.radius).First();
                var maxCircle = circles.OrderByDescending(c => c.radius).First();

                // 그리기 예시
                Cv2.Circle(colorView, (OpenCvSharp.Point)minCircle.center, (int)minCircle.radius, Scalar.Red, 2);   // 내경
                Cv2.Circle(colorView, (OpenCvSharp.Point)maxCircle.center, (int)maxCircle.radius, Scalar.Blue, 2);  // 외경
                System.Drawing.Point clPoint;

                //Rectangle m_clRect2 = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));
                clPoint = new System.Drawing.Point((int)(minCircle.center.X - minCircle.radius), (int)(minCircle.center.Y - minCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(minCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);

                clPoint = new System.Drawing.Point((int)(maxCircle.center.X - maxCircle.radius), (int)(maxCircle.center.Y - maxCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(maxCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            }

            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected colorView ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected colorView ", colorView);
                Cv2.WaitKey(0);
            }
            //// 평균 좌표 계산
            //if (centers.Count > 0)
            //{
            //    double sumX = centers.Sum(c => c.X);
            //    double sumY = centers.Sum(c => c.Y);
            //    Point2d avgCenter = new Point2d(sumX / centers.Count, sumY / centers.Count);
            //    centerPos.X = (int)avgCenter.X;
            //    centerPos.Y = (int)avgCenter.Y;

            //    Rectangle m_clRect = new Rectangle((int)(avgCenter.X - (50)), (int)(avgCenter.Y - (50)), (int)(50 * 2), (int)(50 * 2));

            //    Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);
            //}

            int centerX = srcImage.Cols / 2;
            int centerY = srcImage.Rows / 2;
            int roiSize = 150;
            // ROI 영역 지정 (x, y, width, height)
            Rect roi = new Rect(centerX - roiSize / 2, centerY - roiSize / 2, roiSize, roiSize);
            Mat roiMat = new Mat(srcImage, roi);

            // 평균 밝기 계산
            Scalar mean = Cv2.Mean(roiMat);

            // 결과 출력
            Console.WriteLine($"[CenterFind] 중앙 영역 평균 밝기: {mean.Val0:F2}");

            // 측정 종료
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

            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X - 820, Globalo.visionManager.milLibrary.CAM_SIZE_Y - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint,  str, Color.Blue, 15);

            return centerPos;
        }
    }
}
