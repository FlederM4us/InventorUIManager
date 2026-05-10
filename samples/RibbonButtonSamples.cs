using InventorUITools;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InventorUIToolsSamples
{
	internal static class RibbonButtonSamples
	{
		private static readonly List<RibbonName> RIBBONS = [RibbonName.ZeroDoc, RibbonName.Part, RibbonName.Assembly, RibbonName.Drawing];
		private const string RIBBON_TAB = "UI Tools Samples";
		private const string RIBBON_PANEL = "Ribbon Buttons";
		public static void UseBuilderSample(UIManager uiManager)
		{
			uiManager.NewRibbonButton()
				.WithLabel("Builder Button")
				.WithTooltip("This button was created with the builder (fluent) pattern!")
				.OnExecute((context) => MessageBox.Show("Fluent Button Example"))
				.AddToRibbonTabPanel(RIBBONS, RIBBON_TAB, RIBBON_PANEL)
				.Initialize();
		}

	}
}
