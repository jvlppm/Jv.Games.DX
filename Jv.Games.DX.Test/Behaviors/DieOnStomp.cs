using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Behaviors
{
    class DieOnStomp : Components.Component, IUpdateable
    {
        bool _dying;
        float _scaleY = 1f;

        public string[] Tags;

        public void OnStomp(Collider collider)
        {
            if (Tags == null || (collider.Object != null && Tags.Contains(collider.Object.Tag)))
                _dying = true;
        }

        public void Update(TimeSpan deltaTime)
        {
            if (!_dying)
                return;

            Object.Transform = Matrix.Scaling(1, _scaleY, 1) * Matrix.Translation(Object.Transform.TranslationVector);
            _scaleY -= 3f * (float)deltaTime.TotalSeconds;

            if (_scaleY < 0.1f)
                Object.Dispose();
        }
    }
}
