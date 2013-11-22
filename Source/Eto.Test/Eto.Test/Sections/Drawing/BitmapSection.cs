using Eto.Forms;
using Eto.Drawing;
using ImageView = Eto.Test.Sections.Drawing.DrawableImageView;

namespace Eto.Test.Sections.Drawing
{
	/// <summary>
	/// We use this class instead of ImageView to
	/// hook into the rendering and supply a different
	/// drawing toolkit.
	/// </summary>
	public class DrawableImageView : Drawable
	{
		private Image image;
		public Image Image
		{
			get { return image; }
			set
			{
				image = value;
				this.MinimumSize = image.Size;
			}
		}

		public DrawableImageView()
		{
			this.Paint += (s, e) => {
				if (this.Image != null)
					e.Graphics.DrawImage(this.Image, PointF.Empty);
			};
		}
	}

	public class BitmapSection : Scrollable
	{
		public BitmapSection()
		{
			var layout = new DynamicLayout();

			layout.AddRow(new Label { Text = "Load from Stream" }, LoadFromStream());

			layout.AddRow(
				new Label { Text = "Custom 32-bit" }, CreateCustom32(),
				new Label { Text = "Custom 32-bit alpha" }, CreateCustom32Alpha(),
				null
			);

			layout.AddRow(
				new Label { Text = "Clone" }, Cloning(),
				new Label { Text = "Clone rectangle" }, TableLayout.AutoSized(CloningRectangle(), centered: true),
				null);

			layout.Add(null);

			Content = layout;
		}

		Control LoadFromStream()
		{
			var resourceStream = GetType().Assembly.GetManifestResourceStream("Eto.Test.TestImage.png");

			var image = new Bitmap(resourceStream);

			return new DrawableImageView { Image = image };
		}

		Control CreateCustom32()
		{
			var image = new Bitmap(100, 100, PixelFormat.Format32bppRgb);

			// should always ensure .Dispose() is called when you are done with a Graphics object
			using (var graphics = new Graphics (image))
			{
				graphics.DrawLine(Pens.Blue(), Point.Empty, new Point(image.Size));
				graphics.DrawRectangle(Pens.Blue(), new Rectangle(image.Size - 1));
			}

			return new DrawableImageView { Image = image };
		}

		Control CreateCustom32Alpha()
		{
			var image = new Bitmap(100, 100, PixelFormat.Format32bppRgba);

			// should always ensure .Dispose() is called when you are done with a Graphics object
			using (var graphics = new Graphics (image))
			{
				graphics.DrawLine(Pens.Blue(), Point.Empty, new Point(image.Size));
				graphics.DrawRectangle(Pens.Black(), new Rectangle(image.Size - 1));
			}

			return new DrawableImageView { Image = image };
		}

		Control Cloning()
		{
			var image = TestIcons.TestImage;
			image = image.Clone();
			return new DrawableImageView { Image = image };
		}

		Control CloningRectangle()
		{
			var image = TestIcons.TestImage;
			image = image.Clone(new Rectangle(32, 32, 64, 64));
			return new ImageView { Image = image };
		}
	}
}
