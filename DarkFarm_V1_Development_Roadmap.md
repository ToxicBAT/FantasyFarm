# DarkFarm — Version 1.0 Development Roadmap

**Project documentation**  
**Engine:** Unity 6.6  
**Platform:** Android-first  
**Development model:** Solo development  
**Document status:** Roadmap and working specification

---

## 1. Version 1.0 vision

DarkFarm is a top-down, grid-based farming simulator combining cozy farming with a dark-fantasy mystery. The first release should deliver a small, complete, polished game—not an incomplete version of every long-term idea. Advanced features can arrive through updates after release.

**Core player experience:** arrive at a neglected farm, restore its fields, grow and sell crops, improve the property, and gradually uncover clues about the previous owner's disappearance.

The core gameplay loop is:

**Prepare land → plant seeds → care for crops → harvest → sell → earn money → expand the farm → discover the mystery.**

### Design principles

- **Grid-based world:** objects and player-built structures align with a fixed world grid.
- **Farming first:** farming and economy must be fun before advanced supernatural systems are added.
- **Android-first:** touch controls, readable UI, efficient rendering, and stable mobile performance.
- **One system at a time:** implement, test, and stabilize each milestone before moving forward.
- **Expand later:** build foundations that support future content without prematurely implementing deferred features.

## 2. Confirmed architecture decisions

| Area | Decision |
|---|---|
| World | Fixed grid with initially inaccessible land that can be unlocked |
| Placement | All construct placement requires validation |
| Predefined objects | Immovable by default; can be interacted with |
| Build Mode | Separate mode for placing and rearranging player-built constructs |
| Normal Mode | Movement, farming, harvesting, and facility interactions |
| Occupancy | Hybrid rules: terrain and occupancy are separate; each construct defines permitted coexistence |
| Interaction | Contextual action based on the object or cell in front of the player |
| Progression | Start with a small farm; unlock land and upgrades over time |
| Release strategy | Small, polished Version 1.0 followed by updates |

### Proposed architecture

These are logical responsibilities, not a requirement to create exactly four scripts.

1. **World Grid**
   - Coordinates, snapping, land access, occupancy, and placement validation.
2. **World Objects**
   - Trees, rocks, paths, buildings, crop plots, and facilities.
3. **Interaction System**
   - Contextual actions and target selection.
4. **Object-specific gameplay systems**
   - Crop growth, construction, storage, production, and other specialized behavior.

The existing prototype should be migrated gradually rather than discarded.

## 3. Version 1.0 feature scope

### Required for Version 1.0

- Player movement, camera, touch controls, and contextual interactions.
- Fixed grid, land boundaries, object footprints, and placement validation.
- Build Mode for placing and rearranging player-built objects.
- Till, plant, water, grow, and harvest.
- Multiple crops with distinct growth times and selling values.
- Seed inventory, produce inventory, currency, shop, and selling.
- Land unlocking and a small number of useful upgrades.
- Opening narrative, torn diary, and an initial mystery to investigate.
- Save/load, title screen, pause/settings, and a short tutorial.
- Sound, visual polish, Android testing, and release preparation.

### Optional, only after the core is stable

- A small number of decorative objects.
- One or two additional farm upgrades.
- Simple weather or a limited ambient event.
- A small set of optional diary clues.
- Additional crops that reuse the existing farming system.

### Deferred to future updates

- Crop mutations and mutated-crop markets.
- Supernatural merchant demands and escalating raids.
- Automated towers, turrets, and advanced defense.
- Advanced research, laboratories, and complex production chains.
- Hired helpers, multiplayer, and combat-focused progression.
- Large-scale world expansion and complex narrative branches.

## 4. Development roadmap

The order matters more than the calendar. Each phase has a testable completion condition. Do not move to the next phase until the current one works reliably.

**Planning estimate:** approximately 7–13 weeks for the listed phases, depending on debugging, content creation, and device testing. This is an estimate, not a guaranteed deadline. Store review time is additional.

### Phase 1 — World grid and construct architecture

**Estimated duration:** 1–2 weeks  
**Status:** Next milestone

**Goal:** establish the world model that future farming, buildings, and land expansion will use.

Tasks:

- Refactor `FarmGrid` into a grid manager responsible for coordinates, snapping, bounds, land access, and occupancy.
- Separate terrain information from objects occupying cells.
- Define a shared footprint and placement-validation model for constructs.
- Represent predefined objects as fixed, interactable objects.
- Introduce player-built constructs, beginning with crop plots.
- Add a basic Build Mode with placement preview, confirmation, and cancellation.
- Validate the full footprint, unlocked land, terrain compatibility, and occupancy before placement.
- Keep Normal Mode and Build Mode separate without pausing world simulation.

**Completion test:** place, cancel, and rearrange a crop plot; reject placements that overlap incompatible objects, cross boundaries, or use locked land. Confirm existing player movement and contextual interaction still work.

### Phase 2 — Complete the farming loop

**Estimated duration:** 1–2 weeks

**Goal:** make farming a complete, repeatable gameplay system.

Tasks:

- Move soil and crop state into the appropriate crop-plot or planting-position model.
- Implement seed ownership and consume one seed when planting.
- Implement watering and its effect on harvest yield.
- Support crop growth over time and harvest readiness.
- Add multiple crop definitions with different growth durations and values.
- Track harvest output and add produce to inventory.
- Prevent invalid actions, such as planting without seeds or harvesting immature crops.
- Show useful feedback when an action succeeds or fails.

**Completion test:** plant using owned seeds, water crops, wait for growth, harvest, and verify inventory and yield. Repeat for each initial crop. Confirm growth behaves correctly when the player moves away or changes scenes, and define the intended behavior when the game is closed.

### Phase 3 — Inventory, shop, and progression

**Estimated duration:** 1–2 weeks

**Goal:** give farming a meaningful reward and spending loop.

Tasks:

- Add currency and a clear display for money, seeds, and produce.
- Build a simple seed shop.
- Implement buying seeds and selling harvested crops.
- Validate purchases against available currency and inventory limits, if limits are used.
- Add land unlocking with clear costs and feedback.
- Introduce a small number of farm upgrades.
- Tune initial crop costs, sale prices, and growth times.

**Completion test:** earn money from harvesting, buy more seeds, reinvest in farming, and unlock at least one new area. Ensure currency and inventory never become negative through normal gameplay.

### Phase 4 — Save/load, menus, and onboarding

**Estimated duration:** 1–2 weeks

**Goal:** make the game usable as a real mobile game, not just a test scene.

Tasks:

- Add a title screen and new-game/continue flow.
- Save currency, inventory, crop state, crop growth progress, placed constructs, unlocked land, and upgrades.
- Save enough world-state data to reconstruct objects and their grid occupancy reliably.
- Handle save creation, loading, missing data, and corrupted-save recovery gracefully.
- Add pause/settings controls and a short interactive tutorial.
- Ensure game time and growth behave according to the chosen offline-progression rules.

**Completion test:** save, close the game, relaunch, load, and confirm the farm is restored correctly. Test saving during different crop stages and after moving or unlocking objects.

### Phase 5 — Farm design, atmosphere, and opening mystery

**Estimated duration:** 1–3 weeks

**Goal:** give players a reason to care about their farm.

Tasks:

- Design a small starting farm with a few predefined obstacles and interactive objects.
- Add the first land expansion areas.
- Establish the dark-fantasy palette with readable, cozy farming visuals.
- Add a restrained day/night cycle and ambient audio.
- Write the opening story: the previous owner is missing, and a torn diary begins the investigation.
- Place a small number of discoverable clues.
- Use short, readable dialogue and tutorials suited to mobile play.

**Completion test:** a new player can understand the opening, learn farming, earn money, improve the farm, and discover at least one meaningful clue without developer guidance.

### Phase 6 — Polish, testing, and publishing

**Estimated duration:** 1–2 weeks, plus store review time

**Goal:** make Version 1.0 stable, understandable, and publishable.

Tasks:

- Test on real Android hardware.
- Check touch targets, text readability, screen aspect ratios, and performance.
- Fix crashes, broken saves, placement bugs, and farming exploits.
- Improve feedback for placement, watering, harvests, purchases, and locked land.
- Add basic sound effects, visual effects, and loading feedback.
- Check app icon, screenshots, store description, privacy disclosures, and required Android build settings.
- Prepare a signed release build and follow the selected store's current submission requirements.
- Keep a bug list and avoid adding new systems during final stabilization.

**Completion test:** a fresh install can start, play, save, close, resume, and complete the core loop without a known critical bug.

## 5. World grid and construct specification

This section is the architectural contract for the early phases. It is intended to prevent costly refactoring later.

### Grid responsibilities

The world grid should manage:

- World-to-grid and grid-to-world conversion.
- Grid bounds and fixed world dimensions.
- Whether each cell is unlocked or inaccessible.
- Terrain type and terrain compatibility.
- Object occupancy and object footprints.
- Validation of proposed placement and removal.
- Queries for the object or construct occupying a cell.

### Construct responsibilities

Every construct should define the data needed to validate and place it.

| Property | Purpose |
|---|---|
| Footprint | Grid cells occupied by the construct |
| Rotation support | Whether the construct can rotate and how its footprint changes |
| Allowed terrain | Terrain types where placement is permitted |
| Coexistence rules | Object categories that may share cells or space |
| Interaction behavior | Actions available to the player |
| Ownership/type | Distinguishes predefined objects from player-built constructs |

These are proposed data requirements; exact C# classes and interfaces should be decided during Phase 1.

### Placement validation sequence

1. Snap the proposed position and rotation to the grid.
2. Calculate the complete footprint at that rotation.
3. Reject placement if any footprint cell is outside the grid or on locked land.
4. Check terrain requirements and compatibility with existing occupancy.
5. Show a valid or invalid placement preview.
6. Revalidate when the player confirms placement, then commit the placement atomically.

Validation must happen again on confirmation because the world may change while Build Mode is active. A visual preview alone is not sufficient.

### Keep the existing prototype

Do not discard working code just to match the new architecture. Preserve movement, camera follow, virtual joystick, contextual action button, and inventory UI wherever possible. Migrate the grid and farming state in small steps, running regression tests after each change.

## 6. Version 1.0 content limits

To prevent scope creep, start with a deliberately small content budget.

| Content | Initial target |
|---|---|
| Farm | One designed starting farm |
| Crops | 3–5 distinct crop types |
| Placeable objects | Crop plots plus a small selection of useful structures or decorations |
| Expansion | A few unlockable land sections |
| Upgrades | 2–4 useful upgrades |
| Shop | One straightforward seed-buying and crop-selling interface |
| Mystery | Opening narrative, torn diary, and a few clues |
| Gameplay loop | Farming, earning, spending, expansion, and investigation |

These are planning targets, not finalized design decisions. Expand the list only if the core game is stable and the extra content does not delay release.

## 7. Save-data and technical requirements

A save file should store game state rather than rely on the current scene hierarchy to reconstruct the farm.

At minimum, plan to persist:

- Save format version.
- Player position and relevant progression.
- Currency and item inventories.
- Grid and land-unlock state.
- IDs and grid coordinates of player-built constructs, including rotation and configuration.
- Predefined-object states if any can change.
- Crop-plot state, planted crop type, growth progress, and watering status.
- Upgrades and story progression.
- A time reference if offline growth or other offline systems are enabled.

Use stable object identifiers and grid coordinates instead of saving references to temporary Unity GameObjects. Include a save-version field so the format can evolve as the game receives updates.

**Important design decision still open:** how crop growth progresses while the game is closed. Decide this before save/load is considered complete. A simple elapsed-time model is likely easier to maintain than a complicated simulation.

## 8. Quality gates before launch

### Core gameplay

- [ ] Player movement and camera work on target Android devices.
- [ ] All farming actions enforce the correct state and inventory requirements.
- [ ] Build Mode rejects invalid placement and preserves occupancy consistency.
- [ ] Shop, currency, harvesting, and inventory stay synchronized.

### Persistence

- [ ] Save/load restores the farm and progression accurately.
- [ ] Interrupted or invalid saves fail safely.
- [ ] Crop growth after relaunch follows the intended time rules.

### Mobile quality

- [ ] Touch controls and UI work across target screen sizes.
- [ ] No known critical crashes or save-loss bugs.
- [ ] Performance is acceptable on the chosen minimum test device.
- [ ] Text, icons, and placement previews remain readable.

### Release readiness

- [ ] Opening tutorial explains the core loop.
- [ ] The player can progress without developer-only shortcuts.
- [ ] Store assets and required privacy disclosures are prepared.
- [ ] A release build has been installed and tested outside the Unity Editor.

## 9. Development management

Use one milestone at a time. Each milestone should have a written objective, a small implementation task, a test procedure, and a clear pass/fail result.

- Keep a list of confirmed decisions, open questions, and deferred ideas.
- Avoid rewriting working systems without a specific architectural reason.
- Don't add advanced features simply because they fit the story.
- Test on Android regularly rather than waiting until the end.
- Fix bugs in the core loop before adding dependent content.
- Reserve the final portion of the schedule for testing and stabilization.

## 10. Immediate next step

**Start Phase 1: world grid and construct architecture.** Before implementing the shop or more crop mechanics, define the minimum grid data model and migrate the existing 8×8 prototype without breaking movement or contextual interaction.

The first implementation task should be limited to **separating grid-cell data from crop-plot gameplay state**. Once that separation is tested, introduce construct footprints and placement validation. This keeps the work incremental and protects systems that have already been verified.
