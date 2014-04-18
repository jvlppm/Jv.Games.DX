//using SharpDX;
//using SharpDX.Direct3D9;

//namespace Jv.Games.DX.Test.Materials
//{
//    abstract class BaseMaterial : Material
//    {
//        EffectHandle _gWVP;

//        protected BaseMaterial(string shader, string technique)
//            : base(shader, technique)
//        {
//        }

//        public override void Init(Effect effect)
//        {
//            _gWVP = effect.GetParameter(null, "gWVP");
//            base.Init(effect);
//        }

//        public Matrix WVP { set { Set(_gWVP, value); } }
//    }
//}
