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
            : base(device, PrimitiveType.TriangleList)
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
                0, 1, 2, 0, 2, 3,    // Front face
                4, 5, 6, 4, 6, 7,    // Back face
                8, 9, 10, 8, 10, 11,  // Top face
                12, 13, 14, 12, 14, 15, // Bottom face
                16, 17, 18, 16, 18, 19, // Right face
                20, 21, 22, 20, 22, 23  // Left face
            };
        }
    }
}
