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
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
            textPoint = new System.Drawing.Point(100, 100);
            str = $"Center : ({Circlecenter.X},{Circlecenter.Y})";
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 25);
            return pogoFindFlag;
        }
        public bool MilEdgeKeytest(int index, int roiIndex, string keyType)
        {
            int startTime = Environment.TickCount;
            bool bRtn = true;

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


            int OffsetX = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].X;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            //MIL_INT ImageSizeX = MIL.MbufInquire(Globalo.visionManager.milLibrary.MilProcImageChild[index], MIL.M_SIZE_X, MIL.M_NULL);
            //MIL_INT ImageSizeY = MIL.MbufInquire(Globalo.visionManager.milLibrary.MilProcImageChild[index], MIL.M_SIZE_Y, MIL.M_NULL);
            
            //MIL.MgraArcFill(MIL.M_DEFAULT, Globalo.visionManager.milLibrary.MilProcImageChild[index], ImageSizeX/2, ImageSizeY/2, 1200, 800, 0, 360);


            //MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref tempMilImage);
            //MIL.MbufChild2d(tempMilImage, OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);


            MIL.MgraColor(MIL.M_DEFAULT, 50);
            if (keyType == "A")
            {
                if (roiIndex == 0)
                {
                    //A타입의 왼쪽 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);
                }
                else
                {
                    //A타입의 우측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }
            if (keyType == "B")
            {
                if (roiIndex == 0)
                {
                    //좌측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);
                }
                else
                {
                    //우측 아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
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
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }
            if (keyType == "D")
            {
                if (roiIndex == 0) 
                {
                    //우측 위
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 1, OffsetHeight - 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 0, 90);
                }
                else
                {
                    //우측아래
                    MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 270, 360);
                }
            }

            if (keyType == "E")
            {
                //Key 1개
                MIL.MgraArcFill(MIL.M_DEFAULT, tempMilImage, OffsetWidth - 1, 1, OffsetWidth / 1.6, OffsetWidth / 1.5, 180, 270);//E타입의 왼쪽 아래 (1개)
            }
            
            
            MIL.MbufExport($"d:\\OrgKey{roiIndex}.BMP", MIL.M_BMP, tempMilImage);

            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL); 
            MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_FIXED + MIL.M_GREATER, 50, MIL.M_NULL);
            MIL.MbufExport($"d:\\Key{roiIndex}.BMP", MIL.M_BMP, tempMilImage);


            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);
            MilImage = tempMilImage;
            MIL.MdispSelect(MilDisplay, MilImage);

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
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_LESS, 5.0, MIL.M_NULL);
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_GREATER, 30.0, MIL.M_NULL);

            // Draw edges in the source image to show the result.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MedgeControl(MilEdgeResult, 319L, (double)-OffsetX);
            MIL.MedgeControl(MilEdgeResult, 320L, (double)-OffsetY);


            MIL.MedgeControl(MilEdgeResult, 3203L, Globalo.visionManager.milLibrary.xReduce[index]);
            MIL.MedgeControl(MilEdgeResult, 3204L, Globalo.visionManager.milLibrary.yReduce[index]);

            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, Globalo.visionManager.milLibrary.MilSetCamOverlay, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            //M_DRAW_BOX + M_DRAW_POSITION + M_DRAW_EDGES + M_DRAW_AXIS

            MIL_INT NumResults = 0;                                         // Number of results found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumResults);

            int minIndex = 0;
            int maxIndex = 0;
            double CircleCx = 0.0;
            double CircleErr = 0.0;
            string str = "";
            Rectangle m_clRect = new Rectangle((int)(OffsetX), (int)(OffsetY), OffsetWidth, OffsetHeight);
            // If the right number of edges were found.
            int KeyEdgeSpec = 30;
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
                

                Console.WriteLine($"M_CIRCLE_FIT_CENTER_X : {EdgeCircleFitCx[0]}");
                Console.WriteLine($"M_CIRCLE_FIT_ERROR : {EdgeCircleFitErr[0]}");
                Console.WriteLine($"M_CONVEX_PERIMETER : {EdgeConvex[0]}");
                Console.WriteLine($"M_FAST_LENGTH : {EdgeFastLength[0]}");
                Console.WriteLine($"M_LENGTH : {EdgeLength[0]}");
                Console.WriteLine($"M_POSITION_Y : {EdgePositionY[0]}");
                Console.WriteLine($"M_SIZE : {EdgeSize[0]}");
                Console.WriteLine($"M_STRENGTH : {EdgeStrength[0]}");



                CircleCx = EdgeCircleFitCx[0];
                CircleErr = EdgeCircleFitErr[0];
                str = $"Key{roiIndex + 1}";
                textPoint = new System.Drawing.Point(m_clRect.X, m_clRect.Y);
                if (NumResults < KeyEdgeSpec)
                {
                    //오링 없음
                    bRtn = false;
                    Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Red, 2);
                    

                    
                    Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
                } 
                else
                {
                    bRtn = true;
                    Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Yellow, 2);
                    Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
                }
                
                



                //System.Drawing.Point textPoint;

                //string str = $"CircleCx :{CircleCx.ToString("0.000")} / {circleSpec}";

                //textPoint = new System.Drawing.Point(10, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);


                

                str = $"[CONE] Edge Count:{NumResults}/{KeyEdgeSpec}";

                textPoint = new System.Drawing.Point(10, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 17);

                if (bRtn)
                {
                    str = $"{keyType} Key Detected!";
                    ConeColor = Color.Green;
                }
                else
                {
                    str = $"{keyType} Key Not Detected!";
                    ConeColor = Color.Red;
                }


                int leng = str.Length;
                textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / (leng - 10)), 250);

                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, ConeColor, 50);
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");


                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Red, 2);
                bRtn = false;
            }




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

            System.Drawing.Point timetextPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, timetextPoint, str, Color.Blue, 15);
            return bRtn;
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
        public void GasketTest(int index, Mat srcImage, OpenCvSharp.Point circle1)
        {
            //가스켓 밝기 계산
            bool IMG_VIEW = true;
            int startTime = Environment.TickCount;

            //Mat binary = new Mat();

            int blockSize = 13; // 반드시 홀수
            int C = 13;

            //Cv2.AdaptiveThreshold(srcImage, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            //Cv2.NamedWindow("GasketTest binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("GasketTest binary ", binary);
            //Cv2.WaitKey(0);
            int radiusOuter = 700;
            int radiusInner = 430;
                

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

            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 800, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);
        }
        //public OpenCvSharp.Point Housing_Fakra_Test(int index, Mat srcImage)
        public List<OpenCvSharp.Point> Housing_Fakra_Test(int index, Mat srcImage)
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
            //
            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            var edges = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(5, 5), 0.7);
            //Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화

            int weakedge = 65;//40;      //<-- 이값보다 작으면 무시
            int strongedge = 170;// 150;   //<---이값보다 크면 엣지 강화

            Cv2.Canny(blurred, edges, weakedge, strongedge);  // 윤곽 강화
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected srcImage ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected srcImage ", srcImage);
                Cv2.WaitKey(0);
            }

            ///Cv2.EqualizeHist(srcImage, srcImage);
            int blockSize = 51;// 19; // 반드시 홀수
            int C = 15;

            //int minThresh = 70;
            //Cv2.Threshold(edges, binary, minThresh, 255, ThresholdTypes.Binary);     //
            //Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            Cv2.AdaptiveThreshold(srcImage, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);

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
            int imageCenterX = binary.Width / 2;
            int imageCenterY = binary.Height / 2;
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
                if (distance > 200)
                {
                    Console.WriteLine($"del distance:{distance}");
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
                    

                if (radius < 300 || radius > 600)
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
            // Result Display
            //
            //
            //--------------------------------------------------------------------------------------------------------------------------------------------

            double CamResolX = 0.0;
            double CamResolY = 0.0;
            CamResolX = Globalo.yamlManager.aoiRoiConfig.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.aoiRoiConfig.TopResolution.Y;   //0.0186f;
            if (circles.Count > 0)
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
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(minCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);

                clPoint = new System.Drawing.Point((int)(maxCircle.center.X - maxCircle.radius), (int)(maxCircle.center.Y - maxCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(maxCircle.radius * 2), Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);

                System.Drawing.Point HousingPoint = new System.Drawing.Point();


                HousingPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 460);
                str = $"Fakra In  X:{(minCircle.center.X * CamResolX).ToString("0.00#")},Y:{(minCircle.center.Y * CamResolY).ToString("0.00#")},R:{(minCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);

                HousingPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 390);
                str = $"Fakra Out X:{(maxCircle.center.X * CamResolX).ToString("0.00#")},Y:{(maxCircle.center.Y * CamResolY).ToString("0.00#")},R:{(minCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);


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

            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
            Console.WriteLine(str);
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 920, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);

            return FakraPoints;
        }
        public List<OpenCvSharp.Point> Housing_Dent_Test(int index, Mat srcImage)
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
            //
            //
            //
            // 2. Threshold (밝은 점을 강조)
            Mat binary = new Mat();
            var blurred = new Mat();
            var edges = new Mat();
            Cv2.GaussianBlur(srcImage, blurred, new OpenCvSharp.Size(3, 3), 0.7);
            //Cv2.Canny(blurred, edges, 190, 75);  // 윤곽 강화

            int weakedge = 65;//40;      //<-- 이값보다 작으면 무시
            int strongedge = 170;// 150;   //<---이값보다 크면 엣지 강화

            Cv2.Canny(blurred, edges, weakedge, strongedge);  // 윤곽 강화
            if (IMG_VIEW)
            {
                Cv2.NamedWindow("Detected edges ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
                Cv2.ImShow("Detected edges ", edges);
                Cv2.WaitKey(0);
            }

            ///Cv2.EqualizeHist(srcImage, srcImage);
            int blockSize = 51;// 19; // 반드시 홀수
            int C = 7;

            //int minThresh = 70;
            //Cv2.Threshold(edges, binary, minThresh, 255, ThresholdTypes.Binary);     //
            //Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);
            Cv2.AdaptiveThreshold(srcImage, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, blockSize, C);

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
            int imageCenterX = binary.Width / 2;
            int imageCenterY = binary.Height / 2;
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
                if (distance > 200)
                {
                    Console.WriteLine($"del distance:{distance}");
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

                
                if (radius < 600 || radius > 890)
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
            CamResolX = Globalo.yamlManager.aoiRoiConfig.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.aoiRoiConfig.TopResolution.Y;   //0.0186f;
            if (circles.Count > 0)
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
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(minCircle.radius * 2), Color.Green, 3, System.Drawing.Drawing2D.DashStyle.Solid);

                clPoint = new System.Drawing.Point((int)(maxCircle.center.X - maxCircle.radius), (int)(maxCircle.center.Y - maxCircle.radius));
                Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)(maxCircle.radius * 2), Color.Green, 3, System.Drawing.Drawing2D.DashStyle.Solid);

                System.Drawing.Point HousingPoint = new System.Drawing.Point();


                HousingPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 320);
                str = $"Housing In  X:{(minCircle.center.X * CamResolX).ToString("0.00#")},Y:{(minCircle.center.Y * CamResolY).ToString("0.00#")},R:{(minCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);

                HousingPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                str = $"Housing Out X:{(maxCircle.center.X * CamResolX).ToString("0.00#")},Y:{(maxCircle.center.Y * CamResolY).ToString("0.00#")},R:{(maxCircle.radius * CamResolX).ToString("0.00#")}";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, HousingPoint, str, Color.GreenYellow, 13);


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
            
            if (maxRadius > 1)
            {
                for (int i = 0; i < maxContour.Length; i += 10)
                {
                    // 중심으로부터 거리 계산
                    float dx = maxContour[i].X - maxCenter.X;
                    float dy = maxContour[i].Y - maxCenter.Y;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                    //Console.Write("[CenterFind] dist - radius: x = {0:0.00}\n", (dist - radius));
                    // 기준 반지름과 비교
                   
                    if (Math.Abs(dist - maxRadius) <= 10.0)
                    {
                        Cv2.Circle(colorView, new OpenCvSharp.Point(maxContour[i].X, maxContour[i].Y), 11, Scalar.Yellow, 3);
                        Rectangle m_clRect = new Rectangle((int)(maxContour[i].X - (10)), (int)(maxContour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                        if (IMG_VIEW)
                        {
                            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
                        }
                    }
                    else
                    {
                        Cv2.Circle(colorView, new OpenCvSharp.Point(maxContour[i].X, maxContour[i].Y), 11, Scalar.Red, 3);
                        Rectangle m_clRect = new Rectangle((int)(maxContour[i].X - (10)), (int)(maxContour[i].Y - (10)), (int)(10 * 2), (int)(10 * 2));
                        if (IMG_VIEW)
                        {
                            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Red, 1, System.Drawing.Drawing2D.DashStyle.Solid); 
                        }
                    }
                }
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

            ////int centerX = srcImage.Cols / 2;
            ////int centerY = srcImage.Rows / 2;
            ////int roiSize = 150;
            ////// ROI 영역 지정 (x, y, width, height)
            ////Rect roi = new Rect(centerX - roiSize / 2, centerY - roiSize / 2, roiSize, roiSize);
            ////Mat roiMat = new Mat(srcImage, roi);

            ////// 평균 밝기 계산
            ////Scalar mean = Cv2.Mean(roiMat);

            ////// 결과 출력
            ////Console.WriteLine($"[CenterFind] 중앙 영역 평균 밝기: {mean.Val0:F2}");

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
            str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
            Console.WriteLine(str);
            System.Drawing.Point textPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 920, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint,  str, Color.Blue, 15);

            return HousingPoints;
        }
    }
}
