using SharpDX.Direct3D9;
using System.Collections.Generic;

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
