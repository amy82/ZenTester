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
        public bool MilEdgeOringTest(int index, int roiIndex)//, Mat srcImage)
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

            int OffsetX = Globalo.yamlManager.aoiRoiConfig.ORING_ROI[roiIndex].X;
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


                int leng = str.Length;
                //textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index]/(leng-11)), 250);
                textPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 370);

                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, OringColor, 50);
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", OringColor, 13);
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");

                OringColor = Color.Red;
                //Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, OringColor, 2);
                Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, OringColor, System.Drawing.Drawing2D.DashStyle.Solid);
                bRtn = false;
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
            return bRtn;
        }
        public bool MilEdgeConeTest(int index, int roiIndex)
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


            int OffsetX = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[roiIndex].X;
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


                int leng = str.Length;
                //textPoint = new System.Drawing.Point((int)(Globalo.visionManager.milLibrary.CAM_SIZE_X[index] / (leng - 10)), 250);
                textPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[index] - 370);
                //Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, ConeColor, 50);
                Globalo.visionManager.milLibrary.m_clMilDrawText[index].AddList(textPoint, str, "나눔고딕", ConeColor, 13);
            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");


                //Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Red, 2);
                Globalo.visionManager.milLibrary.m_clMilDrawBox[index].AddList(m_clRect, 2, Color.Red, System.Drawing.Drawing2D.DashStyle.Solid);
                bRtn = false;
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
            return bRtn;
        }

        public double MilEdgeHeight(int index, int roiIndex)
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

            int OffsetX = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].X;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[roiIndex].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[index], OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref tempMilImage);


            //MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            // 1. 고정 임계값 128 이상만 흰색
            MIL.MimBinarize(tempMilImage, tempMilImage, MIL.M_FIXED + MIL.M_GREATER, 150, MIL.M_NULL);

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
                if (roiIndex == 2)
                {
                    txtOffsetX = 530;
                }
                System.Drawing.Point textPoint = new System.Drawing.Point(OffsetX + (OffsetWidth / 2) - txtOffsetX, textCenterY);
                dHeight = (maxValue - minValue) * CamResolY;
                string str = $"{dHeight.ToString("0.0##")}(mm)";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Blue, 15);

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
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            double[] heightData = new double[3];

            heightData[0] = MilEdgeHeight(index, 0);
            heightData[1] = MilEdgeHeight(index, 1);
            heightData[2] = MilEdgeHeight(index, 2);


            string str = "";
            string csvLine = $"{heightData[0]:F3},{heightData[1]:F3},{heightData[2]:F3}";

            string filePath = "data.csv";
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
            return true;

            const int CONTOUR_MAX_RESULTS = 10;
            MIL_ID MilDisplay = MIL.M_NULL;
            MIL_ID tempMilImage = MIL.M_NULL;
            MIL_ID MilImage = MIL.M_NULL;
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID MilEdgeResult = MIL.M_NULL;                              // Edge result identifier.
            MIL_ID MilEdgeContext = MIL.M_NULL;                             // Edge context.

            


            double[] MeanFeretDiameter = new double[CONTOUR_MAX_RESULTS];   // Edge mean Feret diameter.
            double[] EdgeBoxMinY = new double[CONTOUR_MAX_RESULTS];
            double[] EdgeBoxMaxY = new double[CONTOUR_MAX_RESULTS];
            double[] EdgePositionY = new double[CONTOUR_MAX_RESULTS];
            int i = 0;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref tempMilImage);

            MIL_INT ImageSizeX = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageSizeY = MIL.MbufInquire(tempMilImage, MIL.M_SIZE_Y, MIL.M_NULL);

            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], tempMilImage);
            //MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, ImageSizeX, ImageSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);



            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            //  ROI 설정
            //
            //
            //
            //
            int OffsetX = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].X;
            int OffsetY = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].Y;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].Height;
            MIL.MbufChild2d(tempMilImage, OffsetX, OffsetY, OffsetWidth, OffsetHeight, ref MilImage);
            MIL.MimBinarize(MilImage, MilImage, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);

            //MIL.MbufExport("d:\\MilImage.BMP", MIL.M_BMP, MilImage);
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
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_FERET_MEAN_DIAMETER, MeanFeretDiameter);

                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_BOX_Y_MIN, EdgeBoxMinY);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_BOX_Y_MAX, EdgeBoxMaxY);
                MIL.MedgeGetResult(MilEdgeResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, EdgePositionY);

                // Print the results.
                Console.Write("Mean diameter of the {0} outer edges are:\n\n", NumResults);
                Console.Write("Index   Mean diameter \n");

                minValue = EdgeBoxMinY[0];
                maxValue = EdgeBoxMaxY[0];
                for (i = 0; i < NumResults; i++)
                {
                    Console.Write("{0,-11}{1,-13:0.00}\n", i, MeanFeretDiameter[i]);
                    Console.Write("{0,-11} Box Min Y : {1,-13:0.00}\n", i, EdgeBoxMinY[i]);
                    Console.Write("{0,-11} Box Max Y : {1,-13:0.00}\n", i, EdgeBoxMaxY[i]);
                    Console.Write("{0,-11} Position Y : {1,-13:0.00}\n", i, EdgePositionY[i]);

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
                CamResolX = Globalo.yamlManager.configData.CamSettings.SideResolution.X;   // 0.02026f; 
                CamResolY = Globalo.yamlManager.configData.CamSettings.SideResolution.Y;   //0.02026f;//0.0288f;

                Console.WriteLine($"CamResolX:{CamResolX}");
                Console.WriteLine($"CamResolY:{CamResolY}");

                Console.WriteLine($"Height : {(maxValue - minValue)}, {(maxValue - minValue) * CamResolY}");
                //int OffsetX = 800;
                //int OffsetY = 700; 
                Globalo.visionManager.milLibrary.ClearOverlay(index);
                Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + minValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + minValue), Color.Yellow, 1);
                Globalo.visionManager.milLibrary.DrawOverlayLine(index, (int)OffsetX, (int)(OffsetY + maxValue), (int)(OffsetX + OffsetWidth), (int)(OffsetY + maxValue), Color.Yellow, 1);

                Globalo.visionManager.milLibrary.DrawOverlayArrow(index, OffsetX, (int)(OffsetY + minValue), OffsetX, (int)(OffsetY + maxValue), Color.Yellow, 1);


                int textCenterY = (int)((OffsetY + maxValue) - ((OffsetY + maxValue) - (OffsetY + minValue))/2);

                System.Drawing.Point textPoint = new System.Drawing.Point(OffsetX, textCenterY);
                double dis = (maxValue - minValue) * CamResolY;
                str = $"{dis.ToString("0.0##")} [mm]";
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Yellow, 15);

            }
            else
            {
                Console.Write("Edges have not been found or the number of found edges is greater than\n");
                Console.Write("the specified maximum number of edges !\n\n");
            }


            return false;
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

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, Globalo.visionManager.milLibrary.CAM_SIZE_X[index], Globalo.visionManager.milLibrary.CAM_SIZE_Y[index],
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
        public bool CvHeightTest(int index, Mat srcImage)
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
