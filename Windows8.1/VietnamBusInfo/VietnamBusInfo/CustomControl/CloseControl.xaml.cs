﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VietnamBusInfo.CustomControl
{
    public sealed partial class CloseControl : UserControl
    {
        public CloseControl()
        {
            this.InitializeComponent();
        }

        private void CloseIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Grid g = sender as Grid;
            CloseControl cc = g.Parent as CloseControl;
            Grid pGrid = cc.Parent as Grid;

            pGrid.Children.Clear();
        }
    }
}
