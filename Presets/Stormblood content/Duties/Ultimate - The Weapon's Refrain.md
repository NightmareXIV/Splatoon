Marks BOTH potential safespots for Titan's Upheaval Knockback. You will still have to determine the correct one yourself.
```
~Lv2~{"Name":"UWU Titan Upheaval","Group":"UWU","ZoneLockH":[777],"DCond":5,"ElementsL":[{"Name":"left","type":1,"offX":0.5,"offY":4.6,"radius":0.25,"thicc":5.0,"refActorType":2,"includeRotation":true},{"Name":"right","type":1,"offX":-0.5,"offY":4.6,"radius":0.25,"thicc":5.0,"refActorType":2,"includeRotation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":4.0,"Match":"titan readies upheaval"}]}
```

Marks all potential safespots for Predation, and draws lines pointing away from Garuda. You will still need to determine the correct direction yourself based on Titan & Ultima positions.
```
~Lv2~{"Name":"UWU Predation","Group":"UWU","ZoneLockH":[777],"DCond":5,"ElementsL":[{"Name":"Direction","type":3,"refY":3.0,"offX":3.0,"offY":6.0,"radius":0.0,"thicc":5.0,"refActorName":"Garuda","includeRotation":true,"onlyVisible":true},{"Name":"Direction2","type":3,"refY":3.0,"offX":-3.0,"offY":6.0,"radius":0.0,"thicc":5.0,"refActorName":"Garuda","includeRotation":true,"onlyVisible":true},{"Name":"Safespot1","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-6.5,"offY":-17.8,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot2","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":6.5,"offY":-17.8,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot3","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":17.8,"offY":-6.5,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot4","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":17.8,"offY":6.5,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot5","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":6.5,"offY":17.8,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot6","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-6.5,"offY":17.8,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot7","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-17.8,"offY":6.5,"radius":0.25,"color":3372180480,"thicc":5.0},{"Name":"Safespot8","refX":100.0,"refY":100.0,"refZ":1.4305115E-06,"offX":-17.8,"offY":-6.5,"radius":0.25,"color":3372180480,"thicc":5.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"The Ultima Weapon uses Ultimate Predation"}]}
```

Draws a tether to the center to remind you to bait Landslide during Titan Gaols. Also indicates positions for the backmost gaol so it'll get hit by the initial rock explosion. Again, you will have to determine the correct one yourself.
```
~Lv2~{"Name":"UWU Titan Gaols","Group":"UWU","ZoneLockH":[777],"DCond":5,"ElementsL":[{"Name":"center","refX":100.0,"refY":100.0,"color":3372180480,"thicc":5.0,"tether":true},{"Name":"left","refX":98.0,"refY":106.3,"color":3372180480,"thicc":5.0},{"Name":"right","refX":102.0,"refY":106.3,"color":3372180480,"thicc":5.0},{"Name":"left1","refX":106.3,"refY":98.0,"color":3372180480,"thicc":5.0},{"Name":"right1","refX":106.3,"refY":102.0,"color":3372180480,"thicc":5.0},{"Name":"left2","refX":93.7,"refY":98.0,"color":3372180480,"thicc":5.0},{"Name":"right2","refX":93.7,"refY":102.0,"color":3372180480,"thicc":5.0},{"Name":"left3","refX":102.0,"refY":93.7,"color":3372180480,"thicc":5.0},{"Name":"right3","refX":98.0,"refY":93.7,"color":3372180480,"thicc":5.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"titan uses upheaval"}]}
```

Simple border during Suppression to avoid running into feathers.
```
~Lv2~{"Name":"UWU Suppression Border","Group":"UWU","ZoneLockH":[777],"DCond":5,"ElementsL":[{"Name":"border","refX":100.0,"refY":100.0,"radius":15.0,"thicc":10.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":20.0,"Match":"The Ultima Weapon uses Ultimate Suppression"}]}
```
