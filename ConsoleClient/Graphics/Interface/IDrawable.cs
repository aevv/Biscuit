using ConsoleClient.Graphics.UI;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;

namespace ConsoleClient.Graphics.Interface
{
    interface IDrawable
    {
        SizeF Size { get; }
        SizeF SubDrawableSize { get; }
        PointF SubDrawableLocation { get; }
        PointF Location { get; }
        PointF CameraRelative { get; }
        Color4 Colour { get; }
        Camera Camera { get; }
        bool OnRenderFrame(FrameEventArgs ea, bool cameraRelative = true);
        void SetTexture(string filePath);
    }
}
