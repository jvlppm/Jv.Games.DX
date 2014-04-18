using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    class Scene : GameObject
    {
        List<Camera> Cameras;

        public Scene()
        {
            Cameras = new List<Camera>();
        }

        public override T Add<T>(T gameObject)
        {
            var cam = gameObject as Camera;
            if(cam != null)
            {
                if (cam.Parent != null)
                    throw new InvalidOperationException("Specified camera already have a Parent");
                cam.Parent = this;
                Cameras.Add(cam);
                return gameObject;
            }
            return base.Add(gameObject);
        }
    }
}
