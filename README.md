# UltraDamageDisplay
ULTRAKILL Mod that shows you the damage you dealt to enemies

## Installation
Copy the UltraDamageDisplay.dll file into the `BepInEx/plugins` folder inside your ULTRAKILL directory.

## Usage
For now, the damage dealt is displayed in the BepInEx console, in the format below

```[Damage] {mobName} took {multiplier} + {multiplier} * {critMultiplier} = {calcDmg}(actual {finalDamage}) damage!```

Where `calcDmg` is damage calculated using the above formula and `finalDamage` is the damage that is actually dealt to the enemy (taking into account weak spots for example).

## Contributing
To compile, you need to add the needed references in the lib folder. 

You can find them in the `ULTRAKILL\ULTRAKILL_Data\Managed` folder. 

The references you need are:
- Assembly-CSharp.dll
- UnityEngine.dll
- UnityEngine.CoreModule.dll