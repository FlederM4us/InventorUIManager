using FlederM4us.InventorUI.Manager;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NuGetPackageTest
{
	[Guid("e2d77621-8ee9-486c-b8de-cf86024f6906")]
	public class StandardAddInServer : Inventor.ApplicationAddInServer, IUIManager
	{
		private Inventor.Application _ivApplication;
		private UIManager _uiManager;

		public StandardAddInServer()
		{
		}

		#region ApplicationAddInServer Members

		public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
		{
			_ivApplication = addInSiteObject.Application;
			_uiManager = new UIManager(_ivApplication, addInSiteObject.Parent.ClientId);


			UIManager.NewRibbonButton()
				.WithLabel("Test")
				.OnExecute((c) => MessageBox.Show("Test"))
				.AddToRibbonTabPanel([RibbonName.ZeroDoc, RibbonName.Part, RibbonName.Assembly, RibbonName.Drawing], "UI Tools Samples", "Control Buttons")
				.Initialize();
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

	}
}