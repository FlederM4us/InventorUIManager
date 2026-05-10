using System.Collections.Generic;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Provides a fluent API for building and configuring <see cref="RibbonButton"/> controls and placing them on specific ribbons, tabs, and panels in Inventor.
	/// </summary>
	public class RibbonButtonBuilder : ButtonDescriptorBuilderBase<RibbonButtonBuilder>, IRibbonPlacing<RibbonButtonBuilder>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RibbonButtonBuilder"/> class.
		/// </summary>
		/// <param name="uiManager">The UI manager instance used for control creation and management.</param>
		public RibbonButtonBuilder(UIManager uiManager) : base(uiManager) { }
		/// <summary>
		/// Gets or sets the list of ribbon contexts (document types) where the button should be placed.
		/// </summary>
		public List<RibbonName> RibbonContexts { get; set; }
		/// <summary>
		/// Gets or sets the name of the ribbon tab where the button should be placed.
		/// </summary>
		public string RibbonTab { get; set; }
		/// <summary>
		/// Gets or sets the internal name of the target ribbon tab for insertion.
		/// </summary>
		public string TargetRibbonTabInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon tab before the target tab.
		/// </summary>
		public bool InsertBeforeTargetTabPanel { get; set; }
		/// <summary>
		/// Gets or sets the name of the ribbon panel where the button should be placed.
		/// </summary>
		public string RibbonPanel { get; set; }
		/// <summary>
		/// Gets or sets the internal name of the target ribbon pannel for insertion.
		/// </summary>
		public string TargetRibbonPanelInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon panel before the target panel.
		/// </summary>
		public bool InsertBeforeTargetRibbonPanel { get; set; }
		/// <summary>
		/// Specifies the ribbon tab and panel to which the button should be added, and optionally the ribbon contexts.
		/// </summary>
		/// <param name="ribbons">The list of ribbon contexts (document types) to add the button to. If null, applies to all contexts.</param>
		/// <param name="tabName">The name of the ribbon tab. Default is "Automation".</param>
		/// <param name="panelName">The name of the ribbon panel. Default is "Default".</param>
		/// <returns>The <see cref="RibbonButtonBuilder"/> instance for fluent chaining.</returns>
		public RibbonButtonBuilder AddToRibbonTabPanel(List<RibbonName> ribbons = null, string tabName = "Automation", string panelName = "Default")
		{
			RibbonContexts = ribbons ?? RibbonContexts;
			RibbonTab = tabName;
			RibbonPanel = panelName;
			return this;
		}
		/// <summary>
		/// Inserts a new ribbon tab before the specified internal ribbon tab name.
		/// </summary>
		/// <param name="internalTabName">The name of the internal ribbon tab before which the new ribbon tab will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>The <see cref="RibbonButtonBuilder"/> instance for fluent chaining.</returns>
		public RibbonButtonBuilder InsertTabBefore(string internalTabName)
		{
			TargetRibbonTabInternalName = internalTabName;
			InsertBeforeTargetRibbonPanel = true;
			return this;
		}
		/// <summary>
		/// Inserts a new ribbon tab after the specified internal ribbon tab name.
		/// </summary>
		/// <param name="internalTabName">The name of the internal ribbon tab after which the new ribbon tab will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>The <see cref="RibbonButtonBuilder"/> instance for fluent chaining.</returns>
		public RibbonButtonBuilder InsertTabAfter(string internalTabName)
		{
			TargetRibbonTabInternalName = internalTabName;
			InsertBeforeTargetRibbonPanel = false;
			return this;
		}
		/// <summary>
		/// Inserts a new ribbon panel before the specified internal ribbon panel name.
		/// </summary>
		/// <param name="internalPanelName">The name of the internal ribbon panel before which the new ribbon panel will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>The <see cref="RibbonButtonBuilder"/> instance for fluent chaining.</returns>
		public RibbonButtonBuilder InsertPanelBefore(string internalPanelName)
		{
			TargetRibbonPanelInternalName = internalPanelName;
			InsertBeforeTargetRibbonPanel = true;
			return this;
		}
		/// <summary>
		/// Inserts a new ribbon panel after the specified internal ribbon panel name.
		/// </summary>
		/// <param name="internalPanelName">The name of the internal ribbon panel after which the new ribbon panel will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>The <see cref="RibbonButtonBuilder"/> instance for fluent chaining.</returns>
		public RibbonButtonBuilder InsertPanelAfter(string internalPanelName)
		{
			TargetRibbonPanelInternalName = internalPanelName;
			InsertBeforeTargetRibbonPanel = false;
			return this;
		}
		/// <summary>
		/// Builds and returns a dictionary of <see cref="RibbonButton"/> instances for each specified ribbon context.
		/// </summary>
		/// <returns>A dictionary of configured <see cref="RibbonButton"/> controls.</returns>
		public Dictionary<RibbonName, RibbonButton> GetRibonButons()
		{
			Dictionary<RibbonName, RibbonButton> ribbonButtons = [];
			foreach (var ribbonName in RibbonContexts)
			{
				var ribbonButton = _uiManager.CreateRibbonButton(_descriptor);
				ribbonButton.RibbonName = ribbonName;
				ribbonButton.RibbonTabName = RibbonTab;
				ribbonButton.TargetRibbonTabInternalName = TargetRibbonTabInternalName;
				ribbonButton.InsertBeforeTargetTabPanel = InsertBeforeTargetTabPanel;
				ribbonButton.RibbonPanelName = RibbonPanel;
				ribbonButton.TargetRibbonPanelInternalName = TargetRibbonPanelInternalName;
				ribbonButton.InsertBeforeTargetRibbonPanel = InsertBeforeTargetRibbonPanel;
				ribbonButton.UseLargeIcon = _useLargeIcon;
				ribbonButton.ShowText = _showText;
				ribbonButtons.Add(ribbonName, ribbonButton);
			}
			return ribbonButtons;
		}
		/// <summary>
		/// Initializes all created <see cref="RibbonButton"/> controls by adding them to the Inventor UI.
		/// </summary>
		public void Initialize()
		{
			var ribbonButtons = GetRibonButons();
			//ribbonButtons.ForEach(button => button.Initialize());
			foreach (var kvp in ribbonButtons)
				kvp.Value.Initialize();
		}
	}
}