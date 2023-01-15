using System.Windows.Controls;

namespace onTrack.Components
{
    public class NavButton : Button
    {
        public NavButton()
        {
        }

        public override void OnApplyTemplate()
        {
            var textblock = (TextBlock) Template.FindName("textblock", this);
            var border = textblock.Parent as Border;
            textblock.Text = Content.ToString();
            textblock.FontSize = FontSize;
            border.Width = Width;
            border.Height = Height;
            border.Padding = Padding;
        }
    }
}
