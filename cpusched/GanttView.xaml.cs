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
using cpusched.Processes;

namespace cpusched
{
    /// <summary>
    /// Interaction logic for GanttView.xaml
    /// </summary>
    public partial class GanttView : Window
    {
        public GanttView(ContextSwitchManager csm, Window caller)
        {
            InitializeComponent();
            this.DrawGantt(csm);
            this.Owner = caller;
            this.Show();
        }

        /// <summary>
        /// Contextualize Gantt Chart in here.
        /// </summary>
        /// <param name="csm"></param>
        private void ContextualizeGanttChart(ContextSwitchManager csm)
        {

            
            int j = 0;
            //Run through all the elapsed time of the queue.
            for (int i = 0; i <= csm.Switches[csm.Switches.Count - 1].Time; i++)
            {
                //Create New Group.
                if (i % 50 == 0)
                {

                }
                else
                {

                }
            }
        }

        /// <summary>
        /// Draw the Gantt Chart.
        /// </summary>
        /// <param name="csm"></param>
        private void DrawGantt(ContextSwitchManager csm)
        {
            double widthmult = 20.0;
            double minWidthAllowed = 5.0;
            double labelOffset = (widthmult/5);
            double totalWidth = 0.0;
            int prevTime = 0;
            int totalHeight = 0;

            //Iterate through all elements.
            for (int i = 0; i < csm.Switches.Count - 1; i++)
            //for (int i = 0; i<=14; i++)
            {
                //Determine the current time.
                int curTime = csm.Switches[i+1].Time - csm.Switches[i].Time;

                Label PrevTimeLabel = null;

                //double width = curTime * widthmult;
                double width = 50.0;
                double height = 20.0;

                if (totalWidth >= 500)
                {
                    totalWidth = 0.0;
                    totalHeight++;

                    Border previous = (Border)this.CANVAS.Children[this.CANVAS.Children.Count - 2];

                    PrevTimeLabel = new Label()
                    {
                        Margin = new Thickness { Left = previous.Margin.Left + ((TextBlock)previous.Child).Width - 15, Top = ((Label)this.CANVAS.Children[this.CANVAS.Children.Count-1]).Margin.Top },
                        Width = 30.0,
                        Content = csm.Switches[i].Time,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                        FontSize = 10.0
                    };

                }
                


                


                TextBlock tb = new TextBlock()
                {
                    FontFamily = new FontFamily("Arial"),
                    Foreground = this.getRGBABrush(0,0,0,255),
                    Padding = new Thickness(0.0,4.0,0,0),
                    //Width = width >= minWidthAllowed ? width : minWidthAllowed,
                    Width = width,
                    Height = height,
                    Background = csm.Switches[i].Running == null ? this.getRGBABrush(255,0,0,128) : this.getRGBABrush(0,255,0,200),
                    Text = csm.Switches[i].Running == null ? "[idle]" : csm.Switches[i].Running.Name,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    FontSize = 12.0
                };
                tb.Width = width >= minWidthAllowed ? width : minWidthAllowed;

                Border b = new Border()
                {
                    Margin = new Thickness { Left = totalWidth, Top = totalHeight * 50 },
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1.0),
                    Child = tb
                };

                Label time = new Label()
                {
                    Margin = new Thickness { Left = totalWidth-13, Top = totalHeight * 50 + tb.Height - 5 },
                    Width = 30.0,
                    Content = csm.Switches[i].Time,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                    FontSize = 10.0
                };

                if (PrevTimeLabel != null) this.CANVAS.Children.Add(PrevTimeLabel);
                this.CANVAS.Children.Add(b);
                this.CANVAS.Children.Add(time);

                //The last label.
                if (i == csm.Switches.Count - 2)
                {
                    Label finaltime = new Label()
                    {
                        Margin = new Thickness { Left = totalWidth + tb.Width-13, Top = totalHeight * 50 + tb.Height - 5 },
                        Width = 30.0,
                        Content = csm.Switches[i].Time+csm.Switches[i].Running.CurrentTime,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                        FontSize = 10.0
                    };
                    this.CANVAS.Children.Add(finaltime);
                }
                
                //Incrementors
                totalWidth += tb.Width;
                prevTime = curTime;
            }

            this.CANVAS.Height = this.CANVAS.ActualHeight;
            this.CANVAS.Width = this.CANVAS.ActualWidth;
        }

        private SolidColorBrush getRGBABrush(byte r, byte g, byte b, byte a)
        {
            return new SolidColorBrush(Color.FromArgb(a,r,g,b));
        }
    }
}
