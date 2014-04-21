using System;

namespace Jv.Games.DX.Components
{
    public class MeshRenderer : Component
    {
        IDisposable _sceneRegistration;
        public IMesh Mesh;
        public Material Material;

        public override void Init()
        {
            base.Init();

            if (_sceneRegistration != null)
                throw new InvalidOperationException("This component is already registered in a scene.");

            if (Material != null)
                _sceneRegistration = Object.GetParent<Scene>().Register(Material, Object);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _sceneRegistration != null)
                _sceneRegistration.Dispose();

            base.Dispose(disposing);
        }
    }
}
