using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;
using Xunit;

namespace InventorUIManager.UnitTests.BuilderTests
{
	/// <summary>
	/// Unit tests for ButtonDescriptorBuilder fluent API.
	/// </summary>
	public class ButtonDescriptorBuilderTests : InventorTestBase
	{
		[Fact]
		public void WithLabel_SetsDisplayName()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewButtonDescriptor();
			var descriptor = builder.GetButtonDescriptor();
			var label = "Test Label";

			// Act
			var result = builder.WithLabel(label);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(label, descriptor.DisplayName);
		}

		[Fact]
		public void WithLabel_ReturnsBuilderForChaining()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewButtonDescriptor();

			// Act
			var result = builder.WithLabel("Test");

			// Assert
			Assert.Same(builder, result);
		}

		[Fact]
		public void WithTooltip_SetsTooltip()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewButtonDescriptor();
			var tooltip = "Test Tooltip";

			// Act
			builder.WithTooltip(tooltip);

			// Assert
			var descriptor = builder.GetButtonDescriptor();
			Assert.Equal(tooltip, descriptor.Tooltip);
		}

		[Fact]
		public void WithDescription_SetsDescription()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewButtonDescriptor();
			var description = "Description";

			// Act
			builder.WithDescription(description);

			// Assert
			var descriptor = builder.GetButtonDescriptor();
			Assert.Equal(description, descriptor.Description);
		}

		[Fact]
		public void WithClientId_SetsClientId()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewButtonDescriptor();
			var clientId = "CustomClientId";

			// Act
			builder.WithClientId(clientId);

			// Assert
			var descriptor = builder.GetButtonDescriptor();
			Assert.Equal(clientId, descriptor.ClientId);
		}

		[Fact]
		public void FluentChaining_MultipleProperties()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var label = "My Button";
			var tooltip = "Click me";
			var description = "A test button";
			var clientId = "MyClientId";

			// Act
			var descriptor = uiManager.NewButtonDescriptor()
				.WithLabel(label)
				.WithTooltip(tooltip)
				.WithDescription(description)
				.WithClientId(clientId)
				.GetButtonDescriptor();

			// Assert
			Assert.Equal(label, descriptor.DisplayName);
			Assert.Equal(tooltip, descriptor.Tooltip);
			Assert.Equal(description, descriptor.Description);
			Assert.Equal(clientId, descriptor.ClientId);
		}
	}
}
