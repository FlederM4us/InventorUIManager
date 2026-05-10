using Inventor;
using System;
using System.Linq;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Represents a ribbon button control in Inventor, wrapping a <see cref="ButtonDescriptor"/> and managing its lifecycle and events.
	/// </summary>
	public class RibbonButton : RibbonControlBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RibbonButton"/> class.
		/// </summary>
		/// <param name="buttonDescriptor">The <see cref="ButtonDescriptor"/> to associate with this ribbon button.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="buttonDescriptor"/> is null.</exception>
		internal RibbonButton(ButtonDescriptor buttonDescriptor) : base(buttonDescriptor?.IvApplication)
		{
			ButtonDescriptor = buttonDescriptor ?? throw new ArgumentNullException(nameof(buttonDescriptor));
		}
		/// <summary>
		/// Gets or sets the <see cref="ButtonDescriptor"/> associated with this ribbon button.
		/// </summary>
		public ButtonDescriptor ButtonDescriptor { get; set; }
		/// <summary>
		/// Gets the client ID for this ribbon button.
		/// </summary>
		public override string ClientId => ButtonDescriptor.ClientId;
		/// <summary>
		/// Gets or sets a value indicating whether the button is pressed.
		/// </summary>
		public bool Pressed
		{
			get => ButtonDescriptor.Definition.Pressed;
			set => ButtonDescriptor.Definition.Pressed = value;
		}
		/// <summary>
		/// Occurs when the button is executed (clicked).
		/// </summary>
		public event ButtonDefinitionSink_OnExecuteEventHandler OnExecute
		{
			add => ButtonDescriptor.OnExecute += value;
			remove => ButtonDescriptor.OnExecute -= value;
		}
		/// <summary>
		/// Releases resources used by the <see cref="RibbonButton"/> and disposes the associated <see cref="ButtonDescriptor"/>.
		/// </summary>
		public override void Dispose()
		{
			try
			{
				ButtonDescriptor?.Dispose();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debugger.Break();
				System.Diagnostics.Debug.WriteLine($"Error disposing RibbonButton: {ex.Message}");
			}
			finally
			{
				ButtonDescriptor = null;
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
		/// <summary>
		/// Inserts the button into the specified ribbon panel, creating it if it does not already exist.
		/// </summary>
		/// <param name="panel">The ribbon panel to insert the button into.</param>
		/// <returns>The created or existing <see cref="CommandControl"/> for the button.</returns>
		protected override CommandControl InsertIntoPanel(RibbonPanel panel)
		{
			var controls = panel.CommandControls;
			var control = controls
				.OfType<CommandControl>()
				.FirstOrDefault(ctrl => ctrl.InternalName == ButtonDescriptor.InternalName);

			return control ?? controls.AddButton(ButtonDescriptor.Definition, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
		}
	}
}