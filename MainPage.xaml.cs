using System;
using Microsoft.Maui.Controls;

namespace Clock
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Используем Dispatcher(таймер) для обновления каждую секунду
            Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                ClockCanvas.Invalidate(); // Перерисовка графики
                return true;
            });
        }
    }
}