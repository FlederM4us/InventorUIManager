using FlederM4us.InventorUI.Manager;
using InventorUIManager.UnitTests.Fixtures;
using System;
using Xunit;

namespace InventorUIManager.UnitTests.UIManagerTests
{
	public class UIManagerRegistryTests
	{
		[Fact]
		public void RunWhenAvailable_InvokesQueuedActionWhenManagerRegisters()
		{
			var clientId = Guid.NewGuid().ToString();
			UIManager resolvedManager = null;

			UIManagerRegistry.RunWhenAvailable(clientId, manager => resolvedManager = manager);

			using var uiManager = new UIManager(MockInventorObjects.CreateApplicationMock().Object, clientId);

			Assert.Same(uiManager, resolvedManager);
		}

		[Fact]
		public void RunWhenAvailable_InvokesActionImmediatelyForRegisteredManager()
		{
			var clientId = Guid.NewGuid().ToString();
			using var uiManager = new UIManager(MockInventorObjects.CreateApplicationMock().Object, clientId);
			UIManager resolvedManager = null;

			UIManagerRegistry.RunWhenAvailable(clientId, manager => resolvedManager = manager);

			Assert.Same(uiManager, resolvedManager);
		}

		[Fact]
		public void Dispose_RemovesManagerFromRegistry()
		{
			var clientId = Guid.NewGuid().ToString();
			using var firstManager = new UIManager(MockInventorObjects.CreateApplicationMock().Object, clientId);
			firstManager.Dispose();
			int invocationCount = 0;

			UIManagerRegistry.RunWhenAvailable(clientId, _ => invocationCount++);
			using var replacementManager = new UIManager(MockInventorObjects.CreateApplicationMock().Object, clientId);

			Assert.Equal(1, invocationCount);
		}

		[Fact]
		public void RunWhenAvailable_ContinuesAfterQueuedActionFails()
		{
			var clientId = Guid.NewGuid().ToString();
			int successfulActionCalls = 0;

			UIManagerRegistry.RunWhenAvailable(clientId, _ => throw new InvalidOperationException("Expected test failure."));
			UIManagerRegistry.RunWhenAvailable(clientId, _ => successfulActionCalls++);
			using var uiManager = new UIManager(MockInventorObjects.CreateApplicationMock().Object, clientId);

			Assert.Equal(1, successfulActionCalls);
		}
	}
}
