# Football Career Quiz --- Project Context & Development Handoff

> **Purpose:** This document is the handoff context for continuing
> development of Football Career Quiz with GitHub Copilot / GPT-5.6
> Luna.
>
> **Important:** The repository is the source of truth for
> implementation. This document records architectural decisions, UI
> decisions, bugs discovered, intended behavior, and planned work from
> the development conversation. If the repository differs from this
> document, inspect the current code before changing it.

------------------------------------------------------------------------

## 1. Project identity and design direction

**Game:** Football Career Quiz / FootIQ\
**Engine:** Unity\
**Target:** Mobile portrait UI (currently designed/tested at 1080x1920).

The game is a football quiz with a career/progression structure. The
player answers football-player questions, earns coins, manages lives,
progresses through levels, earns stars, unlocks achievements, and can
replay completed levels.

The visual identity established so far:

-   Dark football-stadium background.
-   Dark navy/blue UI panels and cards.
-   Bright green as the primary UI accent.
-   Gold/yellow for stars and coin/reward emphasis.
-   White typography for primary information.
-   Subtle gray text for secondary information.
-   Rounded cards/buttons.
-   Thin green borders/accent lines.
-   Minimal, premium mobile-game aesthetic.
-   Avoid default Unity-looking UI wherever practical.

The UI should feel like one coherent product. Reuse visual conventions
between Levels, Achievements, Profile, Level Complete, etc.

------------------------------------------------------------------------

# 2. Current architecture

The important current systems/classes include:

## Core / managers

-   `GameManager`
-   `MainMenuController`
-   `QuizController`
-   `HintController`
-   `AdManager`

`GameManager` is a persistent singleton and currently owns:

-   `PlayerDatabase`
-   `PlayerProgress`
-   `PlayerStatistics`
-   `PlayerAchievements`
-   `LifeService`
-   `CoinsService`
-   `ProgressionService`
-   `SaveService`
-   `StatisticsService`
-   `AchievementService`
-   `LevelDatabase`

The current initialization order in the inspected code is approximately:

1.  Load save data.
2.  Create `Progress`.
3.  Create `Statistics`.
4.  Create `Achievements`.
5.  Create services using those objects.

------------------------------------------------------------------------

# 3. Data model

## `LevelDefinition`

Current fields:

``` csharp
public int Level;
public int QuestionCount;
public int BaseReward;
public int WrongAnswerPenalty;
public int HintPenalty;
public int FlawlessBonus;
public bool IsBossLevel;
```

This is the ScriptableObject/database-driven definition of a level.

### Important design decision

Level configuration should come from `LevelDatabase` /
`LevelDefinition`, not from hardcoded constants in gameplay code.

The game is intended to support **at least 30 levels**, and the current
planning is around **50 levels**.

Each level can have:

-   custom question count,
-   custom base reward,
-   custom wrong-answer penalty,
-   custom hint penalty,
-   custom flawless bonus,
-   boss-level flag.

------------------------------------------------------------------------

## `LevelProgress`

Current fields in the inspected repository snapshot:

``` csharp
public int Level;
public int BestStars;
public int BestReward;
public int BestCorrectAnswers;
```

This is per-level persistent player progress.

### Important rules

`BestStars` must never decrease.

`BestReward` must never decrease.

`BestCorrectAnswers` should also represent the best result and therefore
must **not decrease on replay**.

Correct logic:

``` csharp
progress.BestCorrectAnswers =
    Mathf.Max(progress.BestCorrectAnswers, result.CorrectAnswers);
```

not:

``` csharp
progress.BestCorrectAnswers = result.CorrectAnswers;
```

------------------------------------------------------------------------

## `LevelInfo`

Used to feed the Levels UI.

Current conceptual fields:

-   `Level`
-   `IsUnlocked`
-   `IsCurrent`
-   `IsBossLevel`
-   `QuestionCount`
-   `BestStars`
-   `BestReward`
-   `BestCorrectAnswers`
-   `IsCompleted`

Current `IsCompleted` implementation:

``` csharp
public bool IsCompleted => BestStars > 0;
```

This is useful for the Levels card UI.

------------------------------------------------------------------------

## `LevelResult`

Contains the result of the current level attempt:

-   Level
-   TotalQuestions
-   CorrectAnswers
-   WrongAnswers
-   HintsUsed
-   BaseReward
-   WrongAnswerPenalty
-   HintPenalty
-   FlawlessBonus
-   FinalReward
-   Stars

`IsFlawless` currently means:

``` csharp
WrongAnswers == 0 && HintsUsed == 0
```

------------------------------------------------------------------------

# 4. Level progression model

The game intentionally has **quiz levels only**. Do not introduce a
separate player-level/RPG-level system.

The player's progression is represented by:

``` text
CurrentLevel
CurrentQuestion
```

and the per-level records in `PlayerProgress.Levels`.

## Main menu behavior

The existing `PLAY` button should take the player directly into the game
at their current level/question.

The `LEVELS` button opens the Levels popup.

The Levels popup lets players replay unlocked/completed levels.

------------------------------------------------------------------------

# 5. Levels screen design

The Levels screen is now considered largely complete visually.

## Card states

Each level card has three main visual states:

### Completed/unlocked

-   Normal dark card.
-   Green accent line.
-   Best stars shown.
-   Chevron shown.
-   Clickable.

Example:

``` text
Level 2              ★ ★ ★ ★ ★    >
5/5 Correct
```

### Current level

-   Green outline/border.
-   Green accent.
-   `CURRENT` badge on the right.
-   No chevron.
-   Not treated as a replay button.
-   Subtitle:
    -   `Ready to Play` if question 0.
    -   Otherwise `Question X/Y`.

Example:

``` text
Level 3                  CURRENT
Ready to Play
```

### Locked

-   Dimmed/disabled card.
-   Gray accent.
-   Lock icon instead of chevron.
-   Not clickable.
-   Subtitle:

``` text
Complete Level N
```

Example:

``` text
Level 5                         🔒
Complete Level 4
```

The lock icon was preferred over the older photographic/large lock icon.
Use the simpler gray lock that matches the final screenshots.

------------------------------------------------------------------------

## Level card interaction

Unlocked non-current cards are clickable.

Clicking one raises:

``` csharp
LevelSelected
```

`MainMenuController` subscribes to this event.

The current implementation does:

``` text
Level card clicked
    ↓
MainMenuController.OnLevelSelected(level)
    ↓
ProgressionService.StartReplay(level)
    ↓
Load Gameplay scene
```

The replay state is stored in `ProgressionService` via `replayLevel`.

Current level should not behave as a replay card.

------------------------------------------------------------------------

# 6. Levels popup layout decisions

The popup uses a `ScrollRect` with dynamically instantiated
`LevelEntryView` prefabs.

Important UI lessons discovered:

## ScrollView sizing

The ScrollView must have a bounded viewport. The content should be
allowed to grow vertically so that the list scrolls inside the viewport
rather than expanding the entire popup.

`Content` uses a `VerticalLayoutGroup` and `ContentSizeFitter`.

## Current working behavior

The vertical card spacing was ultimately set to **0** because
mouse-wheel scrolling only worked when the cursor was over a card if
there were empty gaps between cards.

We tested adding an Image to the Viewport to capture events, but this
caused cards to become hidden in the existing mask setup.

Therefore:

-   Keep the current `RectMask2D` approach.
-   Do not reintroduce a normal `Mask` just to solve scrolling.
-   For now, leave card spacing at `0`.
-   If spacing is revisited later, solve event routing deliberately
    rather than breaking the masking.

## Important rendering bug

A `Maskable` setting on the LevelEntry prefab caused the text/lock
visuals to be hidden until scrolling in the ScrollView.

Disabling the problematic `Maskable` setting fixed the issue.

If this bug reappears:

-   inspect `Maskable` on the instantiated prefab and its child
    graphics,
-   do not assume the layout system is broken,
-   check rendering/masking first.

------------------------------------------------------------------------

# 7. Custom scrollbar

A custom scrollbar was created instead of using Unity's default visual.

Desired style:

-   Very thin.
-   Subtle.
-   Rounded handle.
-   No arrows.
-   White/light gray handle.
-   Track is extremely subtle or can effectively be omitted.
-   Positioned slightly inside the popup edge.
-   It should not compete visually with the level cards.

The custom scrollbar is intended to be **reused for Achievements and
other future scrollable screens**.

Conceptual hierarchy:

``` text
Scrollbar Vertical
├── Background / Track (optional/subtle)
└── Sliding Area
    └── Handle
```

Custom assets created:

-   `Scrollbar_Handle`
-   `Scrollbar_Track`

The current visual preference is to keep the track very subtle or not
visually prominent.

------------------------------------------------------------------------

# 8. Levels popup auto-scroll

The Levels popup should automatically scroll to the current level when
opened.

This was implemented/planned by:

1.  Holding a `ScrollRect` reference.
2.  Creating all entries.
3.  Rebuilding the layout.
4.  Finding the entry whose `LevelInfo.IsCurrent` is true.
5.  Setting the ScrollRect position so the current level is visible.

Preferred UX:

-   Current level should appear near the center of the visible list when
    possible.
-   Do not scroll beyond the top/bottom limits.
-   A future polish improvement is a \~0.3 second smooth scroll
    animation instead of an instant jump.

------------------------------------------------------------------------

# 9. Level database / balancing philosophy

The game should have a defined set of levels rather than an
infinite/hardcoded 5-question loop.

Target planning:

-   At least 30 levels.
-   Current design planning is around **50 levels**.
-   Each level has custom values.

Reward balancing decision:

> Base reward, wrong-answer penalty, hint penalty, and flawless bonus
> should remain relatively stable and increase gradually rather than
> exploding every level.

More significant increases should occur on:

``` text
5
10
15
20
25
30
35
40
45
50
```

These are intended to be more difficult/boss levels.

The exact level values are stored in the LevelDatabase/ScriptableObject
and should be treated as configuration, not re-created with hardcoded
constants.

------------------------------------------------------------------------

# 10. Reward economy --- critical rule

This is one of the most important gameplay decisions.

Players must **not** be able to farm easy levels indefinitely.

Example:

``` text
First Level 5 attempt:
FinalReward = 100
Player receives +100
BestReward = 100
```

Replay:

``` text
FinalReward = 97
Player receives +0
BestReward remains 100
```

Replay again:

``` text
FinalReward = 125
Player receives +25
BestReward becomes 125
```

Therefore:

``` text
CoinsAwarded =
max(0, NewFinalReward - PreviousBestReward)
```

The player receives only the improvement over the previous best reward.

This rule applies to both normal completion and replay completion.

## Important

Never do this for a replay:

``` csharp
GrantCoins(result.FinalReward);
```

That would reintroduce coin farming.

The reward shown on the Level Complete screen may be the **attempt's
calculated reward**, while the actual coin award must be the **new
reward difference**.

------------------------------------------------------------------------

# 11. Replay statistics rules

Statistics must represent the player's career/progression, not repeated
attempts.

Do **not** increment career metrics blindly on every replay.

For example, if a level previously had:

``` text
BestStars = 3
```

and the replay gets:

``` text
5 stars
```

then:

``` text
StarsEarned += 2
```

not:

``` text
StarsEarned += 5
```

If a replay gets 4 stars after the player already has 5:

``` text
StarsEarned += 0
```

Similarly:

## Perfect levels

Only increment `PerfectLevelsCompleted` when the player moves from:

``` text
BestStars < 5
```

to:

``` text
new result = 5
```

Replaying an already-perfect level must not increment it again.

## Boss levels

`BossLevelsCompleted` should increment only on first completion of a
boss level.

Replaying a boss level must not count it twice.

## Levels completed

The Profile screen intentionally does **not** display `LevelsCompleted`.

Reason:

The game's progression is linear:

``` text
Complete Level 1 → Current Level 2
Complete Level 2 → Current Level 3
...
```

Therefore `Levels Completed` is effectively redundant with
`Current Level` for the player's main progression view.

It can remain in data if useful for achievements/analytics, but don't
display it unless there is a compelling reason.

------------------------------------------------------------------------

# 12. PlayerStatistics

Current inspected repository snapshot had:

``` csharp
public int QuestionsAnswered { get; private set; }
public int CorrectAnswers { get; private set; }
public int WrongAnswers { get; private set; }
public int HintsUsed { get; private set; }
public int CurrentStreak { get; private set; }
public int LongestStreak { get; private set; }

public float AccuracyPercentage =>
    QuestionsAnswered == 0
        ? 0
        : CorrectAnswers * 100f / QuestionsAnswered;
```

During the UI work, additional lifetime properties were added/planned:

-   `CoinsEarned`
-   `StarsEarned`
-   `PerfectLevelsCompleted`
-   `BossLevelsCompleted`
-   potentially `LevelsCompleted` as internal data, but not needed for
    the Profile UI.

These must be persisted through `PlayerStatisticsSaveData`.

### Important distinction

`CoinsEarned` means lifetime coins actually awarded to the player.

It must increase by:

``` text
actual coins awarded
```

not by the theoretical reward of every replay.

`Available Coins` is `PlayerProgress.Coins` and is different.

------------------------------------------------------------------------

# 13. Statistics architecture

There is already a `StatisticsService` which wraps `PlayerStatistics`
and triggers achievement checks.

Current responsibilities:

``` text
StatisticsService
    RecordCorrectAnswer()
    RecordWrongAnswer()
    UseHint()
```

This is a good pattern.

Keep UI out of the statistics logic.

A clean future direction is:

``` text
ProgressionService
    decides what changed
        ↓
PlayerStatistics
    records the career delta
```

A possible future abstraction discussed was a progression/statistics
delta object containing things such as:

-   first completion,
-   new stars,
-   coins awarded,
-   perfect completion newly achieved,
-   boss completion newly achieved.

This is not mandatory yet, but is a useful direction if replay logic
becomes complicated.

------------------------------------------------------------------------

# 14. Profile screen --- current design

The Profile UI has been built as a clean list of `StatRowView`s.

Current visual result was considered very strong and should not be
redesigned unnecessarily.

The current design:

``` text
PROFILE

Current Level              2/50
Stars Earned               0/250
Boss Levels Cleared        0/10

--------------------------------

Available Coins             50
Coins Earned                 0

--------------------------------

Correct Answers             4/5
Answer Accuracy             80%
Longest Streak               3
Perfect Levels            0 (0%)

[ Close ]
```

The exact live code has been updated in the editor beyond the older
uploaded code snapshot, so inspect the current repository before
changing fields.

## Profile ordering decision

Use this order:

### Progress

1.  Current Level
2.  Stars Earned
3.  Boss Levels Cleared

### Economy

4.  Available Coins
5.  Coins Earned

### Performance

6.  Correct Answers
7.  Accuracy
8.  Longest Streak
9.  Perfect Levels

This grouping is intentional.

## Do not add

-   A career progress bar. The user explicitly decided against it.
-   A `Levels Completed` row. It is redundant with Current Level in this
    linear progression system.

## Wording

Preferred:

-   `Current Level`
-   `Stars Earned`
-   `Boss Levels Cleared`
-   `Available Coins`
-   `Coins Earned` or `Total Coins Earned`
-   `Correct Answers`
-   `Accuracy` (shorter than `Answer Accuracy`)
-   `Longest Streak`
-   `Perfect Levels`

`Correct Answers` is best displayed as:

``` text
196/223
```

rather than only `196`, because the context is immediately visible.

`Current Level` should be shown as:

``` text
2/50
```

or:

``` text
Level 2 / 50
```

Either is acceptable; the latter is slightly clearer.

------------------------------------------------------------------------

# 15. Profile UI color directives

Do not color every value.

Keep labels white/neutral.

Use color selectively on values:

-   Progress values → game green accent.
-   Stars → gold/yellow.
-   Coin values → gold/yellow.
-   Performance percentages → white/neutral or subtle accent.
-   Locked/disabled information → gray.

The purpose is to make important numbers pop without turning the panel
into a rainbow.

------------------------------------------------------------------------

# 16. Profile UI spacing/directives

Current design principles:

-   No unnecessary section headings if dividers already provide enough
    grouping.
-   Thin white dividers are acceptable between sections.
-   Keep the dark navy panel and subtle topographic pattern.
-   Green vertical accent lines on rows.
-   Rounded dark cards.
-   Close button at bottom.
-   Avoid overloading the screen with extra explanatory text.

The current Profile screen was judged to be one of the strongest UI
screens so far. Do not redesign it merely for the sake of redesigning
it.

------------------------------------------------------------------------

# 17. Level Complete screen architecture

The Level Complete panel was deliberately split into separate child
views owned by `LevelCompleteView`.

Hierarchy/concept:

``` text
LevelCompleteView
├── Title
├── Stars
├── Score / feedback
├── RewardRows / RewardBreakdownView
├── FinalReward / RewardView
└── ContinueButton
```

`LevelCompleteView` owns:

-   `RewardBreakdownView`
-   `StarRatingView`
-   `RewardView`

This separation should be preserved.

------------------------------------------------------------------------

# 18. Level Complete animation sequence

The final desired sequence is:

``` text
Open Level Complete panel
    ↓
Reward breakdown rows appear one by one
    ↓
Stars animate in one by one
    ↓
Final reward appears
    ↓
Reward number counts upward
    ↓
Reward celebration/pulse
    ↓
Continue button appears/enables
```

The reward breakdown rows were previously failing to animate because
they were not being controlled correctly; the final architecture fixed
this by having `RewardBreakdownView` own the row activation/animation.

Current `RewardBreakdownView.Play()` behavior:

-   Hide all rows.
-   Show base reward.
-   Show wrong-answer penalty if applicable.
-   Show hint penalty if applicable.
-   Show flawless bonus if applicable.
-   Each row fades/scales in.

------------------------------------------------------------------------

# 19. Level Complete reward colors

These colors were explicitly chosen:

### Base reward

Green:

``` text
RGB ≈ 124, 210, 61
```

### Wrong-answer penalty

Red:

``` text
RGB ≈ 230, 80, 80
```

### Hint penalty

Orange:

``` text
RGB ≈ 255, 176, 46
```

### Flawless bonus

Gold:

``` text
RGB ≈ 255, 215, 70
```

Keep this visual language consistent elsewhere.

------------------------------------------------------------------------

# 20. Final reward celebration

`RewardView` already has the intended animation pattern:

1.  Start reward text at `0`.
2.  Count up smoothly to the reward.
3.  Pulse the reward container.
4.  Slightly rotate/wiggle the coin icon.
5.  Return everything to its original transform.

The reward celebration is intentionally small and satisfying rather than
a giant animation.

Future enhancement:

-   Add particles/confetti for exceptional rewards or 5-star
    completions.
-   Consider a stronger version for boss-level completion.
-   Do not make every reward animation excessively loud.

------------------------------------------------------------------------

# 21. Star animation

`StarRatingView` animates stars individually.

Current intended behavior:

-   All stars start inactive and scale 0.
-   Each earned star appears sequentially.
-   It overshoots slightly (around 1.2 scale).
-   It settles back to 1.
-   Short delay between stars.

This should remain the standard star presentation.

------------------------------------------------------------------------

# 22. Continue button behavior

The Continue button on Level Complete should:

-   start hidden/disabled,
-   remain unavailable while the animation sequence is running,
-   become visible and interactable only after the reward sequence
    finishes.

This fixed a previous issue where Continue was not behaving correctly.

------------------------------------------------------------------------

# 23. Achievements

Achievements already exist as a separate system:

-   `AchievementService`
-   `PlayerAchievements`
-   `AchievementDefinition`
-   `AchievementsView`
-   `AchievementCardView`
-   `AchievementPopupView`

`StatisticsService` triggers achievement checks after relevant
statistics changes.

Achievement unlocks award coins and raise an `AchievementUnlocked`
event.

### Important future correction

The current inspected `AchievementService` has an
`AchievementType.CoinsEarned` path that uses `PlayerProgress.Coins`.

That is conceptually wrong if the achievement means **lifetime coins
earned**.

Once `PlayerStatistics.CoinsEarned` exists, `CoinsEarned` achievements
should use that lifetime value, not current spendable balance.

This should be fixed before relying heavily on coin-based achievements.

------------------------------------------------------------------------

# 24. Main menu

Current main menu has:

``` text
PLAY
LEVELS
PROFILE
ACHIEVEMENTS
```

`PLAY`:

-   goes directly to Gameplay.
-   uses current progression.

`LEVELS`:

-   opens the Levels popup.

`PROFILE`:

-   opens ProfileView with `PlayerProgress` + `PlayerStatistics`.

`ACHIEVEMENTS`:

-   opens AchievementsView.

`MainMenuController` subscribes to:

``` csharp
levelsPopup.LevelSelected += OnLevelSelected;
```

This is an accepted design decision.

------------------------------------------------------------------------

# 25. Save system

Current persistence uses `PlayerPrefs` with JSON.

Relevant classes:

-   `SaveData`
-   `PlayerProgressSaveData`
-   `PlayerStatisticsSaveData`
-   `PlayerAchievementsSaveData`
-   `SaveService`

When new `PlayerStatistics` properties are added, update:

1.  `PlayerStatistics.ToSaveData()`
2.  `PlayerStatisticsSaveData`
3.  `SaveService.Load()` / `PlayerStatistics.Restore(...)`

Do not add a new persistence mechanism just for the Profile.

------------------------------------------------------------------------

# 26. Important save initialization bug already encountered

When the save file was deleted, there was a `NullReferenceException` in
`LevelRewardCalculator.Calculate`.

The root cause investigation included the fact that `CurrentLevel` must
be valid on a fresh save.

Current `PlayerProgress.Restore()` uses:

``` csharp
int currentLevel = Mathf.Max(1, saveData.CurrentLevel);
```

and assigns that to `CurrentLevel`.

Keep fresh-save defaults robust.

When adding new saved fields, make sure old/missing save data produces
valid defaults rather than null references.

------------------------------------------------------------------------

# 27. Current `ProgressionService` state

Current responsibilities include:

-   determining active level,
-   question progression,
-   replay state,
-   calculating current level result,
-   generating `LevelInfo` for Levels screen,
-   saving per-level best progress.

Current important properties:

``` csharp
CurrentLevelDefinition
CurrentQuestion
QuestionsInCurrentLevel
IsReplay
ActiveLevel
HighestUnlockedLevel
```

The level database is now driving question counts/reward definitions
through `LevelDefinition`.

Do not reintroduce hardcoded:

``` text
5 questions per level
100 base reward
10 wrong penalty
etc.
```

------------------------------------------------------------------------

# 28. Current reward calculator

`LevelRewardCalculator` receives a `LevelDefinition`.

It calculates:

``` text
wrongPenalty
hintPenalty
flawlessBonus
finalReward
stars
```

It has a minimum reward of 25.

Stars currently use:

``` text
0 wrong + 0 hints → 5 stars
0 wrong + >=1 hint → 4 stars
<=1 wrong → 3 stars
<=2 wrong → 2 stars
otherwise → 1 star
```

Keep this as the current star rule unless intentionally redesigning
difficulty.

------------------------------------------------------------------------

# 29. Critical progression bug to fix next

The current inspected `QuizController` still does:

``` csharp
GameManager.Instance.CoinsService.GrantCoins(
    GameManager.Instance.ProgressionService.GetCurrentLevelResult().FinalReward);
```

for normal completion.

And replay currently does:

``` csharp
GameManager.Instance.CoinsService.GrantCoins(result.FinalReward);
```

This violates the agreed replay economy.

### Required next change

Calculate the previous `BestReward` before updating it.

Then:

``` text
coinsAwarded =
max(0, result.FinalReward - previousBestReward)
```

Grant only `coinsAwarded`.

Only after calculating the difference should `BestReward` be updated.

Do not accidentally calculate the difference after `SaveLevelProgress()`
has already updated `BestReward`, or the difference will become zero.

------------------------------------------------------------------------

# 30. Another critical progression issue

Current `ProgressionService.SaveLevelProgress()` does:

``` csharp
progress.BestCorrectAnswers = result.CorrectAnswers;
```

This can reduce the player's best score on replay.

Change to:

``` csharp
progress.BestCorrectAnswers =
    Mathf.Max(progress.BestCorrectAnswers, result.CorrectAnswers);
```

------------------------------------------------------------------------

# 31. Recommended progression architecture

The long-term desired flow is:

``` text
QuizController
    ↓
ProgressionService.CompleteLevel(...)
    ↓
Calculate LevelResult
    ↓
Compare against previous LevelProgress
    ↓
Calculate progression delta
    ├── new best stars
    ├── new best reward
    ├── coins actually awarded
    ├── first completion
    ├── newly achieved perfect level
    └── newly completed boss
    ↓
Update LevelProgress
    ↓
Update PlayerStatistics
    ↓
Update achievements
    ↓
Save
    ↓
Return a completion result to UI
```

The UI should not calculate reward differences.

The UI should display the result it receives.

A future `LevelCompletionResult` or `ProgressionDelta` object is
encouraged if the current method flow becomes cumbersome.

------------------------------------------------------------------------

# 32. Profile statistics semantics

Use these meanings consistently:

### Current Level

The player's current progression level.

### Stars Earned

The sum of the player's **best stars per level**, not stars earned from
every attempt.

Example:

``` text
Level 1 best = 3
Level 2 best = 5
Level 3 best = 0
Stars Earned = 8
```

### Boss Levels Cleared

Number of boss levels completed at least once.

### Available Coins

Current spendable balance.

### Coins Earned

Lifetime coins actually awarded, including only the incremental reward
from replay improvements.

### Correct Answers

Lifetime correct answers.

### Accuracy

Lifetime:

``` text
CorrectAnswers / QuestionsAnswered
```

### Longest Streak

Lifetime best consecutive correct-answer streak.

### Perfect Levels

Number of levels whose best result is currently 5 stars.

------------------------------------------------------------------------

# 33. Profile screen planned final state

Preferred final order:

``` text
PROFILE

Current Level                 2/50
Stars Earned                  0/250
Boss Levels Cleared           0/10

--------------------------------

Available Coins               50
Coins Earned                   0

--------------------------------

Correct Answers               4/5
Accuracy                      80%
Longest Streak                 3
Perfect Levels              0 (0%)
```

The screenshot shown during development had this general structure and
was considered successful.

Do not add a progress bar unless the user explicitly changes their mind.

------------------------------------------------------------------------

# 34. Future UI directives

## General

-   Keep UI compact and readable on portrait mobile.
-   Prefer custom sprites over Unity default controls.
-   Keep decorative UI graphics from unnecessarily intercepting
    raycasts.
-   Use green sparingly as a strong accent.
-   Use gold for stars/coins.
-   Use gray for locked/secondary states.
-   Use red/orange only for penalties/warnings.
-   Avoid excessive animation.

## Buttons

-   Rounded.
-   Dark navy fill.
-   Thin green outline.
-   White bold label.
-   Subtle pressed/highlighted state.
-   Avoid default Unity button appearance.

## Cards

-   Dark navy.
-   Rounded corners.
-   Subtle border.
-   Thin green vertical accent.
-   Current state gets a stronger green outline.
-   Locked state is dimmed.

## Scrollbars

-   Thin.
-   Rounded handle.
-   Minimal track.
-   Reusable between Levels and Achievements.

## Popups

-   Dark navy/topographic background.
-   Centered portrait panel.
-   Clear large title.
-   Consistent Close/Continue button treatment.
-   Reusable popup conventions are desirable.

------------------------------------------------------------------------

# 35. Popup architecture recommendation

There are currently multiple popup views.

A future optional refactor is a small reusable base:

``` csharp
public abstract class PopupView : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
```

Potential descendants:

-   `ProfileView`
-   `LevelsPopupView`
-   `AchievementsView`

Do this only if it provides real value. Do not refactor working UI
simply for abstraction.

------------------------------------------------------------------------

# 36. Boss level presentation

`LevelDefinition.IsBossLevel` already exists.

Planned visual treatment:

-   Gold border/accent instead of normal green.
-   Gold stars.
-   Small `BOSS` or similar badge.
-   More significant reward.
-   Stronger completion animation.
-   Potential golden confetti.

Boss levels are planned at:

``` text
5, 10, 15, 20, 25, 30, 35, 40, 45, 50
```

The user wants these levels to feel more difficult and more important.

------------------------------------------------------------------------

# 37. Planned development phases

## Phase 1 --- Finish progression

### 1. Replay rewards

Highest priority.

Implement:

-   previous best reward comparison,
-   incremental reward only,
-   correct `BestReward` update,
-   correct lifetime `CoinsEarned`,
-   completion result for UI.

### 2. Correct best-result persistence

Ensure:

-   `BestStars` never decreases.
-   `BestReward` never decreases.
-   `BestCorrectAnswers` never decreases.

### 3. Boss level styling

Use existing `IsBossLevel`.

### 4. Levels auto-scroll

Open Levels popup centered/positioned around current level.

------------------------------------------------------------------------

## Phase 2 --- Statistics/Profile

Implement/persist:

-   `CoinsEarned`
-   `StarsEarned`
-   `PerfectLevelsCompleted`
-   `BossLevelsCompleted`
-   any other genuinely useful lifetime metric.

Then make ProfileView display them in the agreed order.

------------------------------------------------------------------------

# 38. Planned achievement improvements

Use statistics to drive more achievements.

Potential achievement concepts discussed:

-   100 correct answers.
-   1000 correct answers.
-   Complete 10 levels.
-   Earn 100 stars.
-   Complete a boss level.
-   Complete Level 50.
-   Complete 5 levels flawlessly.
-   Replay/improve levels.
-   Earn 5000 lifetime coins.

The exact achievement list is not final.

Important: coin-based achievements should use lifetime `CoinsEarned`,
not current spendable `Coins`.

------------------------------------------------------------------------

# 39. Planned gameplay/economy features

After Phase 1 and Phase 2:

### Daily reward

Potential 7-day reward sequence.

Example concept:

``` text
Day 1: 100
Day 2: 150
Day 3: 200
...
Day 7: 500
```

Keep it simple initially.

### Shop

Planned later, after the coin economy has enough sinks/sources to
balance prices properly.

Do not build the Shop prematurely.

------------------------------------------------------------------------

# 40. Features deliberately postponed

Do not introduce these unless explicitly requested:

-   Separate player/RPG levels.
-   Infinite progression.
-   A second "player level" system.
-   Shop before economy is balanced.
-   Large UI redesign while existing screens are working.
-   Excessive popup abstraction.
-   Unnecessary progress bars.
-   Default Unity UI styling.

------------------------------------------------------------------------

# 41. Future polish

After core progression is stable:

-   Smooth popup open/close transitions.
-   Smooth auto-scroll.
-   Button press animations.
-   Better haptic feedback on mobile.
-   Sound effects.
-   Confetti for 5-star completions.
-   Stronger boss-level completion presentation.
-   Subtle reward particle effects.
-   Possibly fade scrollbar after inactivity.

------------------------------------------------------------------------

# 42. Level Complete UI future polish

Current reward animation is good.

Potential future additions:

### Normal completion

-   Existing sequential row animation.
-   Stars pop.
-   Reward count-up.
-   Reward pulse.

### 5-star completion

-   Stronger star celebration.
-   Optional subtle confetti.

### Boss completion

-   Gold-themed celebration.
-   Stronger but still controlled animation.

Avoid turning every completion into a noisy celebration.

------------------------------------------------------------------------

# 43. Important debugging lessons from development

## When runtime UI differs from prefab

Check:

1.  Runtime instantiated object's properties.
2.  Layout rebuild timing.
3.  Masking.
4.  `Maskable`.
5.  Content/Viewport bounds.
6.  Canvas rebuilds.

Do not immediately assume the prefab is the problem.

## ScrollView text/icons appearing only after scrolling

We ultimately found a `Maskable` setting on the prefab was the culprit.

## ScrollView not receiving wheel input between cards

Empty spacing caused mouse-wheel interaction to fail because the pointer
was no longer over a UI element receiving the event.

Current workaround:

``` text
Vertical Layout Group spacing = 0
```

Keep this unless a better event-routing solution is deliberately
implemented.

## Do not add a Viewport Image blindly

Adding an Image to the Viewport caused cards to become hidden with the
current masking setup.

The working approach uses `RectMask2D`.

------------------------------------------------------------------------

# 44. Code quality rules for future work

When modifying this project:

1.  **Inspect the current repository before proposing code.**
2.  Do not assume a class/property/method exists.
3.  Do not recreate architecture that already exists.
4.  Keep UI classes responsible for presentation.
5.  Keep reward/progression logic in services.
6.  Keep persistent data in data/save classes.
7.  Use `LevelDatabase` as the source of level configuration.
8.  Avoid hardcoded level values in gameplay code.
9.  Never reduce a stored "best" result because of a replay.
10. Never award full replay rewards again.
11. Be explicit about whether a statistic is:

-   current state,
-   lifetime total,
-   per-level best,
-   or per-attempt result.

12. Before refactoring, verify the current call sites.

------------------------------------------------------------------------

# 45. Current known implementation gaps

Based on the latest inspected repository snapshot plus subsequent UI
work:

### High priority

-   [ ] Fix replay coin awarding to use only the difference from
    `BestReward`.
-   [ ] Ensure replay completion updates statistics correctly.
-   [ ] Ensure `BestCorrectAnswers` uses `Mathf.Max`.
-   [ ] Persist all newly added `PlayerStatistics` fields.
-   [ ] Fix `CoinsEarned` achievement semantics to use lifetime earned
    coins.
-   [ ] Ensure first-completion vs replay is handled correctly for:
    -   StarsEarned
    -   PerfectLevelsCompleted
    -   BossLevelsCompleted.

### Medium priority

-   [ ] Finish auto-scroll polish.
-   [ ] Implement boss card styling.
-   [ ] Finish ProfileView wiring for all new statistics.
-   [ ] Make Profile values visually differentiated where appropriate.
-   [ ] Reuse custom scrollbar on Achievements.

### Later

-   [ ] Expanded achievements.
-   [ ] Daily rewards.
-   [ ] Shop.
-   [ ] Audio/haptics.
-   [ ] More completion effects.
-   [ ] More content/questions.
-   [ ] Difficulty balancing.

------------------------------------------------------------------------

# 46. Suggested implementation order from here

Do not jump between unrelated systems.

Recommended sequence:

``` text
1. Replay reward economy
        ↓
2. Correct best-result persistence
        ↓
3. Statistics replay deltas
        ↓
4. Save/load new statistics
        ↓
5. Profile final wiring
        ↓
6. Boss level styling
        ↓
7. Levels auto-scroll polish
        ↓
8. Achievement improvements
        ↓
9. UI/audio polish
        ↓
10. Daily reward
        ↓
11. Shop
```

------------------------------------------------------------------------

# 47. How Copilot should work on this project

When asking Copilot to modify the project, prefer prompts like:

> Inspect the current repository first. Do not assume the architecture
> from this document is unchanged. Identify the relevant current classes
> and call sites, then propose the smallest change that follows the
> existing architecture.

For larger changes:

> Before writing code, list the current classes/methods involved,
> explain where the new logic belongs, and identify any existing logic
> that would conflict with it.

For UI:

> Preserve the existing Football Career Quiz visual language: dark navy
> topographic panels, green accents, gold stars/coins, rounded cards,
> minimal custom scrollbar, and restrained animations. Do not redesign
> the screen unless explicitly requested.

For progression:

> Treat LevelDatabase/LevelDefinition as the source of level
> configuration. Do not introduce hardcoded question/reward values.

For replay:

> A replay can only award the improvement over the stored BestReward.
> Never grant the full reward again.

------------------------------------------------------------------------

# 48. Current product philosophy

The game should feel like a **football career journey**, not merely a
collection of quizzes.

The progression loop should be:

``` text
PLAY current level
      ↓
Complete quiz
      ↓
Earn stars + reward
      ↓
Unlock next level
      ↓
Replay completed levels
      ↓
Improve best result
      ↓
Earn only the improvement
      ↓
Reach boss levels
      ↓
Complete the career
```

The Levels screen communicates progression.

The Level Complete screen communicates immediate achievement.

The Profile communicates long-term career progress.

The Achievements screen communicates optional goals.

These four screens should reinforce each other rather than duplicate
each other.

------------------------------------------------------------------------

# 49. Final design principles

If there is a conflict between adding a feature and keeping the game
understandable, prefer clarity.

If a statistic doesn't tell the player something useful, don't display
it.

If an animation doesn't improve feedback, don't add it.

If a system can be driven by the LevelDatabase, don't hardcode it.

If replaying something can generate unlimited currency, fix the economy
before adding more content.

If a UI element looks like a default Unity control, consider whether it
should be custom-styled.

The current UI direction is intentionally restrained and should remain
so.

------------------------------------------------------------------------

# 50. Immediate next task

The most important next implementation task is:

## **Finish replay rewards correctly.**

Specifically:

1.  Read the existing `LevelProgress.BestReward`.
2.  Calculate the current attempt's `LevelResult`.
3.  Calculate:

``` csharp
int coinsAwarded =
    Mathf.Max(0, result.FinalReward - previousBestReward);
```

4.  Grant only `coinsAwarded`.
5.  Update `BestReward` with the new maximum.
6.  Record `coinsAwarded` in lifetime `PlayerStatistics.CoinsEarned`.
7.  Update `StarsEarned` only by the increase in per-level best stars.
8.  Increment `PerfectLevelsCompleted` only when a level first reaches 5
    stars.
9.  Increment `BossLevelsCompleted` only on first completion.
10. Save everything.
11. Return enough information for the Level Complete UI to show both:
    -   the calculated reward for the attempt,
    -   and the actual new coins awarded.

Do this before building the Shop or other economy features.

------------------------------------------------------------------------

# 51. Important source/context note

This handoff was assembled from:

-   the development conversation and decisions made during the project,
-   the latest available code archive in the conversation
    (`FootIQ_Scripts1.zip`),
-   and the subsequent UI screenshots/changes made after that archive.

The code archive is **not necessarily the exact final working tree**
because UI/statistics changes were made after the archive was uploaded.
Therefore, for implementation, always inspect the current repository
first.

The final Profile screenshot and the Levels UI screenshots represent the
later UI direction and should be treated as the more recent visual
reference.
