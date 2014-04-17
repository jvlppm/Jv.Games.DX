using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public static class Extensions
    {
        public static Lock<DataStream> LockData(this VertexBuffer buffer)
        {
            var data = buffer.Lock(0, 0, LockFlags.None);
            return new Lock<DataStream>(data, buffer.Unlock);
        }

        public static Lock<DataStream> LockData(this IndexBuffer buffer)
        {
            var data = buffer.Lock(0, 0, LockFlags.None);
            return new Lock<DataStream>(data, buffer.Unlock);
        }
    }
}
