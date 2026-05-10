using Inventor;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorUITools
{
	/// <summary>
	/// Base class for UI control descriptors (ControlDefinition) in Inventor add-ins.
	/// Provides common properties and functionality for UI controls.
	/// </summary>
	public abstract class ControlDescriptorBase : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ControlDescriptorBase"/> class.
		/// </summary>
		/// <param name="ivApplication">The Inventor application instance.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="ivApplication"/> is null.</exception>
		public ControlDescriptorBase(Inventor.Application ivApplication)
		{
			IvApplication = ivApplication ?? throw new ArgumentNullException(nameof(ivApplication));
		}
		/// <summary>
		/// Gets the Inventor application instance associated with this control.
		/// </summary>
		public Inventor.Application IvApplication { get; }
		/// <summary>
		/// Gets or sets the display name of the control.
		/// </summary>
		public virtual string DisplayName { get; set; }
		/// <summary>
		/// Gets the internal name used for the control definition.
		/// </summary>
		public virtual string InternalName => $"id_{DisplayName.Replace(" ", "")}Definion";
		/// <summary>
		/// Gets or sets the command type for the control.
		/// </summary>
		public virtual CommandTypesEnum IvCommandType { get; set; } = CommandTypesEnum.kQueryOnlyCmdType;
		/// <summary>
		/// Gets or sets the client ID for the control.
		/// </summary>
		public virtual string ClientId { get; set; }
		/// <summary>
		/// Gets or sets the description of the control.
		/// </summary>
		public virtual string Description { get; set; }
		/// <summary>
		/// Gets or sets the tooltip text for the control.
		/// </summary>
		public virtual string Tooltip { get; set; }
		/// <summary>
		/// Gets or sets the icon image for the control.
		/// </summary>
		public virtual Image IconImage { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the control is enabled.
		/// </summary>
		public abstract bool Enabled { get; set; }
		/// <summary>
		/// Gets the small icon as an <see cref="IPictureDisp"/> for use in Inventor UI.
		/// </summary>
		protected virtual IPictureDisp SmallIcon => ImageConverter.ImageToIPictureDisp(ImageConverter.ResizeImage(IconImage));
		/// <summary>
		/// Gets the large icon as an <see cref="IPictureDisp"/> for use in Inventor UI.
		/// </summary>
		protected virtual IPictureDisp LargeIcon => ImageConverter.ImageToIPictureDisp(IconImage);
		/// <summary>
		/// Releases resources used by the control descriptor.
		/// </summary>
		public virtual void Dispose() => GC.SuppressFinalize(this);
		/// <summary>
		/// Provides image conversion utilities for Inventor UI controls.
		/// </summary>
		public class ImageConverter : AxHost
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ImageConverter"/> class.
			/// </summary>
			public ImageConverter() : base(string.Empty) { }

			/// <summary>
			/// Converts a <see cref="Image"/> to an <see cref="IPictureDisp"/> for use in COM interop.
			/// </summary>
			/// <param name="image">The image to convert.</param>
			/// <returns>An <see cref="IPictureDisp"/> representation of the image, or null if the image is null.</returns>
			public static IPictureDisp ImageToIPictureDisp(Image image)
			{
				return image is null ? null : (IPictureDisp)GetIPictureDispFromPicture(image);
			}

			/// <summary>
			/// Resizes an image to the specified width and height.
			/// </summary>
			/// <param name="original">The original image to resize.</param>
			/// <param name="width">The target width. Default is 16.</param>
			/// <param name="height">The target height. Default is 16.</param>
			/// <returns>The resized image, or null if the original is null.</returns>
			public static Image ResizeImage(Image original, int width = 16, int height = 16)
			{
				if (original is null)
					return null;

				var resized = new Bitmap(width, height);
				using var g = Graphics.FromImage(resized);
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(original, 0, 0, width, height);
				return resized;
			}
		}
	}
}
