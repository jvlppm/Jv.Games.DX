using Jv.Games.DX.Components;

namespace Jv.Games.DX.Test.Objects
{
    class Model : GameObject
    {
        public Model(IMesh mesh, Material material)
        {
            this.Components.Add(new MeshRenderer { Mesh = mesh, Material = material });
        }
    }
}
