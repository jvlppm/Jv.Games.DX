using SharpDX;
using SharpDX.Direct3D9;
using System.Runtime.InteropServices;

namespace Jv.Games.DX.Test.Mesh
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TexVertex
    {
        static VertexDeclaration _declaration;

        public Vector3 Position;
        public Vector2 UV;

        public static SharpDX.Direct3D9.VertexDeclaration GetDeclaration(Device device)
        {
            return _declaration ?? (_declaration = new VertexDeclaration(device, new[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                VertexElement.VertexDeclarationEnd
            }));
        }
    }
}
