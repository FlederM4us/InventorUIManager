using FlederM4us.InventorUI.Manager;
using Inventor;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InventorUIManager.SmokeTest
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
				.OnExecute(ShowVersion)
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

		private void ShowVersion(NameValueMap valueMap)
		{
			var assembly = typeof(UIManager).Assembly;
			var assemblyLocation = assembly.Location;
			var fileVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
			var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
			var assemblyVersion = assembly.GetName().Version;

			MessageBox.Show(
				$"AssemblyLocation: {assemblyLocation}\n" +
				$"AssemblyVersion: {assemblyVersion}\n" +
				$"FileVersion: {fileVersion}\n" +
				$"ProductVersion: {productVersion}"
			);
		}

	}
}