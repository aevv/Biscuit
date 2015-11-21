namespace ConsoleClient.Input
{
    /// <summary>
    /// Represents the state of the mouse at a given point in time.
    /// </summary>
    struct MouseState
    {
        public bool LeftButton { get; set; }
        public bool RightButton { get; set; }
        public bool MiddleButton { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
