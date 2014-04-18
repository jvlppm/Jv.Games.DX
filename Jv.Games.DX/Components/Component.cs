using System;

namespace Jv.Games.DX.Components
{
    public abstract class Component : IDisposable
    {
        public GameObject Object;

        public virtual void Init()
        {
            Object.GetParent<Scene>().Register(this);
        }

        ~Component()
        {
            Dispose(false);
        }

        bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                if (Object != null)
                    Object.Dettach(this);
            }
        }
    }
}
