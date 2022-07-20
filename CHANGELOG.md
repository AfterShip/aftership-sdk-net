# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [6.0.4] - 2022-07-20
### Added
- Add API: POST `estimated-delivery-date/predict-batch`

## [6.0.3] - 2022-05-07
### Added
- Update tracking fields

## [6.0.2] - 2022-02-16
### Added
- Update tracking fields

## [6.0.1] - 2021-09-18
### Added
- Update tracking fields

## [6.0.0] - 2021-06-24
### Added
- Upgrade Framework to .NET5

## [5.0.5] - 2021-04-30
### Added
- Add mark tracking as completed function.

## [5.0.4] - 2021-03-26
### Added
- Add Available For Pickup status tag.
- Checkpoint: add Slug, Location, Subtag Message.
- Tracking: Courier Tracking Link, Subtag, Subtag Message.
### Fixed
- Fix SignedBy field (always empty).
- Fix casting and type conversions exception issue.

## [5.0.3] - 2021-03-26
### Fixed
- Fix SSL exception

## [5.0.0] - 2015-05-11
#### Breaking Changes
- Change the way we deal with dates, so differents Date Culture don't have any issues.
- Change dependencies:
	- NewtonSoft 8.0.1 (latest stable). **breaking change**

## [4.0.11] - 2015-05-11
### Changed
- Rearrange all the code (we wont bump the version cause is the same). 
- Change dependecies:
	- Delete dependencies Microsoft added automatically not needed.
- Delete a line printing the request to the Aftership server.

## [4.0.9] - 2015-05-11
### Changed
- Change dependencies:
	- NewtonSoft 6.0.8 (latest stable).
	- Change the test framework to Microsoft unitesting (instead of use Nunit).
	- Delete dependencies, now only need Newtonsoft.Json and System.

## [4.0.8] - 2015-01-05
### Added
- Dependencies:
	-  Newtonsoft.Json 3.5.8
	-  System.Web
	-  System.Net
	-  System

[6.0.4]: https://github.com/AfterShip/aftership-sdk-net/compare/6.0.3...6.0.4
[6.0.3]: https://github.com/AfterShip/aftership-sdk-net/compare/6.0.2...6.0.3
[6.0.2]: https://github.com/AfterShip/aftership-sdk-net/compare/6.0.1...6.0.2
[6.0.1]: https://github.com/AfterShip/aftership-sdk-net/compare/6.0.0...6.0.1
[6.0.0]: https://github.com/AfterShip/aftership-sdk-net/compare/5.0.5...6.0.0
[5.0.5]: https://github.com/AfterShip/aftership-sdk-net/compare/5.0.4...5.0.5
[5.0.4]: https://github.com/AfterShip/aftership-sdk-net/compare/5.0.3...5.0.4
[5.0.3]: https://github.com/AfterShip/aftership-sdk-net/compare/5.0.0...5.0.3
[5.0.0]: https://github.com/AfterShip/aftership-sdk-net/compare/4.0.11...5.0.0
[4.0.11]: https://github.com/AfterShip/aftership-sdk-net/compare/4.0.9...4.0.11
[4.0.9]: https://github.com/AfterShip/aftership-sdk-net/compare/4.0.8...4.0.9
[4.0.8]: https://github.com/AfterShip/aftership-sdk-net/releases/tag/4.0.8