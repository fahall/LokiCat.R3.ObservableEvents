# LokiCat.R3.ObservableEvents

**R3-compatible source generator for turning C# `event` declarations into cached `Observable<T>` properties.**

This package eliminates boilerplate when wiring up C# `event`s into R3 reactive observables.  
It generates cached, lazily-connected observables for every `event` declared on your `partial interface` or `partial class`.

---

## ✨ Features

- Automatically detects C# `event` declarations inside interfaces or classes
- Generates type-safe `Observable<T>` or `Observable<(T1, T2, ...)>` properties
- Supports events with 0 to 5 parameters
- Emits clean diagnostic warnings for events with more than 5 parameters
- Requires no manual `Observable.Create` logic
- Fully R3 idiomatic: observables can be piped, composed, and disposed with `AddTo(this)`
- Generated code respects your `.editorconfig` styling preferences

---

## 📦 Installation

1. Add this NuGet package to your project:

```sh
dotnet add package LokiCat.R3.ObservableEvents
```

2. Define your `event`s normally inside a `partial interface` or `partial class`:

```csharp
public partial interface IPauseMenu
{
  event Action MainMenu;
  event Action Resume;
  event Action<Node, int> SaveProgress;
}
```

3. Build your project. The generator will automatically emit:

```csharp
private Observable<Unit> _onMainMenu;
public Observable<Unit> OnMainMenu =>
  Event(ref _onMainMenu, handler => MainMenu += handler);

private Observable<Unit> _onResume;
public Observable<Unit> OnResume =>
  Event(ref _onResume, handler => Resume += handler);

private Observable<(Node, int)> _onSaveProgress;
public Observable<(Node, int)> OnSaveProgress =>
  Event(ref _onSaveProgress, handler => SaveProgress += handler);
```

You can now subscribe:

```csharp
pauseMenu.OnResume
  .Subscribe(() => Console.WriteLine("Game resumed"))
  .AddTo(this);
```

---

## ✅ Supported Event Forms

| Event form                        | Generated Observable type       |
| ---------------------------------- | ------------------------------- |
| `event Action Something()`        | `Observable<Unit>`              |
| `event Action<Node> Something()`  | `Observable<Node>`              |
| `event Action<Node, int> Something()` | `Observable<(Node, int)>`   |
| ... up to 5 args                  | `Observable<(T1, T2, ..., T5)>` |

> Events with **6+ parameters** will trigger a diagnostic warning and will not generate observables.

---

## 🧠 How It Works

- The generator scans all syntax trees for `event` declarations.
- It finds the corresponding delegate type (if external), and inspects its parameters.
- It emits a `private Observable<T>` cache field and a `public Observable<T>` property.
- It uses the `Event(...)` extension method to lazily subscribe and forward events into R3 pipelines.

---

## 🛠 Example: Full Interface

```csharp
public partial interface ISettingsMenu
{
  event Action Opened;
  event Action<string> LanguageChanged;
}
```

🔻️ Generates:

```csharp
private Observable<Unit> _onOpened;
public Observable<Unit> OnOpened =>
  Event(ref _onOpened, handler => Opened += handler);

private Observable<string> _onLanguageChanged;
public Observable<string> OnLanguageChanged =>
  Event(ref _onLanguageChanged, handler => LanguageChanged += handler);
```

---

## 🧪 Troubleshooting

- Your interfaces or classes must be marked `partial`
- Events must use `Action`, `Action<T>`, or compatible delegate types
- Check `.g.cs` output in `obj/Debug/.../generated/`
- Use `#nullable enable` if you want null-safety
- Rebuild your project to trigger generation

---

## 📄 License

MIT License

---

## 💡 Bonus Tip

Pair this generator with R3, and your own event-driven architectures to create clean, reactive, and highly composable C# APIs without boilerplate.

