using System;

namespace InventorUITools
{
	/// <summary>
	/// Represents a toggleable item that wraps a <see cref="ButtonDescriptor"/> and provides checked/unchecked state management.
	/// </summary>
	public class ToogleItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToogleItem"/> class.
		/// </summary>
		/// <param name="buttonDescriptor">The <see cref="ButtonDescriptor"/> to associate with this toggle item.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="buttonDescriptor"/> is null.</exception>
		internal ToogleItem(ButtonDescriptor buttonDescriptor)
		{
			ButtonDescriptor = buttonDescriptor ?? throw new ArgumentNullException(nameof(buttonDescriptor));
		}

		/// <summary>
		/// Gets or sets the <see cref="ButtonDescriptor"/> associated with this toggle item.
		/// </summary>
		public ButtonDescriptor ButtonDescriptor { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the toggle item should automatically update its checked state when executed.
		/// </summary>
		public bool AutoCheck { get; set; } = true;
		/// <summary>
		/// Gets or sets a value indicating whether the toggle item is enabled.
		/// </summary>
		public bool Enabled
		{
			get => ButtonDescriptor.Enabled;
			set => ButtonDescriptor.Enabled = value;
		}
		/// <summary>
		/// Gets or sets a value indicating whether the toggle item is checked.
		/// Setting this property raises the <see cref="CheckedChanged"/> event.
		/// </summary>
		public bool Checked
		{
			get => ButtonDescriptor.Pressed;
			set
			{
				ButtonDescriptor.Pressed = value;
				CheckedChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		/// <summary>
		/// Initializes the toggle item, wiring up the execution event to handle automatic checking if <see cref="AutoCheck"/> is enabled.
		/// </summary>
		public void Initialize()
		{
			ButtonDescriptor.OnExecute += c =>
			{
				if (AutoCheck)
					Checked ^= true;
			};
		}
		/// <summary>
		/// Occurs when the <see cref="Checked"/> property value changes.
		/// </summary>
		public event EventHandler<EventArgs> CheckedChanged;
	}
}