# Advanced Inventory & Crafting System

[![Unity CI - Automated Tests](https://github.com/philippeander/Unity_inventory_and_crafting_system_sdd/actions/workflows/unity-tests.yml/badge.svg)](https://github.com/philippeander/Unity_inventory_and_crafting_system_sdd/actions/workflows/unity-tests.yml)
[![Unity CD - WebGL Build](https://github.com/philippeander/Unity_inventory_and_crafting_system_sdd/actions/workflows/unity-build.yml/badge.svg)](https://github.com/philippeander/Unity_inventory_and_crafting_system_sdd/actions/workflows/unity-build.yml)
[![Unity 6](https://img.shields.io/badge/Unity-6000.5.9f1-blue.svg?logo=unity)](https://unity.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20Brain--State-emerald.svg)](https://github.com/)
[![SDD & TDD](https://img.shields.io/badge/Methodology-SDD%20%26%20TDD-purple.svg)](https://github.com/)
[![UI](https://img.shields.io/badge/UI-UI%20Toolkit%20%2F%20MVP-blueviolet.svg)](https://docs.unity.com/)

A modular, extensible **Inventory and Crafting System** built for Unity using **Clean Architecture**, **Specification-Driven Development (SDD)**, and **Test-Driven Development (TDD)**.

---

## 🎯 Portfolio Highlights & Design Pillars

This project serves as a reference implementation of enterprise-grade software engineering practices inside Unity game development:

1. **Clean Architecture & Assembly Decoupling:**
   - **`MyGame.Core` (Pure C#):** The domain and business logic have **zero dependencies** on `UnityEngine`, `MonoBehaviour`, or `ScriptableObject`. Business logic runs anywhere (.NET CLI, Unity, Cloud backends) without engine overhead.
   - **`MyGame.Core.Specs` (NUnit Tests):** Pure domain unit test suite designed for sub-second test execution.
   - **`MyGame.UnityView` (Presentation & Bridge):** Unity-specific view layer using **UI Toolkit (UXML/USS)** and the **MVP pattern**. Reads state from Core while Core remains 100% unaware of Unity.

2. **Brain / State Pattern:**
   - **State (`InventoryState`):** Anemic data structures holding state (slots, capacity, item instances). Serializable and snapshot-friendly for save/load or networking.
   - **Brain (`InventoryBrain`):** Central controller and execution engine that applies domain rules, validates constraints, and mutates state deterministically.
   - **Rule-Based Modularity:** Extensible via composition and rule interfaces (e.g., `IInventoryRule`), adhering strictly to the Open/Closed Principle.

3. **Specification-Driven Development (SDD) & TDD:**
   - Features are first specified using **Gherkin syntax (Given-When-Then)** before code is written.
   - Tests are authored in the **Red Phase** against Gherkin criteria before implementing minimal passing code (**Green Phase**), followed by refactoring.

4. **Automated CI/CD with GitHub Actions:**
   - **Continuous Integration (CI):** Automated workflows run EditMode unit tests on every push and pull request via [GameCI](https://game.ci/).
   - **Continuous Deployment (CD):** Builds a deployable WebGL player automatically on pushes to `main` (gated by test passage).

---

## 📐 Architecture Overview

```mermaid
graph TD
    subgraph UnityView Layer ["Unity Presentation Layer (MVP + UI Toolkit)"]
        UV_Boot[GameBootstrapper - Composition Root]
        UV_UI[Inventory UI - UXML / USS]
        UV_View[InventoryView MonoBehaviour]
        UV_Pres[InventoryPresenter Pure C#]
    end

    subgraph Core Layer ["Pure C# Domain Layer"]
        Core_Brain[InventoryBrain]
        Core_State[InventoryState]
        Core_Slot[InventorySlot]
        Core_Item[ItemDefinition]
        Core_DB[ItemDatabase / IItemDatabase]
    end

    subgraph Specs Layer ["Test Suite"]
        Specs_Tests[NUnit Specifications]
    end

    UV_Boot -->|Instantiates & Wires| UV_Pres
    UV_Boot -->|Instantiates| Core_Brain
    UV_View -->|User Events| UV_Pres
    UV_Pres -->|Commands & Queries| Core_Brain
    UV_Pres -->|Updates| UV_View
    Core_Brain -->|Manipulates| Core_State
    Core_Brain -->|Resolves Items| Core_DB
    Core_State --> Core_Slot
    Core_Slot --> Core_Item
    Specs_Tests -->|Validates Specifications| Core_Brain
    Specs_Tests -->|Inspects State| Core_State
```

---

## 📜 Feature Specifications (Gherkin / SDD)

### Feature: Item Stacking Limits

```gherkin
Feature: Item Stacking Limits
  Scenario: Attempting to stack items beyond the maximum limit
    Given the Player has a slot with 98 "Health Potion"
    And the stack limit for potions per slot is 100
    When the player attempts to add 5 "Health Potion" to this inventory
    Then the current slot should contain 100 potions
    And the inventory should create a new slot containing 2 potions (or overflow remainder)
    But if there are no empty slots, the system should return an "Inventory Full" error and reject the remaining potions.
```

---

## 📁 Assembly Structure

| Assembly / Directory | Namespace | Dependencies | Purpose |
|---|---|---|---|
| `Assets/Scripts/Core` | `MyGame.Core` | Pure .NET (`System`, `System.Collections.Generic`, `LINQ`) | Domain logic, rules, brain controller, and state |
| `Assets/Scripts/Specs` | `MyGame.Core.Specs` | `MyGame.Core`, `nunit.framework` | Fast unit test specifications matching Gherkin scenarios |
| `Assets/Scripts/UnityView` | `MyGame.UnityView` | `MyGame.Core`, `UnityEngine`, UI Toolkit (`UIElements`) | MVP View components, Presenter, Bootstrapper, UXML layouts, USS stylesheets |

---

## 🔄 Development Workflow (SDD + TDD)

1. **Specify:** Define domain behavior in Gherkin scenarios (Given-When-Then).
2. **Red Phase:** Write NUnit tests asserting the scenario specifications (fails with `NotImplementedException` or assertion error).
3. **Green Phase:** Write the minimum necessary domain logic in `MyGame.Core` to pass tests.
4. **Refactor:** Clean code, optimize data structures, enforce SOLID principles.
5. **View Integration:** Wire up UI Toolkit UXML/USS and Presenter in `MyGame.UnityView`.
6. **Continuous Integration & Deployment:** Automated tests and WebGL builds on GitHub Actions.

---

## 🚀 CI/CD Pipeline Configuration

- **CI Workflow:** [`.github/workflows/unity-tests.yml`](.github/workflows/unity-tests.yml) — Runs EditMode tests on pull requests & branches.
- **CD Workflow:** [`.github/workflows/unity-build.yml`](.github/workflows/unity-build.yml) — Gated WebGL build triggered on `main` branch pushes & manual dispatch.

### Setting up GitHub Secrets:
To enable automated testing and builds with GameCI on your repository:
1. Navigate to **Settings > Secrets and variables > Actions** in your GitHub repository.
2. Add the following secrets:
   - `UNITY_LICENSE`: (Optional/Recommended for personal license activation via `.ulf` file).
   - `UNITY_EMAIL`: Your Unity account email.
   - `UNITY_PASSWORD`: Your Unity account password.
