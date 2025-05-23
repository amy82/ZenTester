﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using ZenHandler.Data;

namespace ZenHandler.VisionClass
{
    public class CDMotor
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double T { get; set; }

        public CDMotor()
        {
            X = 0.0;
            Y = 0.0;
            T = 0.0;
        }
    }
    public class MarkUtil
    {
        
        public MIL_ID m_MilModModel;
        public MIL_ID m_MilModResult;

        public MIL_ID[] m_MilMarkOverlay;
        public MIL_ID[] m_MilMarkImage;
        public MIL_ID[] m_MilMarkDisplay;
        public MIL_INT m_lTransparentColor;


        public System.Drawing.Point m_clPtMarkSize = new System.Drawing.Point();
        public System.Drawing.Point m_clPtMarkStartPos = new System.Drawing.Point();
        private System.Drawing.Point smallDispSize = new System.Drawing.Point();        //SetControl의 작은 마크 선택 사이즈

        public int m_nSmooth = 10;
        public MarkUtil()
        {
            int i = 0;
            m_MilMarkImage = new MIL_ID[2];
            m_MilMarkDisplay = new MIL_ID[2];
            m_MilMarkOverlay = new MIL_ID[2];
            for (i = 0; i < 2; i++)
            {
                m_MilMarkImage[i] = MIL.M_NULL;
                m_MilMarkDisplay[i] = MIL.M_NULL;
                m_MilMarkOverlay[i] = MIL.M_NULL;
            }
            m_MilModModel = MIL.M_NULL;
            m_MilModResult = MIL.M_NULL;
            
            m_lTransparentColor = MIL.M_NULL;
            

            smallDispSize.X = Globalo.setTestControl.panel_Mark.Width;
            smallDispSize.Y = Globalo.setTestControl.panel_Mark.Height;
            InitMarkViewDlg();

        }
        private void InitMarkViewDlg()
        {
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1L, smallDispSize.X, smallDispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilMarkImage[0]);
            if (m_MilMarkImage[0] != MIL.M_NULL)
            {
                m_MilMarkDisplay[0] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_NULL);

                MIL_INT DisplayType = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(m_MilMarkDisplay[0]);
                    m_MilMarkDisplay[0] = MIL.M_NULL;

                    return;
                }
            }

            if (m_MilMarkDisplay[0] != MIL.M_NULL)
            {
                MIL.MdispSelectWindow(m_MilMarkDisplay[0], m_MilMarkImage[0], Globalo.setTestControl.panel_Mark.Handle);
                MarkEnableOverlay();
            }

            //DisplayMarkView(m_nUnit, m_nMarkNo, m_clMaskViewDlg.m_iMarkSetSizeX, m_clMaskViewDlg.m_iMarkSetSizeY);
            //this->ShowMarkNo();
        }
        public void DisplayMarkView(string markName, int nMarkNo, double dZoomMarkWidth, double dZoomMarkHeight)
        {
            double dZoomX = 0.0;
            double dZoomY = 0.0;

            double dSizeX = 0.0;
            double dSizeY = 0.0;

            double dCenterX = 0.0;
            double dCenterY = 0.0;

            string szPath = "";
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, g_clSysData.m_szModelName, nMarkNo + 1);
            //szPath = "d:\\MARK.bmp";

            string filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, markName, $"Mark-{nMarkNo}.mod");       //LOT DATA
            MIL.MbufClear(m_MilMarkOverlay[0], m_lTransparentColor);
            MIL.MbufClear(m_MilMarkImage[0], 192);

            //if (filePath)       //szPath 파일이 있으면
            if (File.Exists(filePath))
            {
                MIL.MbufImport(szPath, MIL.M_BMP, MIL.M_LOAD, MIL.M_NULL, ref m_MilMarkImage[1]);

                //MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, m_MilMarkImage[0], MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref dSizeX);       //<-----여기서왜 마크 사이즈를 바꾸지?
                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref dSizeY);

                m_clPtMarkSize.X = (int)dSizeX;
                m_clPtMarkSize.Y = (int)dSizeY;

                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, ref dCenterX);
                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, ref dCenterY);

                //m_clPtMarkCenterPos.X = (int)dCenterX;        //필요없는듯
                //m_clPtMarkCenterPos.Y = (int)dCenterY;

                dZoomX = smallDispSize.X / dZoomMarkWidth;
                dZoomY = smallDispSize.Y / dZoomMarkHeight;

                MIL.MimResize(m_MilMarkImage[1], m_MilMarkImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);

                DrawMarkOverlay(nMarkNo);	//<----여기서 작은 마크에 Line Draw
            }

            //m_nMarkNo = nMarkNo;
            //g_clTaskWork[m_nUnit].m_ManualMarkIndex = m_nMarkNo;
        }
        public void DrawMarkOverlay(int nNo)
        {
            double dSizeX = 0.0;
            double dSizeY = 0.0;

            System.Drawing.Point clDptCenter = new System.Drawing.Point();

            double m_dZoomX = 0.0;
            double m_dZoomY = 0.0;


            MIL.MbufClear(m_MilMarkOverlay[0], m_lTransparentColor);

            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref dSizeX);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref dSizeY);

            m_dZoomX = (double)smallDispSize.X / (double)dSizeX;      //마크 이미지 축소 OR 확대
            m_dZoomY = (double)smallDispSize.Y / (double)dSizeY;

            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX); //M_DRAW_SCALE_X
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY); //M_DRAW_SCALE_Y


            double m_clCdCenterX = 0.0;
            double m_clCdCenterY = 0.0;
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, ref m_clCdCenterX);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, ref m_clCdCenterY);

            clDptCenter.X = (int)m_clCdCenterX;
            clDptCenter.Y = (int)m_clCdCenterY;

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, m_MilMarkOverlay[0], MIL.M_DRAW_DONT_CARE, MIL.M_DEFAULT, MIL.M_DEFAULT);//<---노란 마스크 영역


            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_MAGENTA);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX);   //M_DRAW_SCALE_X
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY);   //M_DRAW_SCALE_Y

            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nSmooth);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, m_MilMarkOverlay[0], MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);//<-----EDGE 영역

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);


            //	m_dZoomX = (double)g_clModelFinder.m_clPtSmallMarkDispSize.x / (double)g_clModelFinder.m_clPtZoomMarkDispSize.x;		//마크 이미지 축소 OR 확대
            //m_dZoomY = (double)g_clModelFinder.m_clPtSmallMarkDispSize.y / (double)g_clModelFinder.m_clPtZoomMarkDispSize.y;

            int dCenterX = (int)(clDptCenter.X * m_dZoomX);
            int dCenterY = (int)(clDptCenter.Y * m_dZoomY);

            MIL.MgraLine(MIL.M_DEFAULT, m_MilMarkOverlay[0], dCenterX, 0, dCenterX, dSizeY);    //<-----Center Cross x축
            MIL.MgraLine(MIL.M_DEFAULT, m_MilMarkOverlay[0], 0, dCenterY, dSizeX, dCenterY);    //<-----Center Cross y축
        }
        public bool RegisterMark(int index, double dStartX, double dStartY, double dSizeX, double dSizeY)
        {
            MIL_ID MilTempImage = MIL.M_NULL;
            if (m_MilModModel != MIL.M_NULL)
            {
                MIL.MmodFree(m_MilModModel);
                m_MilModModel = MIL.M_NULL;
            }

            

            m_clPtMarkStartPos.X = (int)(dStartX * Globalo.visionManager.milLibrary.xExpand[index]);
            m_clPtMarkStartPos.Y = (int)(dStartY * Globalo.visionManager.milLibrary.yExpand[index]);

            m_clPtMarkSize.X = (int)(dSizeX * Globalo.visionManager.milLibrary.xExpand[index]);
            m_clPtMarkSize.Y = (int)(dSizeY * Globalo.visionManager.milLibrary.yExpand[index]);

            MIL.MbufClear(MilTempImage, 0);

            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1L, Globalo.visionManager.milLibrary.CAM_SIZE_X[0], Globalo.visionManager.milLibrary.CAM_SIZE_Y[0], 
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_DISP + MIL.M_PROC, ref MilTempImage);

            MIL.MmodAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_GEOMETRIC, MIL.M_DEFAULT, ref m_MilModModel);
            MIL.MmodDefine(m_MilModModel, MIL.M_IMAGE, Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], m_clPtMarkStartPos.X, m_clPtMarkStartPos.Y, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, MilTempImage, MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, m_clPtMarkSize.X / 2);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, m_clPtMarkSize.Y / 2);



            //------------------------------------------------------------------------------------------------------
            //
            //
            //  MASK VIEW
            //
            //------------------------------------------------------------------------------------------------------
            Globalo.visionManager.markViewer.InitMaskViewDlg(0, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            Globalo.visionManager.markViewer.ShowDialog();

            return false;
        }

        //-----------------------------------------------------------------------------
        //
        //	마크 속성 세팅
        //
        //-----------------------------------------------------------------------------
        public void SettingFindMark(int nNo = 0)
        {

            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_POSITION_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_ANGLE_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_SCALE_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_NUMBER, 3/*1*/);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SPEED, MIL.M_MEDIUM/*M_HIGH*/);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_ACCURACY, MIL.M_HIGH);// M_MEDIUM);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SHARED_EDGES, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_ASPECT_RATIO, MIL.M_DEFAULT);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SAVE_TARGET_EDGES, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_TARGET_CACHING, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nSmooth);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_DETAIL_LEVEL, MIL.M_MEDIUM);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_FILTER_MODE, MIL.M_RECURSIVE);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE, 0);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE_DELTA_NEG, 10);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE_DELTA_POS, 10);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE, 1.0);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE_MIN_FACTOR, 0.99);
            /////MIL.MmodControl(m_MilModModel[nUnit][nNo], M_DEFAULT, M_SCALE_MAX_FACTOR, 1.01);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE_MAX_FACTOR, 1.2);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ACCEPTANCE, 90);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ACCEPTANCE_TARGET, 80);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_CERTAINTY, 90);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_CERTAINTY_TARGET, 80);
        }

        //FindModel(int nUnit, int nNo, bool bAreaFlag, CDPoint& clFindPos, double& dScore, double& dAngle, double& dFitError, CDPoint& clMarkSize, CDPoint& clMarkCenter)
        public bool FindModel(int index , int nNo, bool bAreaFlag , Rectangle m_clRectRoi, OpenCvSharp.Point2d dFindPos)
        {
            OpenCvSharp.Point2d clMarkSize = new OpenCvSharp.Point2d();
            OpenCvSharp.Point2d clMarkCenter = new OpenCvSharp.Point2d();

            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref clMarkSize.X);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref clMarkSize.Y);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, ref clMarkCenter.X);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, ref clMarkCenter.Y);

            if (m_MilModResult == MIL.M_NULL)
            {
                MIL.MmodFree(m_MilModResult);
                m_MilModResult = MIL.M_NULL;

                MIL.MmodAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref m_MilModResult);
            }

            MIL.MmodPreprocess(m_MilModModel, MIL.M_DEFAULT);
            if (bAreaFlag == true)
            {
                MIL_ID MilChildLow = MIL.M_NULL;
                MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, m_clRectRoi.Width, m_clRectRoi.Height, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilChildLow);
                MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[0], m_clRectRoi.X, m_clRectRoi.Y, m_clRectRoi.Width, m_clRectRoi.Height, ref MilChildLow);
                MIL.MbufExport("D:\\__MilChildLow.BMP", MIL.M_BMP, MilChildLow);

                MIL.MmodFind(m_MilModModel, MilChildLow, m_MilModResult);
                if (MilChildLow != MIL.M_NULL)
                {
                    MIL.MbufFree(MilChildLow);
                }

                
            }
            else
            {
                MIL.MmodFind(m_MilModModel, Globalo.visionManager.milLibrary.MilProcImageChild[0], m_MilModResult);
            }

            MIL_ID lObbjNo = MIL.M_NULL;

            MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_NUMBER + MIL.M_TYPE_LONG, ref lObbjNo);

            if (lObbjNo < 1)
            {
                return false;
            }

            double[] Find_x = new double[5];
            double[] Find_y = new double[5];
            double[] Find_angle = new double[5];
            double[] Find_Score = new double[5];
            double[] Find_ScoreTarget = new double[5];
            double[] Find_FitError = new double[5];
            if (lObbjNo == 1)
            {
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_POSITION_X, Find_x);
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_POSITION_Y, Find_y);
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_ANGLE, Find_angle);
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_SCORE, Find_Score);
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_SCORE_TARGET, Find_ScoreTarget);
                MIL.MmodGetResult(m_MilModResult, MIL.M_DEFAULT, MIL.M_FIT_ERROR, Find_FitError);
            }
            else
            {

                return false;
            }

            int maxObjNum = 0;
            if (bAreaFlag)
            {
                dFindPos.X = Find_x[maxObjNum] + m_clRectRoi.X;
                dFindPos.Y = Find_y[maxObjNum] + m_clRectRoi.Y;
            }
            else
            {
                dFindPos.X = Find_x[maxObjNum];
                dFindPos.Y = Find_y[maxObjNum];
            }
            //dScore = Find_Score[maxObjNum];
            //dAngle = Find_angle[maxObjNum];
            //dFitError = Find_FitError[maxObjNum];
            return false;
        }
        public bool CalcSingleMarkAlign(int index, int MarkNo, ref CDMotor dAlign)
        {
            bool bFind = false;
            dAlign.X = 0.0;
            dAlign.Y = 0.0;
            dAlign.T = 0.0;

            OpenCvSharp.Point2d dFindPos = new OpenCvSharp.Point2d();
            Rectangle m_clRoi = new Rectangle();

            m_clRoi.X = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].X;
            m_clRoi.Y = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Y;

            m_clRoi.Width = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Width;
            m_clRoi.Height = Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Height;

            bFind = FindModel(index , MarkNo, true, m_clRoi, dFindPos);
            if (bFind)
            {

            }
            else
            {

            }

            MIL.MmodControl(m_MilModResult, MIL.M_DEFAULT, 319L, m_clRoi.X * -1);//M_DRAW_RELATIVE_ORIGIN_X	//- ROI 영역 Offset
            MIL.MmodControl(m_MilModResult, MIL.M_DEFAULT, 320L, m_clRoi.Y * -1);//M_DRAW_RELATIVE_ORIGIN_Y

            MIL.MmodControl(m_MilModResult, MIL.M_DEFAULT, 3203L, Globalo.visionManager.milLibrary.xReduce[0]);//M_DRAW_SCALE_X
            MIL.MmodControl(m_MilModResult, MIL.M_DEFAULT, 3204L, Globalo.visionManager.milLibrary.yReduce[0]);//M_DRAW_SCALE_Y

            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModResult, Globalo.visionManager.milLibrary.MilSetCamOverlay, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION + MIL.M_DRAW_EDGES + MIL.M_DRAW_AXIS, MIL.M_DEFAULT, MIL.M_DEFAULT);
            return false;
        }
        public void ViewMarkMask()
        {
            double dSizeX = 0.0;
            double dSizeY = 0.0;

            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref dSizeX);     //<-----여기서왜 마크 사이즈를 바꾸지?
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref dSizeY);

            Globalo.visionManager.markViewer.InitMaskViewDlg(0, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            Globalo.visionManager.markViewer.ShowDialog();
        }
        public bool SaveMark(string markName, int nNo)
        {
            string filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, markName);       //LOT DATA
            if (!Directory.Exists(filePath)) // 폴더가 존재하지 않으면
            {
                Directory.CreateDirectory(filePath); // 폴더 생성
            }
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.mod"), BASE_MODEL_PATH, (TCHAR*)(LPCTSTR)sModelName, nNo + 1);
            filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, markName, $"Mark-{nNo}.mod");       //LOT DATA


            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nSmooth);
            //m_bMarkState[nUnit][nNo] = true;
            MIL.MmodSave(filePath, m_MilModModel, MIL.M_DEFAULT);


            // BMP 
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, (TCHAR*)(LPCTSTR)sModelName, nNo + 1);
            filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, markName, $"Mark-{nNo}.bmp");       //LOT DATA
            MIL.MbufExport(filePath, MIL.M_BMP, m_MilMarkImage[1]);
            return false;
        }
        public void MarkEnableOverlay()
        {
            if (m_MilMarkDisplay[0] != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
                {
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_WINDOW_SHOW, MIL.M_ENABLE);
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY, MIL.M_ENABLE);
                    MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_OVERLAY_ID, ref m_MilMarkOverlay[0]);
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY_SHOW, MIL.M_DISABLE);

                    m_lTransparentColor = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);

                    MIL.MbufClear(m_MilMarkOverlay[0], m_lTransparentColor);

                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
                    MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);

                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                }

                //if (m_bInitMarkOverlay)
                //{
                //    if (m_bEnableMarkOverlay == false)
                //    {
                //        MdispControl(m_MilMarkDisplay[0], M_OVERLAY_SHOW, M_ENABLE);
                //    }
                //    else
                //    {
                //        MdispControl(m_MilMarkDisplay[0], M_OVERLAY_SHOW, M_DISABLE);
                //        m_bEnableMarkOverlay = FALSE;
                //    }

                //    m_bInitMarkOverlay = FALSE;
                //}
            }
        }
    }
}
