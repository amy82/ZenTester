using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;

namespace ZenHandler.VisionClass
{
    public class AoiTester
    {
        public AoiTester()
        {

        }
        public void CirCleFind(int index)
        {
            Console.WriteLine("CirCleFind");
            Rectangle m_clRectCalRoi = new Rectangle((int)(100), (int)(100), 1000, 1000);

            //BINARY START
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref MilSubImage01);
            MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], MilSubImage01, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            MIL.MimRank(MilSubImage01, MilSubImage01, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);
            //BINARY END

            MIL.MbufExport("D:\\TEST.BMP", MIL.M_BMP, MilSubImage01);
        }

        public void Con1Test(int index)
        {
            Console.WriteLine("Con1Test");

            int mThreshold = 70;

            MIL_ID MilBinary = MIL.M_NULL;
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref MilBinary);
            MIL.MimBinarize(MilBinary, MilBinary, MIL.M_GREATER_OR_EQUAL, mThreshold, MIL.M_NULL);	//! 이진화 반전 140 M_GREATER_OR_EQUAL	M_LESS_OR_EQUAL
            //MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], MilBinary, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);
            //MIL.MimRank(MilBinary, MilBinary, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);


            MIL.MbufChild2d(MilBinary, 1000, 500, 2150, 1900, ref MilSubImage01);
            MIL.MbufExport("D:\\Con1Test.BMP", MIL.M_BMP, MilSubImage01);
            //MIL_ID MilOverlayImage = MIL.M_NULL;       // Overlay buffer identifier.
            MIL_ID MilBeadContext = MIL.M_NULL;        // Bead context identifier.
            MIL_ID MilBeadResult = MIL.M_NULL;         // Bead result identifier.

             // Template attributes definition.
            double CIRCLE_CENTER_X = 1030;
            double CIRCLE_CENTER_Y = 930.0;
            double CIRCLE_RADIUS = 800.0;
            double EDGE_THRESHOLD_VALUE = 5.0;
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

            // 각각의 원 좌표를 가져오기
            int count = (int)numBeads;

            try
            {
                if (count > 0)
                {
                    double xpos = 0.0;
                    double ypos = 0.0;

                    double[] UserVarPtrX = new double[count];
                    double[] UserVarPtrY = new double[count];
                    double[] Offseta = new double[count];
                    double[] anglea = new double[count];

                    long numX = 0, numAngle = 0;
                    long countX = 0;
                    long resultCount = 0;

                    MIL.MbeadGetResult(MilBeadResult, MIL.M_TEMPLATE_LABEL(1), MIL.M_DEFAULT, MIL.M_NUMBER_FOUND + MIL.M_TYPE_MIL_INT + MIL.M_POSITION_X, ref numAngle);

                    MIL.MbeadGetResult(MilBeadResult, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NUMBER_FOUND + MIL.M_TYPE_MIL_INT, ref resultCount);
                    Console.WriteLine("전체 결과 개수: " + resultCount);




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
        }
    }
}
