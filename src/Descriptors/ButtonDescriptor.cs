using Inventor;
using System;
using System.Diagnostics;
using System.Linq;

namespace InventorUITools
{
	/// <summary>
	/// Represents a descriptor (ButtonDefinition) for an Inventor button control, providing access to its definition and events.
	/// </summary>
	public class ButtonDescriptor : ControlDescriptorBase
	{
		private ButtonDefinition _definition;

		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonDescriptor"/> class.
		/// </summary>
		/// <param name="ivApplication">The Inventor application instance.</param>
		internal ButtonDescriptor(Inventor.Application ivApplication) : base(ivApplication) { }

		/// <summary>
		/// Gets or sets a value indicating whether the button is enabled.
		/// </summary>
		public override bool Enabled
		{
			get => Definition.Enabled;
			set => Definition.Enabled = value;
		}
		/// <summary>
		/// Gets or sets a value indicating whether the button is pressed.
		/// </summary>
		public virtual bool Pressed
		{
			get => Definition.Pressed;
			set => Definition.Pressed = value;
		}
		/// <summary>
		/// Gets the underlying Inventor <see cref="ButtonDefinition"/> for this button.
		/// </summary>
		public ButtonDefinition Definition
		{
			get
			{
				if (_definition != null)
					return _definition;

				_definition = EnsureButtonDefinition(IvApplication.CommandManager.ControlDefinitions);
				_definition.Enabled = true;

				return _definition;
			}
		}
		/// <summary>
		/// Occurs when the button is executed (clicked).
		/// </summary>
		public event ButtonDefinitionSink_OnExecuteEventHandler OnExecute
		{
			add => Definition.OnExecute += value;
			remove => Definition.OnExecute -= value;
		}
		/// <summary>
		/// Releases resources used by the <see cref="ButtonDescriptor"/> and deletes the button definition.
		/// </summary>
		public override void Dispose()
		{
			try
			{
				_definition?.Delete();
			}
			catch (Exception ex)
			{
				Debugger.Break();
				Debug.WriteLine($"Error disposing button definition: {ex.Message}");
			}
			finally
			{
				_definition = null;
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
		/// /// <summary>
		/// Ensures that the button definition exists, creating it if necessary.
		/// </summary>
		/// <param name="controlDefinitions">The controlDefinitions to check or add the tab to.</param>
		/// <returns>The existing or newly created <see cref="ButtonDefinition"/>.</returns>
		private ButtonDefinition EnsureButtonDefinition(ControlDefinitions controlDefinitions) =>
			controlDefinitions.OfType<ButtonDefinition>().FirstOrDefault(b => b.InternalName == InternalName) ??
			controlDefinitions.AddButtonDefinition(DisplayName, InternalName, IvCommandType, ClientId, Description, Tooltip, SmallIcon, LargeIcon);
	}
}
