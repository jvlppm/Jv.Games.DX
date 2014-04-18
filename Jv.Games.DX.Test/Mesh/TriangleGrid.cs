using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Mesh
{
    class TriangleGrid : Mesh<SimpleVertex>
    {
        public TriangleGrid(Device device, float width, float depth, int rows, int cols)
            : base(device, PrimitiveType.TriangleList, SimpleVertex.GetDeclaration(device))
        {
            float squareWidth = width / (float)cols;
            float squareDepth = depth / (float)rows;

            float baseX = width / 2;
            float baseY = depth / 2;
            var vertices = new List<SimpleVertex>();

            //for (int row = 0; row <= rows; row++)
            //{
            //    for (int col = 0; col <= cols; col++)
            //    {
            //        float r = 1 - (0.6f * (rows - row) / (float)rows) - (0.2f * (cols - col) / (float)cols);
            //        float g = r;
            //        float b = r;

            //        vertices.Add(new SimpleVertex
            //        {
            //            Position = new Vector3(col * squareWidth - baseX, 0.0f, row * squareDepth - baseY),
            //            Color = new Color((int)(255 * r), (int)(255 * g), (int)(255 * b))
            //        });
            //    }
            //}



            var numVertices = rows * cols;

            //Posiciona no centro do sistema
            var px = -width * 0.5f;
            var pz = depth * 0.5f;

            var cellRows = rows - 1;
            var cellCols = cols - 1;

            float dx = width / cellCols;
            float dz = depth / cellRows;

            int k = 0;
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                {
                    vertices.Add(new SimpleVertex(
                        j * dx + px,
                        0.0f,
                        -i * dz + pz,
                        0.4f, 0.4f, 1.0f));
                    ++k;
                }

            var index = new List<ushort>();

            for (var i = 0; i < cellRows; ++i)
            {
                for (var j = 0; j < cellCols; ++j)
                {
                    index.Add((ushort)(i * cols + j));
                    index.Add((ushort)(i * cols + j + 1));
                    index.Add((ushort)((i + 1) * cols + j));

                    index.Add((ushort)((i + 1) * cols + j));
                    index.Add((ushort)(i * cols + j + 1));
                    index.Add((ushort)((i + 1) * cols + j + 1));
                }
            }

            VertexData = vertices.ToArray();
            IndexData = index.ToArray();
        }
    }
}
