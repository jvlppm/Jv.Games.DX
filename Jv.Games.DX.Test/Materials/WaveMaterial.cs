using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Jv.Games.DX.Test.Materials
{
    class WaveMaterial : Material
    {
        EffectHandle _gTime, _gSource;
        TimeSpan _time;
        Vector3 _source;

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; Set(_gTime, (float)value.TotalSeconds); }
        }

        public Vector3 Source
        {
            get { return _source; }
            set { _source = value; Set(_gSource, value); }
        }

        public WaveMaterial()
            : base("Materials/wave.fx", "TransformTech")
        {
        }

        public override void Init(Effect effect)
        {
            _gTime = effect.GetParameter(null, "gTime");
            Set(_gTime, (float)_time.TotalSeconds);
            _gSource = effect.GetParameter(null, "gSource");
            Set(_gSource, _source);

            base.Init(effect);
        }

        public override void Update(TimeSpan deltaTime)
        {
            Time += deltaTime;
            base.Update(deltaTime);
        }
    }
}
