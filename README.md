<!--[![Build status](https://dev.azure.com/palmund/Typesafe.With/_apis/build/status/Typesafe.With)](https://dev.azure.com/palmund/Typesafe.With/_build/latest?definitionId=9)--->
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![NuGet version](https://badge.fury.io/nu/AutoMapper.Contrib.ImmutableCollections.svg)](https://www.nuget.org/packages/AutoMapper.Contrib.ImmutableCollections)

# AutoFixture.Contrib.ImmutableCollections

Creating specimens for System.Collections.Immutable.

Supports **.NET Core** (.NET Standard 2+)

## Installation

```
PM> Install-Package AutoFixture.Contrib.ImmutableCollections
```

## Usage
```csharp
var fixture = new Fixture().Customize(new ImmutableCollectionsCustomization());
var immutableList = fixture.Create<ImmutableList<string>>();
```
