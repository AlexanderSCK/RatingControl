using SkiaSharp;
using SkiaSharp.Views.Maui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingControlMAUI.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public ValueToColorConverter()
        {
        }

        public SKColor MinColor { get; set; } = MaterialColors.Red;
        public SKColor MaxColor { get; set; } = MaterialColors.LightBlue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = int.Parse(parameter as string);
            var current = (double)value;
            var amount = current / count;
            return new SKColor(
                (byte)(MinColor.Red + amount * (MaxColor.Red - MinColor.Red)),
                (byte)(MinColor.Green + amount * (MaxColor.Green - MinColor.Green)),
                (byte)(MinColor.Blue + amount * (MaxColor.Blue - MinColor.Blue)),
                (byte)(MinColor.Alpha + amount * (MaxColor.Alpha - MinColor.Alpha))).ToMauiColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueToMoviesColorConverter : IValueConverter
    {
        public ValueToMoviesColorConverter()
        {
        }

        public SKColor MinColor { get; set; } = MaterialColors.Lime;
        public SKColor MaxColor { get; set; } = MaterialColors.DeepOrange;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = int.Parse(parameter as string);
            var current = (double)value;
            var amount = current / count;
            return new SKColor(
                (byte)(MinColor.Red + amount * (MaxColor.Red - MinColor.Red)),
                (byte)(MinColor.Green + amount * (MaxColor.Green - MinColor.Green)),
                (byte)(MinColor.Blue + amount * (MaxColor.Blue - MinColor.Blue)),
                (byte)(MinColor.Alpha + amount * (MaxColor.Alpha - MinColor.Alpha))).ToMauiColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
