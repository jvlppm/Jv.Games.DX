using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Jv.Games.DX
{
    public class GameObject : IDisposable, IEnumerable
    {
        #region Attributes
        Matrix _transform;
        Matrix? _globalTransform;

        List<GameObject> _children;
        List<Components.Component> _components;

        public GameObject Parent;
        #endregion

        #region Constructors
        public GameObject()
        {
            _children = new List<GameObject>();
            _components = new List<Components.Component>();
            Transform = Matrix.Identity;
        }
        #endregion

        #region Position
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

        void ClearGlobalTransform()
        {
            _globalTransform = null;
            _children.ForEach(c => c.ClearGlobalTransform());
        }

        public void Translate(Vector3 ammount)
        {
            Transform = _transform * Matrix.Translation(ammount);
        }
        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }
        #endregion

        #region Hierarchy
        public virtual GameObject Add(GameObject gameObject)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (gameObject.Parent != null)
                throw new InvalidOperationException("Specified object already have a Parent");

            gameObject.Parent = this;
            _children.Add(gameObject);
            return gameObject;
        }

        public GameObject Add(Components.Component component)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (component.Object != null)
                throw new InvalidOperationException("Specified component already have an associated object.");
            component.Object = this;
            _components.Add(component);
            return this;
        }
        public void Dettach(Components.Component component)
        {
            _components.Remove(component);
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
        #endregion

        public virtual void Init()
        {
            foreach (var cmp in _components)
                cmp.Init();

            foreach (var child in _children)
                child.Init();
        }

        #region IDisposable
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
                    Parent._children.Remove(this);

                foreach (var cmp in _components)
                {
                    cmp.Object = null;
                    cmp.Dispose();
                }

                foreach (var child in _children)
                {
                    child.Parent = null;
                    child.Dispose();
                }
            }
        }
        #endregion

        public IEnumerator GetEnumerator()
        {
            foreach (var cmp in _components)
                yield return cmp;
            foreach (var child in _children)
                yield return child;
        }
    }
}
