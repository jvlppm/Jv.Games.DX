using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Objects;
using Mage;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test
{
    class Level : Scene
    {
        GameObject _player;

        public Level(GameWindow window, Device device, int number)
            : base(device)
        {
            ClearColor = new Color(0x51, 0xa9, 0xf0);

            string[] mapContent = File.ReadAllLines("Assets/Maps/Level" + number + ".txt");

            var y = mapContent.Length;
            foreach (var line in mapContent)
            {
                var x = 0;
                foreach (var c in line)
                {
                    string blockTexture = null;

                    switch (c)
                    {
                        case 'M':
                            _player = Add(new Mario(device));
                            _player.Transform *= Matrix.RotationY(MathUtil.DegreesToRadians(-90))
                                              * Matrix.Translation(x, y + 0.5f, 0);
                            break;

                        case 'S': blockTexture = "solid"; break;
                        case 'F': blockTexture = "floor"; break;
                    }

                    if (blockTexture != null)
                    {
                        Add(new Block(device, 1, blockTexture)).Translate(x, y, -0.5f);
                        Add(new Block(device, 1, blockTexture)).Translate(x, y, +0.5f);
                    }
                    x++;
                }

                y--;
            }

            var camera = new Camera { new LookAtObject(_player) };
            camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
            camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);

            camera.Translate(_player.Transform.TranslationVector.X, _player.Transform.TranslationVector.Y + 4, _player.Transform.TranslationVector.Z - 15);
            Add(camera);
        }
    }
}
