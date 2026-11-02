# Captain Claw – 2D Platformer (C# Remake)

A side-scrolling 2D platformer inspired by the classic game *Captain Claw*, built from scratch in **C# and Windows Forms** with no game engine.

> Made as a Multimedia course project.

## Features

- **Custom game loop** driven by a timer, with double-buffered rendering for smooth, flicker-free frames
- **Sprite animation** for the hero (60+ frames) and enemy officers
- **Physics**: jumping, gravity, platforms, ladders and an elevator
- **Combat**: pistol shots and a magic multi-shot attack, enemies with health that shoot back
- **Level mechanics**: camera scrolling, laser traps, a key that disables the lasers, collectible magic power-up
- **Multiple stages**, plus loading, game-over, win and restart screens
- **Audio**: background music and sound effects using NAudio

## Controls

| Key | Action |
|---|---|
| ← / → | Move |
| Space | Jump |
| ↑ | Climb ladder |
| X | Fire pistol |
| Q | Magic attack |
| K | Use key to stop the lasers |

## Tech Stack

- C# / .NET Framework 4.7.2
- Windows Forms (GDI+ drawing)
- [NAudio](https://github.com/naudio/NAudio) for music and sound effects

## How to Run

1. Clone the repository.
2. Open `WindowsFormsApp1.sln` in Visual Studio.
3. Build and run (F5). Visual Studio restores the NuGet packages automatically.

The game loads its images and sounds from `WindowsFormsApp1/bin/Debug/`, so run it in the **Debug** configuration.

## Disclaimer

This is a non-commercial, educational fan project. *Captain Claw* and its original artwork, characters and music belong to their respective owners (Monolith Productions). No copyright infringement is intended.
