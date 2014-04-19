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
        Scene scene;
        GameWindow window;

        public void Setup(SharpDX.Direct3D9.Device device, GameWindow window)
        {
            var texture = Texture.FromFile(device, "Textures/block_solid.png");
            var uv = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

            this.window = window;
            scene = new Scene(device);
            var obj = scene.Add(new Model(new TexturedCube(device, 5, 5, 5, uv, uv, uv, uv, uv, uv), new TextureMaterial { Texture = texture }));
            obj.Attach(new Rotating());

            var camera = scene.Add(new Camera());
            camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
            camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);
            camera.Attach(new LookAtObject(obj));
            camera.Transform = camera.Transform * SharpDX.Matrix.Translation(new SharpDX.Vector3(0, 3, 10));

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

        public void Dispose()
        {
            scene.Dispose();
        }
    }
}
