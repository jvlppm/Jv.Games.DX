using System;

namespace Jv.Games.DX.Components
{
    public class MeshRenderer : Component, IUpdateable
    {
        public IMesh Mesh;
        public Material Material;

        public void Update(TimeSpan deltaTime)
        {
            if (Material != null)
                Material.Update(deltaTime);
        }
    }
}
