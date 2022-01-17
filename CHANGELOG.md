# Changelog
_The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/)._

## [1.2.0] - 2022-01-17

### Added
IsGamePaused
NoInlining where appropriate

### Changed
Minor adjustment to GenericFiber

### Fixed
Fixed INI writing for key/controller

## [1.1.1] - 2022-01-01

### Changed
- Minor tweak to the GetAllSettings method in Configuration class

## [1.1.0] - 2021-12-31
### Added
- GetCalloutFromHandle retrieves the underyling Callout from an LHandle
- GetCallouts builds a dictionary of callouts using reflection
- UXMenu for dynamically handling long menu item text
- UXMenuItem to make changes to Configuration via in-game menu

### Changed
- Removed underscores from private members in favor of this keyword

## [1.0.0] - 2021-12-29
- Initial Release
