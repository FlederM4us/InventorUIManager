using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace InventorUITools
{
	public class UIManager
	{
		private readonly string _clientId;

		/// <summary>
		/// Initializes a new instance of the <see cref="UIManager"/> class.
		/// </summary>
		/// <param name="ivApplication">The Inventor application instance.</param>
		/// <param name="clientId">The unique identifier for registering UI elements.</param>
		public UIManager(Inventor.Application ivApplication, string clientId)
		{
			IvApplication = ivApplication;
			_clientId = clientId;
			SetContext();
		}

		/// <summary>
		/// Gets the Inventor application instance.
		/// </summary>
		public Inventor.Application IvApplication { get; }
		/// <summary>
		/// Gets the synchronization context for marshaling UI operations.
		/// </summary>
		public SynchronizationContext Context { get; private set; }
		/// <summary>
		/// Gets the list of UI controls managed by this instance.
		/// </summary>
		public List<UIControlBase> UIControls { get; } = [];
		/// <summary>
		/// Initializes all registered UI controls.
		/// </summary>
		public void Initialize() => UIControls.ForEach(control => control.Initialize());

		#region Builders
		/// <summary>
		/// Creates a new <see cref="ButtonDescriptorBuilder"/> instance.
		/// </summary>
		public ButtonDescriptorBuilder NewButtonDescriptor() => new(this);
		/// <summary>
		/// Creates a new <see cref="RibbonButtonBuilder"/> instance.
		/// </summary>
		public RibbonButtonBuilder NewRibbonButton() => new(this);
		/// <summary>
		/// Creates a new <see cref="RibbonTooglePopupBuilder"/> instance.
		/// </summary>
		public RibbonTooglePopupBuilder NewRibbonTooglePopup() => new(this);
		/// <summary>
		/// Creates a new <see cref="ToogleItemBuilder"/> instance.
		/// </summary>
		public ToogleItemBuilder NewToogleItem() => new(this);
		#endregion

		#region Create Methods
		/// <summary>
		/// Creates a new <see cref="ButtonDescriptor"/> configured with the application and client ID.
		/// </summary>
		public ButtonDescriptor CreateButtonDescriptor() => new(IvApplication) { ClientId = _clientId };
		/// <summary>
		/// Creates a toggle item based on the given button descriptor.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor associated with the toggle item.</param>
		public ToogleItem CreateToogleItem(ButtonDescriptor buttonDescriptor) => new(buttonDescriptor);
		/// <summary>
		/// Creates a ribbon button based on the specified button descriptor and registers it for initialization.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor defining button properties.</param>
		public RibbonButton CreateRibbonButton(ButtonDescriptor buttonDescriptor)
		{
			ArgumentNullException.ThrowIfNull(buttonDescriptor);
			var ribbonButton = new RibbonButton(buttonDescriptor);
			UIControls.Add(ribbonButton);
			return ribbonButton;
		}
		/// <summary>
		/// Creates a ribbon toggle popup with the provided descriptor and collection of buttons.
		/// Registers the control for later initialization.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor for the popup control.</param>
		/// <param name="toogleItems">A collection of buttons to appear in the popup.</param>
		public RibbonTooglePopup CreateRibbonTooglePopup(ButtonDescriptor buttonDescriptor, List<ToogleItem> toogleItems)
		{
			ArgumentNullException.ThrowIfNull(buttonDescriptor);
			ArgumentNullException.ThrowIfNull(toogleItems);
			var ribbonButton = new RibbonTooglePopup(buttonDescriptor, toogleItems);
			UIControls.Add(ribbonButton);
			return ribbonButton;
		}
		#endregion

		private void SetContext()
		{
			var dummyControl = new Control();
			SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
			Context = SynchronizationContext.Current;
		}
	}
}