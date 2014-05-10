using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Mesh
{
    class Billboard : Mesh<Vertex>
    {
        public Billboard(Device device, float width, float height)

            : base(device, PrimitiveType.TriangleList, Vertex.GetDeclaration(device))
        {
            var frontNormal = new Vector3(0, 0, -1);
            var backNormal = new Vector3(0, 0, 1);

            var w = width / 2;
            var h = height / 2;

            VertexData = new[]{
                // Front face - 0
                new Vertex { Position = new Vector3(-w, -h, 0), UV = new Vector2(0, 1), Normal = frontNormal },
                new Vertex { Position = new Vector3(-w, +h, 0), UV = new Vector2(0, 0), Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, -h, 0), UV = new Vector2(1, 1), Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, +h, 0), UV = new Vector2(1, 0), Normal = frontNormal },
                
                // Back face - 4
                new Vertex { Position = new Vector3(+w, -h, 0), UV = new Vector2(1, 1), Normal = backNormal },
                new Vertex { Position = new Vector3(+w, +h, 0), UV = new Vector2(1, 0), Normal = backNormal },
                new Vertex { Position = new Vector3(-w, -h, 0), UV = new Vector2(0, 1), Normal = backNormal },
                new Vertex { Position = new Vector3(-w, +h, 0), UV = new Vector2(0, 0), Normal = backNormal },
            };

            IndexData = new ushort[]{
                0, 1, 2, 1, 3, 2,    // Front face
                4, 5, 6, 5, 7, 6,    // Back face
            };
        }
    }
}
