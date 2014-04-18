using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Jv.Games.DX.Test.Materials
{
    class WaveMaterial : Material
    {
        EffectHandle _gTime, _gSource;
        TimeSpan _time;

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; Set(_gTime, value.TotalSeconds); }
        }

        public WaveMaterial()
            : base("Materials/wave.fx", "TransformTech")
        {
        }

        public override void Init(Effect effect)
        {
            _gTime = effect.GetParameter(null, "gTime");
            _gSource = effect.GetParameter(null, "gSource");
            Source = new Vector3();
            base.Init(effect);
        }

        public override void Update(TimeSpan deltaTime)
        {
            Time += deltaTime;
            base.Update(deltaTime);
        }

        public Vector3 Source { set { Set(_gSource, value); } }
    }
}
