using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cpusched
{
    /// <summary>
    /// Interaction logic for ContextSwitchView.xaml
    /// </summary>
    public partial class ContextSwitchView : Window
    {


        public ContextSwitchView(string s)
        {
            InitializeComponent();
            this.PopulateTextbox(s);
            
        }

        /// <summary>
        /// Populates the text box with the contextSwitch String.
        /// </summary>
        /// <param name="s"></param>
        public void PopulateTextbox(string s){

            //TODO: Clean this up, make it faster - maybe switch to a databinding in place of a direct Text reference?
            this.MAIN_TEXT.Text = s;
        }
    }
}
