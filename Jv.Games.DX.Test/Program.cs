using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jv.Games.DX.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var win = new GameWindow("Meu jogo", 800, 600, fullScreen: false, vsync: true, hardwareAccelerated: true))
            {
                win.Show();
                win.Run(new MyGame());
            }
        }
    }
}
