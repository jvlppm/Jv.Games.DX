using System;

namespace Jv.Games.DX.Components
{
    public abstract class Component : IDisposable
    {
        IDisposable _sceneRegistration;
        public GameObject Object;
        internal bool Attached;

        public virtual void Init()
        {
            if (_sceneRegistration != null)
                throw new InvalidOperationException("This component is already registered in a scene.");
            _sceneRegistration = Object.GetAncestor<Scene>().Register(this);
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
                if (_sceneRegistration != null)
                    _sceneRegistration.Dispose();

                if (Object != null)
                    Object.Dettach(this);
            }
        }
    }
}
