using SharpDX;
using System;
using System.Linq;
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
        IDisposable _sceneRegistration;

        public GameObject Parent;
        public bool Visible = true;
        public bool Enabled = true;
        public string Tag;
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

            if (component.Attached)
                throw new InvalidOperationException("Specified component already have an associated object.");
            component.Attached = true;

            component.Object = component.Object ?? this;
            _components.Add(component);
            return this;
        }
        public void Dettach(Components.Component component)
        {
            if(_components.Remove(component))
                component.Attached = false;
        }

        public T GetAncestor<T>()
            where T : GameObject
        {
            var current = this;
            while (current != null && !(current is T))
                current = current.Parent;

            if (current == null)
                throw new Exception("Parent of type " + typeof(T).FullName + " was not found");

            return (T)current;
        }

        public bool CanRender()
        {
            var current = this;
            do
            {
                if (!current.Visible)
                    return false;

                current = current.Parent;
            } while (current != null);
            return true;
        }

        public bool CanUpdate()
        {
            var current = this;
            do
            {
                if (!current.Enabled)
                    return false;

                current = current.Parent;
            } while (current != null);
            return true;
        }
        #endregion

        public virtual void Init()
        {
            if (!(this is Scene))
            {
                if (_sceneRegistration != null)
                    throw new InvalidOperationException("This GameObject is already registered in a scene.");
                _sceneRegistration = GetAncestor<Scene>().Register(this);
            }

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

                if (_sceneRegistration != null)
                    _sceneRegistration.Dispose();

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

        public IEnumerable<T> SearchObjectsRecursively<T>(Func<T, bool> predicate) where T : GameObject
        {
            var toProcess = new Queue<GameObject>();
            toProcess.Enqueue(this);
            GameObject current;

            while (toProcess.Count > 0)
            {
                current = toProcess.Dequeue();

                var possible = current as T;

                if (possible != null && predicate(possible))
                    yield return possible;

                foreach (var c in current._children)
                    toProcess.Enqueue(c);
            }
        }

        public T SearchComponent<T>(List<GameObject> ignoreObjects = null) where T : Components.Component
        {
            var toProcess = new Queue<GameObject>();
            toProcess.Enqueue(this);

            ignoreObjects = ignoreObjects ?? new List<GameObject>();
            GameObject current;

            while (toProcess.Count > 0)
            {
                current = toProcess.Dequeue();

                if (ignoreObjects.Contains(current))
                    continue;
                ignoreObjects.Add(current);

                var found = current._components.OfType<T>().FirstOrDefault();

                if (found != null)
                    return found;

                foreach (var c in current._children)
                    toProcess.Enqueue(c);
            }

            current = this;
            while (current.Parent != null)
            {
                current = current.Parent;

                var found = current._components.OfType<T>().FirstOrDefault();

                if (found != null)
                    return found;
            }

            return null;
        }

        public void SendMessage(string message, bool recursively, params object[] args)
        {
            var m = GetType().GetMethod(message, args.Select(a => a.GetType()).ToArray());
            if(m != null)
                m.Invoke(this, args);

            foreach(var c in _components)
            {
                m = c.GetType().GetMethod(message, args.Select(a => a.GetType()).ToArray());
                if(m != null)
                    m.Invoke(c, args);
            }

            if (recursively)
                _children.ForEach(c => c.SendMessage(message, recursively, args));
        }
    }
}
