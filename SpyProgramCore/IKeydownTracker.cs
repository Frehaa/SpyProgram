using System;

namespace SpyProgram
{
    public interface IKeydownTracker
    {
        event Action<int> KeyDown;
        void Track();
        void Stop();
    }
}
