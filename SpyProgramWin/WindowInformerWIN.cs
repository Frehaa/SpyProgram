﻿namespace SpyProgram.Windows
{
    public class WindowInformerWIN : IWindowInformer
    {
        public string GetActiveWindowTitle()
        {
            return WINAPI.WindowsAPIHelper.GetActiveWindowTitle();
        }

        public string GetActiveWindowFileName()
        {
            return WINAPI.WindowsAPIHelper.GetActiveWindowProcessFileName();
        }
    }
}
