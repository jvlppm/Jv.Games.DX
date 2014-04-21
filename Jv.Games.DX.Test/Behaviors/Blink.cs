using System;

namespace Jv.Games.DX.Test.Behaviors
{
    class Blink : Components.Component, IUpdateable
    {
        TimeSpan _currentDuration, _currentInteval;
        bool? _originalVisibility;

        public TimeSpan Duration = TimeSpan.FromSeconds(2);
        public TimeSpan Interval = TimeSpan.FromSeconds(0.1);

        public bool IsActive
        {
            get { return _currentDuration > TimeSpan.Zero; }
            set
            {
                if (value)
                {
                    if (!IsActive)
                    {
                        _currentDuration = Duration;
                        _originalVisibility = Object.Visible;
                        _currentInteval = TimeSpan.Zero;
                    }
                }
                else
                {
                    if (_originalVisibility != null)
                    {
                        Object.Visible = _originalVisibility.Value;
                        _originalVisibility = null;
                    }
                    _currentDuration = TimeSpan.Zero;
                }
            }
        }

        public void Update(TimeSpan deltaTime)
        {
            if (!IsActive)
                return;

            _currentDuration -= deltaTime;
            _currentInteval -= deltaTime;

            if(_currentInteval <= TimeSpan.Zero)
            {
                _currentInteval = Interval;
                Object.Visible = !Object.Visible;
            }
            if(_currentDuration <= TimeSpan.Zero)
                IsActive = false;
        }

        public void OnHit()
        {
            IsActive = true;
        }
    }
}
