using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;


namespace ZenTester.FThread
{
    public class ImageGrabThread : BaseThread
    {
        private IntPtr RawPtr;
        private IntPtr BmpPtr;

        private int mWidth;
        private int mHeight;

        private Mat resizedMat;

        public ImageGrabThread()
        {
            this.name = "CcdGrabThread";

        }
        public void RawInit()
        {
            //mWidth = Globalo.GrabberDll.mGetWidth();
            //mHeight = Globalo.GrabberDll.mGetHeight();


            resizedMat?.Dispose();
            resizedMat = new Mat(mHeight, mWidth, MatType.CV_8UC3);       //MatType.CV_8UC3);
        }

        protected override void ThreadInit()
        {
            RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameRawBuffer, 0);
            BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

            
        }
        protected override unsafe void ThreadRun()
        {
            if (Globalo.mLaonGrabberClass.M_GrabDllLoadComplete == false)
            {
                return;
            }
            try
            {
                if (Globalo.mLaonGrabberClass.M_bOpen == false)
                {
                    return;
                }

                //if (Globalo.GrabberDll.mGetFrame((byte*)RawPtr.ToPointer(), (byte*)BmpPtr.ToPointer()) == true)
                //{
                //    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

                //    // 데이터를 Mat에 복사
                //    Marshal.Copy(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0, Globalo.mLaonGrabberClass.imageItp.Data, mHeight * mWidth * 3);
                //    // Grab 버퍼에 저장

                //}

            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }
        }
    }
}
