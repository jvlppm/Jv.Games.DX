using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Objects;
using Mage;
using SharpDX.Direct3D9;
using System;
using System.IO;

namespace Jv.Games.DX.Test
{
    class MyGame : IGame, IDisposable
    {
        Scene _scene;

        public void Setup(Device device, GameWindow window)
        {
            var marioTexture = Texture.FromFile(device, "Assets/Textures/new-mario.png");

            _scene = new Level(window, device, 1);
            _scene.Init();
        }

        public bool Process(Device device, TimeSpan deltaTime)
        {
            _scene.Update(device, deltaTime);
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
