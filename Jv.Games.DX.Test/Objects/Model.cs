using Jv.Games.DX.Components;

namespace Jv.Games.DX.Test.Objects
{
    class Model : GameObject
    {
        public readonly Material Material;

        public Model(IMesh mesh, Material material)
        {
            Material = material;
            this.Add(new MeshRenderer { Mesh = mesh, Material = material });
        }
    }
}
