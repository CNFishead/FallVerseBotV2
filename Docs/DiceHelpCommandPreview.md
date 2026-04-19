# Dice Help Command Preview

## What Users Will See

When users run `/dice-help`, they'll receive a beautiful Discord embed with the following structure:

### 🎲 Advanced Dice Rolling System

_Learn how to use the powerful dice rolling features!_

**📋 Basic Usage**

```
/roll 1d20+3
/roll 3d6 "damage roll"
/roll 2d8+2; 1d4+1 "multiple rolls"
```

**⚖️ Advantage & Disadvantage**

```
/roll 1d20+5 adv - Roll with advantage
/roll 1d20+5 disadv - Roll with disadvantage
/roll 1d20+3 "attack" adv - With label
```

**🧮 Mathematical Operations (PEMDAS)**

```
/roll 3d6x2+1 - Roll 3d6, multiply by 2, add 1
/roll 2d8+5*3-2 - Complex math expressions
/roll 1d10*2/4+3 - Division and multiplication
Operators: + - * x /
```

**🏆 Keep Best/Worst Rolls**

```
/roll 4d6b3 - Roll 4d6, keep best 3
/roll 4d6w3 - Roll 4d6, keep worst 3
/roll 6d8b4 "character stats" - With label
Example: [4,6,3,1] → best 3 → [6,4,3] = 13
```

**🔧 Combined Features**

```
/roll 4d6b3x2+5 "enhanced stats"
Roll 4d6, keep best 3, multiply by 2, add 5
/roll 3d8w2+10 "penalty roll"
All features work together seamlessly!
```

**📝 Multiple Expressions & Labels**

```
/roll 1d20+5 "attack"; 2d6+3 "damage"
/roll 3d6b3 "str"; 3d6b3 "dex"; 3d6b3 "con"
Separate with ; | Add labels with "quotes"
```

**💡 Real-World Examples**
**Character Creation:**

```
/roll 4d6b3; 4d6b3; 4d6b3; 4d6b3; 4d6b3; 4d6b3
```

**Critical Hit:**

```
/roll 1d20+8 "attack" adv; 4d6+4x2 "crit damage"
```

**Fireball Damage:**

```
/roll 8d6 "fireball damage"
```

_💡 Tip: The bot automatically detects when to use advanced features based on your expression
