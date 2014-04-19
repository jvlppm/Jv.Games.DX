using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Objects;
using Mage;
using System;

namespace Jv.Games.DX.Test
{
    class MyGame : IGame, IDisposable
    {
        Scene _scene;

        public void Setup(SharpDX.Direct3D9.Device device, GameWindow window)
        {
            _scene = new Scene(device);

            var obj = _scene.Add(new Block(device, 10, "block_solid", false) { new Rotating() });

            _scene.Add(new Water(device, 50, 50)).Translate(0, -10f, 0);

            var camera = new Camera { new LookAtObject(obj) };
            camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
            camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);

            camera.Translate(0, 40, 1);
            _scene.Add(camera);

            _scene.Init();
        }

        public bool Process(TimeSpan deltaTime)
        {
            _scene.Update(deltaTime);
            return true;
        }

        public void Paint(SharpDX.Direct3D9.Device device)
        {
            _scene.Draw();
        }

        public void OnLostDevice() { }

        public void OnRestoreDevice(SharpDX.Direct3D9.Device device) { }

        public void ShutDown(SharpDX.Direct3D9.Device device) { }

        public void ProcessEvent(System.Windows.Forms.Message msg) { }

        public void Dispose()
        {
            _scene.Dispose();
        }
    }
}
