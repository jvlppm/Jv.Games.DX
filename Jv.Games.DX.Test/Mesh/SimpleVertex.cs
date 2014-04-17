using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Mesh
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SimpleVertex
    {
        static VertexDeclaration _declaration;

        public Vector3 Position;
        public Color Color;

        public SimpleVertex(float x, float y, float z, float r, float g, float b, float a = 1)
        {
            Position = new Vector3(x, y, z);
            Color = new Color(r, g, b, a);
        }

        public static SharpDX.Direct3D9.VertexDeclaration GetDeclaration(Device device)
        {
            return _declaration ?? (_declaration = new VertexDeclaration(device, new[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            }));
        }
    }
}
