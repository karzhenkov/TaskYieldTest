using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sample
{
    class MainButton : Button
    {
        public MainButton()
        {
            Content = "Go!";
            Margin = new Thickness(4);
        }

        protected override void OnClick()
        {
            Console.WriteLine();
            Console.WriteLine("OnClick: enter");
            base.OnClick();
            Console.WriteLine("OnClick: exit");
        }
    }

    class MainWindow : Window
    {
        private bool YieldFlag;

        private static int ThreadId => Thread.CurrentThread.ManagedThreadId;

        public MainWindow()
        {
            Title = "TaskYieldTest";
            SizeToContent = SizeToContent.Height;
            ResizeMode = ResizeMode.NoResize;
            Width = 250;

            var button = new MainButton();
            button.Click += Button_Click;
            AddChild(button);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            YieldFlag = !YieldFlag;
            Console.WriteLine($" Button_Click: enter (Thread: {ThreadId}, yield: {YieldFlag})");
            if (YieldFlag) await Task.Yield();
            Console.WriteLine($" Button_Click: exit  (Thread: {ThreadId})");
        }
    }

    class Program
    {
        [STAThread]
        static void Main()
        {
            new Application().Run(new MainWindow());
        }
    }
}
