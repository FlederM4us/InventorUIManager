using System.Collections.Generic;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Defines a contract for fluent builders that support specifying ribbon placement for UI controls in Inventor.
	/// </summary>
	/// <typeparam name="TBuilder">The type of the concrete builder implementing this interface.</typeparam>
	internal interface IRibbonPlacing<TBuilder>
		where TBuilder : ControlBuilderBase<TBuilder>
	{
		/// <summary>
		/// Gets or sets the list of ribbon contexts (document types) where the control should be placed.
		/// </summary>
		List<RibbonName> RibbonContexts { get; set; }
		/// <summary>
		/// Gets or sets the name of the ribbon tab where the control should be placed.
		/// </summary>
		string RibbonTab { get; set; }
		/// <summary>
		/// Gets or sets the internal name of the target ribbon tab for insertion.
		/// </summary>
		string TargetRibbonTabInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon tab before the target tab.
		/// </summary>
		bool InsertBeforeTargetTabPanel { get; set; }
		/// <summary>
		/// Gets or sets the name of the ribbon panel where the control should be placed.
		/// </summary>
		string RibbonPanel { get; set; }
		/// <summary>
		/// Gets or sets the internal name of the target ribbon pannel for insertion.
		/// </summary>
		string TargetRibbonPanelInternalName { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether to insert this ribbon panel before the target panel.
		/// </summary>
		bool InsertBeforeTargetRibbonPanel { get; set; }
		/// <summary>
		/// Specifies the ribbon tab and panel to which the control should be added, and optionally the ribbon contexts.
		/// </summary>
		/// <param name="ribbons">The list of ribbon contexts (document types) to add the control to. If null, applies to all contexts.</param>
		/// <param name="tabName">The name of the ribbon tab. Default is "Automation".</param>
		/// <param name="panelName">The name of the ribbon panel. Default is "Default".</param>
		/// <returns>The builder instance for fluent chaining.</returns>
		TBuilder AddToRibbonTabPanel(List<RibbonName> ribbons = null, string tabName = "Automation", string panelName = "Default");
		/// <summary>
		/// Inserts a new ribbon tab before the specified internal ribbon tab name.
		/// </summary>
		/// <param name="internalTabName">The name of the internal ribbon tab before which the new ribbon tab will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>An instance of <typeparamref name="TBuilder"/> that allows further configuration.</returns>
		TBuilder InsertTabBefore(string internalTabName);
		/// <summary>
		/// Inserts a new ribbon tab after the specified internal ribbon tab name.
		/// </summary>
		/// <param name="internalTabName">The name of the internal ribbon tab after which the new ribbon tab will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>An instance of <typeparamref name="TBuilder"/> that allows further configuration.</returns>
		TBuilder InsertTabAfter(string internalTabName);
		/// <summary>
		/// Inserts a new ribbon panel before the specified internal ribbon panel name.
		/// </summary>
		/// <param name="internalPanelName">The name of the internal ribbon panel before which the new ribbon panel will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>An instance of <typeparamref name="TBuilder"/> that allows further configuration.</returns>
		TBuilder InsertPanelBefore(string internalPanelName);
		/// <summary>
		/// Inserts a new ribbon panel after the specified internal ribbon panel name.
		/// </summary>
		/// <param name="internalPanelName">The name of the internal ribbon panel after which the new ribbon panel will be inserted. Must not be <see langword="null"/>
		/// or empty.</param>
		/// <returns>An instance of <typeparamref name="TBuilder"/> that allows further configuration.</returns>
		TBuilder InsertPanelAfter(string internalPanelName);
	}
}
