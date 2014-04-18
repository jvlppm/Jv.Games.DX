using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Mesh;
using Jv.Games.DX.Test.Objects;
using Mage;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Jv.Games.DX.Test
{
    class MyGame : IGame
    {
        Scene scene;

        public void Setup(SharpDX.Direct3D9.Device device, GameWindow window)
        {
            scene = new Scene(device);
            var water = scene.Add(new Water(device, 100, 100, 51, 51));

            var camera = scene.Add(new Camera());
            camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);
            camera.Attach(new LookAtObject(water));
            camera.Transform = camera.Transform * SharpDX.Matrix.Translation(new SharpDX.Vector3(0, 30, -50));

            scene.Init();
        }

        public bool Process(TimeSpan deltaTime)
        {
            scene.Update(deltaTime);
            return true;
        }

        public void Paint(SharpDX.Direct3D9.Device device)
        {
            scene.Draw();
        }

        public void OnLostDevice()
        {
        }

        public void OnRestoreDevice(SharpDX.Direct3D9.Device device)
        {
        }

        public void ShutDown(SharpDX.Direct3D9.Device device)
        {
        }

        public void ProcessEvent(System.Windows.Forms.Message msg)
        {
        }
    }
}
