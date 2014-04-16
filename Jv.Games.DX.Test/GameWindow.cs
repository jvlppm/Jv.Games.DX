using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jv.Games.DX.Test
{
    public class GameWindow : Form
    {
        const int DefaultAdapter = 0;
        Device _device;

        public GameWindow(string title, int width, int height, bool fullScreen, bool vsync = false, bool hardwareAccelerated = true)
        {
            SetupWindow(title, width, height, fullScreen);
            SetupDirectX(width, height, fullScreen, vsync, hardwareAccelerated);
        }

        void SetupWindow(string title, int width, int height, bool fullScreen)
        {
            Text = title;
            Width = width;
            Height = height;

            if (fullScreen)
                FormBorderStyle = FormBorderStyle.None;
            WindowState = fullScreen ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        void SetupDirectX(int width, int height, bool fullScreen, bool vsync, bool hardwareAccelerated)
        {
            var direct3D = new Direct3D();

            var deviceType = hardwareAccelerated ? DeviceType.Hardware : DeviceType.Software;
            Format format = fullScreen ? Format.X8R8G8B8 : direct3D.GetAdapterDisplayMode(DefaultAdapter).Format;

            if (!direct3D.CheckDeviceType(DefaultAdapter, deviceType, format, format, !fullScreen))
                throw new PlatformNotSupportedException("Graphics mode not supported by the platform.");

            var caps = direct3D.GetDeviceCaps(DefaultAdapter, deviceType);

            if (caps.VertexShaderVersion < new Version(2, 0))
                throw new PlatformNotSupportedException("Vertex Shader 2.0 is not supported by the platform.");
            if (caps.PixelShaderVersion < new Version(2, 0))
                throw new PlatformNotSupportedException("Vertex Shader 2.0 is not supported by the platform.");

            CreateFlags devBehaviorFlags = CreateFlags.None;

            if (hardwareAccelerated && caps.DeviceCaps.HasFlag(DeviceCaps.HWTransformAndLight))
            {
                devBehaviorFlags |= CreateFlags.HardwareVertexProcessing;
                if (caps.DeviceCaps.HasFlag(DeviceCaps.PureDevice))
                    devBehaviorFlags |= CreateFlags.PureDevice;
            }
            else
                devBehaviorFlags |= CreateFlags.SoftwareVertexProcessing;

            PresentParameters deviceParams = new PresentParameters
            {
                DeviceWindowHandle = Handle,
                BackBufferFormat = format,
                BackBufferWidth = fullScreen ? width : 0,
                BackBufferHeight = fullScreen ? height : 0,
                BackBufferCount = 1,
                Windowed = !fullScreen,
                FullScreenRefreshRateInHz = (int)Present.RateDefault,
                PresentationInterval = vsync ? PresentInterval.Default : PresentInterval.Immediate,
                SwapEffect = SwapEffect.Discard,
                MultiSampleType = MultisampleType.None,
                MultiSampleQuality = 0,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = Format.D24S8,
                PresentFlags = PresentFlags.None
            };

            _device = new Device(direct3D, DefaultAdapter, deviceType, Handle, devBehaviorFlags, deviceParams);
        }

        public void Run(IGame game)
        {
            if (_device == null)
                throw new InvalidOperationException("Setup must be called before Run.");

            while(true)
            {
                Application.DoEvents();
                game.Process(0);
                game.Paint(_device);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }
    }
}
