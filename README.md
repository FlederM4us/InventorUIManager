# InventorUI.Manager

A comprehensive .NET 8 library for building and managing custom user interface elements in Autodesk Inventor through the Inventor API. This project provides a fluent, builder-pattern-based approach to create ribbon buttons, toggle popups, and other UI controls with simplified setup and lifecycle management.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Architecture](#architecture)
- [Usage Examples](#usage-examples)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Overview

InventorUI.Manager simplifies the creation and management of custom UI controls in Autodesk Inventor. Instead of directly working with the complex Inventor API, developers can use an intuitive builder pattern to define buttons, toggle popups, and other ribbon controls with minimal boilerplate code.

### Key Benefits

- **Builder Pattern**: Fluent API for intuitive UI control creation
- **Lifecycle Management**: Automatic initialization and management of UI controls
- **Type Safety**: Strongly-typed configuration with compile-time checking
- **Flexible Ribbon Placement**: Easy placement of controls on specific ribbon tabs and panels
- **Event Handling**: Simplified button click and event subscription
- **Synchronization**: Built-in UI thread synchronization for thread-safe operations

## Features

### Supported UI Controls

- **Ribbon Buttons**: Create custom buttons on Inventor ribbon tabs and panels
- **Toggle Buttons**: Toggle item buttons with on/off states
- **Toggle Popups**: Toggle popup menus with multiple items
- **ComboBox Controls**: Dropdown selection controls

### Advanced Capabilities

- Fluent builder API with method chaining
- Multiple ribbon context support (Part, Assembly, Drawing, etc.)
- Custom ribbon tab and panel creation
- Event-driven architecture with click and state-change handlers
- Thread-safe UI operations via synchronization context
- Icon and image support for controls

## Requirements

- **.NET Framework**: .NET 8.0 (Windows)
- **Autodesk Inventor**: 2026 or compatible
- **Autodesk Inventor Interop Assembly**: `Autodesk.Inventor.Interop.dll`

## Installation

1. Clone or download this project to your local machine.
2. Open the project in Visual Studio 2022 or later.
3. Ensure the Autodesk Inventor 2026 interop assembly reference is correctly configured:
```xml
<Reference Include="Autodesk.Inventor.Interop">
  <HintPath>C:\Program Files\Autodesk\Inventor 2026\Bin\Autodesk.Inventor.Interop.dll</HintPath>
  <EmbedInteropTypes>False</EmbedInteropTypes>
  <Private>True</Private>
</Reference>
```
4. Build the project using `dotnet build` or Visual Studio.

## Quick Start

### 1. Initialize the UIManager

```csharp
using FlederM4us.InventorUI.Manager;
using System;
using System.Runtime.InteropServices;

namespace Samples
{
	[Guid("your_GUID")]
	public class StandardAddInServer : Inventor.ApplicationAddInServer, IUIManager
	{
		private Inventor.Application _ivApplication;
		private UIManager _uiManager;

		public StandardAddInServer() { }

		public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
		{
			_ivApplication = addInSiteObject.Application;
			_uiManager = new UIManager(_ivApplication, addInSiteObject.Parent.ClientId);
		}

		public void Deactivate()
		{
			_ivApplication = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void ExecuteCommand(int commandID) { }

		public object Automation => null;

		public UIManager UIManager => _uiManager;
	}
}
```

### 2. Create a Ribbon Button

```csharp
// Create and configure a ribbon button using the builder pattern
UIManager.NewRibbonButton()
	.WithLabel("My Custom Button")
	.WithTooltip("This is my custom button")
	.WithIcon(Properties.Resources.Update)
	.OnExecute((c) => MessageBox.Show("My Custom Button Handler"))
	.AddToRibbonTabPanel([RibbonName.ZeroDoc, RibbonName.Part, RibbonName.Assembly, RibbonName.Drawing], "My Tab", "My Panel")
	.Initialize();
```


## Architecture

### Core Components

#### UIManager
- Central management class for all UI controls
- Maintains list of registered UI controls
- Provides builder factories for creating UI elements
- Handles synchronization context for thread-safe operations

#### Builder Pattern Classes

- **ButtonDescriptorBuilderBase**: Base fluent builder with common functionality
- **RibbonButtonBuilder**: Fluent builder for ribbon buttons
- **ButtonDescriptorBuilder**: Fluent builder for button descriptors
- **RibbonTogglePopupBuilder**: Fluent builder for toggle popup menus
- **ToogleItemBuilder**: Fluent builder for toggle items

#### Control Classes

- **UIControlBase**: Base class for all UI controls
- **RibbonControlBase**: Base class for all ribbon controls
- **RibbonButton**: Wraps ButtonDescriptor and manages ribbon button lifecycle
- **RibbonTogglePopup**: Manages toggle popup controls
- **ToogleItem**: Represents a single item in a toggle popup

#### Descriptor Classes

- **ControlDescriptorBase**: Base class for control descriptors
- **ButtonDescriptor**: Wraps Inventor's ButtonDefinition with lifecycle management
- **ComboBoxDescriptor**: Descriptor for combo box controls

#### Interfaces

- **IUserInterfaceManager**: Contract for UI management services
- **IRibbonPlacing**: Interface for ribbon placement configuration

### Design Patterns

1. **Builder Pattern**: Fluent API for control configuration
2. **Descriptor Pattern**: Separates control definition from UI representation
3. **Factory Pattern**: UIManager provides factory methods for control creation
4. **Synchronization Pattern**: Thread-safe UI operations via SynchronizationContext

## Usage Examples

Working on it.

## Project Structure

```
src/
├── Builders/
│   ├── Base/
│   │   ├── ButtonDescriptorBuilderBase.cs
│   │   ├── ControlBuilderBase.cs
│   │   └── IRibbonPlacing.cs
│   ├── ButtonDescriptorBuilder.cs
│   ├── RibbonButtonBuilder.cs
│   ├── RibbonTogglePopupBuilder.cs
│   └── ToogleItemBuilder.cs
├── Controls/
│   ├── Base/
│   │   ├── RibbonControlBase.cs
│   │   └── UIControlBase.cs
│   ├── RibbonButton.cs
│   ├── RibbonTogglePopup.cs
│   └── ToogleItem.cs
├── Descriptors/
│   ├── Base/
│   │   └── ControlDescriptorBase.cs
│   ├── ButtonDescriptor.cs
│   └── ComboBoxDescriptor.cs
├── IUIManager.cs
├── UIManager.cs
└── InventorUIManager.csproj
```

### Directory Breakdown

- **Builders**: Contains builder classes for fluent UI control creation
  - `Base`: Abstract base classes and interfaces for builders
  - Individual builders for specific control types

- **Controls**: Contains wrapper classes for UI controls
  - `Base`: Abstract base classes for all controls
  - Individual control implementations

- **Descriptors**: Contains descriptor classes that define UI control properties
  - `Base`: Abstract base class for descriptors
  - Individual descriptor implementations


## Contributing

Contributions are welcome! When contributing:

1. Follow the existing code style and conventions
2. Use the builder pattern for new control types
3. Add XML documentation to public classes and methods
4. Test your changes with Inventor before submitting
5. Ensure null checks and appropriate exception handling

## License

This project is provided as-is for use with Autodesk Inventor. Please ensure compliance with Autodesk's licensing terms when using this library.

---

**Note**: This library requires Autodesk Inventor 2026 and the Inventor API interop assemblies. Ensure you have the appropriate Inventor installation and licenses before using this library.
