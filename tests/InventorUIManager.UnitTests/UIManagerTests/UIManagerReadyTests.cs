using FlederM4us.InventorUI.Manager;
using Inventor;
using Moq;
using System;
using Xunit;

namespace InventorUIManager.UnitTests.UIManagerTests
{
	/// <summary>
	/// Tests the Inventor-ready lifecycle managed by <see cref="UIManager"/>.
	/// </summary>
	public class UIManagerReadyTests
	{
		[Fact]
		public void RunWhenReady_QueuesActionUntilOnReady()
		{
			// Arrange
			var lifecycle = new ApplicationLifecycle(isReady: false);
			using var uiManager = new UIManager(lifecycle.Application.Object, "TestClientId");
			int invocationCount = 0;

			// Act
			uiManager.RunWhenReady(() => invocationCount++);
			lifecycle.RaiseOnReady();

			// Assert
			Assert.Equal(1, invocationCount);
			Assert.Equal(HandlingCodeEnum.kEventNotHandled, lifecycle.LastHandlingCode);
		}

		[Fact]
		public void RunWhenReady_WhenApplicationIsAlreadyReady_ExecutesAction()
		{
			// Arrange
			var lifecycle = new ApplicationLifecycle(isReady: true);
			using var uiManager = new UIManager(lifecycle.Application.Object, "TestClientId");
			int invocationCount = 0;

			// Act
			uiManager.RunWhenReady(() => invocationCount++);

			// Assert
			Assert.Equal(1, invocationCount);
		}

		[Fact]
		public void RunWhenReady_ContinuesAfterAnotherQueuedActionFails()
		{
			// Arrange
			var lifecycle = new ApplicationLifecycle(isReady: false);
			using var uiManager = new UIManager(lifecycle.Application.Object, "TestClientId");
			int successfulActionCalls = 0;
			uiManager.RunWhenReady(() => throw new InvalidOperationException("Expected test failure."));
			uiManager.RunWhenReady(() => successfulActionCalls++);

			// Act
			lifecycle.RaiseOnReady();

			// Assert
			Assert.Equal(1, successfulActionCalls);
		}

		[Fact]
		public void Dispose_UnsubscribesFromOnReadyAndRejectsNewActions()
		{
			// Arrange
			var lifecycle = new ApplicationLifecycle(isReady: false);
			var uiManager = new UIManager(lifecycle.Application.Object, "TestClientId");

			// Act
			uiManager.Dispose();

			// Assert
			Assert.False(lifecycle.HasOnReadySubscriber);
			Assert.Throws<ObjectDisposedException>(() => uiManager.RunWhenReady(() => { }));
		}

		private sealed class ApplicationLifecycle
		{
			private ApplicationEventsSink_OnReadyEventHandler _onReady;

			public ApplicationLifecycle(bool isReady)
			{
				Application = new Mock<Application>(MockBehavior.Loose);
				ApplicationEvents = new Mock<ApplicationEvents>(MockBehavior.Loose);

				Application.SetupGet(application => application.ApplicationEvents).Returns(ApplicationEvents.Object);
				Application.SetupGet(application => application.Ready).Returns(isReady);
				ApplicationEvents
					.SetupAdd(applicationEvents => applicationEvents.OnReady += It.IsAny<ApplicationEventsSink_OnReadyEventHandler>())
					.Callback((ApplicationEventsSink_OnReadyEventHandler handler) => _onReady += handler);
				ApplicationEvents
					.SetupRemove(applicationEvents => applicationEvents.OnReady -= It.IsAny<ApplicationEventsSink_OnReadyEventHandler>())
					.Callback((ApplicationEventsSink_OnReadyEventHandler handler) => _onReady -= handler);
			}

			public Mock<Application> Application { get; }
			public Mock<ApplicationEvents> ApplicationEvents { get; }
			public HandlingCodeEnum LastHandlingCode { get; private set; }
			public bool HasOnReadySubscriber => _onReady is not null;

			public void RaiseOnReady()
			{
				var onReady = _onReady ?? throw new InvalidOperationException("UIManager did not subscribe to Inventor OnReady.");
				var handlingCode = HandlingCodeEnum.kEventNotHandled;
				onReady(EventTimingEnum.kAfter, Mock.Of<NameValueMap>(), out handlingCode);
				LastHandlingCode = handlingCode;
			}
		}
	}
}
