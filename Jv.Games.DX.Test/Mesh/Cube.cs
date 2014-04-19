using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Mesh
{
    class TexturedCube : Mesh<Vertex>
    {
        public TexturedCube(Device device, float width, float height, float depth,
            Vector2[] frontUV, Vector2[] backUV, Vector2[] topUV, Vector2[] bottomUV, Vector2[] leftUV, Vector2[] rightUV)

            : base(device, PrimitiveType.TriangleList, Vertex.GetDeclaration(device))
        {
            if (frontUV == null || frontUV.Length != 4 ||
                backUV == null || backUV.Length != 4 ||
                topUV == null || topUV.Length != 4 ||
                bottomUV == null || bottomUV.Length != 4 ||
                rightUV == null || rightUV.Length != 4 ||
                leftUV == null || leftUV.Length != 4)
                throw new ArgumentException("UV mappings must have 4 coordinates");

            var frontNormal = new Vector3(0, 0, -1);
            var backNormal = new Vector3(0, 0, 1);
            var leftNormal = new Vector3(-1, 0, 0);
            var rightNormal = new Vector3(1, 0, 0);
            var topNormal = new Vector3(0, 1, 0);
            var bottomNormal = new Vector3(0, -1, 0);

            // Convert texture coordinates from OpenGL to DirectX
            var OpenGLTextConv = new Vector2(1, -1);

            var w = width / 2;
            var h = height / 2;
            var d = depth / 2;

            VertexData = new[]{
                // Front face
                new Vertex { Position = new Vector3(-w, -h, -d), UV = frontUV[0] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, -h, -d), UV = frontUV[1] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = frontUV[2] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(-w, +h, -d), UV = frontUV[3] * OpenGLTextConv, Normal = frontNormal },

                // Back face
                new Vertex { Position = new Vector3(+w, -h, +d), UV = backUV[0] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(-w, -h, +d), UV = backUV[1] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(-w, +h, +d), UV = backUV[2] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(+w, +h, +d), UV = backUV[3] * OpenGLTextConv, Normal = backNormal },

                // Top face
                new Vertex { Position = new Vector3(-w, +h, -d), UV = topUV[0] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = topUV[1] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(+w, +h, +d), UV = topUV[2] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(-w, +h, +d), UV = topUV[3] * OpenGLTextConv, Normal = topNormal },

                // Bottom face
                new Vertex { Position = new Vector3(-w, -h, +d), UV = bottomUV[0] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(+w, -h, +d), UV = bottomUV[1] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(+w, -h, -d), UV = bottomUV[2] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(-w, -h, -d), UV = bottomUV[3] * OpenGLTextConv, Normal = bottomNormal },

                // Right face
                new Vertex { Position = new Vector3(+w, -h, -d), UV = rightUV[0] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, -h, +d), UV = rightUV[1] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, +h, +d), UV = rightUV[2] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = rightUV[3] * OpenGLTextConv, Normal = rightNormal },

                // Left face
                new Vertex { Position = new Vector3(-w, -h, +d), UV = leftUV[0] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, -h, -d), UV = leftUV[1] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, +h, -d), UV = leftUV[2] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, +h, +d), UV = leftUV[3] * OpenGLTextConv, Normal = leftNormal },
            };

            IndexData = new ushort[]{
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
