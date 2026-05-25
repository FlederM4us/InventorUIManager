using Inventor;
using System;
using System.Diagnostics;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Represents a descriptor (ComboBoxDefinition) for an Inventor combo box control, providing access to its definition and events.
	/// </summary>
	public class ComboBoxDescriptor : ControlDescriptorBase
	{
		private ComboBoxDefinition _definition;
		internal ComboBoxDescriptor(Inventor.Application ivApplication) : base(ivApplication) { }
		/// <summary>
		/// Gets or sets a drop down width.
		/// </summary>
		public int DropDownWidth { get; set; } = 200;
		/// <summary>
		/// Gets or sets a value indicating whether the combo box is enabled.
		/// </summary>
		public override bool Enabled
		{
			get => Definition.Enabled;
			set => Definition.Enabled = value;
		}
		/// <summary>
		/// Gets the underlying Inventor <see cref="ComboBoxDefinition"/> for this combo box.
		/// </summary>
		public ComboBoxDefinition Definition
		{
			get
			{
				if (_definition != null)
					return _definition;

				_definition = IvApplication.CommandManager.ControlDefinitions.AddComboBoxDefinition(
					DisplayName, InternalName, IvCommandType, DropDownWidth, ClientId, Description, Tooltip, SmallIcon, LargeIcon);

				_definition.Enabled = true;

				return _definition;
			}
		}
		/// <summary>
		/// Releases resources used by the <see cref="ComboBoxDescriptor"/> and deletes the combo box definition.
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
				Debug.WriteLine($"Error disposing combo box definition: {ex.Message}");
			}
			finally
			{
				_definition = null;
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
	}
}
