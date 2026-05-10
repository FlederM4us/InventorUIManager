using System;
using System.Runtime.InteropServices;

namespace InventorUIToolsSamples
{
	[Guid("cb5cebd9-4b11-4e9f-b47a-132ca2cf4926")]
	public class StandardAddInServer : Inventor.ApplicationAddInServer
	{
		private Inventor.Application ivApplication;

		public StandardAddInServer()
		{
		}

		#region ApplicationAddInServer Members

		public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
		{
			ivApplication = addInSiteObject.Application;
		}

		public void Deactivate()
		{
			ivApplication = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void ExecuteCommand(int commandID)
		{
			// Note:this method is now obsolete, you should use the 
			// ControlDefinition functionality for implementing commands.
		}

		public object Automation => null;

		#endregion

	}
}
