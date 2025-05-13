using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenHandler.VisionClass
{
    public class AoiSideTester
    {
        public AoiSideTester()
        {

        }
        public bool MilHeightTest(int index, Mat srcImage)
        {
            bool rtn = false;

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

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y,
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);
            MIL_INT ImageSizeX = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageSizeY = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_Y, MIL.M_NULL);

            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            //MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, ImageSizeX, ImageSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
            MIL.MbufChild2d(tempMilImage, 800, 700, 250, 900, ref MilImage);


            //MilImage = tempMilImage;
            // 기존 MilSystem을 사용
            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MdispSelect(MilDisplay, MilImage);

            /* Allocate a graphic list to hold the subpixel annotations to draw. */
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);

            /* Associate the graphic list to the display for annotations. */
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);



            // 마커 생성
            //MIL.MilMeasMarker = MIL.MmeasAllocMarker(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, MIL.M_NULL);
            // Allocate the measurement marker.
           // MIL_ID MilStripeMarker = MIL.MmeasAllocMarker(Globalo.visionManager.milLibrary.MilSystem, MIL.M_STRIPE, MIL.M_DEFAULT, MIL.M_NULL);

            MIL_ID MilEdgeMarker = MIL.MmeasAllocMarker(Globalo.visionManager.milLibrary.MilSystem, MIL.M_EDGE, MIL.M_DEFAULT, MIL.M_NULL);

            // Y축 방향 두 엣지 측정용 마커 설정

            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_BOX_CENTER, 170, 510);//ROUGH_BOX_CENTER_X, ROUGH_BOX_CENTER_Y);
            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_BOX_SIZE, 250, 900);//ROUGH_BOX_WIDTH, ROUGH_BOX_HEIGHT);
            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_BOX_ANGLE, 180, MIL.M_NULL);    //ROUGH_BOX_ANGLE
            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_SEARCH_REGION_INPUT_UNITS, MIL.M_WORLD, MIL.M_NULL);
            /// MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_EDGE, MIL.M_ORIENTATION, MIL.M_VERTICAL);
            //
            //MIL.MmeasSetMarker(MilStripeMarker, MIL.M_FILTER_SMOOTHNESS, 90, MIL.M_NULL);
            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_FILTER_TYPE, MIL.M_SHEN, MIL.M_NULL);

            MIL.McalUniform(MilImage, 0.0, 0.0, 1.0, 1.0, 0.0, MIL.M_DEFAULT);
            // Set up the drawing.
            MIL.MgraControl(MIL.M_DEFAULT, MIL.M_INPUT_UNITS, MIL.M_WORLD);
            MIL.MmeasSetMarker(MilEdgeMarker, MIL.M_DRAW_PROFILE_SCALE_OFFSET, MIL.M_AUTO_SCALE_PROFILE, MIL.M_DEFAULT);
            // Find the marker.
            MIL.MmeasFindMarker(MIL.M_DEFAULT, MilImage, MilEdgeMarker, MIL.M_POSITION);
            // Get the status.
            MIL_INT ValidFlag = MIL.M_NULL;
            //MIL.MmeasGetResult(MilEdgeMarker, MIL.M_VALID_FLAG + MIL.M_TYPE_MIL_INT, ref ValidFlag, MIL.M_NULL);
            if (ValidFlag == MIL.M_TRUE)
            {
                // Draw the edge annotation in the image.
                //MIL.EDGE_DRAW_LIST.DrawList(MilEdgeMarker, MilGraList);

                // Get the position and angle of the marker.
                MIL_INT FinePosX = MIL.M_NULL;
                MIL_INT FinePosY = MIL.M_NULL;
                MIL_INT FineAngle = MIL.M_NULL;
                MIL.MmeasGetResult(MilEdgeMarker, MIL.M_POSITION, ref FinePosX, ref FinePosY);
                MIL.MmeasGetResult(MilEdgeMarker, MIL.M_ANGLE, ref FineAngle, MIL.M_NULL);
            }


                return rtn;
        }
        public bool HeightTest(int index, Mat srcImage)
        {
            bool rtn = false;
            bool IMG_VIEW = false;
            int startTime = Environment.TickCount;

            int radius = 700;
            Mat gray = new Mat();
            Rect roiRect = new Rect(800, 621, 250, 1020);


            //Mat roiKeyMat = srcImage[roiRect];
            // 마스크 크기와 ROI 영역을 동일하게 설정
            Mat mask = Mat.Zeros(srcImage.Size(), MatType.CV_8UC1);  // 전역 크기의 검정색 마스크 생성

            // 마스크의 ROI 영역만 흰색으로 설정
            Rect roi = new Rect(800, 621, 250, 1020);
            mask.Rectangle(roi, new Scalar(255), -1); // ROI 영역을 흰색으로 설정

            //Mat roiKeyMat = new Mat();
            //srcImage.CopyTo(roiKeyMat, mask);  // 마스크가 적용된 영역만 복사
            // ROI 영역만 흰색으로 설정
            Cv2.Rectangle(mask, roiRect, Scalar.White, -1);

            // 원본 이미지에서 특정 영역을 슬라이싱하여 추출
            //Mat roiKeyMat = srcImage.RowRange(621, 1641).ColRange(800, 1050); // (startRow, endRow)과 (startCol, endCol)로 영역 지정

            //621, 1641
            //Cv2.Rectangle(roiKeyMat, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(10, roiKeyMat.Height), Scalar.All(0), 10); // 경계 20픽셀을 검정으로 채우기
            // Cv2.Rectangle(roiKeyMat, new OpenCvSharp.Point(roiKeyMat.Width - 10, 0), new OpenCvSharp.Point(10, roiKeyMat.Height), Scalar.All(0), 10); // 경계 20픽셀을 검정으로 채우기

            // 경계를 제외할 마스크 생성 (예: 상하좌우 20px 제외)
            //Mat mask = new Mat(roiKeyMat.Size(), MatType.CV_8UC1, new Scalar(255)); // 흰색 마스크
            //Cv2.Rectangle(mask, new OpenCvSharp.Point(20, 20), new OpenCvSharp.Point(mask.Width - 20, mask.Height - 20), new Scalar(0), -1); // 경계부분을 검정색으로 채움

            // 마스크를 적용하여 경계 영역 제외
            //Mat maskedImage = new Mat();
            //roiKeyMat.CopyTo(maskedImage, mask);  // 마스크를 적용한 이미지
            // ROI 주변에 여유 공간을 추가(검정색 패딩 10픽셀)



            int pad = 100;
            //Mat paddedRoi = new Mat();
           // Cv2.CopyMakeBorder(roiKeyMat, paddedRoi, pad, pad, pad, pad, BorderTypes.Constant, new Scalar(0));

            // 이미지 크기에 맞는 빈 마스크

            var binary = new Mat();
            int minThresh = 130;
            Cv2.Threshold(srcImage, binary, minThresh, 255, ThresholdTypes.Binary);
            // 마스크 적용
            Mat masked = new Mat();
            Cv2.BitwiseAnd(binary, mask, masked);

            Cv2.NamedWindow("Detected binary ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected binary ", binary);
            Cv2.WaitKey(0);
            // 윤곽선 추출
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //CvInvoke.FindContours(roiImg, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            Cv2.FindContours(masked, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple); 


            // 가장 큰 contour 찾기
            double maxArea = 0;
            int maxContourIdx = -1;
            int i = 0;
            double imageArea = binary.Width * binary.Height;


            Mat colorView = new Mat();
            Cv2.CvtColor(masked, colorView, ColorConversionCodes.GRAY2BGR);

            List<OpenCvSharp.Point[]> validContours = new List<OpenCvSharp.Point[]>();

            for (i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);

                Cv2.DrawContours(colorView, contours, i, Scalar.FromRgb(100, 255, 100), 3);
                //if (!IsEdgeContour(contours[i], binary.Width, binary.Height, 5)) // margin 5 정도로
                    //validContours.Add(contours[i]);


            }

            Cv2.NamedWindow("Detected result2 ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            Cv2.ImShow("Detected result2 ", colorView);
            Cv2.WaitKey(0);


            //int height = maxY - minY;


            //Mat result = new Mat();
            //Cv2.CvtColor(srcImage, result, ColorConversionCodes.GRAY2BGR);
            //Console.Write("------------FindContours\n");
            //Globalo.visionManager.milLibrary.ClearOverlay(index);
            //// 가장 높은 점과 낮은 점 표시
            //Cv2.Circle(colorView, new OpenCvSharp.Point(minYx, minY), 5, Scalar.Blue, -1);  // 최고점
            //Cv2.Circle(colorView, new OpenCvSharp.Point(maxYx, maxY), 5, Scalar.Red, -1);   // 최저점

            //// 두 점을 선으로 연결
            //Cv2.Line(colorView, new OpenCvSharp.Point(minYx, minY), new OpenCvSharp.Point(maxYx, maxY), Scalar.Yellow, 2);

            //// 높이 텍스트 표시 (Y축 길이)
            //string text = $"Height: {height}px";
            //Cv2.PutText(colorView, text, new OpenCvSharp.Point(10, 30), HersheyFonts.HersheySimplex, 1, Scalar.Green, 2);

            //Cv2.NamedWindow("Detected result ", WindowFlags.Normal);  // 수동 크기 조정 가능 창 생성
            //Cv2.ImShow("Detected result ", colorView);
            //Cv2.WaitKey(0);
            return rtn;
        }
        bool IsEdgeContour(OpenCvSharp.Point[] contour, int width, int height, int margin = 3)
        {
            foreach (var pt in contour)
            {
                if (pt.X <= margin || pt.Y <= margin || pt.X >= width - margin || pt.Y >= height - margin)
                    return true;
            }
            return false;
        }
    }
}
