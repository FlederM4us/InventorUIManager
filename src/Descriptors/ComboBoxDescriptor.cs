using Inventor;
using System;
using System.Diagnostics;

namespace InventorUITools
{
	public class ComboBoxDescriptor : ControlDescriptorBase
	{
		private ComboBoxDefinition _definition;
		internal ComboBoxDescriptor(Inventor.Application ivApplication) : base(ivApplication) { }
		public int DropDownWidth { get; set; } = 200;
		public override bool Enabled
		{
			get => Definition.Enabled;
			set => Definition.Enabled = value;
		}
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
		public override void Dispose()
		{
			try
			{
				_definition?.Delete();
			}
			catch (Exception ex)
			{
				Debugger.Break();
				Debug.WriteLine($"Error disposing combobox definition: {ex.Message}");
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
