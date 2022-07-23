[International] P6 Arena Quarter
Helps with keeping melee uptime when Nidhogg dives one half of the arena in addition to Hraesvelgr cleaving the other half leaving one quarter safe. (exact trigger to be improved):
```
DSR P6 Arena Quarter~{"ZoneLockH":[968],"DCond":5,"Elements":{"P6 Quarter":{"type":2,"refX":100.0,"refY":80.0,"offX":100.0,"offY":120.0,"radius":0.0},"P6 Quarter 2":{"type":2,"refX":80.0,"refY":100.0,"offX":120.0,"offY":100.0,"radius":0.0}},"UseTriggers":true,"Triggers":[{"TimeBegin":660.0,"Duration":240.0}],"Phase":2}
```

[Rework pending, party far/near part is incorrect, the aoe is correct] [International] Hallowed wings with Party Far or Party Near signs:
```
DSR P6 Hallowed Wings~{"ZoneLockH":[968],"Elements":{"Right":{"type":3,"refX":11.0,"refY":44.0,"offX":11.0,"radius":11.0,"color":1190788864,"refActorNPCNameID":4954,"refActorRequireCast":true,"refActorCastId":[27942,27943],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true},"Left":{"type":3,"refX":-11.0,"refY":44.0,"offX":-11.0,"radius":11.0,"color":1190788864,"refActorNPCNameID":4954,"refActorRequireCast":true,"refActorCastId":[27939,27940],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true},"Party far":{"type":1,"offY":22.0,"radius":0.0,"Donut":0.0,"color":3356032768,"overlayBGColor":4293984511,"overlayTextColor":4278190080,"overlayFScale":3.0,"overlayText":"PARTY FAR","refActorNPCNameID":4954,"refActorRequireCast":true,"refActorCastId":[27942,27939],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true},"Party near":{"type":1,"radius":0.0,"Donut":0.0,"color":3356032768,"overlayBGColor":4279631616,"overlayTextColor":4278190080,"overlayFScale":3.0,"overlayText":"PARTY NEAR","refActorNPCNameID":4954,"refActorRequireCast":true,"refActorCastId":[27943,27940],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true}},"Phase":2}
```

[EN] Tankbusters during Hallowed Wings:
```
DSR P6 TankBusters~{"ZoneLockH":[968],"DCond":5,"Elements":{"Marks":{"type":1,"radius":10.0,"Donut":0.0,"color":1191116816,"refActorPlaceholder":["<t1>","<t2>"],"refActorComparisonType":5,"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"Hraesvelgr readies Hallowed Wings."}],"Phase":2}
```

[EN] Nidhogg's Cauterize:
```
DSR P6 Nidhogg's Cauterize~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":12.0,"offY":56.0,"radius":11.0,"color":1509968639,"refActorNPCNameID":3458,"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"I swore to Shiva─swore that I would not take the lives of men... Stop me, I prithee!","MatchDelay":14.0}],"Phase":2}
```

[EN] Hraesvelgr's Cauterize:
```
DSR P6 Hraesvelgr's Cauterize~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":12.0,"offY":56.0,"radius":11.0,"color":1509968639,"refActorNPCNameID":4954,"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"Nidhogg readies Akh Morn."}],"Phase":2}
```
[International] Nidhogg's Hot tail/Hot wings:
```
DSR P6 Hot tail / Hot wings~{"ZoneLockH":[968],"Elements":{"Hot tail":{"type":3,"refY":44.0,"radius":8.0,"color":1510006527,"refActorNPCNameID":3458,"refActorRequireCast":true,"refActorCastId":[27949,27950],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true},"Hot wing 1":{"type":3,"refX":-13.0,"refY":44.0,"offX":-13.0,"radius":9.0,"color":1510006527,"refActorNPCNameID":3458,"refActorRequireCast":true,"refActorCastId":[27947,27948],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true},"Hot wing 2":{"type":3,"refX":13.0,"refY":44.0,"offX":13.0,"radius":9.0,"color":1510006527,"refActorNPCNameID":3458,"refActorRequireCast":true,"refActorCastId":[27947,27948],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true}},"Phase":2}
```

[EN] Wroth flames stack/spread aoes display
```
DSR P6 spread/stack debuff aoe markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spread":{"type":1,"radius":5.0,"Donut":0.0,"color":838861055,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2758],"Filled":true},"Stack":{"type":1,"radius":5.0,"Donut":0.0,"color":841481984,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2759],"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"MatchIntl":{"En":"Nidhogg readies Wroth Flames"},"MatchDelay":19.5}],"Phase":2}
```
