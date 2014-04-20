using Jv.Games.DX.Components;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jv.Games.DX
{
    public class Scene : GameObject
    {
        public Color ClearColor = new Color(0, 0, 0);

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
        Dictionary<string, Dictionary<string, List<RenderInfo>>> RenderersByTechnique;
        List<RenderInfo> SortedRendereres;

        public Scene(Device device)
        {
            _device = device;
            Cameras = new List<Camera>();
            Updateables = new List<IUpdateable>();
            ShadersByName = new Dictionary<string, Effect>();
            RenderersByTechnique = new Dictionary<string, Dictionary<string, List<RenderInfo>>>();
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

            var gWVP = shader.GetParameter(null, "gWVP");

            var technique = shader.GetTechnique(renderer.Material.Technique);

            var renderInfo = new RenderInfo { Object = renderer.Object, Effect = shader, Technique = technique, Renderer = renderer, WVPHandle = gWVP };

            renderInfo.Renderer.Material.Init(shader);

            if (renderer.Material.SortRendering)
            {
                SortedRendereres.Add(renderInfo);
                return Disposable.Create(() => SortedRendereres.Remove(renderInfo));
            }

            if (!RenderersByTechnique.ContainsKey(renderer.Material.Effect))
                RenderersByTechnique[renderer.Material.Effect] = new Dictionary<string, List<RenderInfo>>();

            if (!RenderersByTechnique[renderer.Material.Effect].ContainsKey(renderer.Material.Technique))
                RenderersByTechnique[renderer.Material.Effect][renderer.Material.Technique] = new List<RenderInfo>();

            var regLocation = RenderersByTechnique[renderer.Material.Effect][renderer.Material.Technique];
            regLocation.Add(renderInfo);
            return Disposable.Create(() => regLocation.Remove(renderInfo));
        }
        #endregion

        Effect LoadShaderFromFile(string file)
        {
            return Effect.FromFile(_device, file, ShaderFlags.Debug);
        }

        public virtual void Update(Device device, TimeSpan deltaTime)
        {
            foreach (var obj in Updateables)
                obj.Update(deltaTime);
        }

        public void Draw()
        {
            _device.Clear(SharpDX.Direct3D9.ClearFlags.Target | SharpDX.Direct3D9.ClearFlags.ZBuffer, ClearColor, 1.0f, 0);
            _device.BeginScene();

            foreach (var cam in Cameras)
            {
                _device.Viewport = cam.Viewport;

                var vp = cam.View * cam.Projection;

                foreach (var kvS in RenderersByTechnique)
                {
                    foreach (var kvT in kvS.Value)
                    {
                        foreach (var info in kvT.Value)
                        {
                            var mesh = info.Renderer.Mesh;
                            var shader = info.Effect;

                            var mvp = info.Object.GlobalTransform * vp;
                            shader.SetValue(info.WVPHandle, mvp);

                            _device.VertexDeclaration = mesh.VertexDeclaration;
                            _device.SetStreamSource(0, mesh.VertexBuffer, 0, mesh.VertexSize);
                            _device.Indices = mesh.IndexBuffer;

                            info.Renderer.Material.SetValues();

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
