using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using Jv.Games.DX.Test.Objects;
using Mage;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Jv.Games.DX.Test
{
    class MyGame : IGame, IDisposable
    {
        Scene _scene;

        public void Setup(SharpDX.Direct3D9.Device device, GameWindow window)
        {
            var texture = Texture.FromFile(device, "Textures/block_solid.png");

            _scene = new Scene(device);
            var obj = _scene.Add(new Block(device, 1, texture));
            obj.Attach(new Rotating());

            var camera = _scene.Add(new Camera());
            camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
            camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);
            camera.Attach(new LookAtObject(obj));
            camera.Transform = camera.Transform * SharpDX.Matrix.Translation(new SharpDX.Vector3(0, 1, -3));

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
