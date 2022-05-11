using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoApplication.Behaviors
{
    internal class ScrollViewerHelper
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool),
                typeof(ScrollViewerHelper), new PropertyMetadata(false, OnAutoScrollChanged));

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is ScrollViewer scrollViewer)
            //{
            //    scrollViewer.ScrollChanged += (s, args) =>
            //    {
            //        if (args.ViewportHeightChange >0d)
            //        {
            //            scrollViewer.ScrollToBottom();
            //        }
            //    };
            //}

            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged += OnScrollChanged;
            }
            else if(d is ListBox listBox)
            {
                var childCount = VisualTreeHelper.GetChildrenCount(listBox);
                var border = VisualTreeHelper.GetChild(listBox, 0);
                var sv = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
                sv.ScrollChanged += OnScrollChanged;
            };
        }

        private static void OnScrollChanged(object sender, ScrollChangedEventArgs args)
        {
            var sv = sender as ScrollViewer;
            if (args.ViewportHeightChange > 0d)
            {
                sv.ScrollToBottom();
            }
        }

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }
    }
}
