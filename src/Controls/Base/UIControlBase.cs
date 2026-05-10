using Inventor;
using System;
using System.Diagnostics;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Provides a base class for custom Inventor UI controls, managing their lifecycle and properties.
	/// </summary>
	public abstract class UIControlBase : IDisposable
	{
		/// <summary>
		/// Gets or sets the native Inventor <see cref="CommandControl"/> associated with this UI control.
		/// </summary>
		public CommandControl NativeControl { get; set; }
		/// <summary>
		/// Gets the client ID associated with this control.
		/// </summary>
		public abstract string ClientId { get; }
		/// <summary>
		/// Gets or sets a value indicating whether to use a large icon for the control.
		/// </summary>
		public bool UseLargeIcon { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to show text on the control.
		/// </summary>
		public bool ShowText { get; set; }
		/// <summary>
		/// Gets or sets the internal name of the target control for insertion.
		/// </summary>
		internal string TargetControlInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this control before the target control.
		/// </summary>
		internal bool InsertBeforeTargetControl { get; set; }
		/// <summary>
		/// Initializes the control and returns the associated <see cref="CommandControl"/>.
		/// </summary>
		/// <returns>The initialized <see cref="CommandControl"/>.</returns>
		public abstract CommandControl Initialize();
		/// <summary>
		/// Releases resources used by the control and deletes the native Inventor control if present.
		/// </summary>
		public virtual void Dispose()
		{
			try
			{
				NativeControl?.Delete();
			}
			catch (Exception ex)
			{
				Debugger.Break();
				Debug.WriteLine($"Error disposing control: {ex.Message}");
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}
	}
}
