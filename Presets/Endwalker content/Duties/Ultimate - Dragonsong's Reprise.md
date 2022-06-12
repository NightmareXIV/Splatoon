# Phase 1 - Knights triggers
Ser Grinnaux's knockback helper (draws small circle around it's hitbox so you can precisely position yourself for knockback)
```
DSR Grinnaux knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"Ser Grinnaux","onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75.0,"Duration":10.0}],"Phase":1}
```
P1 Knockback Tether: Tethers Adelphel when he jumps to help locate where the knockback is coming from
```
DSR P1 Knockback Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"Ser Adelphel","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":53.0,"Duration":15.0}],"Phase":1}
```
P1 Empty/Full Dimension Ring: Places a ring around Ser Grinnaux that displays the edge of Empty/Full Dimension when it is being cast.
Note: Visually this looks accurate, but needs more testing.
```
DSR Empty/Full Dimension Ring~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"thicc":5.0,"refActorName":"Ser Grinnaux","includeHitbox":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":18.5,"Duration":5.0},{"TimeBegin":99.0,"Duration":5.0}],"Phase":1}
```

# Phase 2 - Thordan triggers
King Thordan move reminder: (while Splatoon isn't really designed for being general-purpose trigger system, it can be used as such)
```
DSR thordan move reminder~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"overlayBGColor":4278190335,"overlayVOffset":3.0,"overlayFScale":8.0,"thicc":0.0,"overlayText":"MOVE","refActorType":1}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"Match":"King Thordan readies Ascalon's Mercy Concealed.","MatchDelay":2.6}],"Phase":2}
```

Strength of the Ward - "divebomb" helper (shows safe zone)
```
DSR Strength of the Ward~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Vellguine","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Ignasse","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Paulecrain","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":7.0,"Match":"King Thordan readies Strength of the Ward"}],"Phase":2}
```

Strength of the Ward - earthquake radius rings
```
DSR Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":18.0,"Match":"King Thordan readies Strength of the Ward"}],"Phase":2}
```

Strength of the Ward - earthquake radius rings - alternative triggers in case first one doesn't works well for you:
```
DSR Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"Ser Paulecrain readies Spiral Thrust"}],"Phase":2}
```

Strength of the Ward Sequential Quake Marker - Displays the Quake markers sequentially instead of all at once.
```
DSR Strength Quake 1~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":6.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"Ser Guerrique","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":35.0,"Duration":8.0}],"Phase":2}
```
```
DSR Strength Quake 2~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":12.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":41.5,"Duration":4.0}],"Phase":2}
```
```
DSR Strength Quake 3~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":18.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":43.5,"Duration":4.0}],"Phase":2}
```
```
DSR Strength Quake 4~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":24.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":45.5,"Duration":2.0}],"Phase":2}
```

Sanctity DRK Tether. Locates the DRK (Ser Zephirin) with a tether during Sanctity of the Ward for use with the DRK Relative strat:
```
DSR Sanctity DRK Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.5,"Duration":12.5}],"Phase":2}
```

Tether on Thordan's jump for towers + jump on gaze - to make it easier to locate him:
```
DSR King Thordan tether on leap~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"King Thordan","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":48.0,"Duration":3.0},{"TimeBegin":102.0,"Duration":10.0}],"Phase":2}
```

# Phase 3 - Nidhogg triggers:
Baits after tower (Geirskogul cast): (Requires Splatoon version 1.0.8.0 or higher)
```
DSR Geirskogul~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":30.0,"radius":4.0,"color":1174405375,"thicc":4.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.0,"Match":"Nidhogg readies Geirskogul."}],"Phase":2}
```

Drachenlance:
```
DSR Dranchenlance~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":4,"refY":15.0,"radius":14.0,"coneAngleMin":-45,"coneAngleMax":45,"color":4294967040,"thicc":3.0,"refActorName":"Nidhogg","includeRotation":true,"onlyTargetable":true,"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":3.0,"Match":"Nidhogg readies Drachenlance."}],"Phase":2}
```

# Phase 5 - Alternative timeline Thordan triggers:
The second set of quakes seen in P5:
```
DSR Quake P5 markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```

Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR Dive P5 markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Ser Zephirin","includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Vedrfolnir","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```

# Phase 6 - Nidhogg and Hraesvelgr
P6 Arena Quarter
Helps with keeping melee uptime when Nidhogg dives one half of the arena in addition to Hraesvelgr cleaving the other half leaving one quarter safe. (exact trigger to be improved):
```
DSR P6 Arena Quarter~{"ZoneLockH":[968],"DCond":5,"Elements":{"P6 Quarter":{"type":2,"refX":100.0,"refY":80.0,"offX":100.0,"offY":120.0,"radius":0.0},"P6 Quarter 2":{"type":2,"refX":80.0,"refY":100.0,"offX":120.0,"offY":100.0,"radius":0.0}},"UseTriggers":true,"Triggers":[{"TimeBegin":660.0,"Duration":240.0}],"Phase":2}
```
