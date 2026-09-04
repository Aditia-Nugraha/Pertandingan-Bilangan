# Pertandingan Bilangan

**Pertandingan Bilangan** is an educational card battle game where players compete using cards that represent numbers in different forms. Players must choose their cards strategically, manage HP and Energy, and use various actions to gain an advantage over their opponent.

The game features both **Player vs Computer (PvC)** and **Player vs Player (PvP)** modes. PvP allows two players to connect through the same local network and battle against each other.

## 🎮 Features

* 🃏 **Card Battle System**
  Battle your opponent by selecting cards and comparing their numerical values.

* 🔢 **Multiple Number Representations**
  Cards represent numbers as:

  * Fractions
  * Decimals
  * Percentages
  * Visual fractions

* ⚔️ **Battle System**
  The result of each round determines Attack, HP, and Energy changes for both players.

* 🎴 **Draw Card**
  Draw additional cards when you need more options.

* 🔄 **Replace Card**
  Replace a card from your hand to adapt your strategy.

* ❤️ **Heal**
  Spend Energy to restore HP during battle.

* 🤖 **Player vs Computer**
  Play against a computer-controlled opponent with randomized decisions.

* 👥 **Player vs Player**
  Battle another player through a local network connection.

* 🌐 **LAN Discovery**
  Automatically discover available players on the same local network without manually entering an IP address.

* 📚 **Learning Materials**
  Provides learning materials related to:

  * Fractions
  * Decimals
  * Percentages
  * Visual fractions

  The materials help players understand the number concepts used in the game before or during gameplay.

* 🏆 **Battle Result System**
  View the result of each battle and the final outcome of the match.

* ⭐ **Personal Score**
  Keep track of personal battle achievements, including:

  * Win count
  * Lose count
  * Total score

  Score is earned when winning a match based on the player's remaining HP. The score is stored locally on the device using Unity's `PlayerPrefs`.

* 🔁 **Rematch**
  Start another match with the same opponent after finishing a battle.

* 📱 **Android Support**
  The game is designed and tested for Android devices.

## 🎯 Gameplay

Each player starts with a limited amount of **HP**, **Energy**, and a hand of cards.

During a battle, both players select one card. The selected cards are revealed and their numerical values are compared.

The player with the higher value gains the advantage of the round. Players can also use actions such as **Draw**, **Replace**, and **Heal** to manage their hand and resources.

At the end of a match, the winning player earns a score based on their remaining HP. The personal score consists of the number of wins, number of losses, and accumulated score.

Choose your cards carefully and use your resources wisely to defeat your opponent.

## 🕹️ Game Modes

### Player vs Computer

Play against a computer-controlled opponent. The computer makes its decisions automatically during the match.

### Player vs Player

Play against another player over a **local network (LAN)**.

The game uses automatic LAN discovery, so players on the same network can find each other without manually entering the host's IP address.

## 📚 Learning Materials

The game provides learning materials covering the number concepts used throughout the card battle system:

* **Fractions**
* **Decimals**
* **Percentages**
* **Visual fractions**

These materials are intended to help players understand the concepts before applying them through gameplay.

## 🏆 Personal Score

The game includes a **Personal Score** system to record the player's individual battle achievements.

The system stores:

* **Win Count** — total number of matches won.
* **Lose Count** — total number of matches lost.
* **Total Score** — accumulated score from all victories.

The score earned from a victory is equal to the player's **remaining HP at the end of the match**.

Losing a match only increases the **Lose Count** and does not reduce the accumulated score.

Personal Score is stored locally on each device using Unity's `PlayerPrefs`. It is not a global leaderboard and is not synchronized between multiplayer devices.

---

## 📥 Download

### [⬇️ Download Latest Release](../../releases/latest)

Download the latest Android version of **Pertandingan Bilangan** from the GitHub Releases page.

> **Platform:** Android
> **Latest Version:** v1.1.0

For installation, download the `.apk` file from the **Assets** section of the latest release and install it on your Android device.

---

## 🛠️ Built With

* **Unity**
* **C#**
* **Unity UI**
* **TextMeshPro**
* **TCP Networking**
* **UDP Broadcast**
* **ScriptableObject**
* **PlayerPrefs**

## 📱 Platform

Currently supported:

* **Android**

The game has also been tested during development using Windows builds for multiplayer testing.

## 🚀 Getting Started

### Playing the Game

1. Go to the [Latest Release](../../releases/latest).
2. Download the `.apk` file from the **Assets** section.
3. Install the APK on your Android device.
4. Launch **Pertandingan Bilangan**.

For **Player vs Player**:

1. Connect both devices to the same local network.
2. Open the game on both devices.
3. Select **Player vs Player**.
4. The devices will automatically discover each other.
5. The first player to become the Host can start the battle once the opponent is found.

### Running the Project

To open the project for development:

1. Clone this repository.
2. Open the project using Unity.
3. Open the required scene.
4. Build and run the project for the desired platform.

> Make sure the Unity version used by the project is compatible with the project files.

## 📂 Project Overview

The project contains the main systems required to run the game, including:

```text
Assets/
├── Art/
│   ├── Backgrounds/
│   ├── Buttons/
│        ├── BattleMenu/
│        ├── GameScene/
│        ├── MainMenu/
│   ├── Icons/
│   ├── UI/
│        ├── BattleMenu/
│        ├── GameScene/
│        ├── MainMenu/
├── Audio/
├── Fonts/
├── Prefabs/
│        ├── Animation/
│        ├── Audio/
├── Scenes/
├── ScriptableObjects/
│        ├── Card/
├── Scripts/
│   ├── Animator/
│        ├── Background/
│        ├── Battle/
│        ├── Card/
│        ├── Deal/
│        ├── Draw/
│        ├── Hand/
│        ├── Message/
│   ├── Audio/
│   ├── Card/
│   ├── Controller/
│   ├── Gameplay/
│        ├── Battle/
│        ├── Controllers/
│        ├── Deck/
│        ├── Draw/
│        ├── Hand/
│        ├── Heal/
│        ├── Manager/
│        ├── Result/
│        ├── Round/
│        ├── State/
│   ├── Multiplayer/
│        ├── Controller/
│        ├── Manager/
│        ├── Player/
│        ├── Protocol/
│        ├── Session/
│        ├── UI/
│   ├── Player/
│   ├── Score/
│   └── UI/
├── Settings/
├── Sprites/
│        ├── Card/
└── ...
```

The exact folder structure may vary depending on the current version of the project.

## 🌐 Multiplayer Networking

The Player vs Player mode uses a combination of:

* **TCP** for gameplay communication between players.
* **UDP Broadcast** for automatic LAN host discovery.

TCP is used to synchronize gameplay actions such as card selection, drawing cards, replacing cards, healing, battle results, rematch, and exiting the match.

UDP Broadcast allows players connected to the same local network to discover an available host automatically.

## 📸 Screenshots

![image](https://github.com/Aditia-Nugraha/Pertandingan-Bilangan/blob/4df80744ea506f4979b0886f91f6ea50a57712db/Screenshot/WhatsApp%20Image%202026-09-03%20at%2010.01.17.jpeg)
![image](https://github.com/Aditia-Nugraha/Pertandingan-Bilangan/blob/4df80744ea506f4979b0886f91f6ea50a57712db/Screenshot/WhatsApp%20Image%202026-09-03%20at%2010.01.16%20(1).jpeg)
![image](https://github.com/Aditia-Nugraha/Pertandingan-Bilangan/blob/4df80744ea506f4979b0886f91f6ea50a57712db/Screenshot/WhatsApp%20Image%202026-08-11%20at%2015.55.53.jpeg)

## 🎮 Game Information

|                        |                                      |
| ---------------------- | ------------------------------------ |
| **Title**              | Pertandingan Bilangan                |
| **Genre**              | Educational / Card Battle            |
| **Platform**           | Android                              |
| **Game Modes**         | Player vs Computer, Player vs Player |
| **Multiplayer**        | Local Network (LAN)                  |
| **Learning Materials** | Fractions, Decimals, Percentages     |
| **Personal Score**     | Win, Lose, Total Score               |

## 📄 License

This project was developed as an academic game development project.

Please contact the repository owner before redistributing or using the project's assets, source code, or other original content for commercial purposes.
