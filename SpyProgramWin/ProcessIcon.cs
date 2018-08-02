using SpyProgram.Windows.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpyProgram.Windows
{
    public class ProcessIcon : IDisposable
    {
        private readonly NotifyIcon icon = new NotifyIcon();

        public ProcessIcon(string logPath)
        {
            icon.Icon = Resources.S;
            icon.ContextMenuStrip = CreateMenu(logPath);
        }

        public void Dispose()
        {
            icon.Dispose();
        }

        public void Display()
        {
            icon.Visible = true;
        }

        public void Hide()
        {
            icon.Visible = false;
        }

        private ContextMenuStrip CreateMenu(string logPath)
        {
            var strip = new ContextMenuStrip();
            AddOpenLogMenuItem(strip, logPath);            
            AddCloseMenuItem(strip);

            return strip;
        }

        private static void AddCloseMenuItem(ContextMenuStrip strip)
        {
            var item2 = new ToolStripMenuItem();
            item2.Text = Resources.CloseMenuText;
            item2.Click += (s, e) => Application.Exit();
            strip.Items.Add(item2);
        }

        private static void AddOpenLogMenuItem(ContextMenuStrip strip, string logPath)
        {
            var item1 = new ToolStripMenuItem();
            item1.Text = Resources.LogMenuText;
            item1.Click += (s, e) => Process.Start(logPath);
            strip.Items.Add(item1);
        }
    }
}
