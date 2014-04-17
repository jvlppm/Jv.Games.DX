using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public interface IMesh
    {
        SharpDX.Direct3D9.VertexBuffer Vertex { get; }
        SharpDX.Direct3D9.IndexBuffer Index { get; }
        int NumVertices { get; }
        int NumPrimitives { get; }
        SharpDX.Direct3D9.PrimitiveType PrimitiveType { get; }
    }

    public class Mesh<DataType>
        where DataType : struct
    {
        public SharpDX.Direct3D9.VertexBuffer Vertex { get; private set; }
        public SharpDX.Direct3D9.IndexBuffer Index { get; private set; }
        public int NumVertices { get; private set; }
        public int NumPrimitives { get; private set; }
        public SharpDX.Direct3D9.PrimitiveType PrimitiveType { get; private set; }

        public Mesh(SharpDX.Direct3D9.PrimitiveType type)
        {
            PrimitiveType = type;
        }

        public DataType[] VertexDataStream
        {
            set
            {
                using (var stream = Vertex.LockData())
                    stream.Data.WriteRange(value);
                NumVertices = value.Length;
            }
        }

        public int[] IndexDataStream
        {
            set
            {
                using (var stream = Vertex.LockData())
                    stream.Data.WriteRange(value);

                switch (PrimitiveType)
                {
                    case SharpDX.Direct3D9.PrimitiveType.TriangleStrip:
                        NumPrimitives = value.Length - 2;
                        break;
                    case SharpDX.Direct3D9.PrimitiveType.TriangleList:
                        NumPrimitives = value.Length / 3;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
