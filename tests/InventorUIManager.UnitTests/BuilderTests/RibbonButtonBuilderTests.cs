using System.Collections.Generic;
using Xunit;
using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;

namespace InventorUIManager.UnitTests.BuilderTests
{
	/// <summary>
	/// Unit tests for RibbonButtonBuilder fluent API.
	/// </summary>
	public class RibbonButtonBuilderTests : InventorTestBase
	{
		[Fact]
		public void RibbonButtonBuilder_WithLabel_SetsDisplayName()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewRibbonButton();

			// Act
			var result = builder.WithLabel("Ribbon Button");

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void RibbonButtonBuilder_WithTooltip_SetsTooltip()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewRibbonButton();

			// Act
			builder.WithTooltip("Button Tooltip");

			// Assert
			Assert.NotNull(builder);
		}

		[Fact]
		public void RibbonButtonBuilder_WithDescription_SetsDescription()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewRibbonButton();

			// Act
			builder.WithDescription("Button Description");

			// Assert
			Assert.NotNull(builder);
		}

		[Fact]
		public void RibbonButtonBuilder_AddToRibbonTabPanel_SetsRibbonProperties()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewRibbonButton();
			var ribbons = new List<RibbonName> { RibbonName.Part };

			// Act
			var result = builder.AddToRibbonTabPanel(ribbons, "CustomTab", "CustomPanel");

			// Assert
			Assert.NotNull(result);
			Assert.Equal(ribbons, builder.RibbonContexts);
			Assert.Equal("CustomTab", builder.RibbonTab);
			Assert.Equal("CustomPanel", builder.RibbonPanel);
		}

		[Fact]
		public void RibbonButtonBuilder_AddToRibbonTabPanel_WithDefaults()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var builder = uiManager.NewRibbonButton();

			// Act
			builder.AddToRibbonTabPanel();

			// Assert
			Assert.Equal("Automation", builder.RibbonTab);
			Assert.Equal("Default", builder.RibbonPanel);
		}

		[Fact]
		public void RibbonButtonBuilder_FluentChaining()
		{
			// Arrange
			var uiManager = new UIManager(MockApplication.Object, "TestClientId");
			var ribbons = new List<RibbonName> { RibbonName.Assembly };

			// Act
			var result = uiManager.NewRibbonButton()
				.WithLabel("Multi-Property Button")
				.WithTooltip("Test Tooltip")
				.WithDescription("Test Description")
				.AddToRibbonTabPanel(ribbons, "CustomTab", "CustomPanel");

			// Assert
			Assert.NotNull(result);
		}
	}
}
