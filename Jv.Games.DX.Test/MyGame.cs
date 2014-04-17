using Jv.Games.DX.Test.Mesh;
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
        IMesh mesh;

        public void Setup(SharpDX.Direct3D9.Device device)
        {
            mesh = new TriangleGrid(device, 8, 8, 3, 3);
        }

        public bool Process(TimeSpan deltaTime)
        {
            return true;
        }

        public void Paint(SharpDX.Direct3D9.Device device)
        {
            //Limpa a janela de azul escuro
            device.Clear(SharpDX.Direct3D9.ClearFlags.Target | SharpDX.Direct3D9.ClearFlags.ZBuffer, new SharpDX.ColorBGRA(0, 20, 80, byte.MaxValue), 1.0f, 0);

            device.BeginScene();    // Começa a cena 3D

            //O código do desenho vai aqui
            device.VertexDeclaration = SimpleVertex.GetDeclaration(device);
            device.SetStreamSource(0, mesh.Vertex, 0, Marshal.SizeOf(typeof(SimpleVertex)));
            device.Indices = mesh.Index;
            device.DrawIndexedPrimitive(mesh.PrimitiveType, 0, 0, mesh.NumVertices, 0, mesh.NumPrimitives);


            device.EndScene();    // Finaliza a cena 3D.

            device.Present();    // Exibe o quadro recém criado
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
