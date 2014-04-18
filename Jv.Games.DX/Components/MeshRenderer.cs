using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Components
{
    public class MeshRenderer : Component
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _sceneRegistration.Dispose();
            base.Dispose(disposing);
        }
    }
}
