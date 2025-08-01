# .github/copilot-instructions.md

## Mod Overview and Purpose

**Mod Name**: Lord of the Rims - Men and Beasts (Continued)

This mod is a continuation of Jecrell's original "Lord of the Rims - Men and Beasts" mod for RimWorld. It serves to update and maintain the mod in an unofficial capacity until such time as Jecrell returns or the original team resumes development. The primary focus of the mod is to introduce the Gondor faction, along with various creatures and equipment from Middle-Earth lore into the RimWorld universe. Note that the mod is not in a finished state and currently covers only the Gondor faction and a selection of beasts and monsters.

## Key Features and Systems

- **Gondor Faction**: Introduces the Gondorian faction with unique backstories and lowered tech level to Medieval, as opposed to the original game's timeline. This reflects the tribal-like history of the race in Middle-Earth's lore.
- **Creatures from Middle-Earth**: Incorporates mythical beasts and monsters to enrich the RimWorld universe with fantasy elements.
- **Equipment and Armor**: Adds new medieval-themed armor and weapons to reflect the era and theme of Middle-Earth.

## Coding Patterns and Conventions

1. **Class Naming**: Class names follow a clear, descriptive naming convention often indicating their specific purpose, like `Building_Beacon`, `Building_BeaconUnlit`, `JobDriver_LightBeacon`, and `WorkGiver_LightBeacon`.

2. **Method Structure**: Methods are usually public and explicitly describe their actions, such as `Light()` in `Building_BeaconUnlit` and `LightBeacon()` in `JobDriver_LightBeacon`.

3. **Inheritance and Modularity**: Facilitate code reusability and structure by extending classes from RimWorld's own classes like `Building_WorkTable` and `JobDriver`.

## XML Integration

- XML files are used to define races, factions, and equipment. These require identification and ID referencing within the C# code to integrate with game mechanics.
- Ensure consistency with XML tags and C# references for flawless mod operation.
- XML is leveraged to modify backstories and tech levels without altering base game files, enabling compatibility and ease of updates.

## Harmony Patching

- Harmony patches can be applied to override or modify base game functions at runtime without altering the original code, ensuring compatibility between mods.
- Focus on targeted patching for specific methods involving the interaction of new factions, creatures, or equipment.
- Maintain clear and specific Harmony patches to support future changes or potential integration with additional content.

## Suggestions for Copilot

1. Generate code templates for adding new creatures or factions based on the existing pattern, ensuring consistent data structure and namespaces.
2. Utilize Copilot to suggest method bodies and structure when extending new feature functionality like additional faction roles or interactions.
3. Employ Copilot to draft Harmony patches, keeping in mind the overriding patterns used, and conform to best practices such as targeting specific methods or adding new functionality.
4. Suggest XML tag attributes and values based on existing entries for new additions, ensuring a coherent structure and naming pattern.

5. Help with writing test cases and debugging outputs to streamline the development and integration process.

This instruction document is a guide to assist contributors and maintainers with continuing development and error-free maintenance of the mod, leveraging Copilot to enhance productivity and code reliability.
