using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public class Disposable : IDisposable
    {
        public static readonly IDisposable Empty;

        static Disposable()
        {
            Empty = new Disposable(null);
        }

        Action<bool> _onDispose;

        private Disposable(Action<bool> onDispose)
        {
            _onDispose = onDispose;
        }

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_onDispose != null)
            {
                _onDispose(disposing);
                _onDispose = null;
            }
        }

        public static IDisposable Create(Action<bool> onDispose)
        {
            return new Disposable(onDispose);
        }

        public static IDisposable Create(Action onDispose)
        {
            return new Disposable(b => onDispose());
        }
    }
}
