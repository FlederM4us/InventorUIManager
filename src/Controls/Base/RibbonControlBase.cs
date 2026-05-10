using Inventor;
using System;
using System.Linq;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Provides a base class for custom ribbon controls in Inventor, handling ribbon, tab, and panel creation and insertion.
	/// </summary>
	public abstract class RibbonControlBase : UIControlBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RibbonControlBase"/> class.
		/// </summary>
		/// <param name="ivApp">The Inventor application instance.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="ivApp"/> is null.</exception>
		protected RibbonControlBase(Application ivApp)
		{
			IvApplication = ivApp ?? throw new ArgumentNullException(nameof(IvApplication));
		}
		/// <summary>
		/// Gets the Inventor application instance associated with this ribbon control.
		/// </summary>
		public Application IvApplication { get; }
		/// <summary>
		/// Gets or sets the name of the ribbon where the control will be placed.
		/// </summary>
		public RibbonName RibbonName { get; set; }
		/// <summary>
		/// Gets or sets the display name of the ribbon tab.
		/// </summary>
		public string RibbonTabName { get; set; }
		/// <summary>
		/// Gets the internal name of the ribbon tab.
		/// </summary>
		public string RibbonTabInternalName => $"id_{RibbonTabName}Tab";
		/// <summary>
		/// Gets or sets the internal name of the target ribbon tab for insertion.
		/// </summary>
		public string TargetRibbonTabInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon tab before the target tab.
		/// </summary>
		public bool InsertBeforeTargetTabPanel { get; set; }
		/// <summary>
		/// Gets or sets the display name of the ribbon panel.
		/// </summary>
		public string RibbonPanelName { get; set; }
		/// <summary>
		/// Gets the internal name of the ribbon panel.
		/// </summary>
		public string RibbonPanelInternalName => $"id_{RibbonPanelName}Panel";
		/// <summary>
		/// Gets or sets the internal name of the target ribbon pannel for insertion.
		/// </summary>
		public string TargetRibbonPanelInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon panel before the target panel.
		/// </summary>
		public bool InsertBeforeTargetRibbonPanel { get; set; }
		/// <summary>
		/// Initializes the ribbon control, ensuring the ribbon tab and panel exist, and inserts the control into the panel.
		/// </summary>
		/// <returns>The initialized <see cref="CommandControl"/>.</returns>
		public override CommandControl Initialize()
		{
			var ribbon = IvApplication.UserInterfaceManager.Ribbons[RibbonName];
			var tab = EnsureTab(ribbon);
			var panel = EnsurePanel(tab);
			NativeControl = InsertIntoPanel(panel);
			return NativeControl;
		}
		/// <summary>
		/// Ensures the specified ribbon tab exists, creating it if necessary.
		/// </summary>
		/// <param name="ribbon">The ribbon to check or add the tab to.</param>
		/// <returns>The existing or newly created <see cref="RibbonTab"/>.</returns>
		protected RibbonTab EnsureTab(Ribbon ribbon) =>
			ribbon.RibbonTabs.OfType<RibbonTab>().FirstOrDefault(t => t.InternalName == RibbonTabInternalName) ??
			ribbon.RibbonTabs.Add(RibbonTabName, RibbonTabInternalName, ClientId, TargetRibbonTabInternalName ?? "", InsertBeforeTargetTabPanel);
		/// <summary>
		/// Ensures the specified ribbon panel exists within the given tab, creating it if necessary.
		/// </summary>
		/// <param name="tab">The ribbon tab to check or add the panel to.</param>
		/// <returns>The existing or newly created <see cref="RibbonPanel"/>.</returns>
		protected RibbonPanel EnsurePanel(RibbonTab tab) =>
			tab.RibbonPanels.OfType<RibbonPanel>().FirstOrDefault(p => p.InternalName == RibbonPanelInternalName) ??
			tab.RibbonPanels.Add(RibbonPanelName, RibbonPanelInternalName, ClientId, TargetRibbonPanelInternalName ?? "", InsertBeforeTargetRibbonPanel);
		/// <summary>
		/// Inserts the control into the specified ribbon panel.
		/// </summary>
		/// <param name="panel">The ribbon panel to insert the control into.</param>
		/// <returns>The created <see cref="CommandControl"/>.</returns>
		protected abstract CommandControl InsertIntoPanel(RibbonPanel panel);
	}

	/// <summary>
	/// Specifies the available ribbons in Inventor for UI controls.
	/// </summary>
	public enum RibbonName
	{
		/// <summary>
		/// The ribbon shown when no documents are open.
		/// </summary>
		ZeroDoc = 1,
		/// <summary>
		/// The ribbon for part documents.
		/// </summary>
		Part,
		/// <summary>
		/// The ribbon for assembly documents.
		/// </summary>
		Assembly,
		/// <summary>
		/// The ribbon for drawing documents.
		/// </summary>
		Drawing,
		/// <summary>
		/// The ribbon for presentation documents.
		/// </summary>
		Presentation,
		/// <summary>
		/// The ribbon for iFeatures documents.
		/// </summary>
		iFeatures,
		/// <summary>
		/// The ribbon for unknown document types.
		/// </summary>
		UnknownDocument
	}
}
