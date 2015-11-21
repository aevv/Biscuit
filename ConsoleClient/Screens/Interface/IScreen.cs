using ConsoleClient.Graphics;
using OpenTK;
using System;

namespace ConsoleClient.Screens.Interface
{
    interface IScreen : IDisposable
    {
        string Name { get; }
        bool Enabled { get; }
        bool Visible { get; }
        bool Loaded { get; }
        Camera Camera { get; }

        void OnRenderFrame(FrameEventArgs args);
        void OnUpdateFrame(FrameEventArgs args);
        void OnLoad(EventArgs args);
        void Show();
        void Hide();
    }
}
