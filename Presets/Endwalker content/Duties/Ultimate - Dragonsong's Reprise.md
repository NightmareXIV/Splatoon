# Phase 1 - Knights triggers
Ser Grinnaux's knockback helper (draws small circle around it's hitbox so you can precisely position yourself for knockback)
```
DSR Grinnaux knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"Ser Grinnaux","refActorModelID":3268,"refActorObjectID":1073769566,"refActorDataID":12602,"onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75,"Duration":10}],"Phase":1}
```

# Phase 2 - Thordan triggers
Strength of the Ward - "divebomb" helper (shows safe zone)
```
DSR Strength of the Ward~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Vellguine","refActorModelID":2449,"refActorObjectID":1073743651,"refActorDataID":6133,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Ignasse","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Paulecrain","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33,"Duration":10,"Match":"King Thordan readies Strength of the Ward"}],"Phase":2}
```

Tether on Thordan's jump for towers - to make it easier to locate him:
```
DSR King Thordan tether on leap~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"King Thordan","refActorModelID":3269,"refActorObjectID":1073789859,"refActorDataID":12604,"onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":48,"Duration":3}],"Phase":2}
```
