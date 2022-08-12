Ascalon's Mercy Move Reminder: Flashes "MOVE" on the screen when Ascalon's Mercy is fully cast to remind you to move. 

(While Splatoon isn't really designed for being general-purpose trigger system, it can be used as such)
```
DSR P2 Move Trigger~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"overlayBGColor":4278190335,"overlayVOffset":3.0,"overlayFScale":8.0,"thicc":0.0,"overlayText":"MOVE","refActorType":1}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"Match":"King Thordan readies Ascalon's Mercy Concealed.","MatchDelay":2.6}],"Phase":2}
```

Thordan Jump Tether: Tethers Thordan when he jumps to make it easier to locate him during Strength and Sanctity
```
DSR P2 Thordan Jump Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"King Thordan","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":49.5,"Duration":3.0},{"TimeBegin":102.0,"Duration":10.0}],"Phase":2}
```

### Strength of the Ward
[EN] Divebomb Helper: Shows both divebomb safespots [Translation required: trigger]
```
DSR P2 Strength Divebombs~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3637,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":7.0,"MatchIntl":{"En":"King Thordan readies Strength of the Ward."},"MatchDelay":8.0}],"Phase":2}
```

[EN] Heavy Impact Rings: Places rings around Ser Guerrique indicating the size of the quake rings from Heavy Impact [Translation required: trigger]
```
DSR P2 Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":12.0,"MatchIntl":{"En":"Ser Paulecrain readies Spiral Thrust"}}],"Phase":2}
```

Sequential Heavy Impact Rings: Displays the quake markers sequentially instead of all at once.
```
DSR P2 Strength Quake 1~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":6.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"Ser Guerrique","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":35.0,"Duration":10.0},{"Type":3,"Match":"Ser Guerrique uses Heavy Impact.","MatchDelay":1.9}],"Phase":2}
```
```
DSR P2 Strength Quake 2~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":12.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":41.0,"Duration":3.8,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":6.0}],"Phase":2}
```
```
DSR P2 Strength Quake 3~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":18.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":43.5,"Duration":3.8,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":7.9}],"Phase":2}
```
```
DSR P2 Strength Quake 4~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":24.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":45.5,"Duration":1.9,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":9.8}],"Phase":2}
```
Party Positions: Places blue circles on the spots where the party stack, two tankbusters and 3 defam dives should be.
```
DSR P2 Strength Positions~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"offX":6.3,"offY":3.25,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"2":{"type":1,"offX":-6.3,"offY":3.25,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"3":{"type":1,"offY":2.5,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"4":{"type":1,"offY":43.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"5":{"type":1,"offX":20.0,"offY":26.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"6":{"type":1,"offX":-20.0,"offY":26.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":52.0,"Duration":8.0}]}
```

### Sanctity of the Ward
DRK Tether: Locates the DRK (Ser Zephirin) with a tether during Sanctity of the Ward for use with the DRK Relative strat
```
DSR P2 Sanctity DRK Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372154880,"thicc":5.0,"refActorName":"Ser Zephirin","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":9.2}],"Phase":2}
```

DRK Safespots: Indicates the two possible safespots for the DRK Relative strat on both sides of the arena.
```
DSR P2 Sanctity DRK Starting Spots~{"ZoneLockH":[968],"DCond":5,"Elements":{"G1 CCW":{"type":1,"offX":4.0,"offY":-5.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G1 CW":{"type":1,"offX":-4.0,"offY":-5.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G2 CCW":{"type":1,"offX":4.0,"offY":35.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G2 CW":{"type":1,"offX":-4.0,"offY":35.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":9.2}],"Phase":2}
```

PLD Facing Arrows: Places an arrow on both PLDs (Ser Adelphel and Ser Janlenoux) that shows which direction they're facing so you know whether to move Clockwise or Counter-Clockwise.
```
DSR P2 Sanctity PLD Arrows~{"ZoneLockH":[968],"DCond":5,"Elements":{"Adelphel":{"type":3,"refY":4.0,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Adelphel 2":{"type":3,"refY":4.0,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Adelphel 3":{"type":3,"refY":4.0,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Janlenoux":{"type":3,"refY":4.0,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true},"Janlenoux 2":{"type":3,"refY":4.0,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true},"Janlenoux 3":{"type":3,"refY":4.0,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":7.0}],"Phase":2}
```

[International] DSR P2 Sanctity - DPS meteor beacons except self. Very easily see whether you need to swap with someone or not. If you are playing healer, modify placeholders to be `<h2>`, `<t1>`, `<t2>`. If you are playing tank - `<h1>`, `<h2>`, `<t2>`.
```
~Lv2~{"Name":"DSR P2 DPS meteors","Group":"DSR","ZoneLockH":[968],"ElementsL":[{"Name":"","type":3,"offZ":8.0,"radius":0.0,"color":3372154896,"thicc":50.0,"refActorPlaceholder":["<d2>","<d3>","<d4>"],"refActorRequireBuff":true,"refActorBuffId":[562],"refActorUseBuffTime":true,"refActorBuffTimeMin":18.0,"refActorBuffTimeMax":50.0,"refActorComparisonType":5}],"Phase":2}
```

[EN] [Beta] DSR P2 Sanctity - display opposite towers when meteor is on YOU. 
```
~Lv2~{"Name":"DSR P2 Opposite towers","Group":"DSR","ZoneLockH":[968],"DCond":5,"ElementsL":[{"Name":"","type":3,"refZ":10.0,"radius":0.0,"thicc":27.0,"refActorModelID":480,"refActorPlaceholder":[],"refActorNPCNameID":3640,"refActorComparisonAnd":true,"refActorComparisonType":6}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"You suffer the effect of Prey"}}],"MinDistance":26.5,"MaxDistance":100.0,"UseDistanceLimit":true,"DistanceLimitType":1,"Phase":2}
```
