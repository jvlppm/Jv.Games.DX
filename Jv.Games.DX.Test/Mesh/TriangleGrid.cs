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
        public TriangleGrid(Device device, int width, int height, int rows, int cols)
            : base(device, PrimitiveType.TriangleStrip)
        {
            float squareWidth = width / (float)cols;
            float squareHeight = height / (float)rows;

            float baseX = width / 2;
            float baseY = height / 2;
            var vertices = new List<SimpleVertex>();

            for (int row = 0; row <= rows; row++)
            {
                for (int col = 0; col <= cols; col++)
                {
                    float r = 1 - (0.6f * (rows - row) / (float)rows) - (0.2f * (cols - col) / (float)cols);
                    float g = r;
                    float b = r;

                    vertices.Add(new SimpleVertex
                    {
                        Position = new Vector3(col * squareWidth - baseX, 0.0f, row * squareHeight - baseY),
                        Color = new Color((int)(255 * r), (int)(255 * g), (int)(255 * b))
                    });
                }
            }

            var index = new List<int>();

            for (int row = 0; row < rows; row++)
            {
                int rowStartIndex = row * (cols + 1);

                if (row % 2 == 0)
                {
                    for (int i = 0; i <= cols; i++)
                    {
                        index.Add(rowStartIndex + i);
                        index.Add(rowStartIndex + i + cols + 1);
                    }
                }
                else
                {
                    for (int i = cols; i > 0; i--)
                    {
                        index.Add(rowStartIndex + i + cols + 1);
                        index.Add(rowStartIndex + i - 1);
                    }

                    if (row + 1 >= rows)
                        index.Add(rowStartIndex + cols + 1);
                }
            }

            VertexData = vertices.ToArray();
            IndexData = index.ToArray();
        }
    }
}
