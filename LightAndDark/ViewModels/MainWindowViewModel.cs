using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LightAndDark.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Color _BaseColor = Colors.Gray;
        private Color _LightColor;
        private Color _DarkColor;
        private double _Factor = 0.5;

        public MainWindowViewModel()
        {
            this.WhenPropertyChanged(x => x.BaseColor).Subscribe(x =>
            {
                LightColor = ChangeColorBrightness(BaseColor, _Factor);
                DarkColor = ChangeColorBrightness(BaseColor, _Factor * -1);
            });

            this.WhenPropertyChanged(x => x.Factor).Subscribe(x =>
            {
                LightColor = ChangeColorBrightness(BaseColor, _Factor);
                DarkColor = ChangeColorBrightness(BaseColor, _Factor * -1);
            });
        }

        public Color BaseColor
        {
            get => _BaseColor;
            set => this.RaiseAndSetIfChanged(ref _BaseColor, value);
        }
        public Color LightColor
        {
            get => _LightColor;
            private set => this.RaiseAndSetIfChanged(ref _LightColor, value);
        }

        public Color DarkColor
        {
            get => _DarkColor;
            private set => this.RaiseAndSetIfChanged(ref _DarkColor, value);
        }


        public double Factor
        {
            get => _Factor;
            set => this.RaiseAndSetIfChanged(ref _Factor, value);
        }

        public static Color ChangeColorBrightness(Color color, double newluminosityFactor)
        {
            var red = (double)color.R;
            var green = (double)color.G;
            var blue = (double)color.B;

            unchecked
            {
                if (newluminosityFactor < 0)//applies darkness
                {
                    newluminosityFactor++;
                    red *= newluminosityFactor;
                    green *= newluminosityFactor;
                    blue *= newluminosityFactor;
                    Debug.WriteLine($"dark: {red} {green} {blue}");
                }
                else if (newluminosityFactor >= 0) //applies lightness
                {
                    red = (255 - red) * newluminosityFactor + red;
                    green = (255 - green) * newluminosityFactor + green;
                    blue = (255 - blue) * newluminosityFactor + blue;

                    Debug.WriteLine($"light: {red} {green} {blue}");
                }
                else
                {
                    throw new ArgumentOutOfRangeException("The Luminosity Factor must be a finite number.");
                }

                var c = Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);

                Debug.WriteLine(c);
                return c;
            }
        }
    }
}
