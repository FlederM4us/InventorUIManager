using System;
using Xunit;
using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;

namespace InventorUIManager.UnitTests.UIManagerTests
{
	/// <summary>
	/// Unit tests for UIManager initialization and context management.
	/// </summary>
	public class UIManagerInitializationTests : InventorTestBase
	{
		[Fact]
		public void Constructor_WithValidParameters_InitializesSuccessfully()
		{
			// Arrange
			var clientId = "TestClientId";

			// Act
			var uiManager = new UIManager(MockApplication.Object, clientId);

			// Assert
			Assert.NotNull(uiManager);
			Assert.Equal(MockApplication.Object, uiManager.IvApplication);
		}

		[Fact]
		public void Constructor_WithEmptyClientId_InitializesSuccessfully()
		{
			// Arrange
			var clientId = string.Empty;

			// Act
			var uiManager = new UIManager(MockApplication.Object, clientId);

			// Assert
			Assert.NotNull(uiManager);
		}

		[Fact]
		public void UIControls_AfterConstruction_ReturnsEmptyList()
		{
			// Arrange
			var clientId = "TestClientId";
			var uiManager = new UIManager(MockApplication.Object, clientId);

			// Act
			var controls = uiManager.UIControls;

			// Assert
			Assert.NotNull(controls);
			Assert.Empty(controls);
		}

		[Fact]
		public void Context_IsSetAfterConstruction()
		{
			// Arrange
			var clientId = "TestClientId";

			// Act
			var uiManager = new UIManager(MockApplication.Object, clientId);

			// Assert
			Assert.NotNull(uiManager.Context);
		}
	}
}
