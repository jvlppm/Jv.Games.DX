using Mage;
using System;

namespace Jv.Games.DX.Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var win = new GameWindow("Meu jogo", 800, 600, fullScreen: false, vsync: true, hardwareAccelerated: true))
            {
                win.Run(new MyGame());
            }
        }
    }
}
