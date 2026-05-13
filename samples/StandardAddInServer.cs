using FlederM4us.InventorUI.Manager;
using System;
using System.Runtime.InteropServices;

namespace InventorUIManager.Samples
{
	[Guid("cb5cebd9-4b11-4e9f-b47a-132ca2cf4926")]
	public class StandardAddInServer : Inventor.ApplicationAddInServer, IUIManager
	{
		private Inventor.Application _ivApplication;
		private UIManager _uiManager;

		public StandardAddInServer() { }

		#region ApplicationAddInServer Members

		public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
		{
			_ivApplication = addInSiteObject.Application;
			_uiManager = new UIManager(_ivApplication, addInSiteObject.Parent.ClientId);

			CreateSamples();
		}

		public void Deactivate()
		{
			_ivApplication = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void ExecuteCommand(int commandID)
		{
			// Note:this method is now obsolete, you should use the 
			// ControlDefinition functionality for implementing commands.
		}

		public object Automation => null;

		public UIManager UIManager => _uiManager;

		#endregion

		private void CreateSamples()
		{
			UIManager.NewRibbonButton()
				.WithLabel("Update Button")
				.WithLabel("Update all samples")
				.WithIcon(Properties.Resources.Update)
				.OnExecute(UpdateSamples)
				.AddToRibbonTabPanel([RibbonName.ZeroDoc, RibbonName.Part, RibbonName.Assembly, RibbonName.Drawing], "UI Tools Samples", "Control Buttons")
				.Initialize();

			UpdateSamples(null);
		}
		private void UpdateSamples(Inventor.NameValueMap context)
		{
			RibbonButtonSamples.UseBuilderSample(UIManager);
		}
	}
}