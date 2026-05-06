using Inventor;
using System.Drawing;

namespace InventorUITools
{
	/// <summary>
	/// Provides a fluent API base for building and configuring <see cref="ButtonDescriptor"/> instances.
	/// </summary>
	/// <typeparam name="TBuilder">The type of the concrete builder.</typeparam>
	public abstract class ButtonDescriptorBuilderBase<TBuilder> : ControlBuilderBase<TBuilder>
		where TBuilder : ButtonDescriptorBuilderBase<TBuilder>
	{
		/// <summary>
		/// The <see cref="ButtonDescriptor"/> being configured by this builder.
		/// </summary>
		protected readonly ButtonDescriptor _descriptor;
		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonDescriptorBuilderBase{TBuilder}"/> class.
		/// </summary>
		/// <param name="uiManager">The UI manager instance.</param>
		public ButtonDescriptorBuilderBase(UIManager uiManager) : base(uiManager)
		{
			_descriptor = _uiManager.CreateButtonDescriptor();
		}
		/// <summary>
		/// Sets the label (display name) for the button.
		/// </summary>
		/// <param name="label">The label to display on the button.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithLabel(string label)
		{
			_descriptor.DisplayName = label;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the tooltip text for the button.
		/// </summary>
		/// <param name="tooltip">The tooltip text.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithTooltip(string tooltip)
		{
			_descriptor.Tooltip = tooltip;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the description for the button.
		/// </summary>
		/// <param name="description">The description text.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithDescription(string description)
		{
			_descriptor.Description = description;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the icon image for the button.
		/// </summary>
		/// <param name="icon">The icon image.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithIcon(Image icon)
		{
			_descriptor.IconImage = icon;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the client ID for the button.
		/// </summary>
		/// <param name="clientId">The client ID string.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithClientId(string clientId)
		{
			_descriptor.ClientId = clientId;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the command type for the button.
		/// </summary>
		/// <param name="type">The command type.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithCommandType(CommandTypesEnum type)
		{
			_descriptor.IvCommandType = type;
			return this as TBuilder;
		}
		/// <summary>
		/// Registers an event handler for the button's execute event.
		/// </summary>
		/// <param name="handler">The event handler to invoke when the button is executed.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder OnExecute(ButtonDefinitionSink_OnExecuteEventHandler handler)
		{
			_descriptor.OnExecute += handler;
			return this as TBuilder;
		}
	}
}
