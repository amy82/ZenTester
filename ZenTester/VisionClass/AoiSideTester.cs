using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenTester.VisionClass
{
    public class AoiSideTester
    {
        private Stopwatch TeststopWatch = new Stopwatch();
        public AoiSideTester()
        {

        }
        //public bool Mark_Find_Standard(int index, VisionClass.eMarkList MarkPos, ref OpenCvSharp.Point conePos, ref double dScore)
        public bool Mark_Pos_Standard(int index, VisionClass.eMarkList MarkPos, ref OpenCvSharp.Point centerPos, ref double dScore, bool bDraw = false)        //사이드 카메라 기준점 찾기 - 
        {
            bool bRtn = true;
            //Mark Find
            VisionClass.CDMotor dAlign = new VisionClass.CDMotor();
            //Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].name;
            int MarkIndex = 0;
            centerPos.X = 0;
            centerPos.Y = 0;

            MarkIndex = (int)MarkPos;   // VisionClass.eMarkList.SIDE_HEIGHT;

            bRtn = Globalo.visionManager.markUtil.CalcSingleMarkAlign(index, MarkIndex, ref dAlign, ref dScore, bDraw);

            centerPos.X = (int)dAlign.X;
            centerPos.Y = (int)dAlign.Y;

            Console.WriteLine($"X:{dAlign.X},Y: {dAlign.Y}, T:{dAlign.T}");


            return bRtn;

        }
        public bool MilEdgeOringTest(int index, int roiIndex, System.Drawing.Point OffsetPos, bool bAutoRun = false)//, Mat srcImage)
        {
            int startTime = Environment.TickCount;
            bool bRtn = true;

            const int CONTOUR_MAX_RESULTS = 10;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilEdgeResult = MIL.M_NULL;                              // Edge result identifier.
            MIL_ID MilEdgeContext = MIL.M_NULL;                             // Edge context.

            double[] EdgeSize = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeCircleFitCx = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeCircleFitErr = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeCircleRadius = new double[CONTOUR_MAX_RESULTS];

            int xGap = OffsetPos.X;
            int YGap = OffsetPos.Y;

            int OffsetX = Globalo.yamlManager.aoiRoiConfig.ORING_ROI[roiIndex].X + xGap;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.ORING_ROI[roiIndex].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.ORING_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.ORING_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);

            //MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);
            //MIL.MbufChild2d(tempMilImage, OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);

            MIL.MimBinarize(MilImage, MilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);

            MIL.MbufExport("d:\\oring.BMP", MIL.M_BMP, MilImage);


            MIL.MbufExport("d:\\MilImage2.BMP", MIL.M_BMP, MilImage);
            /* Allocate a graphic list to hold the subpixel annotations to draw. */
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);
            /* Associate the graphic list to the display for annotations. */
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);
            // Allocate a Edge Finder context.
            MIL.MedgeAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_CONTOUR, MIL.M_DEFAULT, ref MilEdgeContext);

            // Allocate a result buffer.
            MIL.MedgeAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilEdgeResult);

            // Enable features to compute.
            MIL.MedgeControl(MilEdgeContext, MIL.M_SIZE, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_CIRCLE_FIT_CENTER_X, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_CIRCLE_FIT_ERROR, MIL.M_ENABLE);


            // Calculate edges and features.
            MIL.MedgeCalculate(MilEdgeContext, MilImage, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MilEdgeResult, MIL.M_DEFAULT);


            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_LESS, 100.0, MIL.M_NULL);

            MIL_INT NumEdgeFound = 0;                                       // Number of edges found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumEdgeFound);

            // Draw edges in the source image to show the result.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MedgeControl(MilEdgeResult, 319L, (double)-OffsetX);
            MIL.MedgeControl(MilEdgeResult, 320L, (double)-OffsetY);


            MIL.MedgeControl(MilEdgeResult, 3203L, Globalo.visionManager.milLibrary.xReduce[index]);
            MIL.MedgeControl(MilEdgeResult, 3204L, Globalo.visionManager.milLibrary.yReduce[index]);

            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, Globalo.visionManager.milLibrary.MilSetCamOverlay, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            //M_DRAW_BOX + M_DRAW_POSITION + M_DRAW_EDGES + M_DRAW_AXIS
            //-----------------------------------------------------------------------------------------------------------------------------
            //
            //불필요한 edge 제거하기
            //
            //
            //-----------------------------------------------------------------------------------------------------------------------------
            //
            // Draw remaining edges and their index to show the result.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            


            MIL_INT NumResults = 0;                                         // Number of results found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumResults);

            int minIndex = 0;
            int maxIndex = 0;
            double CircleCx = 0.0;
            double CircleErr = 0.0;
            double circleSpec = 350.0;
            string str = ""; 
            Color OringColor;
            Rectangle m_clRect = new Rectangle((int)(OffsetX), (int)(OffsetY), OffsetWidth, OffsetHeight);
            // If the right number of edges were found.
            if (NumResults <= CONTOUR_MAX_RESULTS)
            {
                // Draw the index of each edge.
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_INDEX, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Get the mean Feret diameters.
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_SIZE, EdgeSize);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_CENTER_X, EdgeCircleFitCx);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_ERROR, EdgeCircleFitErr);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_RADIUS, EdgeCircleRadius);

                Console.WriteLine($"M_SIZE : {EdgeSize[0]}");
                Console.WriteLine($"M_CIRCLE_FIT_CENTER_X : {EdgeCircleFitCx[0]}");
                Console.WriteLine($"M_CIRCLE_FIT_ERROR : {EdgeCircleFitErr[0]}");
                Console.WriteLine($"M_CIRCLE_FIT_RADIUS : {EdgeCircleRadius[0]}");

                // Print the results.
                Console.Write("Mean diameter of the {0} outer edges are:\n\n", NumResults);
                //Console.Write("Index   Mean diameter \n");

                CircleCx = EdgeCircleFitCx[0];
                CircleErr = EdgeCircleFitErr[0];

                if (CircleErr < circleSpec)//CircleCx < circleSpec || 
                {
                    //오링 없음
                    bRtn = false;
                }
                else
                {
                    bRtn = true;
                }
                Console.WriteLine($"Circle Fit Center X : {CircleCx}");
                Console.WriteLine($"Circle Fit Error : {CircleErr}");

                //Console.WriteLine($"Height : {(maxValue - minValue)}, {(maxValue - minValue) * Globalo.visionManager.CamResol.Y}");
                //int OffsetX = 800;
                //int OffsetY = 700; 

                //Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + minValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + minValue), Color.Yellow, 1);
                //Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + maxValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + maxValue), Color.Yellow, 1);

                //Globalo.visionManager.milLibrary.DrawOverlayArrow(index, OffsetX + (OffsetWidth / 2), (int)(OffsetY + minValue), OffsetX + (OffsetWidth / 2), (int)(OffsetY + maxValue), Color.Yellow, 1);


                //int textCenterY = (int)((OffsetY + maxValue) - ((OffsetY + maxValue) - (OffsetY + minValue)) / 2);

                //int txtOffsetX = 0;
                //if (roiIndex == 2)
                //{
                //    txtOffsetX = 530;
                //}
                //System.Drawing.Point textPoint = new System.Drawing.Point(OffsetX + (OffsetWidth / 2) - txtOffsetX, textCenterY);
                //dHeight = (maxValue - minValue) * Globalo.visionManager.CamResol.Y;
                //string str = $"{dHeight.ToString("0.0##")}(mm)";
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);

                //Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Green, 2);
                Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, Color.Green, System.Drawing.Drawing2D.DashStyle.Solid);
                System.Drawing.Point textPoint;

                str = $"[O-RING] Circle Fit:{CircleErr.ToString("0.000")}/{circleSpec.ToString("0.00#")}";
                Console.WriteLine(str);
                //textPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 17);
                
                if (bRtn)
                {
                    str = $"O-RING - 1";
                    OringColor = Color.Green;
                }
                else
                {
                    str = $"O-RING - 0";
                    OringColor = Color.Red;
                }

                if (bAutoRun == false)
                {
                    int leng = str.Length;
                    //textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index]/(leng-11)), 250);
                    textPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 370);

                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, OringColor, 50);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", OringColor, 13);
                }
                
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");

                OringColor = Color.Red;
                //Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, OringColor, 2);
                if (bAutoRun == false)
                {
                    Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, OringColor, System.Drawing.Drawing2D.DashStyle.Solid);
                }
                    
                bRtn = false;
            }
            if (bAutoRun == false)
            {
                int elapsedMs = Environment.TickCount - startTime;
                // 시간 출력
                double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
                double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;


                str = $"Test Time: {elapsedMs} ms";
                Console.WriteLine(str);
                Globalo.LogPrint("", str);

                str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
                Console.WriteLine(str);
                Globalo.LogPrint("", str);

            
                System.Drawing.Point timetextPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, timetextPoint, str, Color.Blue, 15);
            }
            
            return bRtn;
        }
        public bool MilEdgeConeTest(int index, int roiIndex, System.Drawing.Point OffsetPos, bool bAutoRun = false)
        {
            int startTime = Environment.TickCount;
            bool bRtn = true;

            const int CONTOUR_MAX_RESULTS = 100;
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

            int xGap = OffsetPos.X;
            int YGap = OffsetPos.Y;

            int OffsetX = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[roiIndex].X + xGap;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[roiIndex].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            //MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref tempMilImage);
            //MIL.MbufChild2d(tempMilImage, OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);


            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);

            MIL.MbufExport("d:\\cone.BMP", MIL.M_BMP, tempMilImage);


            MilImage = tempMilImage;
            //MIL.MdispSelect(MilDisplay, MilImage);

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
            MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_SIZE, MIL.M_LESS, 300.0, MIL.M_NULL);

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
            double circleSpec = 900.0;
            Color ConeColor;
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
                Console.Write("Mean diameter of the {0} outer edges are:\n", NumResults);

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

                if (CircleErr < circleSpec)      //CircleCx < circleSpec || 
                {
                    //오링 없음
                    bRtn = false;
                }
                else
                {
                    bRtn = true;
                }
                

                //Console.WriteLine($"Height : {(maxValue - minValue)}, {(maxValue - minValue) * Globalo.visionManager.CamResol.Y}");
                //int OffsetX = 800;
                //int OffsetY = 700; 

                //Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)OffsetX, (int)(OffsetY + minValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + minValue), Color.Yellow, 1);
                //Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)OffsetX, (int)(OffsetY + maxValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + maxValue), Color.Yellow, 1);

                //Globalo.visionManager.milLibrary.DrawOverlayArrow(0, OffsetX + (OffsetWidth / 2), (int)(OffsetY + minValue), OffsetX + (OffsetWidth / 2), (int)(OffsetY + maxValue), Color.Yellow, 1);


                //int textCenterY = (int)((OffsetY + maxValue) - ((OffsetY + maxValue) - (OffsetY + minValue)) / 2);

                //int txtOffsetX = 0;
                //if (roiIndex == 2)
                //{
                //    txtOffsetX = 530;
                //}
                //System.Drawing.Point textPoint = new System.Drawing.Point(OffsetX + (OffsetWidth / 2) - txtOffsetX, textCenterY);
                //dHeight = (maxValue - minValue) * Globalo.visionManager.CamResol.Y;
                //string str = $"{dHeight.ToString("0.0##")}(mm)";
                //Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, str, Color.Blue, 15);

                ///Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Green, 2);
                Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, Color.Green, System.Drawing.Drawing2D.DashStyle.Solid);


                //System.Drawing.Point textPoint;

                //string str = $"CircleCx :{CircleCx.ToString("0.000")} / {circleSpec}";

                //textPoint = new System.Drawing.Point(10, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);


                System.Drawing.Point textPoint;

                str = $"[CONE] Circle Fit:{CircleErr.ToString("0.000")}/{circleSpec.ToString("0.00#")}";
                Console.WriteLine(str);
                //textPoint = new System.Drawing.Point(10, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 250);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 17);

                if (bRtn)
                {
                    str = $"CONE - 1";
                    ConeColor = Color.Green;
                }
                else
                {
                    str = $"CONE - 0";
                    ConeColor = Color.Red;
                }
                if (bAutoRun == false)
                {
                    int leng = str.Length;
                    //textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / (leng - 10)), 250);
                    textPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 370);
                    //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, ConeColor, 50);
                    Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", ConeColor, 13);
                }

                    
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");

                if (bAutoRun == false)
                {
                    //Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Red, 2);
                    Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, Color.Red, System.Drawing.Drawing2D.DashStyle.Solid);
                }

                    
                bRtn = false;
            }


            if (bAutoRun == false)
            {
                int elapsedMs = Environment.TickCount - startTime;
                // 시간 출력
                double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
                double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;


                str = $"Test Time: {elapsedMs} ms";
                Console.WriteLine(str);
                Globalo.LogPrint("", str);

                str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
                Console.WriteLine(str);
                Globalo.LogPrint("", str);

                System.Drawing.Point timetextPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, timetextPoint, str, Color.Blue, 15);
            }

            
            return bRtn;
        }

        public double MilEdgeHeight(int index, int roiIndex, System.Drawing.Point OffsetPos, bool bAutoRun = false)
        {
            double dHeight = 0.0;

            const int CONTOUR_MAX_RESULTS = 10;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilEdgeResult = MIL.M_NULL;                              // Edge result identifier.
            MIL_ID MilEdgeContext = MIL.M_NULL;                             // Edge context.

            //double[] MeanFeretDiameter = new double[CONTOUR_MAX_RESULTS];   // Edge mean Feret diameter.
            //double[] EdgePositionY = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeBoxMinY = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeBoxMaxY = new double[CONTOUR_MAX_RESULTS];

            int xGap = OffsetPos.X;
            int YGap = OffsetPos.Y;


            int OffsetX = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].X + xGap;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Y;

            

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref tempMilImage);


            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            // 1. 고정 임계값 128 이상만 흰색
            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_FIXED + MIL.M_GREATER, 150, MIL.M_NULL);//150

            MilImage = tempMilImage;

            MIL.MbufExport($"d:\\MimBinarize{roiIndex}.BMP", MIL.M_BMP, MilImage);
            /* Allocate a graphic list to hold the subpixel annotations to draw. */
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);
            /* Associate the graphic list to the display for annotations. */
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);
            // Allocate a Edge Finder context.
            MIL.MedgeAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_CONTOUR, MIL.M_DEFAULT, ref MilEdgeContext);

            // Allocate a result buffer.
            MIL.MedgeAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref MilEdgeResult);

            // Enable features to compute.
            MIL.MedgeControl(MilEdgeContext, MIL.M_MOMENT_ELONGATION, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_FERET_MEAN_DIAMETER + MIL.M_SORT1_DOWN, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_BOX_Y_MIN, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_BOX_Y_MAX, MIL.M_ENABLE);
            MIL.MedgeControl(MilEdgeContext, MIL.M_POSITION_Y, MIL.M_ENABLE);


            // Calculate edges and features.
            MIL.MedgeCalculate(MilEdgeContext, MilImage, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, MilEdgeResult, MIL.M_DEFAULT);

            MIL_INT NumEdgeFound = 0;                                       // Number of edges found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumEdgeFound);

            // Draw edges in the source image to show the result.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

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
            //MIL.MedgeSelect(MilEdgeResult, MIL.M_EXCLUDE, MIL.M_BOX_Y_MAX, MIL.M_LESS, 580.0, MIL.M_NULL);

            // Draw remaining edges and their index to show the result.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL_INT NumResults = 0;                                         // Number of results found.
            // Get the number of edges found.
            MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref NumResults);

            int minIndex = 0;
            int maxIndex = 0;
            double minValue = 0.0;
            double maxValue = 0.0;
            // If the right number of edges were found.
            if ((NumResults >= 1) && (NumResults <= CONTOUR_MAX_RESULTS))
            {
                // Draw the index of each edge.
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MedgeDraw(MIL.M_DEFAULT, MilEdgeResult, GraphicList, MIL.M_DRAW_INDEX, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Get the mean Feret diameters.
                //MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_FERET_MEAN_DIAMETER, MeanFeretDiameter);
                //MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, EdgePositionY);

                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_BOX_Y_MIN, EdgeBoxMinY);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_BOX_Y_MAX, EdgeBoxMaxY);

                // Print the results.
                Console.Write("Mean diameter of the {0} outer edges are:\n\n", NumResults);
                Console.Write("Index   Mean diameter \n");

                minValue = EdgeBoxMinY[0];
                maxValue = EdgeBoxMaxY[0];
                for (int i = 0; i < NumResults; i++)
                {
                    //Console.Write("{0,-11}{1,-13:0.00}\n", i, MeanFeretDiameter[i]);
                    Console.Write("{0,-11} Box Min Y : {1,-13:0.00}\n", i, EdgeBoxMinY[i]);
                    Console.Write("{0,-11} Box Max Y : {1,-13:0.00}\n", i, EdgeBoxMaxY[i]);
                    //Console.Write("{0,-11} Position Y : {1,-13:0.00}\n", i, EdgePositionY[i]);

                    if (EdgeBoxMinY[i] < minValue)
                    {
                        minValue = EdgeBoxMinY[i];
                        minIndex = i;
                    }
                    if (EdgeBoxMaxY[i] > maxValue)
                    {
                        maxValue = EdgeBoxMaxY[i];
                        maxIndex = i;
                    }
                }

                Console.WriteLine($"Min Index: {minIndex}, Min Value: {minValue}, Min Y: {minValue}");
                Console.WriteLine($"Max Index: {maxIndex}, Max Value: {maxValue}, Max Y: {maxValue}");

                double CamResolX = 0.0;
                double CamResolY = 0.0;
                CamResolX = Globalo.yamlManager.configData.CamSettings.SideResolution.X;   // 0.0186f;
                CamResolY = Globalo.yamlManager.configData.CamSettings.SideResolution.Y;   //0.0186f;

                Console.WriteLine($"CamResolX:{CamResolX}");
                Console.WriteLine($"CamResolY:{CamResolY}");

                Console.WriteLine($"Height : {(maxValue - minValue)}, {(maxValue - minValue) * CamResolY}");
                //int OffsetX = 800;
                //int OffsetY = 700; 
                
                Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + minValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + minValue), Color.Yellow, 1);
                Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + maxValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + maxValue), Color.Yellow, 1);
                Globalo.visionManager.milLibrary.DrawOverlayArrow(index, OffsetX + (OffsetWidth/2), (int)(OffsetY + minValue), OffsetX + (OffsetWidth / 2), (int)(OffsetY + maxValue), Color.Yellow, 1);

                int textCenterY = (int)((OffsetY + maxValue) - ((OffsetY + maxValue) - (OffsetY + minValue)) / 2);

                int txtOffsetX = 0;
                int txtOffsetY = 0;
                if (roiIndex == 2)
                {
                    if (bAutoRun)
                    {
                        txtOffsetX = 610;
                    }
                    else
                    {
                        txtOffsetX = 380;
                    }
                    
                }
                if (roiIndex == 0 || roiIndex == 2)
                {
                    txtOffsetY = 170;
                }
                if (roiIndex == 1)
                {
                    txtOffsetY = -200;
                }
                System.Drawing.Point textPoint = new System.Drawing.Point(OffsetX + (OffsetWidth / 2) - txtOffsetX, textCenterY - txtOffsetY);

                dHeight = (maxValue - minValue) * CamResolY;
                string str = $"{dHeight.ToString("0.0###")}(mm)";
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Yellow, 11);
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", Color.Yellow, 11);

            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");
            }

            return dHeight;
        }
        public bool HeightTest(int index)
        {
            int startTime = Environment.TickCount;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(index);
            double[] heightData = new double[3];


            OpenCvSharp.Point markPos = new OpenCvSharp.Point();
            double score = 0.0;
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(index, VisionClass.eMarkList.SIDE_HEIGHT, ref markPos, ref score);


            System.Drawing.Point OffsetPos = new System.Drawing.Point(0, 0);
            if (bRtn)
            {
                OffsetPos.X = markPos.X - Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].X;
                OffsetPos.Y = markPos.Y - Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].Y;

            }
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(index);
            //

            heightData[0] = MilEdgeHeight(index, 0, OffsetPos);
            heightData[1] = MilEdgeHeight(index, 1, OffsetPos);
            heightData[2] = MilEdgeHeight(index, 2, OffsetPos);


            string str = "";
            string csvLine = $"{heightData[0]:F3},{heightData[1]:F3},{heightData[2]:F3}";

            string filePath = "_HEIGHT_data.csv";
            // 파일이 없으면 헤더 추가
            if (!File.Exists(filePath))
            {
                File.AppendAllText(filePath, "LH,MH,RH" + Environment.NewLine);
            }
            try
            {
                File.AppendAllText(filePath, csvLine + Environment.NewLine);
            }
            catch (IOException)
            {

            }


            int elapsedMs = Environment.TickCount - startTime;
            // 시간 출력
            double elapsedMilliseconds = TeststopWatch.Elapsed.TotalMilliseconds;
            double elapsedSeconds = TeststopWatch.Elapsed.TotalSeconds;


            str = $"Test Time: {elapsedMs} ms";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            str = $"Test Time: {elapsedMs / 1000.0:F3}(s)";
            Console.WriteLine(str);
            Globalo.LogPrint("", str);

            System.Drawing.Point timetextPoint = new System.Drawing.Point(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] - 950, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, timetextPoint, str, Color.Blue, 15);


            Globalo.visionManager.milLibrary.DrawOverlayAll(index);
            return true;
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
