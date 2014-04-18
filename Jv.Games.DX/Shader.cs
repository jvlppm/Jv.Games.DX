using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public class Material
    {
        struct Uniform
        {
            public string Name;
            public Action<Effect> SetValue;
            public EffectHandle Handle;
        }

        public readonly string Effect;
        public readonly string Technique;

        Dictionary<string, Uniform> Uniforms;

        public Material(string effect, string technique)
        {
            Effect = effect;
            Technique = technique;
            Uniforms = new Dictionary<string, Uniform>();
        }

        protected void Set<T>(string name, T value)
            where T : struct
        {
            Uniform uniform;
            if (!Uniforms.TryGetValue(name, out uniform))
                Uniforms[name] = uniform = new Uniform { Name = name };
            uniform.SetValue = e =>
            {
                if (uniform.Handle == null)
                    uniform.Handle = e.GetParameter(null, uniform.Name);
                e.SetValue(uniform.Handle, value);
            };
        }

        public void SetValues(SharpDX.Direct3D9.Effect shader)
        {
            foreach(var uniform in Uniforms.Values)
                uniform.SetValue(shader);
        }
    }
}
