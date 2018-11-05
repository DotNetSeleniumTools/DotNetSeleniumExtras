# DotNetSeleniumExtras

Nuget: [![NuGet](https://img.shields.io/nuget/dt/DotNetSeleniumExtras.PageObjects.Core.svg)](https://www.nuget.org/packages/DotNetSeleniumExtras.PageObjects.Core/)

Library contains PageFactory and supporting classes.

This fork of [DotNetSeleniumExtras](https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras) has the following changes:
* .NET Core support
* Ability to create custom FindsBy attributes (by inheriting from AbsractFindsByAttribute)
* Simplified FindsBy syntax: `[FindsBy(How.CssSelector, "div.some-class")]`

Besides, some internal changes were made:
* Removed dependencies on buck
* Fixed browser tests

Ideally, if the new maintainer for upstream repository is found, those changed would be pushed there, and this repository won't be needed.
