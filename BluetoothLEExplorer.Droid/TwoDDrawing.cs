using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;

namespace BluetoothLEExplorer.Droid
{
	class TwoDDrawing : View
	{
		private readonly ShapeDrawable _shape;

		public TwoDDrawing(Context context) : base(context)
		{
			var paint = new Paint();
			paint.SetARGB(255, 200, 255, 0);
			paint.SetStyle(Paint.Style.Stroke);
			paint.StrokeWidth = 4;

			_shape = new ShapeDrawable(new OvalShape());
			_shape.Paint.Set(paint);

			_shape.SetBounds (20, 20, 300, 200);
		}

		protected override void OnDraw(Canvas canvas)
		{
			_shape.Draw(canvas);
		}
	}
}

