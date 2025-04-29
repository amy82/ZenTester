using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;


namespace ZenHandler.VisionClass
{
    public class VisionManager
    {
        private CancellationTokenSource camera1TokenSource;
        private CancellationTokenSource camera2TokenSource;

        private Dictionary<int, IntPtr> _cameraDisplayHandles = new Dictionary<int, IntPtr>();

        public MilLibrary milLibrary;

        public Action<Bitmap> OnCamera1Frame;
        public Action<Bitmap> OnCamera2Frame;

        public int CamControlWidth = 0;
        public int CamControlHeight = 0;

        public int CameraResolutionWidth = 600;
        public int CameraResolutionHeight = 480;

        public double m_CamReduceFactorX = 0.0;
        public double m_CamReduceFactorY = 0.0;

        public double m_CamExpandFactorX = 0.0;
        public double m_CamExpandFactorY = 0.0;

        

        //카메라 연결,해제
        //Get Frame

        public VisionManager(int camWidth , int camHeight)
        {
            CamControlWidth = camWidth;
            CamControlHeight = camHeight;

        }

        public void MilSet()
        {
            int i = 0;
            

            milLibrary = new MilLibrary();
            

            milLibrary.AllocMilApplication(CamControlWidth, CamControlHeight);

            m_CamReduceFactorX = ((double)CamControlWidth / (double)milLibrary.CAM_SIZE_X);
            m_CamReduceFactorY = ((double)CamControlHeight / (double)milLibrary.CAM_SIZE_Y);

            m_CamExpandFactorX = ((double)milLibrary.CAM_SIZE_X / (double)CamControlWidth);
            m_CamExpandFactorY = ((double)milLibrary.CAM_SIZE_Y / (double)CamControlHeight);
            
            for (i = 0; i < milLibrary.CamFixCount; i++)
            {
                milLibrary.AllocMilCamDisplay(_cameraDisplayHandles[i], i);
                milLibrary.EnableCamOverlay(i);
            }

            //milLibrary.DrawOverlay(0);
            //milLibrary.DrawOverlay(1);

            StartCameras();
        }
        
        private Bitmap GetCamera1Frame()    // TODO: 실제 카메라 SDK에서 프레임 얻는 메서드로 바꿔야 함
        {
            // 예시: 카메라 SDK에서 프레임 가져오기
            return new Bitmap(640, 480); // 임시
        }

        private Bitmap GetCamera2Frame()
        {
            return new Bitmap(640, 480); // 임시
        }
        
        public void SetLoadBmp(int index, string filePath)
        {
            milLibrary.setCamImage(index, filePath);
            
        }
        
        public void RegisterDisplayHandle(int cameraIndex, IntPtr handle)
        {
            _cameraDisplayHandles[cameraIndex] = handle;
        }

        private void StartCamera1()
        {
            camera1TokenSource = new CancellationTokenSource();
            CancellationToken token = camera1TokenSource.Token;
            try
            {
                Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        //Bitmap frame = GetCamera1Frame(); // 여기는 네 카메라 SDK로!
                        //OnCamera1Frame?.Invoke(frame);
                        milLibrary.MilGrabRun(0);


                        Thread.Sleep(10); // 혹은 FPS에 맞춰 조절
                    }
                }, token);
            }
            catch (ThreadInterruptedException err)
            {
                Console.WriteLine("ThreadInterruptedException StartCamera1 :" + err);
            }
        }

        private void StartCamera2()
        {
            camera2TokenSource = new CancellationTokenSource();
            CancellationToken token = camera2TokenSource.Token;

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    //Bitmap frame = GetCamera2Frame(); // 여기도 카메라 SDK
                    //OnCamera2Frame?.Invoke(frame);

                    milLibrary.MilGrabRun(1);
                    Thread.Sleep(10);
                }
            }, token);
        }
        private void StopCamera1()
        {
            camera1TokenSource?.Cancel();
            camera1TokenSource?.Dispose();
        }

        private void StopCamera2()
        {
            camera2TokenSource?.Cancel();
            camera2TokenSource?.Dispose();
        }

        public void StartCameras()
        {
            StartCamera1();
            StartCamera2();
        }
        public void StopCameras()
        {
            StopCamera1();
            StopCamera2();
        }

        public void Dispose()
        {
            StopCameras();
        }


        
    }
}
