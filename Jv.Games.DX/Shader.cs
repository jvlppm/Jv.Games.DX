using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public class Shader
    {
        public Shader(Device device, string file)
        {

        }

        protected void Set(string name, object value)
        {
            throw new NotImplementedException();
        }
    }

    public class Material
    {
        class Uniform
        {
            public string Name;
            public object Value;
            //Location
        }

        public readonly string Shader;
        public readonly string Technique;

        Dictionary<string, Uniform> Uniforms;

        public Material(string shader, string technique)
        {
            Shader = shader;
            Technique = technique;
            Uniforms = new Dictionary<string, Uniform>();
        }

        protected void Set(string name, Matrix value)
        {
            Uniform uniform;
            if (!Uniforms.TryGetValue(name, out uniform))
                Uniforms[name] = uniform = new Uniform { Name = name };
            uniform.Value = value;
        }
    }
}
