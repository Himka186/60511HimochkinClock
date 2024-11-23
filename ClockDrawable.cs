using Microsoft.Maui.Graphics;
using System;

namespace Clock
{
    public class ClockDrawable : IDrawable
    {
        public static ClockDrawable Instance { get; } = new ClockDrawable();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Очистка фона
            canvas.FillColor = Colors.Black;
            canvas.FillRectangle(dirtyRect);

            // Получаем размеры экрана
            float screenWidth = dirtyRect.Width;
            float screenHeight = dirtyRect.Height;

            // Центр экрана
            float centerX = screenWidth / 2;
            float centerY = screenHeight / 2;

            // Настраиваем размер цифр и промежутков
            float digitWidth = screenWidth / 15; // Уменьшенный размер цифр
            float space = digitWidth / 2;       // Пробел между символами

            // Получаем текущее время
            DateTime now = DateTime.Now;
            string time = now.ToString("HH:mm:ss");

            // Рассчитываем общую ширину строки
            float totalWidth = (digitWidth * 6) + (space * 5) + (digitWidth * 2); // Учитываем ширину всех цифр и двоеточий

            // Начальная позиция для отрисовки (центрируем строку)
            float x = centerX - (totalWidth / 2);
            float y = centerY - (digitWidth); // Центрируем относительно высоты цифры

            // Рисуем каждую цифру или символ
            foreach (char c in time)
            {
                if (c == ':')
                {
                    DrawColon(canvas, x, y, digitWidth / 7); // Рисуем двоеточие
                    x += digitWidth / 4 + space;            // Добавляем промежуток
                }
                else
                {
                    int digit = c - '0';
                    DrawDigit(canvas, x, y, digitWidth, digit);
                    x += digitWidth + space;                // Добавляем промежуток
                }
            }
        }

        private void DrawColon(ICanvas canvas, float x, float y, float dotSize)
        {
            canvas.FillColor = Colors.LimeGreen;

            // Верхняя точка
            canvas.FillCircle(x + dotSize, y + dotSize * 3, dotSize);

            // Нижняя точка
            canvas.FillCircle(x + dotSize, y + dotSize * 10, dotSize);
        }

        private void DrawDigit(ICanvas canvas, float x, float y, float width, int digit)
        {
            // Высота цифры
            float height = width * 2;
            float lineWidth = width / 10; // Толщина линий относительно ширины цифры

            // Устанавливаем толщину линии
            canvas.StrokeSize = lineWidth;
            canvas.StrokeColor = Colors.LimeGreen;

            // Массив сегментов для цифр
            // Каждый сегмент: {x1, y1, x2, y2} (координаты начала и конца линии)
            (float x1, float y1, float x2, float y2)[] segments =
            {
                (x, y, x + width, y),                         //0 Верхняя горизонтальная
                (x, y, x, y + height / 2),                   //1 Верхняя левая вертикальная
                (x + width, y, x + width, y + height / 2),   //2 Верхняя правая вертикальная
                (x, y + height / 2, x + width, y + height / 2), //3 Средняя горизонтальная
                (x, y + height / 2, x, y + height),          //4 Нижняя левая вертикальная
                (x + width, y + height / 2, x + width, y + height), //5 Нижняя правая вертикальная
                (x, y + height, x + width, y + height)       //6 Нижняя горизонтальная
            };

            // Определяем, какие сегменты активны для цифры
            int[][] digitMap =
            {
                new[] { 0, 1, 2, 4, 5, 6 },    // 0
                new[] { 2, 5 },                // 1
                new[] { 0, 2, 3, 4, 6 },       // 2
                new[] { 0, 2, 3, 5, 6 },       // 3
                new[] { 1, 2, 3, 5 },          // 4
                new[] { 0, 1, 3, 5, 6 },       // 5
                new[] { 0, 1, 3, 4, 5, 6 },    // 6
                new[] { 0, 2, 5 },             // 7
                new[] { 0, 1, 2, 3, 4, 5, 6 }, // 8
                new[] { 0, 1, 2, 3, 5, 6 }     // 9
            };

            // Рисуем активные сегменты
            foreach (int segmentIndex in digitMap[digit])
            {
                var segment = segments[segmentIndex];
                canvas.DrawLine(segment.x1, segment.y1, segment.x2, segment.y2);
            }
        }
    }
}
