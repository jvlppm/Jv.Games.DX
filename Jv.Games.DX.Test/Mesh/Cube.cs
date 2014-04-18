using Jv.Games.DX.Test.Mesh;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Mesh
{
    class Cube : Mesh<SimpleVertex>
    {
        public Cube(Device device, float width, float height, float depth)
            : base(device, PrimitiveType.TriangleList, SimpleVertex.GetDeclaration(device))
        {
            var w = width / 2;
            var h = height / 2;
            var d = depth / 2;

            VertexData = new[]{
                // Front face
                new SimpleVertex(-w, -h, d, 0, 0, 1),
                new SimpleVertex(w, -h, d, 1, 0, 1),
                new SimpleVertex( w, h, d, 1, 1, 1),
                new SimpleVertex(-w, h, d, 0, 1, 1),

                // Back face
                new SimpleVertex(w, -h, -d, 1, 0, 0),
                new SimpleVertex(-w, -h, -d, 0, 0, 0),
                new SimpleVertex(-w, h, -d, 0, 1, 0),
                new SimpleVertex(w, h, -d, 1, 1, 0),

                // Top face
                new SimpleVertex(-w, h, d, 0, 1, 1),
                new SimpleVertex(w, h, d, 1, 1, 1),
                new SimpleVertex(w, h, -d, 1, 1, 0),
                new SimpleVertex(-w, h, -d, 0, 1, 0),

                // Bottom face
                new SimpleVertex(-w, -h, -d, 0, 0, 0),
                new SimpleVertex(w, -h, -d, 1, 0, 0),
                new SimpleVertex(w, -h, d, 1, 0, 1),
                new SimpleVertex(-w, -h, d, 0, 0, 1),

                // Right face
                new SimpleVertex(w, -h, d, 1, 0, 1),
                new SimpleVertex(w, -h, -d, 1, 0, 0),
                new SimpleVertex(w, h, -d, 1, 1, 0),
                new SimpleVertex(w, h, d, 1, 1, 1),

                // Left face
                new SimpleVertex(-w, -h, -d, 0, 0, 0),
                new SimpleVertex(-w, -h, d, 0, 0, 1),
                new SimpleVertex(-w, h, d, 0, 1, 0),
                new SimpleVertex(-w, h, -d, 0, 1, 0),
            };

            IndexData = new[]{
                0, 2, 1, 0, 3, 2,    // Front face
                4, 6, 5, 4, 7, 6,    // Back face
                8, 10, 9, 8, 11, 10,  // Top face
                12, 14, 13, 12, 15, 14, // Bottom face
                16, 18, 17, 16, 19, 18, // Right face
                20, 22, 21, 20, 23, 22  // Left face
            };
        }
    }
}
