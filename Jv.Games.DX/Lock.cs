using System;

namespace Jv.Games.DX
{
    public struct Lock<T> : IDisposable
    {
        public readonly T Data;
        Action _onUnlock;

        public Lock(T data, Action onUnlock)
        {
            Data = data;
            _onUnlock = onUnlock;
        }

        public void Dispose()
        {
            if (_onUnlock == null)
                return;

            _onUnlock();
            _onUnlock = null;
        }
    }
}
