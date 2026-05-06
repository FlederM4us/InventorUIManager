using Inventor;

namespace InventorUITools
{
	/// <summary>
	/// Provides a fluent API for building and configuring <see cref="ButtonDescriptor"/> instances.
	/// </summary>
	public class ButtonDescriptorBuilder : ButtonDescriptorBuilderBase<ButtonDescriptorBuilder>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonDescriptorBuilder"/> class.
		/// </summary>
		/// <param name="uiManager">The UI manager instance used for control creation and management.</param>
		public ButtonDescriptorBuilder(UIManager uiManager) : base(uiManager) { }
		/// <summary>
		/// Gets the configured <see cref="ButtonDescriptor"/> instance.
		/// </summary>
		/// <returns>The built <see cref="ButtonDescriptor"/>.</returns>
		public ButtonDescriptor GetButtonDescriptor() => _descriptor;
		/// <summary>
		/// Gets the underlying Inventor <see cref="ButtonDefinition"/> for the configured button.
		/// </summary>
		/// <returns>The <see cref="ButtonDefinition"/> associated with the built <see cref="ButtonDescriptor"/>.</returns>
		public ButtonDefinition GetButtonDefinition() => _descriptor.Definition;
	}
}
