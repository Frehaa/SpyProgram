using System;

namespace SpyProgram
{
    public interface IKeyDownTracker
    {
        event Action<int> KeyDown;
        void Track();
        void Stop();
    }
}
