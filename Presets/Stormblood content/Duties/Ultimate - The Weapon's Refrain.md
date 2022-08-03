Marks BOTH potential safespots for Titan's Upheaval Knockback. You will still have to determine the correct one yourself.
```
UWU Titan Upheaval~{"ZoneLockH":[777],"DCond":5,"Elements":{"left":{"type":1,"offX":0.5,"offY":4.6,"radius":0.25,"thicc":5.0,"refActorType":2,"includeRotation":true},"right":{"type":1,"offX":-0.5,"offY":4.6,"radius":0.25,"thicc":5.0,"refActorType":2,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":4.0,"Match":"titan readies upheaval"}]}
```

Marks all potential safespots for Predation, and draws lines pointing away from Garuda. You will still need to determine the correct direction yourself based on Titan & Ultima positions.
```
UWU Predation~{"ZoneLockH":[777],"DCond":5,"Elements":{"Direction":{"type":3,"refY":3.0,"offX":3.0,"offY":6.0,"radius":0.0,"thicc":5.0,"refActorName":"Garuda","includeRotation":true},"Direction2":{"type":3,"refY":3.0,"offX":-3.0,"offY":6.0,"radius":0.0,"thicc":5.0,"refActorName":"Garuda","includeRotation":true},"Safespot1":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-6.5,"offY":-17.8,"radius":0.25,"thicc":5.0},"Safespot2":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":6.5,"offY":-17.8,"radius":0.25,"thicc":5.0},"Safespot3":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":17.8,"offY":-6.5,"radius":0.25,"thicc":5.0},"Safespot4":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":17.8,"offY":6.5,"radius":0.25,"thicc":5.0},"Safespot5":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":6.5,"offY":17.8,"radius":0.25,"thicc":5.0},"Safespot6":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-6.5,"offY":17.8,"radius":0.25,"thicc":5.0},"Safespot7":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-17.8,"offY":6.5,"radius":0.25,"thicc":5.0},"Safespot8":{"refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-17.8,"offY":-6.5,"radius":0.25,"thicc":5.0}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"The Ultima Weapon uses Ultimate Predation"}]}
```

Draws a tether to the center to remind you to bait Landslide during Titan Gaols. Also indicates positions for the backmost gaol so it'll get hit by the initial rock explosion. Again, you will have to determine the correct one yourself.
```
UWU Titan Gaols~{"ZoneLockH":[777],"DCond":5,"Elements":{"center":{"refX":100.0,"refY":100.0,"thicc":5.0,"tether":true},"left":{"refX":98.0,"refY":106.3,"thicc":5.0},"right":{"refX":102.0,"refY":106.3,"thicc":5.0},"left1":{"refX":106.3,"refY":98.0,"thicc":5.0},"right1":{"refX":106.3,"refY":102.0,"thicc":5.0},"left2":{"refX":93.7,"refY":98.0,"thicc":5.0},"right2":{"refX":93.7,"refY":102.0,"thicc":5.0},"left3":{"refX":102.0,"refY":93.7,"thicc":5.0},"right3":{"refX":98.0,"refY":93.7,"thicc":5.0}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"titan uses upheaval"}]}
```

Simple border during Suppression to avoid running into feathers.
```
~Lv2~{"Name":"UWU Suppression Border","Group":"UWU","ZoneLockH":[777],"DCond":5,"ElementsL":[{"Name":"border","refX":100.0,"refY":100.0,"radius":15.0,"thicc":10.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":20.0,"Match":"The Ultima Weapon uses Ultimate Suppression"}]}
```
