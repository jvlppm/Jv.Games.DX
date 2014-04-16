using Mage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX.Test
{
    class MyGame : IGame
    {
        public void Setup(SharpDX.Direct3D9.Device device)
        {
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
