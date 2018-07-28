using System;
using System.Collections.Generic;
using System.Text;

namespace SpyProgramCore
{
    public interface IKeydownTracker
    {
        event Action<int> KeyDown;
        void Track();
        void Stop();
    }
}
