using System;
using Xunit;
using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;

namespace InventorUIManager.UnitTests.UIManagerTests
{
	/// <summary>
	/// Unit tests for UIManager control registration and management.
	/// </summary>
	public class UIManagerControlRegistrationTests : InventorTestBase
	{
		[Fact]
		public void UIControls_IsInitialized_AsEmptyList()
		{
			// Arrange & Act
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Assert
			Assert.NotNull(uiManager.UIControls);
			Assert.Empty(uiManager.UIControls);
		}

		[Fact]
		public void CreateButtonDescriptor_SetsCorrectClientId()
		{
			// Arrange
			var clientId = "SpecificClientId";
			var uiManager = new UIManager(MockApplication.Object, clientId);

			// Act
			var descriptor = uiManager.CreateButtonDescriptor();

			// Assert
			Assert.NotNull(descriptor);
			Assert.Equal(clientId, descriptor.ClientId);
		}

		[Fact]
		public void CreateRibbonButton_RegistersControlInUIControls()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var buttonDescriptor = uiManager.CreateButtonDescriptor();
			buttonDescriptor.DisplayName = "Test Button";

			// Act
			var ribbonButton = uiManager.CreateRibbonButton(buttonDescriptor);

			// Assert
			Assert.NotNull(ribbonButton);
			Assert.Single(uiManager.UIControls);
			Assert.Equal(ribbonButton, uiManager.UIControls[0]);
		}

		[Fact]
		public void CreateRibbonButton_WithNullDescriptor_ThrowsArgumentNullException()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => uiManager.CreateRibbonButton(null));
		}

		[Fact]
		public void UIControls_RegistersMultipleButtons()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var buttonDescriptor1 = uiManager.CreateButtonDescriptor();
			var buttonDescriptor2 = uiManager.CreateButtonDescriptor();
			buttonDescriptor1.DisplayName = "Button 1";
			buttonDescriptor2.DisplayName = "Button 2";

			// Act
			var button1 = uiManager.CreateRibbonButton(buttonDescriptor1);
			var button2 = uiManager.CreateRibbonButton(buttonDescriptor2);

			// Assert
			Assert.Equal(2, uiManager.UIControls.Count);
			Assert.Equal(button1, uiManager.UIControls[0]);
			Assert.Equal(button2, uiManager.UIControls[1]);
		}
	}
}
