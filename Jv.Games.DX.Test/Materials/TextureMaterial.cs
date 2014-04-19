using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Jv.Games.DX.Test.Materials
{
    class TextureMaterial : Material
    {
        EffectHandle _gTexture;
        Texture _texture;

        public Texture Texture
        {
            get { return _texture; }
            set { _texture = value; SetTexture(_gTexture, value); }
        }

        public TextureMaterial()
            : base("Materials/texture.fx", "TransformTech")
        {
        }

        public override void Init(Effect effect)
        {
            _gTexture = effect.GetParameter(null, "gTexture");
            SetTexture(_gTexture, _texture);

            base.Init(effect);
        }
    }
}
