using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.WindowsAPICodePack.Taskbar;
using System.Drawing;


namespace UGS
{
    /// <summary>
    /// Interaction logic for UgsMainWindow.xaml
    /// </summary>
    public partial class UgsMainWindow : Window
    {
        public UgsMainWindow()
        {
            this.InitializeComponent();
        }

        private ViewModels.UGSViewModel UGSvm { get { return ((ViewModels.UGSViewModel)this.DataContext); } }

        #region Volume Slider
        bool mouseCaptured;
        private void volumeSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && ((ProgressBar)sender).Value < ((ProgressBar)sender).Maximum)
                ((ProgressBar)sender).Value++;

            else if (e.Delta < 0 && ((ProgressBar)sender).Value > ((ProgressBar)sender).Minimum)
                ((ProgressBar)sender).Value--;

            e.Handled = true;
        }

        private void volumeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && mouseCaptured)
            {
                var x = e.GetPosition(((ProgressBar)sender)).X;
                var ratio = x / ((ProgressBar)sender).ActualWidth;
                ((ProgressBar)sender).Value = (int)(ratio * ((ProgressBar)sender).Maximum);
            }
        }

        private void volumeSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = true;
            var s = (ProgressBar)sender;
            var x = e.GetPosition(s).X;
            var ratio = x / s.ActualWidth;
            s.Value = (int)(s.Minimum + ratio * (s.Maximum - s.Minimum));
        }

        private void volumeSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = false;
        }
        #endregion

        #region balance slider

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && mouseCaptured)
            {
                var x = e.GetPosition(((Slider)sender)).X;
                var ratio = x / ((Slider)sender).ActualWidth;
                ((Slider)sender).Value = (int)((ratio - 0.5) * ((Slider)sender).Maximum * 2);
            }
        }

        private void Slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = true;
            var x = e.GetPosition((Slider)sender).X;
            var ratio = x / ((Slider)sender).ActualWidth;
            ((Slider)sender).Value = (int)((ratio - 0.5) * ((Slider)sender).Maximum * 2);
        }

        private void Slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = false;
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && ((Slider)sender).Value < ((Slider)sender).Maximum)
                ((Slider)sender).Value++;

            else if (e.Delta < 0 && ((Slider)sender).Value > ((Slider)sender).Minimum)
                ((Slider)sender).Value--;

            e.Handled = true;
        }
        #endregion

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                UGSvm.ugs.CloseSerial();
            else
                UGSvm.ugs.OpenSerial(UGSvm.ugs.SerialPortName);
        }

        private void serialLogTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && e.LeftButton == MouseButtonState.Released)
            {
                UGSvm.ClearSerialLogCommand.Execute(null);
            }
        }

    }
}