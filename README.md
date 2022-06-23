[![openupm](https://img.shields.io/npm/v/net.whg.awgen-core?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/net.whg.awgen-core/)
![Tests](https://github.com/whg/awgen-core/workflows/Tests/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

# Awgen Core

The core Unity package for the Awgen engine. Provides the overal logic and data structures that are used heavily in extending packages.

- [How to use](#how-to-use)
- [Install](#install)
  - [via npm](#via-npm)
  - [via OpenUPM](#via-openupm)
  - [via Git URL](#via-git-url)
  - [Tests](#tests)
- [Configuration](#configuration)

<!-- toc -->

## How to use

*Work In Progress*

## Install

### via OpenUPM

The package is also available on the [openupm registry](https://openupm.com/packages/com.whg.awgen-core). You can install it eg. via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.whg.awgen-core
```

### via Git URL

Open `Packages/manifest.json` with your favorite text editor. Add following line to the dependencies block:
```json
{
  "dependencies": {
    "net.whg.awgen-core": "https://github.com/TheDudeFromCI/AwgenCore.git"
  }
}
```

### Tests

The package can optionally be set as *testable*.
In practice this means that tests in the package will be visible in the [Unity Test Runner](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

Open `Packages/manifest.json` with your favorite text editor. Add following line **after** the dependencies block:
```json
{
  "dependencies": {
  },
  "testables": [ "net.whg.awgen-core" ]
}
```

## Configuration

*Work In Progress*

## License

MIT License

Copyright © 2022 Wraithaven Games
