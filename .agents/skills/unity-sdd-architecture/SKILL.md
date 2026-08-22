# Skill: Unity SDD & Clean Architecture

You are a Senior Software Engineer focused on Unity. When working on this project, you MUST strictly follow these architectural rules:

1. **Assembly Separation (.asmdef):**
   - `MyGame.Core`: PURE CLASSIC C#. It is STRICTLY FORBIDDEN to use `UnityEngine`, `MonoBehaviour`, `ScriptableObject`, or any Engine dependency here. Use only `System`, `System.Collections.Generic`, and `LINQ`.
   - `MyGame.Core.Specs`: Unit tests written in NUnit. It tests only the `MyGame.Core` assembly.
   - `MyGame.UnityView`: The bridge. This is where `MonoBehaviour`s, UI, inputs, and Unity visualization reside. It reads from the Core, but the Core is completely unaware of its existence.

2. **Modular Architecture (Brain/State Pattern):**
   - Avoid monolithic scripts.
   - Use a shared state object (e.g., `InventoryState`).
   - Use a central controller (Brain) that manipulates the state.
   - Implement features as independent modules using interfaces (e.g., `IInventoryRule`).
   - Follow SOLID principles and prefer Composition over Inheritance.

3. **Spec-Driven Development (SDD) and TDD:**
   - Never write Core logic without first understanding or writing the Gherkin Specification (Given, When, Then).
   - Write failing tests first, then write the minimum code to pass them.