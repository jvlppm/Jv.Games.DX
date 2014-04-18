using System;

namespace Jv.Games.DX
{
    public interface IUpdateable
    {
        void Update(TimeSpan deltaTime);
    }
}
