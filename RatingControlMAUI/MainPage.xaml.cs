using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace RatingControlMAUI;

public partial class MainPage : ContentPage
{
    double lop;

    public MainPage()
	{
		InitializeComponent();
        this.ratingView1.RatingType = RatingType.Half;
        this.ratingView2.Path = PathConstants.Star;
        this.ratingView2.RatingType = RatingType.Half;
        this.ratingView2.PaintSurface += RatingView2_PaintSurface;
        
      
    }

    private void RatingView2_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void ratingView2_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        lop = ratingView2.Value;
        System.Diagnostics.Debug.WriteLine($"{lop}");

    }

    private void ratingView2_PaintSurface_1(object sender, SKPaintSurfaceEventArgs e)
    {
        this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
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




    //private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    //{
    //    this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
    //}

    public void Draw(SKCanvas canvas, int width, int height)
    {
        canvas.Clear(this.CanvasBackgroundColor);

        var path = SKPath.ParseSvgPathData(this.ratingView2.Path);

        //var itemWidth = ((width - (this.Count - 1) * this.Spacing)) / this.Count;
        //var scaleX = (itemWidth / (path.Bounds.Width));
        //scaleX = (itemWidth - scaleX * this.StrokeWidth) / path.Bounds.Width;

        //this.ItemHeight = height;
        //var scaleY = this.ItemHeight / (path.Bounds.Height);
        //scaleY = (this.ItemHeight - scaleY * this.StrokeWidth) / (path.Bounds.Height);

        //this.CanvasScale = Math.Min(scaleX, scaleY);
        //this.ItemWidth = path.Bounds.Width * this.CanvasScale;

        //canvas.Scale(this.CanvasScale);
        //canvas.Translate(this.StrokeWidth / 2, this.StrokeWidth / 2);
        //canvas.Translate(-path.Bounds.Left, 0);
        //canvas.Translate(0, -path.Bounds.Top);

        //using (var strokePaint = new SKPaint
        //{
        //    Style = SKPaintStyle.Stroke,
        //    Color = this.OutlineOffColor.ToSKColor(),
        //    StrokeWidth = this.StrokeWidth,
        //    StrokeCap = SKStrokeCap.Round,
        //    IsAntialias = true,
        //})
        //using (var fillPaint = new SKPaint
        //{
        //    Style = SKPaintStyle.Fill,
        //    Color = this.SKColorOn,
        //    IsAntialias = true,
        //})
        //{
        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        if (i <= this.Value - 1) // Full
        //        {
        //            canvas.DrawPath(path, fillPaint);
        //            canvas.DrawPath(path, strokePaint);
        //        }
        //        else if (i < this.Value) //Partial
        //        {
        //            float filledPercentage = (float)(this.Value - Math.Truncate(this.Value));
        //            strokePaint.Color = this.SKOutlineOffColor;
        //            canvas.DrawPath(path, strokePaint);

        //            using (var rectPath = new SKPath())
        //            {
        //                var rect = SKRect.Create(path.Bounds.Left + path.Bounds.Width * filledPercentage, path.Bounds.Top, path.Bounds.Width * (1 - filledPercentage), this.ItemHeight * 2);
        //                rectPath.AddRect(rect);
        //                canvas.ClipPath(rectPath, SKClipOperation.Difference);
        //                canvas.DrawPath(path, fillPaint);
        //            }
        //        }
        //        else //Empty
        //        {
        //            strokePaint.Color = this.SKOutlineOffColor;
        //            canvas.DrawPath(path, strokePaint);
        //        }

        //        canvas.Translate((this.ItemWidth + this.Spacing) / this.CanvasScale, 0);
        //    }
        //}

    }

}




