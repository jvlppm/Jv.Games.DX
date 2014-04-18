using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX
{
    public class GameObject : IDisposable
    {
        Matrix _transform;
        Matrix? _globalTransform;

        public GameObject Parent;
        public List<GameObject> Children;
        public List<Components.Component> Components;

        public Matrix Transform
        {
            get { return _transform; }
            set
            {
                _transform = value;
                ClearGlobalTransform();
            }
        }

        public Matrix GlobalTransform
        {
            get
            {
                if (Parent == null)
                    return _transform;
                return (_globalTransform ?? (_globalTransform = _transform * Parent.GlobalTransform)).Value;
            }
        }

        public GameObject()
        {
            Children = new List<GameObject>();
            Components = new List<Components.Component>();
            Transform = Matrix.Identity;
        }

        public GameObject Add(Components.Component component)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (component.Object != null)
                throw new InvalidOperationException("Specified component already have an associated object.");
            component.Object = this;
            Components.Add(component);
            return this;
        }

        public virtual T Add<T>(T gameObject)
            where T : GameObject
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (gameObject.Parent != null)
                throw new InvalidOperationException("Specified object already have a Parent");

            gameObject.Parent = this;
            Children.Add(gameObject);
            return gameObject;
        }

        void ClearGlobalTransform()
        {
            _globalTransform = null;
            Children.ForEach(c => c.ClearGlobalTransform());
        }

        public virtual void Init()
        {
            foreach (var cmp in Components)
                cmp.Init();

            foreach (var child in Children)
                child.Init();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GameObject()
        {
            Dispose(false);
        }

        bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                if (Parent != null)
                    Parent.Children.Remove(this);

                foreach (var cmp in Components)
                {
                    cmp.Object = null;
                    cmp.Dispose();
                }

                foreach (var child in Children)
                {
                    child.Parent = null;
                    child.Dispose();
                }
            }
        }

        public T GetParent<T>()
            where T : GameObject
        {
            var current = Parent;
            while (current != null && !(current is T))
                current = current.Parent;

            if (current == null)
                throw new Exception("Parent of type " + typeof(T).FullName + " was not found");

            return (T)current;
        }
    }
}
