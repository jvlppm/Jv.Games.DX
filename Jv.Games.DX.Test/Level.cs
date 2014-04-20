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
        GameObject _map;
        DateTime _mapModifyDate;
        string _mapFile;

        public Level(GameWindow window, Device device, int number)
            : base(device)
        {
            ClearColor = new Color(0x51, 0xa9, 0xf0);
#if DEBUG
            _mapFile = "../../Assets/Maps/Level" + number + ".txt";
#else
            _mapFile = "Assets/Maps/Level" + number + ".txt";
#endif

            ReloadScene(device);

            if (_player != null)
            {
                var camera = new Camera { new LookAtObject(_player) };
                camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
                camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);

                camera.Translate(_player.Transform.TranslationVector.X, _player.Transform.TranslationVector.Y + 4, _player.Transform.TranslationVector.Z - 15);
                Add(camera);
            }
        }

        void ReloadScene(Device device)
        {
            string[] mapContent = File.ReadAllLines(_mapFile);
            _mapModifyDate = File.GetLastWriteTime(_mapFile);

            _map = Add(new GameObject());

            var y = mapContent.Length;
            foreach (var line in mapContent)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    string blockTexture = null;

                    switch (c)
                    {
                        case 'M':
                            if (_player == null)
                            {
                                _player = Add(new Mario(device));
                                _player.Transform *= Matrix.RotationY(MathUtil.DegreesToRadians(-90))
                                                  * Matrix.Translation(x, y + 0.5f, 0);
                            }
                            break;

                        case '?':
                            _map.Add(new ItemBlock(device)).Translate(x, y, 0);
                            break;

                        case 'B':
                            _map.Add(new BrickBlock(device)).Translate(x, y, 0);
                            break;

                        case 'W':
                            {
                                int width = 1;
                                while (x + 1 < line.Length && line[x + 1] == 'W')
                                {
                                    width++;
                                    x++;
                                }
                                var water = _map.Add(new Water(device, 10 * width, 20));
                                water.Transform *= Matrix.Scaling(0.1f)
                                              * Matrix.Translation(x - (float)(width - 1) / 2, y, 0);
                                break;
                            }

                        case 'S': blockTexture = "solid"; break;
                        case 'F': blockTexture = "floor"; break;
                    }

                    if (blockTexture != null)
                    {
                        _map.Add(new Block(device, blockTexture)).Translate(x, y, -0.5f);
                        _map.Add(new Block(device, blockTexture)).Translate(x, y, +0.5f);
                    }
                }

                y--;
            }
        }

        public override void Update(Device device, TimeSpan deltaTime)
        {
            var fileDate = File.GetLastWriteTime(_mapFile);
            if (fileDate > _mapModifyDate)
            {
                _map.Dispose();
                ReloadScene(device);
                _mapModifyDate = fileDate;
                _map.Init();
            }

            base.Update(device, deltaTime);
        }
    }
}
