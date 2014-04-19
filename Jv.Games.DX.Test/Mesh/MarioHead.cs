using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Mesh
{
    class MarioHead : Mesh<Vertex>
    {
        static Vector2[] FrontUV = new[]{
            new Vector2(0.555218084865440f, 0.372123763830412f),
            new Vector2(0.701497572842308f, 0.188534221090767f),
            new Vector2(0.844870845826258f, 0.373347694115343f),
            new Vector2(0.699560096180363f, 0.555713306570058f)
        };

        static Vector2[] BackUV = new[]{
            new Vector2(0.411844811881490f, 0.554489376285127f),
            new Vector2(0.511624859971672f, 0.681778125917948f),
            new Vector2(0.367282848656749f, 0.864143738372662f),
            new Vector2(0.265565323904622f, 0.738078919024772f)
        };

        static Vector2[] TopUV = new[]{
            new Vector2(0.700528834511336f, 0.554489376285127f),
            new Vector2(0.843902107495286f, 0.372123763830412f),
            new Vector2(0.996962763788962f, 0.564280818564574f),
            new Vector2(0.849714537481121f, 0.747870361304220f)
        };

        static Vector2[] BottomUV = new[]{
            new Vector2(0.412813550212463f, 0.187310290805836f),
            new Vector2(0.556186823196413f, 0.004944678351121f),
            new Vector2(0.702466311173281f, 0.189758151375698f),
            new Vector2(0.558124299858358f, 0.370899833545481f)
        };

        static Vector2[] RightUV = new[]{
            new Vector2(0.845839584157231f, 0.007392538920983f),
            new Vector2(0.946588370578385f, 0.134681288553804f),
            new Vector2(0.844870845826258f, 0.370899833545481f),
            new Vector2(0.701497572842308f, 0.188534221090767f)
        };

        static Vector2[] LeftUV = new[]{
            new Vector2(0.409907335219545f, 0.555713306570058f),
            new Vector2(0.556186823196413f, 0.370899833545481f),
            new Vector2(0.701497572842308f, 0.554489376285127f),
            new Vector2(0.510656121640699f, 0.683002056202879f)
        };

        public MarioHead(Device device, float size, float slope = 0.6f )
            : base(device, PrimitiveType.TriangleList, Vertex.GetDeclaration(device))
        {
            var frontNormal = new Vector3(0, 0, -1);
            var backNormal = new Vector3(0, 0, 1);
            var leftNormal = new Vector3(-1, 0, 0);
            var rightNormal = new Vector3(1, 0, 0);
            var topNormal = new Vector3(0, 1, 0);
            var bottomNormal = new Vector3(0, -1, 0);

            // Convert texture coordinates from OpenGL to DirectX
            var OpenGLTextConv = new Vector2(1, -1);

            var w = size / 2;
            var h = size / 2;
            var d = size / 2;

            var p = h * slope;

            VertexData = new[]{
                // Front face
                new Vertex { Position = new Vector3(-w, -h, -d), UV = FrontUV[0] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, -h, -d), UV = FrontUV[1] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = FrontUV[2] * OpenGLTextConv, Normal = frontNormal },
                new Vertex { Position = new Vector3(-w, +h, -d), UV = FrontUV[3] * OpenGLTextConv, Normal = frontNormal },

                // Back face
                new Vertex { Position = new Vector3(-w, -h, +d), UV = BackUV[0] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(-w, +p, +d), UV = BackUV[1] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(+w, +p, +d), UV = BackUV[2] * OpenGLTextConv, Normal = backNormal },
                new Vertex { Position = new Vector3(+w, -h, +d), UV = BackUV[3] * OpenGLTextConv, Normal = backNormal },

                // Top face
                new Vertex { Position = new Vector3(-w, +p, +d), UV = TopUV[0] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(-w, +h, -d), UV = TopUV[1] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = TopUV[2] * OpenGLTextConv, Normal = topNormal },
                new Vertex { Position = new Vector3(+w, +p, +d), UV = TopUV[3] * OpenGLTextConv, Normal = topNormal },

                // Bottom face
                new Vertex { Position = new Vector3(-w, -h, +d), UV = BottomUV[0] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(+w, -h, +d), UV = BottomUV[1] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(+w, -h, -d), UV = BottomUV[2] * OpenGLTextConv, Normal = bottomNormal },
                new Vertex { Position = new Vector3(-w, -h, -d), UV = BottomUV[3] * OpenGLTextConv, Normal = bottomNormal },

                // Right face
                new Vertex { Position = new Vector3(+w, -h, +d), UV = RightUV[0] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, +p, +d), UV = RightUV[1] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, +h, -d), UV = RightUV[2] * OpenGLTextConv, Normal = rightNormal },
                new Vertex { Position = new Vector3(+w, -h, -d), UV = RightUV[3] * OpenGLTextConv, Normal = rightNormal },

                // Left face
                new Vertex { Position = new Vector3(-w, -h, +d), UV = LeftUV[0] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, -h, -d), UV = LeftUV[1] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, +h, -d), UV = LeftUV[2] * OpenGLTextConv, Normal = leftNormal },
                new Vertex { Position = new Vector3(-w, +p, +d), UV = LeftUV[3] * OpenGLTextConv, Normal = leftNormal },
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
