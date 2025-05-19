using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;


namespace ZenHandler.VisionClass
{
    public class VisionManager
    {
        private CancellationTokenSource camera1TokenSource;
        private CancellationTokenSource camera2TokenSource;

        private Dictionary<int, IntPtr> _cameraDisplayHandles = new Dictionary<int, IntPtr>();

        public MilLibraryUtil milLibrary;
        public AoiTester aoiTester;
        public OpencvAoiTest opencvTester;
        public AoiTopTester aoiTopTester;
        public AoiSideTester aoiSideTester;
        public Action<Bitmap> OnCamera1Frame;
        public Action<Bitmap> OnCamera2Frame;

        public int CamControlWidth = 0;
        public int CamControlHeight = 0;
        public int SetCamControlWidth = 0;
        public int SetCamControlHeight = 0;

        //public PointF CamResol;
        //카메라 연결,해제
        //Get Frame

        public VisionManager()
        {
            Event.EventManager.PgExitCall += OnPgExit;

        }
        public void SetPanelSize(int camWidth, int camHeight, int setcamWidth, int setcamHeight)
        {
            CamControlWidth = camWidth;
            CamControlHeight = camHeight;
            SetCamControlWidth = setcamWidth;
            SetCamControlHeight = setcamHeight;
        }
        public void MilSet()
        {
            int i = 0;
            milLibrary = new MilLibraryUtil();
            aoiTester = new AoiTester();
            opencvTester = new OpencvAoiTest();
            aoiTopTester = new AoiTopTester();
            aoiSideTester = new AoiSideTester();

            milLibrary.AllocMilApplication();

            milLibrary.AllocMilCamBuffer(0, CamControlWidth, CamControlHeight);    //Top Camera
            milLibrary.AllocMilCamBuffer(1, CamControlWidth, CamControlHeight);    //Side Camera

            milLibrary.setCamSize(0, CamControlWidth, CamControlHeight);
            milLibrary.setCamSize(1, CamControlWidth, CamControlHeight);

            milLibrary.AllocMilSetCamBuffer(0, SetCamControlWidth, SetCamControlHeight);    //Setting Camera
            milLibrary.AllocMilSetCamBuffer(1, SetCamControlWidth, SetCamControlHeight);    //Setting Camera

            for (i = 0; i < milLibrary.CamFixCount; i++)
            {
                milLibrary.AllocMilCamDisplay(_cameraDisplayHandles[i], i);

                
                milLibrary.EnableCamOverlay(i);
                

            }
            milLibrary.AllocMilSetCamDisplay(_cameraDisplayHandles[2]);
            milLibrary.EnableSetCamOverlay();
            //milLibrary.DrawOverlay(0);
            //milLibrary.DrawOverlay(1);

            StartCameras();
        }
        private void OnPgExit(object sender, EventArgs e)
        {
            StopCamera1();
            StopCamera2();

            milLibrary.MilClose();
        }

        public void SetLoadBmp(int index, string filePath)
        {
            milLibrary.setCamImage(index, filePath);
            
        }
        public void ChangeDisplayHandle(int camIndex, Panel panel)
        {
            milLibrary.SelectDisplay(camIndex, panel.Handle, panel.Width , panel.Height);
        }
        public void ChangeSettingDisplayHandle(int camIndex, Panel panel)
        {
            milLibrary.SelectSetDisplay(camIndex, panel.Handle, panel.Width , panel.Height);
        }
        public void RecoverDisplayHandle()
        {
            milLibrary.SelectDisplay(0, _cameraDisplayHandles[0], CamControlWidth, CamControlHeight);
            milLibrary.SelectDisplay(1, _cameraDisplayHandles[1], CamControlWidth, CamControlHeight);
        }

        public void RegisterDisplayHandle(int cameraIndex, IntPtr handle)
        {
            _cameraDisplayHandles[cameraIndex] = handle;
        }
        private void StartGrabCamera()
        {
            camera1TokenSource = new CancellationTokenSource();
            CancellationToken token = camera1TokenSource.Token;
            try
            {
                Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        milLibrary.MilGrabRun(0);
                        
                        Thread.Sleep(10); // 혹은 FPS에 맞춰 조절
                    }
                }, token);
            }
            catch (ThreadInterruptedException err)
            {
                Console.WriteLine("ThreadInterruptedException StartCamera1 :" + err);
            }

            Console.WriteLine("StartCamera1 end");
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
                        milLibrary.MilGrabRun(0);
                        Thread.Sleep(10); // 혹은 FPS에 맞춰 조절
                    }
                }, token);
            }
            catch (ThreadInterruptedException err)
            {
                Console.WriteLine("ThreadInterruptedException StartCamera1 :" + err);
            }

            Console.WriteLine("StartCamera1 end");
        }

        private void StartCamera2()
        {
            camera2TokenSource = new CancellationTokenSource();
            CancellationToken token = camera2TokenSource.Token;
            try
            {
                Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        milLibrary.MilGrabRun(1);
                        Thread.Sleep(10);
                    }
                }, token);
            }
            catch (ThreadInterruptedException err)
            {
                Console.WriteLine("ThreadInterruptedException StartCamera1 :" + err);
            }

            Console.WriteLine("StartCamera1 end");
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
            ////StartGrabCamera();
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
