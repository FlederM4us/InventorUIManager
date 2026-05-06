# InventorUITools

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

InventorUITools simplifies the creation and management of custom UI controls in Autodesk Inventor. Instead of directly working with the complex Inventor API, developers can use an intuitive builder pattern to define buttons, toggle popups, and other ribbon controls with minimal boilerplate code.

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
- **Windows Forms**: For UI control management

## Installation

1. Clone or download this project to your local machine.
2. Open the project in Visual Studio 2022 or later.
3. Ensure the Autodesk Inventor 2026 interop assembly reference is correctly configured:
   ```
   C:\Program Files\Autodesk\Inventor 2026\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll
   ```
4. Build the project using `dotnet build` or Visual Studio.

## Quick Start

### 1. Initialize the UIManager

```csharp
using InventorUITools;
using Inventor;

// Get the Inventor application instance
Application ivApp = GetInventorApplication();

// Create a UIManager instance
var uiManager = new UIManager(ivApp, "com.example.myapp");
```

### 2. Create a Ribbon Button

```csharp
// Create and configure a ribbon button using the builder pattern
uiManager.NewRibbonButton()
	.WithDisplayName("My Custom Button")
	.WithDescription("This is my custom button")
	.WithClientId("cmd.mybutton")
	.OnExecuted(button => {
		// Handle button click
		MessageBox.Show("Button clicked!");
	})
	.AddToRibbonTabPanel(
		ribbons: new List<RibbonName> { RibbonName.PartRibbon },
		tabName: "Automation",
		panelName: "Custom"
	)
	.Build();
```

### 3. Initialize UI Controls

```csharp
// Initialize all registered UI controls
uiManager.Initialize();
```

## Architecture

### Core Components

#### UIManager
- Central management class for all UI controls
- Maintains list of registered UI controls
- Provides builder factories for creating UI elements
- Handles synchronization context for thread-safe operations

#### Builder Pattern Classes

- **RibbonButtonBuilder**: Fluent builder for ribbon buttons
- **ButtonDescriptorBuilder**: Builder for button descriptors
- **RibbonTogglePopupBuilder**: Builder for toggle popup menus
- **ToogleItemBuilder**: Builder for toggle items
- **ButtonDescriptorBuilderBase**: Base builder with common functionality

#### Control Classes

- **RibbonButton**: Wraps ButtonDescriptor and manages ribbon button lifecycle
- **RibbonControlBase**: Base class for all ribbon controls
- **RibbonTogglePopup**: Manages toggle popup controls
- **ToogleItem**: Represents a single item in a toggle popup
- **UIControlBase**: Base class for all UI controls

#### Descriptor Classes

- **ButtonDescriptor**: Wraps Inventor's ButtonDefinition with lifecycle management
- **ControlDescriptorBase**: Base class for control descriptors
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

### Example 1: Simple Button with Click Handler

```csharp
var uiManager = new UIManager(ivApp, "com.example.app");

uiManager.NewRibbonButton()
	.WithDisplayName("Analyze")
	.WithDescription("Analyze selected geometry")
	.WithClientId("cmd.analyze")
	.OnExecuted(button => {
		// Perform analysis
	})
	.AddToRibbonTabPanel()
	.Build();

uiManager.Initialize();
```

### Example 2: Multiple Buttons on Custom Tab

```csharp
var uiManager = new UIManager(ivApp, "com.example.modeling");

// Create a custom ribbon tab
uiManager.NewRibbonButton()
	.WithDisplayName("Export")
	.WithClientId("cmd.export")
	.OnExecuted(button => {
		// Handle export
	})
	.AddToRibbonTabPanel(
		ribbons: new List<RibbonName> { 
			RibbonName.PartRibbon, 
			RibbonName.AssemblyRibbon 
		},
		tabName: "Custom Tools",
		panelName: "Export"
	)
	.Build();

uiManager.NewRibbonButton()
	.WithDisplayName("Import")
	.WithClientId("cmd.import")
	.OnExecuted(button => {
		// Handle import
	})
	.AddToRibbonTabPanel(
		ribbons: new List<RibbonName> { 
			RibbonName.PartRibbon, 
			RibbonName.AssemblyRibbon 
		},
		tabName: "Custom Tools",
		panelName: "Export"
	)
	.Build();

uiManager.Initialize();
```

### Example 3: Toggle Popup Menu

```csharp
var togglePopup = uiManager.NewRibbonTogglePopup()
	.WithDisplayName("Options")
	.WithClientId("cmd.options")
	.AddToRibbonTabPanel()
	.Build();

// Add items to the toggle popup
var item1 = uiManager.NewToggleItem()
	.WithDisplayName("Option 1")
	.OnExecuted(item => {
		// Handle option 1
	})
	.Build();

var item2 = uiManager.NewToggleItem()
	.WithDisplayName("Option 2")
	.OnExecuted(item => {
		// Handle option 2
	})
	.Build();
```

## Project Structure

```
InventorUITools/
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
├── Properties/
│   ├── Resources.resx
│   └── Resources.Designer.cs
├── Resources/
│   └── images.png
├── IUserInterfaceManager.cs
├── UIManager.cs
└── InventorUITools.csproj
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

- **Properties**: Generated resource files and designers

- **Resources**: Image and icon assets for UI controls

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
