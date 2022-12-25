using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CustomButtons.Controls
{
    public class CustomButton : Button 
    {
        public CustomButton()
        {
        }

        public override void OnApplyTemplate()
        {
            var text = (TextBlock)Template.FindName("text", this);
            text.Text = this.Content.ToString();
        }
    }
}
