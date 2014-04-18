using SharpDX.Direct3D9;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Mage
{
    public class GameWindow : Form
    {
        const int DefaultAdapter = 0;
        Device _device;
        PresentParameters _deviceParams;
        IGame _game;

        #region Properties
        public float Aspect { get { return Width / (float)Height; } }
        public bool IsFullScreen { get { return !_deviceParams.Windowed; } }
        #endregion

        public GameWindow(string title, int width, int height, bool fullScreen, bool vsync = false, bool hardwareAccelerated = true)
        {
            SetupWindow(title, width, height, fullScreen);
            SetupDirectX(width, height, fullScreen, vsync, hardwareAccelerated);
        }

        #region Setup
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
            if(!Stopwatch.IsHighResolution)
                throw new PlatformNotSupportedException("High precision timing is not supported");

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

            _deviceParams = new PresentParameters
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

            _device = new Device(direct3D, DefaultAdapter, deviceType, Handle, devBehaviorFlags, _deviceParams);
        }
        #endregion

        #region Game Loop
        public void Run(IGame game)
        {
            if (_game != null)
                throw new InvalidOperationException("A game is already running.");

            if (_device == null)
                throw new InvalidOperationException("Setup must be called before Run.");

            Show();
            _game = game;

            try
            {
                var sw = Stopwatch.StartNew();
                _game.Setup(_device);

                while (_game != null)
                {
                    Application.DoEvents();
                    if (!IsDeviceLost())
                    {
                        var deltaTime = sw.Elapsed;
                        sw.Restart();
                        if (game.Process(deltaTime))
                            game.Paint(_device);
                        else
                        {
                            Close();
                            break;
                        }
                    }
                }
            }
            catch
            {
                Close();
                throw;
            }
            game.ShutDown(_device);
        }

        bool IsDeviceLost()
        {
            // Retorna verdadeiro se o Device está perdido
            // Ou falso se ele está, ou foi recuperado.

            // Obtém o estado do dispositivo gráfico
            var hr = _device.TestCooperativeLevel();

            //Se ele se perdeu e não podemos recuperar, dormimos um pouco e esperamos o
            //próximo loop
            if (hr == ResultCode.DeviceLost)
            {
                Thread.Sleep(20);
                return true;
            }

            //Se houve um error de driver, não há muito o que fazer a não ser
            //encerrar a aplicação.
            if (hr == ResultCode.DriverInternalError)
                throw new Exception("Driver Internal Error");

            // Se o Device está perdido, mas pode ser recuperado, resetamos e o 
            //restauramos.
            if (hr == ResultCode.DeviceNotReset)
            {
                _game.OnLostDevice();			//Avisamos o jogo que iremos resetar.
                _device.Reset(_deviceParams);	//Resetamos o dispositivo
                _game.OnRestoreDevice(_device);		//Avisamos o jogo que o dispositivo foi restaurado.
                // E agora podemos retornar false, já que ele não estará mais perdido.
                return false;
            }

            //Se não tem erros, então não está perdido.
            return false;
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            const int WM_QUIT = 0x10;

            if (_game != null)
                _game.ProcessEvent(m);

            base.WndProc(ref m);

            if (m.Msg == WM_QUIT)
                _game = null;
        }
    }
}
