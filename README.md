# Unity Idle Game Demo

A modular idle game built with Unity 6000.0.56f1 showcasing clean architecture, character progression, and gacha mechanics.

## 🎮 Features

**Core Gameplay**
- **Idle Combat**: Characters automatically battle and earn Silver currency
- **Character Progression**: Level up characters to increase rewards and attack speed
- **Gacha System**: Collect new characters through weighted random pulls
- **Offline Rewards**: Earn Silver while away from the game
- **Multiple Characters**: Switch between owned characters

## 🚀 Quick Start

1. Open the project in Unity 6000.0.56f1+
2. Load the main scene
3. Press Play to start battling and earning Silver
4. Level up your character and try the gacha system

## 🏗️ Architecture Overview

The project uses a **Systems-Based Architecture** with dependency injection for clean separation of concerns.

### Key Systems
- **Save/Load System**: JSON serialization with PlayerPrefs storage
- **Asset Loading**: Unity Resources-based asset management
- **Item Catalog**: ScriptableObject-based item definitions
- **Inventory Management**: Player item and currency tracking

### Project Structure
```
Assets/Scripts/
├── Features/
│   ├── IdleBattling/     # Combat and character management
│   └── Gacha/            # Character collection system
└── Systems/              # Core game systems
```

## 🎯 Core Systems

### Idle Battle System
- Automatic Silver generation based on character stats
- Character leveling with configurable progression curves
- Offline time calculation for idle rewards
- Character switching between owned units

### Gacha System
- Weighted random selection by rarity (Common → Legendary)
- Configurable costs and rewards per pull
- Multiple items per pull support

### Item & Inventory
**Item Types**: Currency, Characters, Consumables  
**Rarity Tiers**: Common, Uncommon, Rare, Epic, Legendary

## 🛠️ Technical Stack

- **Unity**: 6000.0.56f1
- **Serialization**: Newtonsoft.Json
- **UI**: TextMeshPro

## 📝 Credits

- [yet-another-icons](https://prinbles.itch.io/yet-another-icons)
- [pixel font thaleah](https://tinyworlds.itch.io/free-pixel-font-thaleah)
- [Tiny RPG Character Asset Pack](https://zerie.itch.io/tiny-rpg-character-asset-pack)