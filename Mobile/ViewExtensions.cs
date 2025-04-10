namespace Mobile
{
    public static class ViewExtensions
    {
        public static Point GetAbsoluteLocation(this VisualElement view)
        {
            var x = view.X;
            var y = view.Y;
            var parent = view.Parent;

            while (parent is VisualElement p)
            {
                x += p.X;
                y += p.Y;
                parent = p.Parent;
            }

            return new Point(x, y);
        }
    }
}
