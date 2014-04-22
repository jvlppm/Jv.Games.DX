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
        Mario _player;
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
                        Speed = 40,
                        SlowingDist = 5,
                        Offset = new Vector3(0, 4, 0),
                        Mask = new Vector3(1, 1, 0)
                    },
                    new RigidBody
                    {
                        Friction = new Vector3(8, 20, 8),
                        //MaxSpeed = new Vector3(5f)
                    }
                };
                camera.Viewport = new SharpDX.Viewport(0, 0, window.Width, window.Height);
                camera.SetPerspective(60, window.Width / (float)window.Height, 1, 5000);
                //camera.SetOrthographic(20, 20 * (window.Height / (float)window.Width), 1, 5000);

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
            for (var l = 0; l < mapContent.Length; l++)
            {
                var line = mapContent[l];
                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    string blockTexture = null;

                    switch (c)
                    {
                        case 'm':
                        case 'M':
                            if (_player == null || _oldStartPos != new Vector2(x, y))
                            {
                                if (_player == null)
                                    _player = (Mario)Add(new Mario(device));

                                _player.Transform = Matrix.Translation(x, y + 0.5f, 0);

                                _oldStartPos = new Vector2(x, y);
                                _player.IsSmall = c == 'm';
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

                        case 'P':
                            if (l > 0 && mapContent[l - 1].Length > x && mapContent[l - 1][x] == c)
                                break;
                            var height = 1;
                            while (l + height < mapContent.Length && mapContent[l + height][x] == c)
                                height++;

                            _map.Add(new Pipe(device, height)).Translate(x, y - (float)(height - 1) / 2, 0);
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

            foreach(var u in Updateables)
            {
                u.Owner.Enabled = false;
                var pos = u.Owner.GlobalTransform.TranslationVector;
                foreach(var cam in Cameras)
                {
                    var posC = cam.GlobalTransform.TranslationVector;
                    if ((new Vector2(posC.X - pos.X, posC.Y - pos.Y)).Length() < 30)
                    {
                        u.Owner.Enabled = true;
                        break;
                    }
                }
            }

            base.Update(device, deltaTime);
        }
    }
}
