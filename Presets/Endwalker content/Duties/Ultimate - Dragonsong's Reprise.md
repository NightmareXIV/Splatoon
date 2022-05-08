# Phase 1 - Knights triggers
Ser Grinnaux's knockback helper (draws small circle around it's hitbox so you can precisely position yourself for knockback)
```
DSR Grinnaux knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"Ser Grinnaux","onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75.0,"Duration":10.0}],"Phase":1}
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

Tether on Thordan's jump for towers + jump on gaze - to make it easier to locate him:
```
DSR King Thordan tether on leap~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"King Thordan","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":48.0,"Duration":3.0},{"TimeBegin":102.0,"Duration":10.0}],"Phase":2}
```

