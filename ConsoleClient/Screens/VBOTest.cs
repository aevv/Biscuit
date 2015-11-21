using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace ConsoleClient.Screens
{
    class VBOTest : BaseScreen
    {
        private uint[] _vboId;
        public VBOTest(Game game, string name)
            : base(game, name)
        {

        }

        readonly uint[,] _vertices = { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 1, 0 } };
        public override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            _vboId = new uint[2];
            GL.GenBuffers(2, _vboId);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vboId[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_vertices.Length * 2 * sizeof(uint)), _vertices, BufferUsageHint.StaticDraw);
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vboId[1]);
            GL.DrawElements(BeginMode.Triangles, _vertices.Length, DrawElementsType.UnsignedInt, _vertices);
        }
    }
}
