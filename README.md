🗓️ Last Updated
July 28, 2025

# 🧪 Selenium Reqnroll Framework (C#)

A modular, thread-safe Selenium automation framework built with Reqnroll (C#), designed for parallel execution, dynamic reporting, and high-performance diagnostics.

<div style="border: 1px solid #f5c518; background-color: #fff8dc; padding: 10px; border-radius: 5px;">
  ⚠️ <strong>Currently under active development</strong> — expect frequent changes and enhancements.
</div>

---

## 📂 Framework Structure

```text
├── SeleniumFrameworkBase   # Core thread-safe framework
├── TestProject             # Sample feature files to demo usage and parallel behaviors
```


## ✨ Current Features

- 🔄 **Parallel Execution Support**
  - Scenario-wise and feature-wise parallel scopes
  - Using isolated `FrameContext` per test thread for concurrency safety

- 🔍 **Custom Context Manager**
  - FrameContext stores WebDriver, logger, scenario/feature info, and reporting objects per thread

- 🌐 **BrowserFactory**
  - Supports Local, Selenium Grid, and Cloud environments (configurable)

- 📊 **Reporting**
  - Generates execution results using Extent Reports
  - Compressed screenshot embedding to keep reports lightweight and CI-friendly

- 🗂 **Modular Architecture**
  - Hook lifecycle, page classes, config properties, and reporting logic are split cleanly across layers

  - CI/CD compatible and built for scalable artifact generation

---

## 👤 Author
--   [Bala](https://github.com/Bala-murugan-tr) 
## 📌 License

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

This project is licensed under the [MIT License](LICENSE) — feel free to use, modify, and distribute with proper attribution.





