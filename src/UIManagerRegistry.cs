using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlederM4us.InventorUI.Manager
{
	/// <summary>
	/// Coordinates access to UI managers that are registered by their Inventor client IDs.
	/// </summary>
	public static class UIManagerRegistry
	{
		private static readonly object _syncRoot = new();
		private static readonly Dictionary<string, UIManager> _managers = new(StringComparer.OrdinalIgnoreCase);
		private static readonly Dictionary<string, List<Action<UIManager>>> _pendingActions = new(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Attempts to get the UI manager registered for the specified client identifier.
		/// </summary>
		/// <param name="clientId">The client identifier used to locate the UI manager.</param>
		/// <param name="uIManager">
		/// When this method returns <see langword="true"/>, contains the UI manager associated with the specified client identifier;
		/// otherwise, <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if a UI manager exists for the specified <paramref name="clientId"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool TryGetManager(string clientId, out UIManager uIManager) => _managers.TryGetValue(clientId, out uIManager);

		/// <summary>
		/// Runs an action when a UI manager for the specified Inventor client ID is available.
		/// </summary>
		/// <param name="clientId">The Inventor add-in client ID.</param>
		/// <param name="action">The action to run with the available UI manager.</param>
		public static void RunWhenAvailable(string clientId, Action<UIManager> action)
		{
			ArgumentNullException.ThrowIfNull(clientId);
			ArgumentNullException.ThrowIfNull(action);

			var normalizedClientId = NormalizeClientId(clientId);
			UIManager uiManager;

			lock (_syncRoot)
			{
				if (!_managers.TryGetValue(normalizedClientId, out uiManager))
				{
					if (!_pendingActions.TryGetValue(normalizedClientId, out var actions))
					{
						actions = [];
						_pendingActions.Add(normalizedClientId, actions);
					}

					actions.Add(action);
					return;
				}
			}

			action(uiManager);
		}

		internal static void Register(UIManager uiManager, string clientId)
		{
			ArgumentNullException.ThrowIfNull(uiManager);

			List<Action<UIManager>> actions = null;
			var normalizedClientId = NormalizeClientId(clientId);

			lock (_syncRoot)
			{
				_managers[normalizedClientId] = uiManager;
				_pendingActions.Remove(normalizedClientId, out actions);
			}

			if (actions is null)
				return;

			foreach (var action in actions)
			{
				try
				{
					action(uiManager);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Unable to run a UI manager availability action: {ex}");
				}
			}
		}

		internal static void Unregister(UIManager uiManager, string clientId)
		{
			ArgumentNullException.ThrowIfNull(uiManager);

			var normalizedClientId = NormalizeClientId(clientId);

			lock (_syncRoot)
			{
				if (_managers.TryGetValue(normalizedClientId, out var registeredManager) && ReferenceEquals(registeredManager, uiManager))
					_managers.Remove(normalizedClientId);
			}
		}

		private static string NormalizeClientId(string clientId) => clientId?.Trim() ?? string.Empty;
	}
}
