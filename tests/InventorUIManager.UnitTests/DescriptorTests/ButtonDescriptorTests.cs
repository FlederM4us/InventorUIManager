using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;
using Xunit;

namespace InventorUIManager.UnitTests.DescriptorTests
{
	/// <summary>
	/// Unit tests for UI control descriptors.
	/// </summary>
	public class ButtonDescriptorTests : InventorTestBase
	{
		[Fact]
		public void ButtonDescriptor_IvApplication_IsSet()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();

			// Assert
			Assert.NotNull(descriptor.IvApplication);
			Assert.Equal(MockApplication.Object, descriptor.IvApplication);
		}

		[Fact]
		public void ButtonDescriptor_IvApplication_Matches_UIManager()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();

			// Assert
			Assert.Equal(uiManager.IvApplication, descriptor.IvApplication);
		}

		[Fact]
		public void ButtonDescriptor_WithDisplayName_StoresValue()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var displayName = "Test Button";

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();
			descriptor.DisplayName = displayName;

			// Assert
			Assert.Equal(displayName, descriptor.DisplayName);
		}

		[Fact]
		public void ButtonDescriptor_WithTooltip_StoresValue()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var tooltip = "Test Tooltip";

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();
			descriptor.Tooltip = tooltip;

			// Assert
			Assert.Equal(tooltip, descriptor.Tooltip);
		}

		[Fact]
		public void ButtonDescriptor_WithDescription_StoresValue()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var description = "Test Description";

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();
			descriptor.Description = description;

			// Assert
			Assert.Equal(description, descriptor.Description);
		}

		[Fact]
		public void ButtonDescriptor_InternalName_GeneratedFromDisplayName()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var descriptor = uiManager.CreateButtonDescriptor();
			descriptor.DisplayName = "Test Button";

			// Act
			var internalName = descriptor.InternalName;

			// Assert
			Assert.NotNull(internalName);
			Assert.Contains("TestButton", internalName); // Spaces removed
		}
	}
}