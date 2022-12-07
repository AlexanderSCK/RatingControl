using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKCanvasView = SkiaSharp.Views.Maui.Controls.SKCanvasView;

namespace RatingControlMAUI
{
    public partial class RatingView : SKCanvasView
    {
        private PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
        private double touchX;
        private double touchY;
        public EventHandler<SKPaintSurfaceEventArgs> EventHandler { get; set; }
        public RatingView()
        {
            this.BackgroundColor = Colors.Transparent;
            this.EnableTouchEvents = true;
            this.PaintSurface += RatingView_PaintSurface;
            this.panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
            this.GestureRecognizers.Add(panGestureRecognizer);
        }

        private void RatingView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        #region BindableProperties

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(RatingView), default(double), propertyChanged: OnValueChanged);
        public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(string), typeof(RatingView), PathConstants.Star, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty CountProperty = BindableProperty.Create(nameof(Count), typeof(int), typeof(RatingView), 5, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty ColorOnProperty = BindableProperty.Create(nameof(ColorOn), typeof(Color), typeof(RatingView), MaterialColors.Amber.ToMauiColor(), propertyChanged: ColorOnChanged);
        public static readonly BindableProperty OutlineOnColorProperty = BindableProperty.Create(nameof(OutlineOnColor), typeof(Color), typeof(RatingView), SKColors.Transparent.ToMauiColor(), propertyChanged: OutlineOnColorChanged);
        public static readonly BindableProperty OutlineOffColorProperty = BindableProperty.Create(nameof(OutlineOffColor), typeof(Color), typeof(RatingView), MaterialColors.PlateYellow.ToMauiColor(), propertyChanged: OutlineOffColorChanged);
        public static readonly BindableProperty RatingTypeProperty = BindableProperty.Create(nameof(RatingType), typeof(RatingType), typeof(RatingView), RatingType.Floating, propertyChanged: OnPropertyChanged);

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, this.ClampValue(value)); }
        }

        public double ClampValue(double val)
        {
            if (val < 0)
                return 0;
            else if (val > this.Count)
                return this.Count;
            else
                return val;
        }
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public Color ColorOn
        {
            get { return (Color)GetValue(ColorOnProperty); }
            set { SetValue(ColorOnProperty, value); }
        }

        public Color OutlineOnColor
        {
            get { return (Color)GetValue(OutlineOnColorProperty); }
            set { SetValue(OutlineOnColorProperty, value); }
        }

        public Color OutlineOffColor
        {
            get { return (Color)GetValue(OutlineOffColorProperty); }
            set { SetValue(OutlineOffColorProperty, value); }
        }

        public RatingType RatingType
        {
            get { return (RatingType)GetValue(RatingTypeProperty); }
            set { SetValue(RatingTypeProperty, value); }
        }

        public float Spacing { get; set; } = 8;

        /// <summary>
        /// Gets or sets the color of the canvas background.
        /// </summary>
        /// <value>The color of the canvas background.</value>
        public SKColor CanvasBackgroundColor { get; set; } = SKColors.Transparent;

        /// <summary>
        /// Gets or sets the width of the stroke.
        /// </summary>
        /// <value>The width of the stroke.</value>
        public float StrokeWidth { get; set; } = 12f;

        private float ItemWidth { get; set; }
        private float ItemHeight { get; set; }
        private float CanvasScale { get; set; }
        private SKColor SKColorOn { get; set; } = MaterialColors.PlateYellow;
        private SKColor SKOutlineOnColor { get; set; } = MaterialColors.Black;
        private SKColor SKOutlineOffColor { get; set; } = MaterialColors.PlateYellow;

        #endregion


        //private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        //}

        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(this.CanvasBackgroundColor);

            var path = SKPath.ParseSvgPathData(this.Path);

            var itemWidth = ((width - (this.Count - 1) * this.Spacing)) / this.Count;
            var scaleX = (itemWidth / (path.Bounds.Width));
            scaleX = (itemWidth - scaleX * this.StrokeWidth) / path.Bounds.Width;

            this.ItemHeight = height;
            var scaleY = this.ItemHeight / (path.Bounds.Height);
            scaleY = (this.ItemHeight - scaleY * this.StrokeWidth) / (path.Bounds.Height);
            
            this.CanvasScale = Math.Min(scaleX, scaleY);
            this.ItemWidth = path.Bounds.Width * this.CanvasScale;

            canvas.Scale(this.CanvasScale);
            canvas.Translate(this.StrokeWidth / 2, this.StrokeWidth / 2);
            canvas.Translate(-path.Bounds.Left, 0);
            canvas.Translate(0, -path.Bounds.Top);

            using (var strokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = this.OutlineOffColor.ToSKColor(),
                StrokeWidth = this.StrokeWidth,
                StrokeCap = SKStrokeCap.Round,  
                IsAntialias = true,
            })
            using (var fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = this.SKColorOn,
                IsAntialias = true,
            })
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (i <= this.Value - 1) // Full
                    {
                        canvas.DrawPath(path, fillPaint);
                        canvas.DrawPath(path, strokePaint);
                    }
                    else if (i < this.Value) //Partial
                    {
                        float filledPercentage = (float)(this.Value - Math.Truncate(this.Value));
                        strokePaint.Color = this.SKOutlineOffColor;
                        canvas.DrawPath(path, strokePaint);

                        using (var rectPath = new SKPath())
                        {
                            var rect = SKRect.Create(path.Bounds.Left + path.Bounds.Width * filledPercentage, path.Bounds.Top, path.Bounds.Width * (1 - filledPercentage), this.ItemHeight*2);
                            rectPath.AddRect(rect);
                            canvas.ClipPath(rectPath, SKClipOperation.Difference);
                            canvas.DrawPath(path, fillPaint);
                        }
                    }
                    else //Empty
                    {
                        strokePaint.Color = this.SKOutlineOffColor;
                        canvas.DrawPath(path, strokePaint);
                    }

                    canvas.Translate((this.ItemWidth + this.Spacing) / this.CanvasScale, 0);
                }
            }

        }
        protected override void OnTouch(SKTouchEventArgs e)
        {
            this.touchX = e.Location.X;
            this.touchY = e.Location.Y;
            this.SetValue(touchX, touchY);
            this.InvalidateSurface();
        }

        public void SetValue(double x, double y)
        {
            var val = this.CalculateValue(x);
            switch (this.RatingType)
            {
                case RatingType.Full:
                    this.Value = ClampValue((double)Math.Ceiling(val));
                    break;
                case RatingType.Half:
                    this.Value = ClampValue((double)Math.Round(val * 2) / 2);
                    break;
                case RatingType.Floating:
                    this.Value = ClampValue(val);
                    break;
            }
        }

        private double CalculateValue(double x)
        {
            if (x < this.ItemWidth)
                return (double)x / this.ItemWidth;
            else if (x < this.ItemWidth + this.Spacing)
                return 1;
            else
                return 1 + CalculateValue(x - (this.ItemWidth + this.Spacing));
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var point = ConvertToPixel(new Point(e.TotalX, e.TotalY));
            if (e.StatusType != GestureStatus.Completed)
            {
                this.SetValue(touchX + point.X, touchY + e.TotalY);
                this.InvalidateSurface();
            }
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingView;
            view.InvalidateSurface();
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingView;
            view.Value = view.ClampValue((double)newValue);
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void ColorOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingView;
            view.SKColorOn = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void OutlineOffColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingView;
            view.SKOutlineOffColor = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void OutlineOnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingView;
            view.SKOutlineOnColor = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(this.CanvasSize.Width * pt.X / this.Width),
                               (float)(this.CanvasSize.Height * pt.Y / this.Height));
        }

    }
}
