using Jv.Games.DX.Components;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jv.Games.DX
{
    public class Scene : GameObject
    {
        struct RenderInfo
        {
            public GameObject Object;
            public EffectHandle WVPHandle;
            public MeshRenderer Renderer;
            public Effect Effect;
            public EffectHandle Technique;
        }

        readonly Device _device;
        List<IUpdateable> Updateables;
        List<Camera> Cameras;
        Dictionary<string, Effect> ShadersByName;
        Dictionary<Effect, Dictionary<EffectHandle, List<MeshRenderer>>> RenderersByTechnique;
        List<RenderInfo> SortedRendereres;

        public Scene(Device device)
        {
            _device = device;
            Cameras = new List<Camera>();
            Updateables = new List<IUpdateable>();
            ShadersByName = new Dictionary<string, Effect>();
            RenderersByTechnique = new Dictionary<Effect, Dictionary<EffectHandle, List<MeshRenderer>>>();
            SortedRendereres = new List<RenderInfo>();
        }

        #region Register
        public IDisposable Register(object item)
        {
            var disposables = new List<IDisposable>();

            var camera = item as Camera;
            if (camera != null)
                disposables.Add(RegisterCamera(camera));

            var renderer = item as MeshRenderer;
            if (renderer != null)
                disposables.Add(RegisterRenderer(renderer));

            var updateable = item as IUpdateable;
            if (updateable != null)
                disposables.Add(RegisterUpdateable(updateable));

            if (disposables.Count <= 0)
                return Disposable.Empty;

            return Disposable.Create(() => disposables.ForEach(d => d.Dispose()));
        }

        IDisposable RegisterUpdateable(IUpdateable updateable)
        {
            Updateables.Add(updateable);
            return Disposable.Create(() => Updateables.Remove(updateable));
        }

        IDisposable RegisterCamera(Camera camera)
        {
            Cameras.Add(camera);
            return Disposable.Create(() => Cameras.Remove(camera));
        }

        IDisposable RegisterRenderer(MeshRenderer renderer)
        {
            Effect shader;
            if (!ShadersByName.TryGetValue(renderer.Material.Effect, out shader))
                ShadersByName[renderer.Material.Effect] = (shader = LoadShaderFromFile(renderer.Material.Effect));

            var technique = shader.GetTechnique(renderer.Material.Technique);

            if (renderer.Material.SortRendering)
            {
                var gWVP = shader.GetParameter(null, "gWVP");
                var renderInfo = new RenderInfo { Object = renderer.Object, Effect = shader, Technique = technique, Renderer = renderer, WVPHandle = gWVP };
                SortedRendereres.Add(renderInfo);
                return Disposable.Create(() => SortedRendereres.Remove(renderInfo));
            }

            if (!RenderersByTechnique.ContainsKey(shader))
                RenderersByTechnique[shader] = new Dictionary<EffectHandle, List<MeshRenderer>>();

            if (!RenderersByTechnique[shader].ContainsKey(renderer.Material.Technique))
                RenderersByTechnique[shader][technique] = new List<MeshRenderer>();

            var regLocation = RenderersByTechnique[shader][technique];
            regLocation.Add(renderer);
            return Disposable.Create(() => regLocation.Remove(renderer));
        }
        #endregion

        Effect LoadShaderFromFile(string file)
        {
            return Effect.FromFile(_device, file, ShaderFlags.Debug);
        }

        public override void Init()
        {
            base.Init();

            foreach (var item in from kvS in RenderersByTechnique
                                 from kvT in kvS.Value
                                 from r in kvT.Value
                                 select new { Renderer = r, Shader = kvS.Key })
                item.Renderer.Material.Init(item.Shader);

            foreach (var item in SortedRendereres)
                item.Renderer.Material.Init(item.Effect);
        }

        public void Update(TimeSpan deltaTime)
        {
            foreach (var obj in Updateables)
                obj.Update(deltaTime);
        }

        public void Draw()
        {
            _device.Clear(SharpDX.Direct3D9.ClearFlags.Target | SharpDX.Direct3D9.ClearFlags.ZBuffer, new SharpDX.Color(0, 0, 0), 1.0f, 0);
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
                        shader.Technique = technique;

                        foreach (var renderer in kvT.Value)
                        {
                            var mesh = renderer.Mesh;

                            var mvp = renderer.Object.GlobalTransform * vp;
                            shader.SetValue(wvpHandler, mvp);

                            _device.VertexDeclaration = renderer.Mesh.VertexDeclaration;
                            _device.SetStreamSource(0, renderer.Mesh.VertexBuffer, 0, renderer.Mesh.VertexSize);
                            _device.Indices = renderer.Mesh.IndexBuffer;

                            renderer.Material.SetValues();

                            var passes = shader.Begin();
                            for (var pass = 0; pass < passes; pass++)
                            {
                                shader.BeginPass(pass);
                                _device.DrawIndexedPrimitive(mesh.PrimitiveType, 0, 0, mesh.NumVertices, 0, mesh.NumPrimitives);
                                shader.EndPass();
                            }
                            shader.End();
                        }
                    }
                }

                foreach (var renderInfo in SortedRendereres.OrderByDescending(o => (o.Object.GlobalTransform.TranslationVector - cam.GlobalTransform.TranslationVector).LengthSquared()))
                {
                    var mvp = renderInfo.Object.GlobalTransform * vp;
                    renderInfo.Effect.SetValue(renderInfo.WVPHandle, mvp);

                    var mesh = renderInfo.Renderer.Mesh;

                    _device.VertexDeclaration = mesh.VertexDeclaration;
                    _device.SetStreamSource(0, mesh.VertexBuffer, 0, mesh.VertexSize);
                    _device.Indices = mesh.IndexBuffer;

                    renderInfo.Renderer.Material.SetValues();

                    var passes = renderInfo.Effect.Begin();
                    for (var pass = 0; pass < passes; pass++)
                    {
                        renderInfo.Effect.BeginPass(pass);
                        _device.DrawIndexedPrimitive(mesh.PrimitiveType, 0, 0, mesh.NumVertices, 0, mesh.NumPrimitives);
                        renderInfo.Effect.EndPass();
                    }
                    renderInfo.Effect.End();
                }
            }
            _device.EndScene();
            _device.Present();
        }
    }
}
