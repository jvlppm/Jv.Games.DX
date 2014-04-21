using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Objects;
using Mage;
using Microsoft.Xna.Framework.Input;
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
#if DEBUG
            window.TopMost = true;
#endif

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

        public void ProcessEvent(System.Windows.Forms.Message msg)
        {
            const int WM_KEYFIRST = 0x0100;
            const int WM_KEYLAST = 0x0108;

            if (msg.Msg >= WM_KEYFIRST && msg.Msg <= WM_KEYLAST)
                HandleKeyboardInput(msg);
        }

        void HandleKeyboardInput(System.Windows.Forms.Message msg)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;

            var key = (System.Windows.Forms.Keys)msg.WParam;

            switch(msg.Msg)
            {
                case WM_KEYDOWN: if(!Keyboard.Keys.Contains(key)) Keyboard.Keys.Add(key); break;
                case WM_KEYUP: Keyboard.Keys.Remove(key); break;
            }
        }

        public void Dispose()
        {
            _scene.Dispose();
        }
    }
}
