using System;

namespace SpyProgram
{
    public interface IMouseDownTracker
    {
        event Action<int> MouseDown;
        void Track();
        void Stop();
    }
}
