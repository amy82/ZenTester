using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.VisionClass
{
    public struct STRUC_MIL_BOX
    {
        public int nWidth;
        public DashStyle nStyle;
        public Rectangle clRectBox;
        public Color color;
    }
    public struct STRUC_MIL_ELLIPSE
    {
        public int Radius;
        public int nWidth;
        public DashStyle nStyle;
        public System.Drawing.Point clPoint;
        public Color color;
    }
    public struct STRUC_MIL_TEXT
    {
        public string szFontName;
        public string szText;
        public System.Drawing.Point clPoint;
        public int clFontSize;
        public Color color;
    }
    public struct STRUC_MIL_CROSS
    {
        public int nWidth;
        public int nSize;
        public System.Drawing.Point clPoint;
        public Color color;
    }
    public class CMilDrawBox
    {
        private List<STRUC_MIL_BOX> m_clArrayBox = new List<STRUC_MIL_BOX>();
        public CMilDrawBox()
        {
        }
        ~CMilDrawBox()
        {
            m_clArrayBox.Clear();
        }
        public void AddList(int nLeft, int nTop, int nRight, int nBottom, int nWidth, int nStyle, Color color)//COLORREF color);
        {

        }
        public void AddList(Rectangle clRect, int nWidth, DashStyle nStyle, Color color)
        {
            STRUC_MIL_BOX stMilBox = new STRUC_MIL_BOX();

            stMilBox.clRectBox = clRect;
            stMilBox.nWidth = nWidth;
            stMilBox.nStyle = nStyle;
            stMilBox.color = color;

            m_clArrayBox.Add(stMilBox);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)  //HDC hOverlayDC
        {
            STRUC_MIL_BOX stMilBox;
            int nCount;
            int i;

            nCount = (int)m_clArrayBox.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        for (i = 0; i < nCount; i++)
                        {
                            stMilBox = m_clArrayBox[i];
                            using (Pen DrawingPen = new Pen(stMilBox.color))
                            {
                                DrawingPen.DashStyle = stMilBox.nStyle;
                                DrawingPen.Width = stMilBox.nWidth;
                                //
                                int x1 = (int)((stMilBox.clRectBox.X * dZoomX) + 0.5);
                                int x2 = (int)((stMilBox.clRectBox.Width * dZoomX) + 0.5);
                                int y1 = (int)((stMilBox.clRectBox.Y * dZoomY) + 0.5);
                                int y2 = (int)((stMilBox.clRectBox.Height * dZoomY) + 0.5);

                                DrawingGraphics.DrawRectangle(DrawingPen, new Rectangle(x1, y1, x2, y2));

                            }
                        }
                    }
                }

            }
        }

        public void RemoveAll()
        {
            // m_clArrayBox.RemoveAll();
            m_clArrayBox.Clear();
        }
    };
    public class CMilDrawCircle
    {
        private List<STRUC_MIL_ELLIPSE> m_clArrayCircle = new List<STRUC_MIL_ELLIPSE>();
        public CMilDrawCircle()
        {
        }
        ~CMilDrawCircle()
        {
            m_clArrayCircle.Clear();
        }
        public void AddList(int centerx, int centery, int radius, int wid, DashStyle style, Color color)//COLORREF color);
        {
            STRUC_MIL_ELLIPSE stMilCircle = new STRUC_MIL_ELLIPSE();

            stMilCircle.clPoint.X = centerx;
            stMilCircle.clPoint.Y = centery;
            stMilCircle.nWidth = wid;
            stMilCircle.nStyle = style;
            stMilCircle.Radius = radius;
            stMilCircle.color = color;

            m_clArrayCircle.Add(stMilCircle);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)  //HDC hOverlayDC
        {
            STRUC_MIL_ELLIPSE stMilEllipse;
            int nCount;
            int i;

            nCount = (int)m_clArrayCircle.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        for (i = 0; i < nCount; i++)
                        {
                            stMilEllipse = m_clArrayCircle[i];
                            using (Pen DrawingPen = new Pen(stMilEllipse.color))
                            {
                                DrawingPen.DashStyle = stMilEllipse.nStyle;
                                DrawingPen.Width = stMilEllipse.nWidth;
                                //
                                int x1 = (int)((stMilEllipse.clPoint.X * dZoomX) + 0.5);
                                int y1 = (int)((stMilEllipse.clPoint.Y * dZoomY) + 0.5);

                                int x2 = (int)((stMilEllipse.Radius * dZoomX) + 0.5);
                                int y2 = (int)((stMilEllipse.Radius * dZoomY) + 0.5);

                                DrawingGraphics.DrawEllipse(DrawingPen, x1, y1, x2, y2);

                            }
                        }
                    }
                }

            }
        }
        public void RemoveAll()
        {
            m_clArrayCircle.Clear();
        }
    }
    public class CMilDrawText
    {
        public List<STRUC_MIL_TEXT> m_clArrayText = new List<STRUC_MIL_TEXT>();
        public CMilDrawText()
        {
        }
        ~CMilDrawText()
        {
            m_clArrayText.Clear();
        }
        public void AddList(int nPosX, int nPosY, int nSizeX, int nSizeY, string szText, string szFontName, Color color)
        {

        }
        public void AddList(System.Drawing.Point clPoint, string szText, string szFontName, Color color, int nFontSize)
        {
            STRUC_MIL_TEXT stMilText = new STRUC_MIL_TEXT();

            stMilText.clPoint = clPoint;
            stMilText.clFontSize = nFontSize;
            stMilText.szText = szText;
            stMilText.szFontName = szFontName;
            stMilText.color = color;

            m_clArrayText.Add(stMilText);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)
        {
            STRUC_MIL_TEXT stMilText;

            int nCount;
            int i;

            nCount = (int)m_clArrayText.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        using (Pen DrawingPen = new Pen(Color.Blue))
                        {
                            for (i = 0; i < nCount; i++)
                            {
                                stMilText = m_clArrayText[i];
                                using (SolidBrush mBrush = new SolidBrush(stMilText.color))
                                {
                                    using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, stMilText.clFontSize, FontStyle.Bold))
                                    {
                                        int x = (int)((stMilText.clPoint.X * dZoomX) + 0.5);
                                        int y = (int)((stMilText.clPoint.Y * dZoomY) + 0.5);

                                        DrawingGraphics.DrawString(stMilText.szText, OverlayFont, mBrush, x, y);
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        public void RemoveAll()
        {
            m_clArrayText.Clear();
        }

    };
    public class CMilDrawCross
    {
        public List<STRUC_MIL_CROSS> m_clArrayCross = new List<STRUC_MIL_CROSS>();

        public CMilDrawCross()
        {

        }
        ~CMilDrawCross()
        {
            m_clArrayCross.Clear();
        }

        public void AddList(int nPosX, int nPosY, int nSize, int nWidth, Color color)
        {
            STRUC_MIL_CROSS stMilCross;
            stMilCross.clPoint = new Point(nPosX, nPosY);

            stMilCross.nSize = nSize;
            stMilCross.nWidth = nWidth;
            stMilCross.color = color;

            m_clArrayCross.Add(stMilCross);
        }
        public void AddList(System.Drawing.Point clPoint, int nSize, int nWidth, Color color)
        {
            STRUC_MIL_CROSS stMilCross;

            stMilCross.clPoint = clPoint;
            stMilCross.nSize = nSize;
            stMilCross.nWidth = nWidth;
            stMilCross.color = color;

            m_clArrayCross.Add(stMilCross);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)
        {
            STRUC_MIL_CROSS stMilCross;
            int nCount;
            int i;
            System.Drawing.Point clCrossPos = new System.Drawing.Point();
            nCount = (int)m_clArrayCross.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        // Draw a blue cross.
                        for (i = 0; i < nCount; i++)
                        {
                            stMilCross = m_clArrayCross[i];
                            using (Pen DrawingPen = new Pen(stMilCross.color))
                            {
                                clCrossPos.X = stMilCross.clPoint.X + stMilCross.nSize;
                                clCrossPos.Y = stMilCross.clPoint.Y + stMilCross.nSize;


                                int x1 = (int)((stMilCross.clPoint.X - stMilCross.nSize) * dZoomX + 0.5);
                                int x2 = (int)((stMilCross.clPoint.X + stMilCross.nSize) * dZoomX + 0.5);
                                int y1 = (int)((stMilCross.clPoint.Y * dZoomY) + 0.5);

                                int x3 = (int)((stMilCross.clPoint.X) * dZoomX + 0.5);
                                int y2 = (int)((stMilCross.clPoint.Y - stMilCross.nSize) * dZoomY + 0.5);
                                int y3 = (int)((stMilCross.clPoint.Y + stMilCross.nSize) * dZoomY + 0.5);


                                DrawingGraphics.DrawLine(DrawingPen, new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y1));
                                DrawingGraphics.DrawLine(DrawingPen, new System.Drawing.Point(x3, y2), new System.Drawing.Point(x3, y3));


                            }
                        }

                    }
                }

            }
        }
        public void RemoveAll()
        {
            m_clArrayCross.Clear();
        }
    };
}
