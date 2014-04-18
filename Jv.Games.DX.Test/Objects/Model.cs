using Jv.Games.DX.Components;

namespace Jv.Games.DX.Test.Objects
{
    class Model<T> : GameObject
        where T : Material
    {
        public readonly T Material;

        public Model(IMesh mesh, T material)
        {
            Material = material;
            this.Attach(new MeshRenderer { Mesh = mesh, Material = material });
        }
    }
}
