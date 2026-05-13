using Moq;

namespace InventorUIManager.UnitTests.Fixtures
{
	/// <summary>
	/// Provides reusable mock fixtures for Inventor COM interop types.
	/// </summary>
	public static class MockInventorObjects
	{
		/// <summary>
		/// Creates a mock Inventor.Application with configurable behavior.
		/// </summary>
		public static Mock<Inventor.Application> CreateApplicationMock()
		{
			var mockApp = new Mock<Inventor.Application>(MockBehavior.Loose);
			return mockApp;
		}

		/// <summary>
		/// Creates a mock Inventor.UserInterfaceManager for ribbon UI operations.
		/// </summary>
		public static Mock<Inventor.UserInterfaceManager> CreateUserInterfaceManagerMock()
		{
			var mockUIManager = new Mock<Inventor.UserInterfaceManager>(MockBehavior.Loose);
			return mockUIManager;
		}

		/// <summary>
		/// Creates a mock Inventor.Ribbon for ribbon control operations.
		/// </summary>
		public static Mock<Inventor.Ribbon> CreateRibbonMock()
		{
			var mockRibbon = new Mock<Inventor.Ribbon>(MockBehavior.Loose);
			return mockRibbon;
		}

		/// <summary>
		/// Creates a mock Inventor.RibbonTab for ribbon tab operations.
		/// </summary>
		public static Mock<Inventor.RibbonTab> CreateRibbonTabMock(string displayName = "Test Tab")
		{
			var mockTab = new Mock<Inventor.RibbonTab>(MockBehavior.Loose);
			mockTab.Setup(t => t.DisplayName).Returns(displayName);
			return mockTab;
		}
	}
}
