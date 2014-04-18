using Jv.Games.DX.Components;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    class Scene : GameObject
    {
        readonly Device _device;
        List<Camera> Cameras;
        Dictionary<string, Shader> ShadersByName;
        Dictionary<Shader, Dictionary<string, List<MeshRenderer>>> RenderersByTechnique;

        public Scene(Device device)
        {
            _device = device;
            Cameras = new List<Camera>();
            ShadersByName = new Dictionary<string, Shader>();
            RenderersByTechnique = new Dictionary<Shader, Dictionary<string, List<MeshRenderer>>>();
        }

        public IDisposable Register(Camera camera)
        {
            Cameras.Add(camera);
            return Disposable.Create(() => Cameras.Remove(camera));
        }

        public IDisposable Register(MeshRenderer renderer)
        {
            Shader shader;
            if (!ShadersByName.TryGetValue(renderer.Material.Shader, out shader))
                ShadersByName[renderer.Material.Shader] = (shader = LoadShaderFromFile(renderer.Material.Shader));

            //no load do shader preenche Dictionary<shader, Dictionary<techs, List{}>>
            if (!RenderersByTechnique.ContainsKey(shader))
                RenderersByTechnique[shader] = new Dictionary<string, List<MeshRenderer>>();

            if (!RenderersByTechnique[shader].ContainsKey(renderer.Material.Technique))
                RenderersByTechnique[shader][renderer.Material.Technique] = new List<MeshRenderer>();

            var regLocation = RenderersByTechnique[shader][renderer.Material.Technique];
            regLocation.Add(renderer);
            return Disposable.Create(() => regLocation.Remove(renderer));
        }

        Shader LoadShaderFromFile(string file)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            foreach (var cam in Cameras)
            {
                //set viewport

                foreach (var kvS in RenderersByTechnique)
                {
                    var shader = kvS.Key;
                    //activate shader

                    foreach (var kvT in kvS.Value)
                    {
                        var technique = kvT.Key;

                        //activate technique
                        //draw passes
                        //deactivate technique
                    }
                }
            }
        }
    }
}
