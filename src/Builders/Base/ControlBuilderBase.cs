using System;

namespace InventorUITools
{
	/// <summary>
	/// Provides a fluent API base for building and configuring UI controls.
	/// </summary>
	/// <typeparam name="TBuilder">The type of the concrete builder.</typeparam>
	public abstract class ControlBuilderBase<TBuilder>
		where TBuilder : ControlBuilderBase<TBuilder>
	{
		/// <summary>
		/// The UI manager used to manage controls.
		/// </summary>
		protected readonly UIManager _uiManager;
		/// <summary>
		/// The Inventor application instance.
		/// </summary>
		protected readonly Inventor.Application _ivApplication;
		/// <summary>
		/// Indicates whether to use a large buttron (icon) for the control.
		/// </summary>
		protected bool _useLargeIcon = true;
		/// <summary>
		/// Indicates whether to show text on the control.
		/// </summary>
		protected bool _showText = true;
		/// <summary>
		/// The internal name of the target control for placement.
		/// </summary>
		protected string _targetControlInternalName = "";
		/// <summary>
		/// Indicates whether to insert this control before the target control.
		/// </summary>
		protected bool _insertBeforeTargetControl = false;
		/// <summary>
		/// Initializes a new instance of the <see cref="ControlBuilderBase{TBuilder}"/> class.
		/// </summary>
		/// <param name="uiManager">The UI manager instance.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="uiManager"/> is null.</exception>
		public ControlBuilderBase(UIManager uiManager)
		{
			_uiManager = uiManager ?? throw new ArgumentNullException(nameof(uiManager));
			_ivApplication = _uiManager.IvApplication;
		}
		/// <summary>
		/// Sets whether the control should use a large icon.
		/// </summary>
		/// <param name="largeIcon">True to use a large button (icon); otherwise, false. Default is true.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithLargeButton(bool largeIcon)
		{
			_useLargeIcon = largeIcon;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets whether the control should display text.
		/// </summary>
		/// <param name="showText">True to show text; otherwise, false.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder ShowText(bool showText)
		{
			_showText = showText;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets the internal name of the target control for placement.
		/// </summary>
		/// <param name="targetControlInternalName">The internal name of the target control.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder WithTargetControlInternalName(string targetControlInternalName)
		{
			_targetControlInternalName = targetControlInternalName;
			return this as TBuilder;
		}
		/// <summary>
		/// Sets whether to insert this control before the target control.
		/// </summary>
		/// <param name="insertBeforeTargetControl">True to insert before the target control; otherwise, false.</param>
		/// <returns>The builder instance for chaining.</returns>
		public TBuilder InsertBeforeTargetControl(bool insertBeforeTargetControl)
		{
			_insertBeforeTargetControl = insertBeforeTargetControl;
			return this as TBuilder;
		}
	}
}
