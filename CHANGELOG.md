# Changelog
_The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/)._

## [2.0.0] - 2025-08-03
### Added
- Settings now raise OnValueChanged event when value changes
- New DependencyManager static class, utilized by CommonPlugin

### Changed
- InputHandler now uses a single fiber to handle all inputs
- Converted PlayerHandler into a singleton
- Static event bus has been removed in favor of class events

## [1.4.1] - 2023-05-27

### Added
- Player Status Handler so that GrammarPolice and CalloutInterface can sync
- The ability to safely retrieve the current weather
- MathHelperExtensions for some vector math and wrap angle
- Notification shortcuts have been added as well

### Removed
- All new GUI elements have been moved to RawCanvasUI
- The callouts.cs code has been moved to CalloutInterface

## [1.4] - 2023-04-17

### Added
- GUI classes (BaseFrame, Cursor, TextureFrame, TextureSprite)
- New functions in Math class to handle some trig
- New static Player class to interact with aforementioned functions

## [1.3] - 2022-01-31

### Added
- Holdable combos
- Overload for UXMenu constructor

### Changed
- Visibility of IsPressed in GenericCombo

### Fixed
- GetCalloutFromHandler is now safer

## [1.2] - 2022-01-17

### Added
- IsGamePaused
- NoInlining where appropriate

### Changed
- Minor adjustment to GenericFiber

### Fixed
- Fixed INI writing for key/controller

## [1.1.1] - 2022-01-01

### Changed
- Minor tweak to the GetAllSettings method in Configuration class

## [1.1] - 2021-12-31
### Added
- GetCalloutFromHandle retrieves the underyling Callout from an LHandle
- GetCallouts builds a dictionary of callouts using reflection
- UXMenu for dynamically handling long menu item text
- UXMenuItem to make changes to Configuration via in-game menu

### Changed
- Removed underscores from private members in favor of this keyword

## [1.0] - 2021-12-29
- Initial Release
