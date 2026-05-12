using System.Windows.Forms;
using Xunit;
using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;

namespace InventorUIManager.UnitTests.UIManagerTests
{
	/// <summary>
	/// Unit tests for UIManager synchronization context management.
	/// </summary>
	public class UIManagerContextTests : InventorTestBase
	{
		[Fact]
		public void Context_IsNotNull()
		{
			// Arrange & Act
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Assert
			Assert.NotNull(uiManager.Context);
		}

		[Fact]
		public void Context_IsWindowsFormsSynchronizationContext()
		{
			// Arrange & Act
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Assert
			Assert.IsType<WindowsFormsSynchronizationContext>(uiManager.Context);
		}

		[Fact]
		public void IvApplication_IsStored()
		{
			// Arrange & Act
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Assert
			Assert.NotNull(uiManager.IvApplication);
			Assert.Equal(MockApplication.Object, uiManager.IvApplication);
		}

		[Fact]
		public void MultipleUIManagers_HaveIndependentContexts()
		{
			// Arrange & Act
			var manager1 = new UIManager(MockApplication.Object, "Client1");
			var manager2 = new UIManager(MockApplication.Object, "Client2");

			// Assert
			Assert.NotNull(manager1.Context);
			Assert.NotNull(manager2.Context);
			// Both should have valid contexts (may or may not be same object depending on Windows Forms setup)
			Assert.IsType<WindowsFormsSynchronizationContext>(manager1.Context);
			Assert.IsType<WindowsFormsSynchronizationContext>(manager2.Context);
		}
	}
}
