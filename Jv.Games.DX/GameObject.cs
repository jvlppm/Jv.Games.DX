using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX
{
    public class GameObject
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

        void Add(Components.Component component)
        {
            if (component.Object != null)
                throw new InvalidOperationException("Specified component already have an associated object.");
            component.Object = this;
            Components.Add(component);
        }

        void Add(GameObject gameObject)
        {
            if (gameObject.Parent != null)
                throw new InvalidOperationException("Specified object already have a Parent");

            gameObject.Parent = this;
            Children.Add(gameObject);
        }

        private void ClearGlobalTransform()
        {
            _globalTransform = null;
            Children.ForEach(c => c.ClearGlobalTransform());
        }

        public void Destroy()
        {
            Parent.Children.Remove(this);
        }
    }
}
