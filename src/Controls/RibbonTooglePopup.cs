using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Represents a ribbon toggle popup control in Inventor, allowing a group of buttons to be displayed in a popup menu.
	/// </summary>
	public class RibbonTooglePopup : RibbonButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RibbonTooglePopup"/> class.
		/// </summary>
		/// <param name="buttonDescriptor">The <see cref="ButtonDescriptor"/> associated with this toggle popup.</param>
		/// <param name="toogleItems">The <see cref="ObjectCollection"/> of buttons to display in the popup.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="toogleItems"/> is null.</exception>
		internal RibbonTooglePopup(ButtonDescriptor buttonDescriptor, List<ToogleItem> toogleItems) : base(buttonDescriptor)
		{
			ToogleItems = toogleItems ?? throw new ArgumentNullException(nameof(toogleItems));

			NativeToogleItems = IvApplication.TransientObjects.CreateObjectCollection();
			foreach (var toogleItem in toogleItems)
			{
				if (toogleItem is null)
					continue;
				NativeToogleItems.Add(toogleItem.ButtonDescriptor.Definition);
			}
		}

		/// <summary>
		/// Gets or sets the collection of <see cref="ToogleItem"/> to display in the toggle popup.
		/// </summary>
		public List<ToogleItem> ToogleItems { get; set; }

		/// <summary>
		/// Gets the collection of native objects to display in the toggle popup.
		/// </summary>
		public ObjectCollection NativeToogleItems { get; }
		/// <summary>
		/// Inserts the toggle popup into the specified ribbon panel, creating it if it does not already exist.
		/// </summary>
		/// <param name="panel">The ribbon panel to insert the toggle popup into.</param>
		/// <returns>The created or existing <see cref="CommandControl"/> for the toggle popup.</returns>
		protected override CommandControl InsertIntoPanel(RibbonPanel panel)
		{
			var controls = panel.CommandControls;
			var control = controls
				.OfType<CommandControl>()
				.FirstOrDefault(ctrl => ctrl.InternalName == ButtonDescriptor.InternalName);

			return control ?? controls.AddTogglePopup(ButtonDescriptor.Definition, NativeToogleItems, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
		}
	}
}