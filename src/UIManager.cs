using Inventor;
using InventorApplication = Inventor.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Manages UI controls and the Inventor UI-thread lifecycle for an add-in.
	/// </summary>
	public class UIManager : IDisposable
	{
		private readonly string _clientId;
		private readonly int _ownerThreadId;
		private readonly object _readyLock = new();
		private readonly List<Action> _readyActions = [];

		private ApplicationEvents _applicationEvents;
		private bool _isReady;
		private bool _isDisposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="UIManager"/> class.
		/// This constructor must be called on the Inventor UI thread.
		/// </summary>
		/// <param name="ivApplication">The Inventor application instance.</param>
		/// <param name="clientId">The unique identifier for registering UI elements.</param>
		public UIManager(InventorApplication ivApplication, string clientId)
		{
			IvApplication = ivApplication ?? throw new ArgumentNullException(nameof(ivApplication));
			_clientId = clientId;
			_ownerThreadId = System.Environment.CurrentManagedThreadId;

			// This creates an explicit dispatcher for the current thread without replacing
			// SynchronizationContext.Current for the whole Inventor UI thread.
			Context = new WindowsFormsSynchronizationContext();
			InitializeReadyLifecycle();
			UIManagerRegistry.Register(this, _clientId);
		}

		/// <summary>
		/// Gets the Inventor application instance.
		/// </summary>
		public InventorApplication IvApplication { get; }

		/// <summary>
		/// Gets the synchronization context bound to the Inventor UI thread that created this manager.
		/// Prefer <see cref="RunWhenReady(Action)"/> for add-in ribbon initialization.
		/// </summary>
		public SynchronizationContext Context { get; }

		/// <summary>
		/// Gets the list of UI controls managed by this instance.
		/// </summary>
		public List<UIControlBase> UIControls { get; } = [];

		/// <summary>
		/// Initializes all registered UI controls.
		/// </summary>
		public void Initialize() => UIControls.ForEach(control => control.Initialize());

		/// <summary>
		/// Executes <paramref name="action"/> on the Inventor UI thread after Inventor is ready.
		/// If Inventor is not ready yet, the action is queued until its one-shot OnReady event arrives.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when this manager has been disposed.</exception>
		public void RunWhenReady(Action action)
		{
			ArgumentNullException.ThrowIfNull(action);

			lock (_readyLock)
			{
				ObjectDisposedException.ThrowIf(_isDisposed, this);

				if (!_isReady)
				{
					_readyActions.Add(action);
					return;
				}
			}

			DispatchToInventorThread(action);
		}

		#region Builders
		/// <summary>
		/// Creates a new <see cref="ButtonDescriptorBuilder"/> instance.
		/// </summary>
		public ButtonDescriptorBuilder NewButtonDescriptor() => new(this);
		/// <summary>
		/// Creates a new <see cref="RibbonButtonBuilder"/> instance.
		/// </summary>
		public RibbonButtonBuilder NewRibbonButton() => new(this);
		/// <summary>
		/// Creates a new <see cref="RibbonTooglePopupBuilder"/> instance.
		/// </summary>
		public RibbonTooglePopupBuilder NewRibbonTooglePopup() => new(this);
		/// <summary>
		/// Creates a new <see cref="ToogleItemBuilder"/> instance.
		/// </summary>
		public ToogleItemBuilder NewToogleItem() => new(this);
		#endregion

		#region Create Methods
		/// <summary>
		/// Creates a new <see cref="ButtonDescriptor"/> configured with the application and client ID.
		/// </summary>
		public ButtonDescriptor CreateButtonDescriptor() => new(IvApplication) { ClientId = _clientId };
		/// <summary>
		/// Creates a toggle item based on the given button descriptor.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor associated with the toggle item.</param>
		public ToogleItem CreateToogleItem(ButtonDescriptor buttonDescriptor) => new(buttonDescriptor);
		/// <summary>
		/// Creates a ribbon button based on the specified button descriptor and registers it for initialization.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor defining button properties.</param>
		public RibbonButton CreateRibbonButton(ButtonDescriptor buttonDescriptor)
		{
			ArgumentNullException.ThrowIfNull(buttonDescriptor);
			var ribbonButton = new RibbonButton(buttonDescriptor);
			UIControls.Add(ribbonButton);
			return ribbonButton;
		}
		/// <summary>
		/// Creates a ribbon toggle popup with the provided descriptor and collection of buttons.
		/// Registers the control for later initialization.
		/// </summary>
		/// <param name="buttonDescriptor">The descriptor for the popup control.</param>
		/// <param name="toogleItems">A collection of buttons to appear in the popup.</param>
		public RibbonTooglePopup CreateRibbonTooglePopup(ButtonDescriptor buttonDescriptor, List<ToogleItem> toogleItems)
		{
			ArgumentNullException.ThrowIfNull(buttonDescriptor);
			ArgumentNullException.ThrowIfNull(toogleItems);
			var ribbonButton = new RibbonTooglePopup(buttonDescriptor, toogleItems);
			UIControls.Add(ribbonButton);
			return ribbonButton;
		}
		#endregion

		/// <summary>
		/// Unsubscribes from Inventor lifecycle events. Registered UI controls are not deleted.
		/// </summary>
		public void Dispose()
		{
			ApplicationEvents applicationEvents;

			lock (_readyLock)
			{
				if (_isDisposed)
					return;

				_isDisposed = true;
				_readyActions.Clear();
				applicationEvents = _applicationEvents;
				_applicationEvents = null;
			}

			UIManagerRegistry.Unregister(this, _clientId);
			UnsubscribeFromOnReady(applicationEvents);
			GC.SuppressFinalize(this);
		}

		private void InitializeReadyLifecycle()
		{
			_applicationEvents = IvApplication.ApplicationEvents;
			_applicationEvents.OnReady += ApplicationEvents_OnReady;

			// Subscribe first, then inspect Ready. This closes the race in which
			// Inventor becomes ready between a readiness check and event subscription.
			if (IvApplication.Ready)
				CompleteReady();
		}

		private void ApplicationEvents_OnReady(EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
		{
			handlingCode = HandlingCodeEnum.kEventNotHandled;

			if (beforeOrAfter != EventTimingEnum.kAfter)
				return;

			try
			{
				DispatchToInventorThread(CompleteReady);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unable to process Inventor OnReady: {ex}");
			}
		}

		private void CompleteReady()
		{
			List<Action> actions;
			ApplicationEvents applicationEvents;

			lock (_readyLock)
			{
				if (_isDisposed || _isReady)
					return;

				_isReady = true;
				actions = [.. _readyActions];
				_readyActions.Clear();
				applicationEvents = _applicationEvents;
				_applicationEvents = null;
			}

			UnsubscribeFromOnReady(applicationEvents);

			foreach (var action in actions)
			{
				try
				{
					DispatchToInventorThread(action);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Unable to run an Inventor-ready action: {ex}");
				}
			}
		}

		private void DispatchToInventorThread(Action action)
		{
			if (System.Environment.CurrentManagedThreadId == _ownerThreadId)
			{
				action();
				return;
			}

			Context.Post(_ => action(), null);
		}

		private void UnsubscribeFromOnReady(ApplicationEvents applicationEvents)
		{
			if (applicationEvents is null)
				return;

			try
			{
				applicationEvents.OnReady -= ApplicationEvents_OnReady;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unable to unsubscribe from Inventor OnReady: {ex}");
			}
		}
	}
}
