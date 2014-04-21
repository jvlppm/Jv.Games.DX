using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test
{
    static class Extensions
    {
        public static Matrix ExtractRotation(this Matrix matrix)
        {
            return Matrix.Scaling(matrix.ScaleVector) * Matrix.Translation(matrix.TranslationVector);
        }
    }
}
