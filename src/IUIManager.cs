namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Provides access to the <see cref="UIManager"/> used by the application.
	/// </summary>
	public interface IUIManager
	{
		/// <summary>
		/// Gets the <see cref="UIManager"/> instance.
		/// </summary>
		public UIManager UIManager { get; }
	}
}
