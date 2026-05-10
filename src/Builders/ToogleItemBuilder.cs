using Inventor;
using System;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Provides a fluent API for building and configuring <see cref="ToogleItem"/> instances.
	/// </summary>
	public class ToogleItemBuilder : ButtonDescriptorBuilderBase<ToogleItemBuilder>
	{
		private readonly ToogleItem _toogleItem;
		/// <summary>
		/// Initializes a new instance of the <see cref="ToogleItemBuilder"/> class.
		/// </summary>
		/// <param name="uiManager">The UI manager instance used for control creation and management.</param>
		public ToogleItemBuilder(UIManager uiManager) : base(uiManager)
		{
			_toogleItem = uiManager.CreateToogleItem(_descriptor);
		}
		/// <summary>
		/// Registers an event handler for the <see cref="ToogleItem.CheckedChanged"/> event.
		/// </summary>
		/// <param name="eventHandler">The event handler to invoke when the checked state changes.</param>
		/// <returns>The builder instance for fluent chaining.</returns>
		public ToogleItemBuilder CheckedChanged(EventHandler<EventArgs> eventHandler)
		{
			_toogleItem.CheckedChanged += eventHandler;
			return this;
		}
		/// <summary>
		/// Gets the configured <see cref="ToogleItem"/> instance, initializing it before returning.
		/// </summary>
		/// <returns>The built <see cref="ToogleItem"/>.</returns>
		public ToogleItem GetToogleItem()
		{
			_toogleItem.Initialize();
			return _toogleItem;
		}
		/// <summary>
		/// Gets the underlying Inventor <see cref="ButtonDefinition"/> for the configured toggle item.
		/// </summary>
		/// <returns>The <see cref="ButtonDefinition"/> associated with the built <see cref="ToogleItem"/>.</returns>
		public ButtonDefinition GetButtonDefinition() => _descriptor.Definition;
	}
}