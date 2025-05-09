using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenHandler.VisionClass
{
    public class OpencvAoiTest
    {
        public OpencvAoiTest()
        {

        }
        public void MilPopup(int index)
        {
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID MilDisplayNew = MIL.M_NULL;
            // Allocate defaults.
            //MIL.MappAllocDefault(MIL.M_DEFAULT, ref MilApplication, ref MilSystem, ref MilDisplay, MIL.M_NULL, MIL.M_NULL);
            string CONTOUR_IMAGE = MIL.M_IMAGE_PATH + "Seals.mim";
            // Restore the image and display it.
            //MIL.MbufRestore(CONTOUR_IMAGE, Globalo.visionManager.milLibrary.MilSystem, ref MilImage);

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, 1000, 1000,(8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_DISP + MIL.M_PROC, ref MilImage);
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], MilImage);


            // 기존 MilSystem을 사용
            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MdispSelect(MilDisplay, MilImage);
        }
        

        public bool OpencvKeyCheck(int index)
        {
            bool rtn = false;
            double minArea = 45.0;
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            OpenCvSharp.Point centerPos = new OpenCvSharp.Point();
            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;
            byte[] buffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], buffer);
            Mat gray = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(buffer, 0, gray.Data, dataSize);

            Rect keyRoi = new Rect((int)2190 + 250, (int)1429 + 250, 650, 650);
            Mat roiKeyMat = gray[keyRoi];
            


            int SizeX = roiKeyMat.Width;
            int SizeY = roiKeyMat.Height;

            // 블러 후 이진화
            var blurred = new Mat();
            Cv2.GaussianBlur(roiKeyMat, blurred, new OpenCvSharp.Size(1, 1), 0);
            var binary = new Mat();
            //Cv2.Threshold(blurred, binary, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
            int blockSize = 13;// 21; // 반드시 홀수
            int C = 19;
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (mean.Val0 / 2.5), 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (255 - mean.Val0), 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv
            Cv2.AdaptiveThreshold(roiKeyMat, binary, 200, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blockSize, C);

            // 윤곽선 찾기
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            int i = 0;
            
            for (i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                double perimeter = Cv2.ArcLength(contours[i], true);

                if (hierarchy[i].Parent != -1 && area > 10 && area < 1000) // 조건: 면적이 100 이상
                {
                    Cv2.DrawContours(roiKeyMat, contours, i, new Scalar(0, 255, 255), 2);

                    // 윤곽선 그리기 (노란색)

                    Console.WriteLine($"Contour with area {area} detected.");
                    List<System.Drawing.Point> points = new List<System.Drawing.Point>();
                    // contour는 List<Point> 형식이므로, 이를 Graphics.DrawPolygon에 맞게 사용
                    //points = contours[i].Select(p => new System.Drawing.Point(p.X, p.Y)).ToList();
                    foreach (var p in contours[i])
                    {
                        points.Add(new System.Drawing.Point((int)((p.X + 2190 + 250) * Globalo.visionManager.milLibrary.xReduce) , (int)((p.Y + 1429 + 250) * Globalo.visionManager.milLibrary.yReduce)));
                    }
                    Globalo.visionManager.milLibrary.DrawOverlayPolygon(index, points, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                }
            }

            //
            Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected binary ", roiKeyMat);
            Cv2.WaitKey(0);
            
            return rtn;
        }
        public bool MilKeyCheck(Mat roiImage)
        {
            bool rtn = false;
            //Mat gray = new Mat();
            //Cv2.CvtColor(roiImage, gray, ColorConversionCodes.BGR2GRAY);
            //Cv2.GaussianBlur(roiImage, roiImage, new OpenCvSharp.Size(5, 5), 1.5);

            //Mat roiEdges = new Mat();
            //Cv2.Canny(roiImage, roiEdges, 200, 50); // 임계값 조절 가능
            //Cv2.NamedWindow("Detected roiEdges ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            // Cv2.ImShow("Detected roiEdges ", roiEdges);
            //Cv2.WaitKey(0);
            // MIL.Mbuf
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID MilEdgeContext = MIL.M_NULL;
            MIL_ID MilEdgeResult = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilDisplay = MIL.M_NULL;

            MIL_INT NumEdgeFound = 0;
            MIL_INT NumResults = 0;
            int CONTOUR_MAX_RESULTS = 100;
            double[] MeanFeretDiameter = new double[CONTOUR_MAX_RESULTS];
            // Allocate a graphic list to hold the subpixel annotations to draw.

            // Associate the graphic list to the display for annotations.
            //MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);
            // Allocate a graphic list to hold the subpixel annotations to draw.
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);

            // Associate the graphic list to the display for annotations.
            //MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);

            int SizeX = roiImage.Width;
            int SizeY = roiImage.Height;

            // Allocate defaults.
            //MIL.MappAllocDefault(MIL.M_DEFAULT, ref Globalo.visionManager.milLibrary.MilApplication, ref Globalo.visionManager.milLibrary.MilSystem, ref MilDisplay, MIL.M_NULL, MIL.M_NULL);
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, SizeX, SizeY, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_DISP + MIL.M_PROC, ref MilImage);

            if (MilImage == MIL.M_NULL)
                throw new Exception("MIL 버퍼 생성 실패");

            if (!roiImage.IsContinuous())
            {
                roiImage = roiImage.Clone();  // 메모리 정렬 보장
            }
            int kk = roiImage.Channels();
            byte[] bytes = new byte[SizeY * SizeX];
            Console.WriteLine($"Mat Type: {roiImage.Type()}");


            Marshal.Copy(roiImage.Data, bytes, 0, bytes.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(MilImage, bytes);
            MIL.MbufExport("D:\\rawMilImage.BMP", MIL.M_BMP, MilImage);
            // 5. 디스플레이 윈도우에 이미지 연결
            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);
            MIL.MdispSelect(MilDisplay, MilImage);

            
            // Enable features to compute.
            //MIL.MedgeControl(MilEdgeContext, MIL.M_MOMENT_ELONGATION, MIL.M_ENABLE);
            //MIL.MedgeControl(MilEdgeContext, MIL.M_FERET_MEAN_DIAMETER + MIL.M_SORT1_DOWN, MIL.M_ENABLE);

            // Allocate a Edge Finder context.
            MIL.MedgeAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_CONTOUR, MIL.M_DEFAULT, ref MilEdgeContext);

            // Allocate a result buffer.
            MIL.MedgeAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilEdgeResult);

            MIL.MedgeControl(MilEdgeResult, MIL.M_FILTER_TYPE, MIL.M_SHEN);
            MIL.MedgeControl(MilEdgeResult, MIL.M_ACCURACY, MIL.M_HIGH);
            MIL.MedgeControl(MilEdgeResult, MIL.M_ANGLE, MIL.M_HIGH);
            MIL.MedgeControl(MilEdgeResult, MIL.M_MAGNITUDE_TYPE, MIL.M_SQR_NORM);
            MIL.MedgeControl(MilEdgeResult, MIL.M_THRESHOLD_MODE, MIL.M_HIGH); 
            MIL.MedgeControl(MilEdgeResult, MIL.M_THRESHOLD_TYPE, MIL.M_HYSTERESIS);
            MIL.MedgeControl(MilEdgeResult, MIL.M_SMOOTH, 90.0);

            // Calculate edges and features.
            MIL.MedgeCalculate(MilEdgeContext, MilImage, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MilEdgeResult, MIL.M_DEFAULT);

            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumEdgeFound);

            // Draw edges in the source image to show the result.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, MilImage, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            //
            double CONTOUR_MAXIMUM_ELONGATION = 0.8;
            // Exclude elongated edges.
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_MOMENT_ELONGATION, MIL.M_LESS, CONTOUR_MAXIMUM_ELONGATION, MIL.M_NULL);

            // Exclude inner chains.
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_INCLUDED_EDGES, MIL.M_INSIDE_BOX, MIL.M_NULL, MIL.M_NULL);

            // Draw remaining edges and their index to show the result.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmodControl(MilEdgeResult, MIL.M_DEFAULT, 3203L, Globalo.visionManager.milLibrary.xReduce);//M_DRAW_SCALE_X
            MIL.MmodControl(MilEdgeResult, MIL.M_DEFAULT, 3204L, Globalo.visionManager.milLibrary.yReduce);//M_DRAW_SCALE_Y
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, MilImage, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumResults);

            // If the right number of edges were found.
            if ((NumResults >= 1) && (NumResults <= CONTOUR_MAX_RESULTS))
            {
                // Draw the index of each edge.
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_INDEX, MIL.M_DEFAULT, MIL.M_DEFAULT);
                MIL.MmodControl(MilEdgeResult, MIL.M_DEFAULT, 3203L, Globalo.visionManager.milLibrary.xReduce);//M_DRAW_SCALE_X
                MIL.MmodControl(MilEdgeResult, MIL.M_DEFAULT, 3204L, Globalo.visionManager.milLibrary.yReduce);//M_DRAW_SCALE_Y
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, MilImage, MIL.M_DRAW_INDEX, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Get the mean Feret diameters.
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_FERET_MEAN_DIAMETER, MeanFeretDiameter);

            }
            //
            MIL.MbufExport("D:\\MilImage.BMP", MIL.M_BMP, MilImage);

            /* Free all allocations. */
            //MIL.MblobFree(MilBlobResult);
            //MIL.MblobFree(MilBlobFeatureList);
            //MIL.MbufFree(MilImage);

            
            return rtn;
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
            Console.WriteLine($"[KeyFind] 중앙 영역 평균 밝기: {mean.Val0:F2} / {255-mean.Val0:F2}");

            Rectangle m_clRect = new Rectangle(roi.X , roi.Y , roi.Width , roi.Height);
            Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);


            int blockSize = 31;// 21; // 반드시 홀수
            int C = 3;
            Mat binary = new Mat();
            //Cv2.Threshold(blurred, binary, 130, 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (mean.Val0 / 2.5), 255, ThresholdTypes.Binary); // 배경 밝기에 따라 Binary 또는 BinaryInv
            //Cv2.Threshold(blurred, binary, (255 - mean.Val0), 255, ThresholdTypes.BinaryInv); // 배경 밝기에 따라 Binary 또는 BinaryInv
            Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
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

            OpenCvSharp.Point Circlecenter = new OpenCvSharp.Point();
            int radiusInner = 30;  // 작은 원 반지름
            int radiusOuter = 60;  // 큰 원 반지름
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

                double dx = centerX - center.X;
                double dy = centerY - center.Y;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (circularity > 0.01 && radius > 200 && area >100 && radius < 1200 && distance < 500) // 원형이고 일정 크기 이상
                {
                    

                    Cv2.Circle(result, (OpenCvSharp.Point)center, (int)radius, Scalar.Blue, 2);

                    i++;
                    Console.Write("[{0:0}] measured circle: x = {1:0.00}, y = {2:0.00}, radius = {3:0.00}\n", i, center.X, center.Y, radius);

                    //Rectangle m_clRect = new Rectangle((int)(center.X - (radius)), (int)(center.Y - (radius)), (int)(radius * 2), (int)(radius * 2));

                    if (contour.Length >= 5)
                    {
                        RotatedRect ellipse = Cv2.FitEllipse(contour);
                        double axisRatio = Math.Min(ellipse.Size.Width, ellipse.Size.Height) / Math.Max(ellipse.Size.Width, ellipse.Size.Height);

                        System.Drawing.Point clPoint = new System.Drawing.Point((int)(centerX - radius), (int)(centerY - radius));

                        Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)radius * 2, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);

                        Console.WriteLine($"Axis ratio (타원 비율): {axisRatio:F2} {circularity}");
                    }
                    if (i == 1)
                    {
                        Circlecenter = new OpenCvSharp.Point(center.X, center.Y);
                        radiusInner = (int)radius;
                    }
                    if (radius > 800)
                    {
                        radiusOuter = (int)radius;
                    }
                }
            }

            //가스켓 밝기 계산
            // 이미지 크기에 맞는 빈 마스크
            Mat mask = Mat.Zeros(binary.Size(), MatType.CV_8UC1);

            // 바깥쪽 원 그리기 (하얀색)
            Cv2.Circle(mask, Circlecenter, radiusOuter, Scalar.White, -1);

            // 안쪽 원을 검정으로 덮기 (원 내부 제거)
            Cv2.Circle(mask, Circlecenter, radiusInner, Scalar.Black, -1);

            // 원과 원 사이의 영역만 남긴 결과
            Mat ringRegion = new Mat();
            binary.CopyTo(ringRegion, mask);  // src는 그레이스케일 이미지

            // 평균 밝기 계산
            Scalar avg = Cv2.Mean(binary, mask);  // mask로 지정된 영역만

            Console.WriteLine($"가스켓 영역 평균 밝기: {avg.Val0:F2}");

            // 평균 밝기를 전체 마스크 영역에 적용해서 결과 이미지로 보기
            Mat avgImg = Mat.Zeros(binary.Size(), MatType.CV_8UC1);
            avgImg.SetTo(new Scalar(avg.Val0), mask);  // 도넛 영역만 밝기값으로 채움

            Cv2.NamedWindow("Detected avgImg", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected avgImg", avgImg);
            Cv2.WaitKey(0);


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
                param1: 10,       // Canny 상한값
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
