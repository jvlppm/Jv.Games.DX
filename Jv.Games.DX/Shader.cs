using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace Jv.Games.DX
{
    public class Material
    {
        struct Uniform
        {
            public Action<Effect> SetValue;
            public EffectHandle Handle;
        }

        public readonly string Effect;
        public readonly string Technique;

        public readonly bool SortRendering;
        Effect _shader;

        Dictionary<EffectHandle, Uniform> Uniforms;

        public Material(string effect, string technique, bool sortRendering)
        {
            Effect = effect;
            Technique = technique;
            SortRendering = sortRendering;
            Uniforms = new Dictionary<EffectHandle, Uniform>();
        }

        protected void Set<T>(EffectHandle handle, T value)
            where T : struct
        {
            if (handle == null)
                return;
            Uniforms[handle] = new Uniform {
                Handle = handle,
                SetValue = e => e.SetValue(handle, value)
            };
        }

        protected void SetTexture(EffectHandle handle, Texture value)
        {
            if (handle == null)
                return;
            Uniforms[handle] = new Uniform
            {
                Handle = handle,
                SetValue = e => e.SetTexture(handle, value)
            };
        }

        public virtual void Init(Effect shader)
        {
            _shader = shader;
        }

        public void SetValues()
        {
            foreach (var uniform in Uniforms.Values)
                uniform.SetValue(_shader);
        }
    }
}
