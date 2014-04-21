using Jv.Games.DX.Components;
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
        static TimeSpan MaxFrameDelay = TimeSpan.FromSeconds(1 / 8.0f);

        Vector2 _oldStartPos;

        TimeSpan _checkFileCount;
        TimeSpan VerifyMapFile = TimeSpan.FromSeconds(2);
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
                var camera = new Camera
                {
                    new LookAtObject(_player),
                    new Follow(_player)
                    {
                        Offset = new Vector3(0, 5, 0),
                        Mask = new Vector3(1, 1, 0)
                    },
                    new RigidBody
                    {
                        Friction = new Vector3(10),
                        MaxSpeed = new Vector3(5f)
                    }
                };
                camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
                camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);

                camera.Translate(_player.Transform.TranslationVector.X, _player.Transform.TranslationVector.Y + 4, _player.Transform.TranslationVector.Z - 15);
                Add(camera);
            }
        }

        public override void Init()
        {
            RigidBody.MeterSize = 3;
            base.Init();
        }

        void ReloadScene(Device device)
        {
            string[] mapContent = File.ReadAllLines(_mapFile);
            _mapModifyDate = File.GetLastWriteTime(_mapFile);

            _map = Add(new GameObject());

            var y = mapContent.Length;
            foreach (var line in mapContent)
            {
                int xOffset = 0;
                for (int x = 0; x - xOffset < line.Length; x++)
                {
                    var c = line[x - xOffset];
                    string blockTexture = null;

                    switch (c)
                    {
                        case '\t':
                            x += 3;
                            xOffset += 3;
                            break;

                        case 'M':
                            if (_player == null || _oldStartPos != new Vector2(x, y))
                            {
                                if (_player == null)
                                    _player = Add(new Mario(device));

                                _player.Transform = Matrix.Translation(x, y + 0.5f, 0);

                                _oldStartPos = new Vector2(x, y);
                            }
                            break;

                        case 'G':
                            _map.Add(new Goomba(device)).Translate(x, y + 0.5f, 0);
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
                        int width = 1;
                        while (x + 1 < line.Length && line[x + 1] == c)
                        {
                            width++;
                            x++;
                        }
                        var obj = _map.Add(new Block(device, width, 1, 2, blockTexture));
                        obj.Translate(x - (float)(width - 1) / 2, y, 0);
                    }
                }

                y--;
            }
        }

        public override void Update(Device device, TimeSpan deltaTime)
        {
            _checkFileCount += deltaTime;
            if (_checkFileCount > VerifyMapFile)
            {
                _checkFileCount = TimeSpan.Zero;
                var fileDate = File.GetLastWriteTime(_mapFile);
                if (fileDate > _mapModifyDate)
                {
                    _map.Dispose();
                    ReloadScene(device);
                    _mapModifyDate = fileDate;
                    _map.Init();
                }
            }

            if (deltaTime > MaxFrameDelay)
                deltaTime = MaxFrameDelay;

            base.Update(device, deltaTime);
        }
    }
}
