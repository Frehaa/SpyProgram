using System;
using System.Collections.Generic;
using System.Text;

namespace SpyProgramCore
{
    public interface IMouseDownTracker
    {
        event Action<int> MouseDown;
        void Track();
        void Stop();
    }
}
