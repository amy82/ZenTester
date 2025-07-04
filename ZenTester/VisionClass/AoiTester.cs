using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;

namespace ZenTester.VisionClass
{
    public class AoiTester
    {
        public readonly MIL_INT MODEL_MAX_OCCURRENCES = 23L;
        public AoiTester()
        {

        }

        public void FinalLogSave(TcpSocket.AoiApdData finalData)
        {
            string FinalLogPath = Data.CPath.BASE_TP_PATH;
            string MiddleLogPath = "AoiResult";
            string currentDate = DateTime.Now.ToString("yyyy_MM_dd");
            string FinalLogName = $"AoiFinalLog_{currentDate}.csv";

            string filePath = Path.Combine(FinalLogPath, MiddleLogPath, FinalLogName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string[] logTitle = { "Barcode", "LH", "MH", "RH", "Gasket", "KeyType", "CircleDented", "Concentrycity_A", 
                "Concentrycity_D", "Cone", "ORing", "SocketNumber", "Result" };

            // 값들을 배열로 저장
            string[] logValues = {
                finalData.Barcode.ToString(),
                finalData.LH.ToString(),
                finalData.MH.ToString(),
                finalData.RH.ToString(),
                finalData.Gasket.ToString(),
                finalData.KeyType.ToString(),
                finalData.CircleDented.ToString(),
                finalData.Concentrycity_A.ToString(),
                finalData.Concentrycity_D.ToString(),
                finalData.Cone.ToString(),
                finalData.ORing.ToString(),
                finalData.Socket_Num.ToString(),
                finalData.Result.ToString()
            };
            // 파일이 없으면 헤더 추가
            if (!File.Exists(filePath))
            {
                string header = string.Join(",", logTitle);
                File.AppendAllText(filePath, header + Environment.NewLine);
            }
            try
            {
                // CSV 라인 생성
                string csvLine = string.Join(",", logValues);

                // 파일에 추가
                File.AppendAllText(filePath, csvLine + Environment.NewLine);
            }
            catch (IOException)
            {

            }

        }


        public void CirCleFind(int index)
        {
            Console.WriteLine("CirCleFind");
            Rectangle m_clRectCalRoi = new Rectangle((int)(100), (int)(100), 1000, 1000);

            //BINARY START
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index], ref MilSubImage01);
            MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], MilSubImage01, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            MIL.MimRank(MilSubImage01, MilSubImage01, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);
            //BINARY END

            MIL.MbufExport("D:\\TEST.BMP", MIL.M_BMP, MilSubImage01);
        }
        public void RunMeasCase(int index)
        {
            MIL_ID MilImage = MIL.M_NULL;
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC, ref MilImage);
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], MilImage);
            
            // Allocate a marker and set its region.
            MIL_ID MilMeasMarker = MIL.MmeasAllocMarker(Globalo.visionManager.milLibrary.MilSystem, MIL.M_CIRCLE, MIL.M_DEFAULT, MIL.M_NULL);
            int[] CIRCLE_REGION = { 1907, 1369, 10, 1000, 360 }; //CenterX, CenterY, InnerRadius, OuterRadius, ChordAngle
            MIL.MmeasSetMarker(MilMeasMarker, MIL.M_RING_CENTER, CIRCLE_REGION[0], CIRCLE_REGION[1]);
            MIL.MmeasSetMarker(MilMeasMarker, MIL.M_RING_RADII, CIRCLE_REGION[2], CIRCLE_REGION[3]);
            MIL.MmeasSetMarker(MilMeasMarker, MIL.M_SUB_REGIONS_CHORD_ANGLE, CIRCLE_REGION[4], MIL.M_NULL);
            MIL.MmeasSetMarker(MilMeasMarker, MIL.M_SEARCH_REGION_INPUT_UNITS, MIL.M_WORLD, MIL.M_NULL);

            // Set up the marker.
            MIL.MmeasSetMarker(MilMeasMarker, MIL.M_CIRCLE_ACCURACY, MIL.M_LOW, MIL.M_NULL);
            MIL.MmeasSetScore(MilMeasMarker, MIL.M_STRENGTH_SCORE, 0, 0, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);
            MIL.MmeasSetScore(MilMeasMarker, MIL.M_RADIUS_SCORE, 0, 0, 0, MIL.M_MAX_POSSIBLE_VALUE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);


            //MIL.McalUniform(MilImage, 0.0, 0.0, 1.0, 1.0, 0.0, MIL.M_DEFAULT);

            // Set up the drawing.
            MIL.MgraControl(MIL.M_DEFAULT, MIL.M_INPUT_UNITS, MIL.M_WORLD);

           


            // Find the marker.
            MIL.MmeasFindMarker(MIL.M_DEFAULT, MilImage, MilMeasMarker, MIL.M_DEFAULT);

            // Check the status.
            MIL_INT ValidFlag = MIL.M_NULL;
            MIL.MmeasGetResult(MilMeasMarker, MIL.M_VALID_FLAG + MIL.M_TYPE_MIL_INT, ref ValidFlag, MIL.M_NULL);
            if (ValidFlag == MIL.M_TRUE)
            {
                // Allocate a child of the image to avoid modifying the calibration of the original image.
                MIL_ID MilImageChild = MIL.MbufChildColor(MilImage, 0, MIL.M_NULL);

                // Retrieve information about the found search region box.
                double[] Corners = new double[8];
                double FoundBoxAngle = 0.0;
                MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_ANGLE_FOUND, ref FoundBoxAngle, MIL.M_NULL);
                if (MIL.MmeasInquire(MilMeasMarker, MIL.M_ORIENTATION, MIL.M_NULL, MIL.M_NULL) == MIL.M_VERTICAL)
                {
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_TOP_LEFT, ref Corners[0], ref Corners[1]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_TOP_RIGHT, ref Corners[2], ref Corners[3]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_BOTTOM_RIGHT, ref Corners[4], ref Corners[5]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_BOTTOM_LEFT, ref Corners[6], ref Corners[7]);
                }
                else
                {
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_TOP_LEFT, ref Corners[6], ref Corners[7]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_TOP_RIGHT, ref Corners[0], ref Corners[1]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_BOTTOM_RIGHT, ref Corners[2], ref Corners[3]);
                    MIL.MmeasGetResult(MilMeasMarker, MIL.M_BOX_CORNER_BOTTOM_LEFT, ref Corners[4], ref Corners[5]);
                    FoundBoxAngle -= 90;
                }
            }
        }
        public void ChangeBinary1(int index)
        {
            Globalo.visionManager.milLibrary.bGrabOnFlag[index] = false;

            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC, ref MilSubImage01);
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], MilSubImage01);
            //MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref MilSubImage01);
            MIL.MimBinarize(MilSubImage01, MilSubImage01, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            MIL.MimRank(MilSubImage01, MilSubImage01, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY); 
            
            //BINARY END

            // Display the image buffer. 
            //MIL.MdispSelect(MilDisplay, MilSubImage01);

            string path = $"D:\\ChangeBinary1_{index}.BMP";
            MIL.MbufExport(path, MIL.M_BMP, MilSubImage01);


            Globalo.visionManager.milLibrary.bGrabOnFlag[index] = true;
        }
        public void MmetTest(int index)
        {
            // Region parameters
            const int BOTTOM_RECT_POSITION_X = 320; 
            const int BOTTOM_RECT_POSITION_Y = 265;
            const int BOTTOM_RECT_WIDTH = 170;
            const int BOTTOM_RECT_HEIGHT = 20;
            const int BOTTOM_RECT_ANGLE = 180;

            // Tolerance parameters
            const double PERPENDICULARITY_MIN = 0.5;
            const double PERPENDICULARITY_MAX = 0.5;

            // Color definitions
            double FAIL_COLOR = MIL.M_RGB888(255, 0, 0);
            double PASS_COLOR = MIL.M_RGB888(0, 255, 0);
            double REGION_COLOR = MIL.M_RGB888(0, 100, 255);
            double FEATURE_COLOR = MIL.M_RGB888(255, 0, 255);

            MIL_ID MilImage = MIL.M_NULL;                    // Image buffer identifier.
            MIL_ID GraphicList = MIL.M_NULL;                 // Graphic list identifier.
            MIL_ID MilMetrolContext = MIL.M_NULL;            // Metrology Context
            MIL_ID MilMetrolResult = MIL.M_NULL;             // Metrology Result

            double Status = 0.0;
            double Value = 0.0;
            MIL_INT FeatureIndexForTopConstructedPoint = MIL.M_FEATURE_INDEX(1);
            MIL_INT FeatureIndexForMiddleConstructedPoint = MIL.M_FEATURE_INDEX(2);
            MIL_INT[] FeatureIndexForConstructedSegment = new MIL_INT[2];
            MIL_INT[] FeatureIndexForTolerance = new MIL_INT[2];
            FeatureIndexForConstructedSegment[0] = MIL.M_FEATURE_INDEX(3);
            FeatureIndexForConstructedSegment[1] = MIL.M_FEATURE_INDEX(4);
            FeatureIndexForTolerance[0] = MIL.M_FEATURE_INDEX(5);
            FeatureIndexForTolerance[1] = MIL.M_FEATURE_INDEX(6);

            ///MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref MilImage);
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC, ref MilImage);

            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], MilImage);
            // Restore and display the source image.
            //MIL.MbufRestore(METROL_SIMPLE_IMAGE_FILE, MilSystem, ref MilImage);
            //MIL.MdispSelect(MilDisplay, MilImage);

            // Allocate a graphic list to hold the subpixel annotations to draw.
            //MIL.MgraAllocList(MilSystem, MIL.M_DEFAULT, ref GraphicList);

            // Allocate a graphic list to hold the subpixel annotations to draw.
            //MIL.MgraAllocList(MilSystem, MIL.M_DEFAULT, ref GraphicList);

            // Associate the graphic list to the display for annotations.
            //MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);
            // Allocate metrology context and result.
            MIL.MmetAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilMetrolContext);
            MIL.MmetAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilMetrolResult);
            
            

            int setFineCircleCount = 5;
            int[] RingPositionX = { 1899, 1899, 1899,  0, 0};
            int[] RingPositionY = { 1363, 1363, 1363,  0, 0}; 
            int[] RingStartRadius = { 0, 405, 773,  0, 0}; 
            int[] RingEndRadius = { 21, 407, 840,  0, 0};

            for (int i = 0; i < setFineCircleCount; i++)
            {
                MIL.MmetAddFeature(MilMetrolContext, MIL.M_MEASURED, MIL.M_CIRCLE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NULL, MIL.M_NULL, 0, MIL.M_DEFAULT);
                MIL.MmetSetRegion(MilMetrolContext, MIL.M_FEATURE_INDEX(1 + i), MIL.M_DEFAULT, MIL.M_RING_SECTOR,
                          RingPositionX[i], RingPositionY[i], RingStartRadius[i], RingEndRadius[i], 
                          MIL.M_NULL, MIL.M_NULL);
            }

            //// Add a first measured circle feature to context and set its search region
            //MIL.MmetAddFeature(MilMetrolContext, MIL.M_MEASURED, MIL.M_CIRCLE, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NULL, MIL.M_NULL, 0, MIL.M_DEFAULT);


            //MIL.MmetSetRegion(MilMetrolContext, MIL.M_FEATURE_INDEX(1), MIL.M_DEFAULT, MIL.M_RING,
            //              TOP_RING_POSITION_X, TOP_RING_POSITION_Y, TOP_RING_START_RADIUS,
            //              TOP_RING_END_RADIUS, MIL.M_NULL, MIL.M_NULL);

            //// Add a second measured circle feature to context and set its search region
            //MIL.MmetAddFeature(MilMetrolContext, MIL.M_MEASURED, MIL.M_CIRCLE, MIL.M_DEFAULT,MIL.M_DEFAULT, MIL.M_NULL, MIL.M_NULL, 0, MIL.M_DEFAULT);

            //MIL.MmetSetRegion(MilMetrolContext, MIL.M_FEATURE_INDEX(2), MIL.M_DEFAULT, MIL.M_RING,
            //              MIDDLE_RING_POSITION_X, MIDDLE_RING_POSITION_Y, MIDDLE_RING_START_RADIUS,
            //              MIDDLE_RING_END_RADIUS, MIL.M_NULL, MIL.M_NULL);

            // Add a first constructed point feature to context
            MIL.MmetAddFeature(MilMetrolContext, MIL.M_CONSTRUCTED, MIL.M_POINT, MIL.M_DEFAULT,MIL.M_CENTER, ref FeatureIndexForTopConstructedPoint, MIL.M_NULL, 1, MIL.M_DEFAULT);

            // Add a second constructed point feature to context
            MIL.MmetAddFeature(MilMetrolContext, MIL.M_CONSTRUCTED, MIL.M_POINT, MIL.M_DEFAULT,
                           MIL.M_CENTER, ref FeatureIndexForMiddleConstructedPoint, MIL.M_NULL, 1, MIL.M_DEFAULT);

            // Add a constructed segment feature to context passing through the two points
            MIL.MmetAddFeature(MilMetrolContext, MIL.M_CONSTRUCTED, MIL.M_SEGMENT, MIL.M_DEFAULT,
                           MIL.M_CONSTRUCTION, FeatureIndexForConstructedSegment, MIL.M_NULL, 2, MIL.M_DEFAULT);

            // Add a first segment feature to context and set its search region
            MIL.MmetAddFeature(MilMetrolContext, MIL.M_MEASURED, MIL.M_SEGMENT, MIL.M_DEFAULT,
                           MIL.M_DEFAULT, MIL.M_NULL, MIL.M_NULL, 0, MIL.M_DEFAULT);

            MIL.MmetSetRegion(MilMetrolContext, MIL.M_FEATURE_INDEX(6), MIL.M_DEFAULT, MIL.M_RECTANGLE,
                          BOTTOM_RECT_POSITION_X, BOTTOM_RECT_POSITION_Y, BOTTOM_RECT_WIDTH,
                          BOTTOM_RECT_HEIGHT, BOTTOM_RECT_ANGLE, MIL.M_NULL);

            // Add perpendicularity tolerance
            MIL.MmetAddTolerance(MilMetrolContext, MIL.M_PERPENDICULARITY, MIL.M_DEFAULT, PERPENDICULARITY_MIN,
                             PERPENDICULARITY_MAX, FeatureIndexForTolerance, MIL.M_NULL, 2, MIL.M_DEFAULT);

            // Calculate
            MIL.MmetCalculate(MilMetrolContext, MilImage, MilMetrolResult, MIL.M_DEFAULT);

            // Draw region
            MIL.MgraColor(MIL.M_DEFAULT, REGION_COLOR);
            MIL.MmetDraw(MIL.M_DEFAULT, MilMetrolResult, GraphicList, MIL.M_DRAW_REGION, MIL.M_DEFAULT, MIL.M_DEFAULT);
            Console.Write("Regions used to calculate measured features:\n");
            Console.Write("- two measured circles\n");
            Console.Write("- one measured segment\n");
            Console.Write("Press <Enter> to continue.\n\n");
            //Console.ReadKey();


            // Clear annotations.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);

            MIL.MgraColor(MIL.M_DEFAULT, FEATURE_COLOR);
            MIL.MmetDraw(MIL.M_DEFAULT, MilMetrolResult, GraphicList, MIL.M_DRAW_FEATURE, MIL.M_DEFAULT, MIL.M_DEFAULT);
            Console.Write("Calculated features:\n");

            double centerX = 0.0;
            double centerY = 0.0;

            MIL_INT valid = 0;
            MIL.MmetGetResult(MilMetrolResult, MIL.M_DEFAULT, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref valid);
            Console.WriteLine($"측정된 피처 수: {valid}");



            Globalo.visionManager.milLibrary.ClearOverlay(index); 
            for (int i = 0; i < setFineCircleCount; i++)
            {
                MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1 + i), MIL.M_RADIUS, ref Value);
                MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1+i), MIL.M_POSITION_X, ref centerX);
                MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1+i), MIL.M_POSITION_Y, ref centerY);

                Console.Write("[{0:0}] measured circle: radius={1:0.00}\n", i+1, Value);
                Console.Write("[{0:0}] measured circle: x = {1:0.00}, y = {2:0.00}\n", i + 1, centerX, centerY);
                if (Value > 0.0)
                {
                    //Rectangle m_clRect = new Rectangle((int)(centerX - (Value)), (int)(centerY - (Value)), (int)(Value * 2), (int)(Value * 2));

                    System.Drawing.Point clPoint = new System.Drawing.Point((int)(centerX - Value), (int)(centerY - Value));

                    Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)Value * 2, Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);
                }
                
            }


            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1), MIL.M_RADIUS, ref Value);
            //Console.Write("- first measured circle:  radius={0:0.00}\n", Value);

            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1), MIL.M_POSITION_X, ref centerX);
            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(1), MIL.M_POSITION_Y, ref centerY); 
            //Console.Write("- first measured circle:  x = {0:0.00}, y = {0:0.00}\n", centerX, centerY);

            //Rectangle m_clRect = new Rectangle((int)(centerX - (Value)), (int)(centerY - (Value)), (int)(Value * 2), (int)(Value * 2));
            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Blue, 2, System.Drawing.Drawing2D.DashStyle.Solid);

            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(2), MIL.M_RADIUS, ref Value);
            //Console.Write("- second measured circle: radius={0:0.00}\n", Value);
            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(2), MIL.M_POSITION_X, ref centerX);
            //MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(2), MIL.M_POSITION_Y, ref centerY);
            //m_clRect = new Rectangle((int)(centerX - (Value)), (int)(centerY - (Value)), (int)(Value * 2), (int)(Value * 2));
            //Globalo.visionManager.milLibrary.DrawOverlayCircle(index, m_clRect, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);





            MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(5), MIL.M_LENGTH, ref Value);
            Console.Write("- constructed segment between the two circle centers: length={0:0.00}\n", Value);

            MIL.MmetGetResult(MilMetrolResult, MIL.M_FEATURE_INDEX(6), MIL.M_LENGTH, ref Value);
            Console.Write("- measured segment: length={0:0.00}\n", Value);

            Console.Write("Press <Enter> to continue.\n\n");
            //Console.ReadKey();


            // Get angularity tolerance status and value
            MIL.MmetGetResult(MilMetrolResult, MIL.M_TOLERANCE_INDEX(0), MIL.M_STATUS, ref Status);
            MIL.MmetGetResult(MilMetrolResult, MIL.M_TOLERANCE_INDEX(0), MIL.M_TOLERANCE_VALUE, ref Value);

            if (Status == MIL.M_PASS)
            {
                MIL.MgraColor(MIL.M_DEFAULT, PASS_COLOR);
                Console.Write("Perpendicularity between the two segments: {0:0.00} degrees.\n", Value);
            }
            else
            {
                MIL.MgraColor(MIL.M_DEFAULT, FAIL_COLOR);
                Console.Write("Perpendicularity between the two segments - Fail.\n");
            }
            //MIL.MmetDraw(MIL.M_DEFAULT, MilMetrolResult, GraphicList, MIL.M_DRAW_TOLERANCE, MIL.M_TOLERANCE_INDEX(0), MIL.M_DEFAULT);

            Console.Write("Press <Enter> to continue.\n\n");
            //Console.ReadKey();

            // Free all allocations.
            MIL.MgraFree(GraphicList);
            MIL.MmetFree(MilMetrolResult);
            MIL.MmetFree(MilMetrolContext);
            MIL.MbufFree(MilImage);


        }
        public void ComplexCircleSearchExample1(int index)
        {
            double SMOOTHNESS_VALUE_1 = 50.0;
            double MIN_SCALE_FACTOR_VALUE_1 = 0.01;
            double NUMBER_OF_MODELS_1 = 10.0;
            double MODEL_RADIUS_1 = 740.0;//
            double MODEL_RADIUS_2 = 1.0;//
            double ACCEPTANCE_VALUE_2 = 50.0;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilSearchContext = MIL.M_NULL;
            MIL_ID MilResult = MIL.M_NULL;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);
            MIL_INT ImageSizeX = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageSizeY = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_Y, MIL.M_NULL);

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, ImageSizeX, ImageSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);

            MilImage = tempMilImage;
            // 기존 MilSystem을 사용
            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MdispSelect(MilDisplay, MilImage);

            /* Allocate a graphic list to hold the subpixel annotations to draw. */
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);

            /* Associate the graphic list to the display for annotations. */
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);


            /* Allocate a Circle Finder context. */
            MIL.MmodAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_SHAPE_CIRCLE, MIL.M_DEFAULT, ref MilSearchContext);
            /* Allocate a Circle Finder result buffer. */
            MIL.MmodAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_SHAPE_CIRCLE, ref MilResult);

            /* Define the model. */
            MIL.MmodDefine(MilSearchContext, MIL.M_DEFAULT, MIL.M_DEFAULT, MODEL_RADIUS_2, 
                MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);

            /* Increase the detail level and smoothness for the edge extraction in the search context. */
            MIL.MmodControl(MilSearchContext, MIL.M_CONTEXT, MIL.M_DETAIL_LEVEL, MIL.M_VERY_HIGH);
            MIL.MmodControl(MilSearchContext, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, SMOOTHNESS_VALUE_1);

            /* Modify the acceptance for all the model that was defined. */
            MIL.MmodControl(MilSearchContext, MIL.M_DEFAULT, MIL.M_ACCEPTANCE, ACCEPTANCE_VALUE_2);
            /* Enable Large search scale range*/
            //MIL.MmodControl(MilSearchContext, 0, MIL.M_SCALE_MIN_FACTOR, MIN_SCALE_FACTOR_VALUE_1);

           

            

            /* Minimum separation scale value. */
            double MIN_SEPARATION_SCALE_VALUE_2 = 1.5;
            double MIN_SEPARATION_XY_VALUE_2 = 30.0;

            /* Set minimum separation between occurrences constraints. */
            MIL.MmodControl(MilSearchContext, 0, MIL.M_MIN_SEPARATION_SCALE, MIN_SEPARATION_SCALE_VALUE_2);
            MIL.MmodControl(MilSearchContext, 0, MIL.M_MIN_SEPARATION_X, MIN_SEPARATION_XY_VALUE_2);
            MIL.MmodControl(MilSearchContext, 0, MIL.M_MIN_SEPARATION_Y, MIN_SEPARATION_XY_VALUE_2);

            /* Set The Polarity Constraints. */
            MIL.MmodControl(MilSearchContext, 0, MIL.M_POLARITY, MIL.M_REVERSE);

            /* Set the number of occurrences to 4. */
            MIL.MmodControl(MilSearchContext, MIL.M_DEFAULT, MIL.M_NUMBER, NUMBER_OF_MODELS_1);

            /* Preprocess the search context. */
            MIL.MmodPreprocess(MilSearchContext, MIL.M_DEFAULT);

            /* Reset the timer. */
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET + MIL.M_SYNCHRONOUS, MIL.M_NULL);

            /* Find the models. */
            MIL.MmodFind(MilSearchContext, MilImage, MilResult);

            /* Read the find time. */
            double Time = 0.0;
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ + MIL.M_SYNCHRONOUS, ref Time);

            MIL_INT NumResults = MIL.M_NULL;
            /* Get the number of models found. */
            MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref NumResults);

            const int MODEL_MAX_OCCURRENCES = 23;
            double[] Score = new double[MODEL_MAX_OCCURRENCES];
            double[] XPosition = new double[MODEL_MAX_OCCURRENCES];
            double[] YPosition = new double[MODEL_MAX_OCCURRENCES];
            double[] Radius = new double[MODEL_MAX_OCCURRENCES];

            if ((NumResults >= 1) && (NumResults <= MODEL_MAX_OCCURRENCES))
            {
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_POSITION_X, XPosition);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, YPosition);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_RADIUS, Radius);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_SCORE, Score);


                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MmodDraw(MIL.M_DEFAULT, MilResult, GraphicList, MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_DEFAULT);
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
                MIL.MmodDraw(MIL.M_DEFAULT, MilResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            }


            
        }
        public void SimpleCircleSearchExample(int index)
        {
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilSearchContext = MIL.M_NULL;
            MIL_ID MilResult = MIL.M_NULL;


            MIL_INT PositionDrawColor = MIL.M_COLOR_RED;  /* Position symbol draw color. */
            MIL_INT ModelDrawColor = MIL.M_COLOR_GREEN;   /* Model draw color.           */
            MIL_INT BoxDrawColor = MIL.M_COLOR_BLUE;      /* Model box draw color.       */
            MIL_INT NumResults = 0L;                  /* Number of results found.    */

            MIL_ID NUMBER_OF_MODELS = 18L;
            double MODEL_RADIUS = 840.0; 
            double Time = 0.0;
            int i;                                /* Loop variable.              */

            double[] Score = new double[MODEL_MAX_OCCURRENCES];
            double[] XPosition = new double[MODEL_MAX_OCCURRENCES];
            double[] YPosition = new double[MODEL_MAX_OCCURRENCES];
            double[] Radius = new double[MODEL_MAX_OCCURRENCES];

            //MIL.MbufRestore(SIMPLE_CIRCLE_SEARCH_TARGET_IMAGE, Globalo.visionManager.milLibrary.MilSystem, &tempMilImage);

           // MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref tempMilImage);
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC, ref tempMilImage);
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);

            MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], tempMilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            MIL.MimRank(tempMilImage, tempMilImage, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);

            MIL_INT ImageSizeX = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageSizeY = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_Y, MIL.M_NULL);

            if (MIL.MbufInquire(tempMilImage, MIL.M_SIZE_BAND, MIL.M_NULL) == 3)
            {
                MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, ImageSizeX, ImageSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);

                MIL.MimConvert(tempMilImage, MilImage, MIL.M_RGB_TO_L);
            }
            else
            {
                MilImage = tempMilImage;
            }

            /* Allocate a Circle Finder context. */
            MIL.MmodAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_SHAPE_CIRCLE, MIL.M_DEFAULT, ref MilSearchContext);

            /* Allocate a Circle Finder result buffer. */
            MIL.MmodAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_SHAPE_CIRCLE, ref MilResult);
           

            /* Define the model. */
            MIL.MmodDefine(MilSearchContext, MIL.M_DEFAULT, MIL.M_DEFAULT, MODEL_RADIUS, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MmodControl(MilSearchContext, 0, MIL.M_NUMBER, NUMBER_OF_MODELS);

            /* Preprocess the search context. */
            MIL.MmodPreprocess(MilSearchContext, MIL.M_DEFAULT);

            /* Reset the timer. */
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET + MIL.M_SYNCHRONOUS, MIL.M_NULL);

            /* Find the model. */
            MIL.MmodFind(MilSearchContext, MilImage, MilResult);

            /* Read the find time. */
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ + MIL.M_SYNCHRONOUS, ref Time);

            /* Get the number of models found. */
            MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref NumResults);

            //MIL.MosPrintf(MIL_TEXT("\nUsing circle finder in a simple situation:\n"));
            //MIL.MosPrintf(MIL_TEXT("------------------------------------------\n\n"));
            //MIL.MosPrintf(MIL_TEXT("A circle model was defined with "));
            //MIL.MosPrintf(MIL_TEXT("a nominal radius of %-3.1f%.\n\n"), MODEL_RADIUS);

            Console.WriteLine($"fine circle : {NumResults}");
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            //If a model was found above the acceptance threshold.
            
            if ((NumResults >= 1) && (NumResults <= MODEL_MAX_OCCURRENCES))
            {
                /* Get the results of the circle search. */
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_POSITION_X, XPosition);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, YPosition);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_RADIUS, Radius);
                MIL.MmodGetResult(MilResult, MIL.M_DEFAULT, MIL.M_SCORE, Score);

                /* Print the results for each circle found. */
                //MIL.MosPrintf(MIL_TEXT("The circles were found in the target image:\n\n"));
                //MIL.MosPrintf(MIL_TEXT("Result   X-Position   Y-Position   Radius   Score\n\n"));

                for (i = 0; i < NumResults; i++)
                {
                    //MIL.MosPrintf(MIL_TEXT("%-9d%-13.2f%-13.2f%-8.2f%-5.2f%%\n"), i, XPosition[i],YPosition[i], Radius[i], Score[i]);
                    Console.Write("[{0:0}] circle: x = {1:0.00}, y = {2:0.00}, radius = {3:0.00}, score = {4:0.00}\n", i + 1, XPosition[i], YPosition[i], Radius[i], Score[i]);
                    //Rectangle m_clRect = new Rectangle((int)(XPosition[i] - (Radius[i])), (int)(YPosition[i] - (Radius[i])), (int)(Radius[i] * 2), (int)(Radius[i] * 2));
                    System.Drawing.Point clPoint = new System.Drawing.Point((int)(XPosition[i] - Radius[i]), (int)(YPosition[i] - Radius[i]));
                    Globalo.visionManager.milLibrary.DrawOverlayCircle(index, clPoint, (int)Radius[i]*2, Color.Yellow, 3, System.Drawing.Drawing2D.DashStyle.Solid);
                }

                //MIL.MosPrintf(MIL_TEXT("\nThe search time was %.1f ms.\n\n"), Time * 1000.0);

                /* Draw edges, position and box over the occurrences that were found. */


                //MIL.MgraColor(MIL.M_DEFAULT, PositionDrawColor);
                //MIL.MmodDraw(MIL.M_DEFAULT, MilResult, Globalo.visionManager.milLibrary.MilCamSmallImageChild[index], MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_DEFAULT);

                ////Globalo.visionManager.milLibrary.drawTest(MilResult, 1);

                //MIL.MgraColor(MIL.M_DEFAULT, BoxDrawColor);
                //MIL.MmodDraw(MIL.M_DEFAULT, MilResult, Globalo.visionManager.milLibrary.MilCamSmallImageChild[index], MIL.M_DRAW_BOX, MIL.M_DEFAULT, MIL.M_DEFAULT);
                ////Globalo.visionManager.milLibrary.drawTest(MilResult, 1);
                //MIL.MgraColor(MIL.M_DEFAULT, ModelDrawColor);
                //MIL.MmodDraw(MIL.M_DEFAULT, MilResult, Globalo.visionManager.milLibrary.MilCamSmallImageChild[index], MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
                //Globalo.visionManager.milLibrary.drawTest(MilResult, 1);


                //MIL.MmodDraw(MIL.M_DEFAULT, MilResult, MilCamSmallImageChild[index], MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            }
            else
            {
                //MIL.MosPrintf(MIL.MIL_TEXT("The model was not found or the number of models ")


                //          MIL.MIL_TEXT("found is greater than\n"));
                //MIL.MosPrintf(MIL.MIL_TEXT("the specified maximum number of occurrence !\n\n"));
            }

            /* Wait for a key to be pressed. */
            //MIL.MosPrintf(MIL.MIL_TEXT("Press <Enter> to continue.\n\n"));
            //MIL.MosGetch();

            /* Free MIL objects. */
           // MIL.MgraFree(GraphicList);
            //MIL.MbufFree(MilImage);
            //MIL.MmodFree(MilSearchContext);
            //MIL.MmodFree(MilResult);
        }
        public void Con1Test(int index)
        {
            Console.WriteLine("Con1Test");

            int mThreshold = 70;

            MIL_ID MilBinary = MIL.M_NULL;
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index], ref MilSubImage01);
           // MIL.MimBinarize(MilBinary, MilBinary, MIL.M_GREATER_OR_EQUAL, mThreshold, MIL.M_NULL);	//! 이진화 반전 140 M_GREATER_OR_EQUAL	M_LESS_OR_EQUAL
            //MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], MilBinary, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            //MIL.MimRank(MilBinary, MilBinary, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);


            //MIL.MbufChild2d(MilBinary, 1000, 500, 2150, 1900, ref MilSubImage01);

            MIL.MbufExport("D:\\Con1Test.BMP", MIL.M_BMP, MilSubImage01);

            //MIL_ID MilOverlayImage = MIL.M_NULL;       // Overlay buffer identifier.
            MIL_ID MilBeadContext = MIL.M_NULL;        // Bead context identifier.
            MIL_ID MilBeadResult = MIL.M_NULL;         // Bead result identifier.

             // Template attributes definition.
            double CIRCLE_CENTER_X = 2030.0;
            double CIRCLE_CENTER_Y = 1434.0;
            double CIRCLE_RADIUS = 780.0;
            double EDGE_THRESHOLD_VALUE = 20.0;

            double MAX_CONTOUR_DEVIATION_OFFSET = 5.0;
            double MAX_CONTOUR_FOUND_OFFSET = 20.0;
            double MaximumOffset = 0.0;     // Maximum offset result value.


            MIL_INT USER_TEMPLATE_COLOR = MIL.M_COLOR_CYAN;
            MIL_INT TRAINED_BEAD_WIDTH_COLOR = MIL.M_RGB888(255, 128, 0);
            MIL_INT PASS_BEAD_POSITION_COLOR = MIL.M_COLOR_GREEN;
            MIL_INT FAIL_EDGE_OFFSET_COLOR = MIL.M_COLOR_RED;

            // Allocate a MIL bead context.
            MIL.MbeadAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, MIL.M_DEFAULT, ref MilBeadContext);
            // Allocate a MIL bead result.
            MIL.MbeadAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilBeadResult);
            // Add the bead templates.
            MIL.MbeadTemplate(MilBeadContext, MIL.M_ADD, MIL.M_BEAD_EDGE, MIL.M_TEMPLATE_LABEL(1), 0, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MIL.M_DEFAULT);

            // Set the bead shape properties.
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TRAINING_PATH, MIL.M_CIRCLE);

            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_CENTER_X, CIRCLE_CENTER_X);
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_CENTER_Y, CIRCLE_CENTER_Y);
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_RADIUS, CIRCLE_RADIUS);

            // Set the edge threshold value to extract the object shape.
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_THRESHOLD_VALUE, EDGE_THRESHOLD_VALUE);

            // Using the default fixed user defined nominal edge width.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_WIDTH_NOMINAL_MODE, MIL.M_USER_DEFINED);

            // Set the maximal expected contour deformation.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_FAIL_WARNING_OFFSET, MAX_CONTOUR_FOUND_OFFSET);

            // Set the maximum valid bead deformation.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_OFFSET_MAX, MAX_CONTOUR_DEVIATION_OFFSET);



            // Display the bead in the overlay image.
            MIL.MgraColor(MIL.M_DEFAULT, USER_TEMPLATE_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadContext, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_POSITION,
               MIL.M_USER, MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // The bead template is entirely defined and is trained without sample image.
            MIL.MbeadTrain(MilBeadContext, MIL.M_NULL, MIL.M_DEFAULT);

            // Display the trained bead.
            MIL.MgraColor(MIL.M_DEFAULT, TRAINED_BEAD_WIDTH_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadContext, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_SEARCH_BOX,
               MIL.M_TRAINED, MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);






            // Perform the inspection of the bead in the fixtured target image.
            MIL.MbeadVerify(MilBeadContext, MilSubImage01, MilBeadResult, MIL.M_DEFAULT);

            // Clear the overlay annotation.
            MIL.MdispControl(Globalo.visionManager.milLibrary.MilCamDisplay[index], MIL.M_OVERLAY_CLEAR, MIL.M_TRANSPARENT_COLOR);

            // Display the pass bead sections.
            MIL.MgraColor(MIL.M_DEFAULT, PASS_BEAD_POSITION_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadResult, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_POSITION, MIL.M_PASS,
               MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // Display the offset bead sections.
            MIL.MgraColor(MIL.M_DEFAULT, FAIL_EDGE_OFFSET_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadResult, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_POSITION, MIL.M_FAIL_OFFSET,
               MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // Retrieve and display general bead results.
            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_OFFSET_MAX, ref MaximumOffset);

            // 검출된 원 개수 가져오기
            double numBeads = 0.0;
            MIL.MbeadGetResult(MilBeadResult, MIL.M_ALL, MIL.M_GENERAL, MIL.M_NUMBER_FOUND, ref numBeads);
            Console.WriteLine($"검출된 원 개수: {numBeads}");
            //M_ALL
            //
            long numTemplates = 0;
            MIL.MbeadGetResult(MilBeadResult, MIL.M_DEFAULT, MIL.M_GENERAL, MIL.M_NUMBER_FOUND, ref numTemplates);
            Console.WriteLine("템플릿 개수: " + numTemplates);
            

            try
            {
                // 각각의 원 좌표를 가져오기
                int count = (int)numBeads;
                if (count > 0)
                {
                    double NominalWidth = 0.0;                  // Nominal width result value.
                    double AvWidth = 0.0;                       // Average width result value.
                    double GapCov = 0.0;                        // Gap coverage result value.
                    double MaxGap = 0.0;                        // Maximum gap result value.
                    double Score = 0.0;                         // Bead score result value.


                    // Retrieve and display general bead results.
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_SCORE, ref Score);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_GAP_COVERAGE, ref GapCov);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_WIDTH_AVERAGE, ref AvWidth);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_GAP_MAX_LENGTH, ref MaxGap);

                    Console.Write("The bead 'has been inspected:\n");
                    Console.Write(" -Passing bead sections (green) cover {0:0.00}% of the bead\n", Score);
                    Console.Write(" -Missing bead sections (red) cover {0:0.00}% of the bead\n", GapCov);
                    Console.Write(" -Sections outside the specified width tolerances are drawn in orange\n");
                    Console.Write(" -The bead's average width is {0:0.00} pixels\n", AvWidth);
                    Console.Write(" -The bead's longest gap section is {0:0.00} pixels\n\n", MaxGap);



                    double xpos = 0.0;
                    double ypos = 0.0;

                    double[] UserVarPtrX = new double[count];
                    double[] UserVarPtrY = new double[count];
                    double[] Offseta = new double[count];
                    double[] anglea = new double[count];

                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_POSITION_X, UserVarPtrX);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_POSITION_Y, UserVarPtrY);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_OFFSET, Offseta);
                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_ANGLE, anglea);

                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine($"원 {i + 1}: X = {UserVarPtrX[i]}, Y = {UserVarPtrY[i]}, M_OFFSET = {Offseta[i]}, M_ANGLE = {anglea[i]}");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WriteCsvFromList 처리 중 예외 발생: {ex.Message}");
            }
            

            // Free all allocations.
            MIL.MbeadFree(MilBeadContext);
            MIL.MbeadFree(MilBeadResult);
        }

        public void Con2Test(int index)
        {
            Console.WriteLine("Con2Test");

            // Template attributes definition.
            double CIRCLE_CENTER_X = 2030.0;
            double CIRCLE_CENTER_Y = 1434.0;
            double CIRCLE_RADIUS = 780.0;
            double EDGE_THRESHOLD_VALUE = 30.0;

            double MAX_CONTOUR_DEVIATION_OFFSET = 30.0;
            double MAX_CONTOUR_FOUND_OFFSET = 500.0;
            double MaximumOffset = 0.0;     // Maximum offset result value.


            MIL_INT USER_TEMPLATE_COLOR = MIL.M_COLOR_CYAN;
            MIL_INT TRAINED_BEAD_WIDTH_COLOR = MIL.M_RGB888(255, 128, 0);
            MIL_INT PASS_BEAD_POSITION_COLOR = MIL.M_COLOR_GREEN;
            MIL_INT FAIL_EDGE_OFFSET_COLOR = MIL.M_COLOR_RED;

            MIL_ID MilBinary = MIL.M_NULL;
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index], ref MilSubImage01);

            MIL_ID MilDisplay = MIL.M_NULL;             // Display identifier.
            MIL_ID MilOverlayImage = MIL.M_NULL;       // Overlay buffer identifier.
            MIL_ID MilImageTarget = MIL.M_NULL;        // Image buffer identifier.
            MIL_ID MilBeadContext = MIL.M_NULL;        // Bead context identifier.
            MIL_ID MilBeadResult = MIL.M_NULL;         // Bead result identifier.

           // double MaximumOffset = 0.0;     // Maximum offset result value.

            // Restore target image into an automatically allocated image buffers.
            //MIL.MbufRestore(CAP_FILE_TARGET, MilSystem, ref MilImageTarget);

            // Display the training image buffer.
            MIL.MdispSelect(MilDisplay, MilImageTarget);

            // Prepare the overlay for annotations.
            MIL.MdispControl(MilDisplay, MIL.M_OVERLAY, MIL.M_ENABLE);
            MIL.MdispInquire(MilDisplay, MIL.M_OVERLAY_ID, ref MilOverlayImage);
            MIL.MdispControl(MilDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_TRANSPARENT_COLOR);

            // Original template image.
            Console.Write("\nPREDEFINED BEAD INSPECTION:\n");
            Console.Write("---------------------------\n\n");
            Console.Write("This program performs a bead inspection of a bottle\n");
            Console.Write("cap's contour using a predefined circular bead.\n");
            Console.Write("Press <Enter> to continue.\n\n");
            //Console.ReadKey();

            // Allocate a MIL bead context.
            MIL.MbeadAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, MIL.M_DEFAULT, ref MilBeadContext);

            // Allocate a MIL bead result.
            MIL.MbeadAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilBeadResult);

            // Add the bead templates.
            MIL.MbeadTemplate(MilBeadContext, MIL.M_ADD, MIL.M_BEAD_EDGE, MIL.M_TEMPLATE_LABEL(1),
               0, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MIL.M_DEFAULT);

            // Set the bead shape properties.
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TRAINING_PATH, MIL.M_CIRCLE);

            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_CENTER_X, CIRCLE_CENTER_X);
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_CENTER_Y, CIRCLE_CENTER_Y);
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_TEMPLATE_CIRCLE_RADIUS, CIRCLE_RADIUS);

            // Set the edge threshold value to extract the object shape.
            MIL.MbeadControl(MilBeadContext, MIL.M_TEMPLATE_LABEL(1), MIL.M_THRESHOLD_VALUE, EDGE_THRESHOLD_VALUE);

            // Using the default fixed user defined nominal edge width.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_WIDTH_NOMINAL_MODE, MIL.M_USER_DEFINED);

            // Set the maximal expected contour deformation.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_FAIL_WARNING_OFFSET, MAX_CONTOUR_FOUND_OFFSET);

            // Set the maximum valid bead deformation.
            MIL.MbeadControl(MilBeadContext, MIL.M_ALL, MIL.M_OFFSET_MAX, MAX_CONTOUR_DEVIATION_OFFSET);

            // Display the bead in the overlay image.
            MIL.MgraColor(MIL.M_DEFAULT, USER_TEMPLATE_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadContext, MilOverlayImage, MIL.M_DRAW_POSITION,
               MIL.M_USER, MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // The bead template is entirely defined and is trained without sample image.
            MIL.MbeadTrain(MilBeadContext, MIL.M_NULL, MIL.M_DEFAULT);

            // Display the trained bead.
            MIL.MgraColor(MIL.M_DEFAULT, TRAINED_BEAD_WIDTH_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadContext, MilOverlayImage, MIL.M_DRAW_SEARCH_BOX,
               MIL.M_TRAINED, MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // Pause to show the template image and user points.
            Console.Write("A circular template that was parametrically defined by the user\n");
            Console.Write("is displayed (in cyan). The template has been trained and the resulting\n");
            Console.Write("search is displayed (in orange).\n");
            Console.Write("Press <Enter> to continue.\n\n");
            //Console.ReadKey();

            // Perform the inspection of the bead in the fixtured target image.
            MIL.MbeadVerify(MilBeadContext, MilSubImage01, MilBeadResult, MIL.M_DEFAULT);

            // Clear the overlay annotation.
            MIL.MdispControl(MilDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_TRANSPARENT_COLOR);

            // Display the pass bead sections.
            MIL.MgraColor(MIL.M_DEFAULT, PASS_BEAD_POSITION_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadResult, MilOverlayImage, MIL.M_DRAW_POSITION, MIL.M_PASS,
               MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // Display the offset bead sections.
            MIL.MgraColor(MIL.M_DEFAULT, FAIL_EDGE_OFFSET_COLOR);
            MIL.MbeadDraw(MIL.M_DEFAULT, MilBeadResult, MilOverlayImage, MIL.M_DRAW_POSITION, MIL.M_FAIL_OFFSET,
               MIL.M_ALL, MIL.M_ALL, MIL.M_DEFAULT);

            // Retrieve and display general bead results.
            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_GENERAL, MIL.M_OFFSET_MAX, ref MaximumOffset);

            // 검출된 원 개수 가져오기
            double numBeads = 0.0;
            MIL.MbeadGetResult(MilBeadResult, MIL.M_ALL, MIL.M_GENERAL, MIL.M_NUMBER_FOUND, ref numBeads);
            Console.WriteLine($"검출된 원 개수: {numBeads}");
            //M_ALL
            //

            // 각각의 원 좌표를 가져오기
            int count = (int)numBeads;


            double xpos = 0.0;
            double ypos = 0.0;

            double[] UserVarPtrX = new double[count];
            double[] UserVarPtrY = new double[count];
            double[] Offseta = new double[count];
            double[] anglea = new double[count];

            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_POSITION_X, UserVarPtrX);
            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_POSITION_Y, UserVarPtrY);
            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_OFFSET, Offseta);
            MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_ALL, MIL.M_ANGLE, anglea);

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"원 {i + 1}: X = {UserVarPtrX[i]}, Y = {UserVarPtrY[i]}, M_OFFSET = {Offseta[i]}, M_ANGLE = {anglea[i]}");
            }

            // 경과 시간 측정 종료
            //stopwatch.Stop();
            //Console.WriteLine($"Total Test Time ms: {stopwatch.ElapsedMilliseconds} ms");
            //
            //
            Console.Write("The bottle cap shape has been inspected:\n");
            Console.Write(" -Sections outside the specified offset tolerance are drawn in red\n");
            Console.Write(" -The maximum offset value is {0:0.00} pixels.\n\n", MaximumOffset);

            // Pause to show the result.
            //Console.Write("Press <Enter> to terminate.\n");
            //Console.ReadKey();

            // Free all allocations.
            MIL.MbeadFree(MilBeadContext);
            MIL.MbeadFree(MilBeadResult);
            MIL.MbufFree(MilImageTarget);
        }
    }
}
