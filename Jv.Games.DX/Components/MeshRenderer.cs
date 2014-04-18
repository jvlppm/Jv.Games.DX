using System;

namespace Jv.Games.DX.Components
{
    public class MeshRenderer : Component, IUpdateable
    {
        IDisposable _sceneRegistration;

        public IMesh Mesh;
        public Material Material;

        public override void Init()
        {
            if (_sceneRegistration != null)
                throw new InvalidOperationException("The MeshRenderer is already registered in a scene.");

            _sceneRegistration = Object.GetParent<Scene>().Register(this);
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            if (Material != null)
                Material.Update(deltaTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _sceneRegistration.Dispose();
            base.Dispose(disposing);
        }
    }
}
