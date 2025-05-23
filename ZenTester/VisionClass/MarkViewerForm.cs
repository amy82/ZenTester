﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
namespace ZenHandler.VisionClass
{
    public partial class MarkViewerForm : Form
    {
        private OpenCvSharp.Point2d DispSize = new OpenCvSharp.Point2d();
        private System.Drawing.Point m_clPtMarkSize = new System.Drawing.Point();
        private System.Drawing.Point m_clCdCenter = new System.Drawing.Point();
        private MIL_ID m_MilMaskOverlay;
        private MIL_ID m_MilMask;
        private MIL_INT m_MilTransparentColor;

        private bool m_bInitOverlay = false;
        byte[] m_pMaskBuff = null;

        private double m_dZoomX = 0.0;
        private double m_dZoomY = 0.0;
        private bool m_bDrawEdge = true;
        private int m_nBrushSize = 10;
        private int m_nEdgeSmooth = 10;
        private bool m_bMaskDrag = false;
        private bool m_bEraseMask = false;
        public MarkViewerForm()
        {
            InitializeComponent();

            this.CenterToScreen();

            m_MilMask = MIL.M_NULL;
            m_MilMaskOverlay = MIL.M_NULL;
            m_MilTransparentColor = MIL.M_NULL;

            DispSize.X = (double)panel_MarkZoomImage.Width;
            DispSize.Y = (double)panel_MarkZoomImage.Height;

            trackBar_Mask_Brush_Size.Value = m_nBrushSize;
            label_Mask_Brush_Size_Val.Text = m_nBrushSize.ToString();
            label_Mask_Edge_Smooth_Val.Text = m_nEdgeSmooth.ToString();

            //MbufAllocColor(g_clVision.m_MilSystem[0], 1L, CCD1_CAM_SIZE_X, CCD1_CAM_SIZE_Y, (8 + M_UNSIGNED), M_IMAGE + M_DISP + M_PROC, &g_clModelFinder.m_MilMarkImage[1]);
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1, Globalo.visionManager.milLibrary.CAM_SIZE_X[0], Globalo.visionManager.milLibrary.CAM_SIZE_Y[0], (8 + MIL.M_UNSIGNED), 
                MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage[1]);


            if (Globalo.visionManager.markUtil.m_MilMarkImage[1] != MIL.M_NULL)
            {
                Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_NULL);
                MIL_INT DisplayType = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(Globalo.visionManager.markUtil.m_MilMarkDisplay[1]);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.M_NULL;
                    return;
                }
            }
            //panel_MarkZoomImage
        }

        public void DisplayMarkView(int nMarkNo, double dZoomMarkWidth, double dZoomMarkHeight)
        {
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, g_clSysData.m_szModelName, nMarkNo + 1);

            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkOverlay[0], Globalo.visionManager.markUtil.m_lTransparentColor);
            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkImage[1], 192);
        }
        public void InitMaskViewDlg(int nMarkNo, int m_iSizeX , int m_iSizeY)
        {
            //panel_MarkZoomImage
            
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, (int)DispSize.X, (int)DispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage[1]);

            if (Globalo.visionManager.markUtil.m_MilMarkImage[1] != MIL.M_NULL)
            {
                if (Globalo.visionManager.markUtil.m_MilMarkDisplay[1] != MIL.M_NULL)
                {
                    MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], 0);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.M_NULL;
                    m_MilMaskOverlay = MIL.M_NULL;
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, MIL.M_NULL);

                    MIL.MdispSelectWindow(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], Globalo.visionManager.markUtil.m_MilMarkImage[1], panel_MarkZoomImage.Handle);
                }
            }


            MaskEnableOverlay();


            //m_nUnit = nUnit;
            //m_nMarkNo = nMarkNo;

            m_bMaskDrag = false;
            m_bInitOverlay = false;
            //m_bEnableOverlay = false;

            m_pMaskBuff = null;
            //m_nEdgeSmooth = g_clMarkData[nUnit].m_nSmooth[nMarkNo];

            m_dZoomX = DispSize.X / (double)m_iSizeX;      //마크 이미지 축소 OR 확대
            m_dZoomY = DispSize.Y / (double)m_iSizeY;

            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX);//M_DRAW_SCALE_X
            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY);//M_DRAW_SCALE_Y


            MIL.MmodDraw(MIL.M_DEFAULT, Globalo.visionManager.markUtil.m_MilModModel, Globalo.visionManager.markUtil.m_MilMarkImage[1], MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL_INT m_clCdCenterX = MIL.M_NULL;
            MIL_INT m_clCdCenterY = MIL.M_NULL;
            double dCenterX = 0.0;
            double dCenterY = 0.0;

            MIL.MmodInquire(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X + MIL.M_TYPE_DOUBLE, ref dCenterX);  //드래그된 영역에서 중심 X
            MIL.MmodInquire(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y + MIL.M_TYPE_DOUBLE, ref dCenterY); //드래그된 영역에서 중심 Y

            m_clCdCenter.X = (int)dCenterX;
            m_clCdCenter.Y = (int)dCenterY;

            m_clPtMarkSize.X = (int)(m_iSizeX + 0.5);
            m_clPtMarkSize.Y = (int)(m_iSizeY + 0.5);
            // 마스크 이미지 초기화
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1, m_iSizeX, m_iSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilMask);

            if (m_MilMask != MIL.M_NULL)
            {
                m_pMaskBuff = new byte[m_iSizeX * m_iSizeY];
                // m_pMaskBuff = (unsigned char*)malloc(m_iSizeX * m_iSizeY * sizeof(unsigned char));
                //memset(m_pMaskBuff, 0, (m_iSizeX * m_iSizeY * sizeof(unsigned char)));
            }
            // 센터라인 그리기
            DrawCenterLine(m_clCdCenter);
        }

        private void MaskEnableOverlay()
        {
            if (m_bInitOverlay == false)
            {
                m_bInitOverlay = true;

                if (Globalo.visionManager.markUtil.m_MilMarkDisplay[1] != MIL.M_NULL)
                {
                    MIL_INT DisplayType = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                    if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
                    {
                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY, MIL.M_ENABLE);
                        MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_ID, ref m_MilMaskOverlay);

                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

                        m_MilTransparentColor = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);

                        MIL.MbufClear(m_MilMaskOverlay, m_MilTransparentColor);
                        MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
                    }
                }
            }
        }

        private void DrawMask()
        {
            MIL.MbufClear(m_MilMask, 0);
            if (Globalo.visionManager.markUtil.m_MilModModel != MIL.M_NULL)
            {
                double m_dZoomX = DispSize.X / (double)Globalo.visionManager.markUtil.m_clPtMarkSize.X;      //마크 이미지 축소 OR 확대 
                double m_dZoomY = DispSize.Y / (double)Globalo.visionManager.markUtil.m_clPtMarkSize.Y;
                double m_dSmallX = (double)Globalo.visionManager.markUtil.m_clPtMarkSize.X / DispSize.X;      //마크 이미지 축소 OR 확대 
                double m_dSmallY = (double)Globalo.visionManager.markUtil.m_clPtMarkSize.Y / DispSize.Y;

                MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3203L, 1.0);//M_DRAW_SCALE_X
                MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3204L, 1.0);//M_DRAW_SCALE_Y 

                MIL.MmodDraw(MIL.M_DEFAULT, Globalo.visionManager.markUtil.m_MilModModel, m_MilMask, MIL.M_DRAW_DONT_CARE, MIL.M_DEFAULT, MIL.M_DEFAULT);
                MIL.MbufGet(m_MilMask, m_pMaskBuff);

                MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX);//M_DRAW_SCALE_X
                MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY);//M_DRAW_SCALE_Y
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_CYAN);// M_COLOR_GREEN);
                MIL.MmodDraw(MIL.M_DEFAULT, Globalo.visionManager.markUtil.m_MilModModel, m_MilMaskOverlay, MIL.M_DRAW_DONT_CARE, MIL.M_DEFAULT, MIL.M_DEFAULT);

                if (m_bDrawEdge)
                {
                    MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_MAGENTA);
                    MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nEdgeSmooth);
                    MIL.MmodDraw(MIL.M_DEFAULT, Globalo.visionManager.markUtil.m_MilModModel, m_MilMaskOverlay, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
                }
            }
        }

        private void panel_MarkZoomImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int nSx = 0;
                int nEx = 0;
                int nSy = 0;
                int nEy = 0;
                System.Drawing.Point clPoint = new System.Drawing.Point();

                clPoint.X = e.X;
                clPoint.Y = e.Y;

                Rectangle rMask = new Rectangle();
                rMask.X = (int)(e.X - m_nBrushSize/2);
                rMask.Y = (int)(e.Y - m_nBrushSize/2);
                rMask.Width = m_nBrushSize;
                rMask.Height = m_nBrushSize;
                m_bMaskDrag = true;

                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);

                if (m_pMaskBuff != null)
                {

                    nSx = (int)(((clPoint.X - m_nBrushSize) / m_dZoomX) + 0.5);
                    nEx = (int)(((clPoint.X + m_nBrushSize) / m_dZoomX) + 0.5);
                    nSy = (int)(((clPoint.Y - m_nBrushSize) / m_dZoomY) + 0.5);
                    nEy = (int)(((clPoint.Y + m_nBrushSize) / m_dZoomY) + 0.5);

                    if (nSx < 0) nSx = 0;
                    if (nSy < 0) nSy = 0;
                    if (nEx > m_clPtMarkSize.X) nEx = m_clPtMarkSize.X;
                    if (nEy > m_clPtMarkSize.Y) nEy = m_clPtMarkSize.Y;


                    int i = 0;
                    int j = 0;
                    for (i = nSy; i < nEy; i++)
                    {
                        for (j = nSx; j < nEx; j++)
                        {
                            if (m_bEraseMask)
                            {
                                m_pMaskBuff[i * m_clPtMarkSize.X + j] = 0x00;
                            }
                            else
                            {
                                m_pMaskBuff[i * m_clPtMarkSize.X + j] = 0xFF;
                            }
                        }
                    }

                    MIL.MgraRectFill(MIL.M_DEFAULT, m_MilMaskOverlay, rMask.X, rMask.Y, rMask.X + rMask.Width, rMask.Y + rMask.Height);
                }
            }
        }

        private void panel_MarkZoomImage_MouseUp(object sender, MouseEventArgs e)
        {
            m_bMaskDrag = false;
        }

        private void panel_MarkZoomImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bMaskDrag && e.Button == MouseButtons.Left)
            {
                int nSx = 0;
                int nEx = 0;
                int nSy = 0;
                int nEy = 0;
                System.Drawing.Point clPoint = new System.Drawing.Point();

                clPoint.X = e.X;
                clPoint.Y = e.Y;
                Rectangle rMask = new Rectangle();
                rMask.X = (int)(e.X - m_nBrushSize / 2);
                rMask.Y = (int)(e.Y - m_nBrushSize / 2);
                rMask.Width = m_nBrushSize;
                rMask.Height = m_nBrushSize;

                if (m_bEraseMask)
                {
                    MIL.MgraColor(MIL.M_DEFAULT, 0x00);
                }
                else
                {
                    MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
                }

                if (m_pMaskBuff != null)
                {
                    nSx = (int)((clPoint.X - m_nBrushSize) / m_dZoomX);
                    nEx = (int)((clPoint.X + m_nBrushSize) / m_dZoomX);
                    nSy = (int)((clPoint.Y - m_nBrushSize) / m_dZoomY);
                    nEy = (int)((clPoint.Y + m_nBrushSize) / m_dZoomY);

                    if (nSx < 0) nSx = 0;
                    if (nSy < 0) nSy = 0;

                    if (nEx > m_clPtMarkSize.X) nEx = m_clPtMarkSize.X;
                    if (nEy > m_clPtMarkSize.Y) nEy = m_clPtMarkSize.Y;

                    //if ((nSx > m_clPtMarkSize.x) || (nSy > m_clPtMarkSize.y))

                    int i = 0;
                    int j = 0;
                    for (i = nSy; i < nEy; i++)
                    {
                        for (j = nSx; j < nEx; j++)
                        {
                            if (m_bEraseMask)
                            {
                                m_pMaskBuff[i * m_clPtMarkSize.X + j] = 0x00;
                            }
                            else
                            {
                                m_pMaskBuff[i * m_clPtMarkSize.X + j] = 0xFF;
                            }
                        }
                    }

                    MIL.MgraRectFill(MIL.M_DEFAULT, m_MilMaskOverlay, rMask.X, rMask.Y, rMask.X + rMask.Width, rMask.Y + rMask.Height);
                }
            }
        }

        private void button_Mask_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trackBar_Mask_Brush_Size_Scroll(object sender, EventArgs e)
        {
            m_nBrushSize = trackBar_Mask_Brush_Size.Value;
            label_Mask_Brush_Size_Val.Text = trackBar_Mask_Brush_Size.Value.ToString();
        }

        private void DrawCenterLine(System.Drawing.Point centerPoint)
        {
            MIL.MbufClear(m_MilMaskOverlay, m_MilTransparentColor);

            DrawMask();

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);

            MIL.MgraLine(MIL.M_DEFAULT, m_MilMaskOverlay, (int)(centerPoint.X * m_dZoomX + 0.5), 0, (int)(centerPoint.X * m_dZoomX + 0.5), DispSize.Y);
            MIL.MgraLine(MIL.M_DEFAULT, m_MilMaskOverlay, 0, (int)(centerPoint.Y * m_dZoomY + 0.5), DispSize.X, (int)(centerPoint.Y * m_dZoomY + 0.5));

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);

            int m_nCircleSize = 20;
            MIL.MgraArc(MIL.M_DEFAULT, m_MilMaskOverlay, centerPoint.X * m_dZoomX, centerPoint.Y * m_dZoomY, m_nCircleSize, m_nCircleSize, 0, 360);
        }

        private void label_Mask_Clear_Click(object sender, EventArgs e)
        {
            MIL.MbufClear(m_MilMask, 0);
            if (m_pMaskBuff != null)
            {
                Array.Clear(m_pMaskBuff, 0, m_pMaskBuff.Length);
                MIL.MbufPut(m_MilMask, m_pMaskBuff);

                MIL.MmodMask(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, m_MilMask, MIL.M_DONT_CARE, MIL.M_DEFAULT);//<---왜 들어가있지?
                DrawCenterLine(m_clCdCenter);
                Array.Clear(m_pMaskBuff, 0, m_pMaskBuff.Length);
            }
        }

        private void label_Mask_Erase_Click(object sender, EventArgs e)
        {
            m_bEraseMask = !m_bEraseMask;
        }

        private void label_Mask_Bg_Click(object sender, EventArgs e)
        {

        }
        
        private bool SaveData()
        {
            return false;
        }
        private void button_Mask_Save_Click(object sender, EventArgs e)
        {
            MIL.MbufClear(m_MilMask, 0x00);
            MIL.MbufPut(m_MilMask, m_pMaskBuff);

            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, m_clCdCenter.X);
            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, m_clCdCenter.Y);

            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3203L, 1.0);    //M_DRAW_SCALE_X
            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3204L, 1.0);	  //M_DRAW_SCALE_Y

            MIL.MmodMask(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, m_MilMask, MIL.M_DONT_CARE, MIL.M_DEFAULT);  //210707		<---여기서 작은 화면에 마스크 그린다
            MIL.MmodPreprocess(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT);



            Globalo.visionManager.markUtil.SaveMark("A_MODEL", 0);

            Globalo.visionManager.markUtil.m_nSmooth = m_nEdgeSmooth;

            //g_clMarkData[m_nUnit].SaveData(g_clSysData.m_szModelName);

            Globalo.visionManager.markUtil.SettingFindMark(0);
            //
            Globalo.visionManager.markUtil.DisplayMarkView("A_MODEL",0, DispSize.X, DispSize.Y); 

            this.Close();
        }
        private void MaskCloseForm()
        {
            int i = 0;
            if (m_MilMask != MIL.M_NULL)
            {
                MIL.MbufFree(m_MilMask);
                m_MilMask = MIL.M_NULL;
            }
            //g_clModelFinder.LoadMark(g_clSysData.m_szModelName);
            //MdispDeselect(g_clModelFinder.m_MilMarkDisplay[1], g_clModelFinder.m_MilMarkImage[1]);

        }
        private void label_Mask_Edge_Smooth_Val_Click(object sender, EventArgs e)
        {
            string labelValue = label_Mask_Edge_Smooth_Val.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    int dNumData = int.Parse(popupForm.NumPadResult);
                    if (dNumData < 10)
                    {
                        dNumData = 10;
                    }
                    if (dNumData > 100)
                    {
                        dNumData = 100;
                    }
                    m_nEdgeSmooth = dNumData;
                    label_Mask_Edge_Smooth_Val.Text = m_nEdgeSmooth.ToString();
                }
            }
        }
    }
}
