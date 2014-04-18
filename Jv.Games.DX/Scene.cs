using Jv.Games.DX.Components;
using SharpDX;
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
        Dictionary<string, Effect> ShadersByName;
        Dictionary<Effect, Dictionary<string, List<MeshRenderer>>> RenderersByTechnique;

        public Scene(Device device)
        {
            _device = device;
            Cameras = new List<Camera>();
            ShadersByName = new Dictionary<string, Effect>();
            RenderersByTechnique = new Dictionary<Effect, Dictionary<string, List<MeshRenderer>>>();
        }

        public IDisposable Register(Camera camera)
        {
            Cameras.Add(camera);
            return Disposable.Create(() => Cameras.Remove(camera));
        }

        public IDisposable Register(MeshRenderer renderer)
        {
            Effect shader;
            if (!ShadersByName.TryGetValue(renderer.Material.Effect, out shader))
                ShadersByName[renderer.Material.Effect] = (shader = LoadShaderFromFile(renderer.Material.Effect));

            if (!RenderersByTechnique.ContainsKey(shader))
                RenderersByTechnique[shader] = new Dictionary<string, List<MeshRenderer>>();

            if (!RenderersByTechnique[shader].ContainsKey(renderer.Material.Technique))
                RenderersByTechnique[shader][renderer.Material.Technique] = new List<MeshRenderer>();

            var regLocation = RenderersByTechnique[shader][renderer.Material.Technique];
            regLocation.Add(renderer);
            return Disposable.Create(() => regLocation.Remove(renderer));
        }

        Effect LoadShaderFromFile(string file)
        {
            return Effect.FromFile(_device, file, ShaderFlags.Debug);
        }

        public void Draw()
        {
            _device.BeginScene();

            foreach (var cam in Cameras)
            {
                _device.Viewport = cam.Viewport;

                var vp = cam.View * cam.Projection;

                foreach (var kvS in RenderersByTechnique)
                {
                    var shader = kvS.Key;
                    var wvpHandler = shader.GetParameter(null, "gWVP");

                    foreach (var kvT in kvS.Value)
                    {
                        var technique = kvT.Key;
                        shader.Technique = shader.GetTechnique(technique);

                        foreach (var renderer in kvT.Value)
                        {
                            var mesh = renderer.Mesh;

                            var mvp = renderer.Object.GlobalTransform * vp;
                            shader.SetValue(wvpHandler, mvp);
                            renderer.Material.SetValues(shader);

                            _device.VertexDeclaration = renderer.Mesh.VertexDeclaration;
                            _device.SetStreamSource(0, renderer.Mesh.Vertex, 0, renderer.Mesh.VertexSize);
                            _device.Indices = renderer.Mesh.Index;

                            var passes = shader.Begin();
                            for (var pass = 0; pass < passes; pass++ )
                            {
                                shader.BeginPass(pass);
                                _device.DrawIndexedPrimitive(mesh.PrimitiveType, 0, 0, mesh.NumVertices, 0, mesh.NumPrimitives);
                                shader.EndPass();
                            }
                        }
                    }
                }
            }
            _device.EndScene();
            _device.Present();
        }
    }
}
