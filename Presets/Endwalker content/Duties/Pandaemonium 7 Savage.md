## All of these presets are not much tested, not precisely tweaked and not final
[EN] Stack tankbuster:
```
~Lv2~{"Name":"P7S Stack Tankbuster","Group":"P7S","ZoneLockH":[1086],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":9.0,"color":1342242601,"refActorPlaceholder":["<t1>","<t2>"],"refActorComparisonType":5,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"Agdistis begins casting Condensed Aero II."}}]}
```

[EN] Spread tankbuster:
```
~Lv2~{"Name":"P7S Spread Tankbuster","Group":"P7S","ZoneLockH":[1086],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":9.0,"color":1342242792,"refActorPlaceholder":["<t1>","<t2>"],"refActorComparisonType":5,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"Agdistis begins casting Dispersed Aero II."}}]}
```

[International] First exaflare safespot finder
```
~Lv2~{"Name":"P7S First Exaflare Safespot","Group":"P7S","ZoneLockH":[1086],"DCond":5,"ElementsL":[{"Name":"","type":3,"refY":40.0,"radius":7.0,"color":1342242303,"refActorNPCNameID":11374,"refActorRequireCast":true,"refActorCastId":[30767],"refActorComparisonType":6,"includeRotation":true}],"UseTriggers":true,"Triggers":[{"TimeBegin":10.0,"Duration":60.0}]}
```

[International] Birds line AOE
```
~Lv2~{"Name":"P7S Immature Stymphalide Line","Group":"P7S","ZoneLockH":[1086],"ElementsL":[{"Name":"","type":3,"refY":50.0,"radius":4.0,"color":1174405375,"refActorNPCNameID":11379,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}]}
```

[International] Circle behemoth AOE
```
~Lv2~{"Name":"P7S Immature Io Circle AOE","Group":"P7S","ZoneLockH":[1086],"ElementsL":[{"Name":"","type":1,"radius":10.0,"color":1006633215,"refActorNPCNameID":11378,"refActorComparisonType":6,"onlyVisible":true,"Filled":true}]}
```

[International] Spread: first preset displays AOE circle around you when debuff is about to expire. 
```
~Lv2~{"Name":"P7S Spreads","Group":"P7S","ZoneLockH":[1086],"ElementsL":[{"Name":"3397 - spread","type":1,"radius":6.2,"color":1175846656,"overlayBGColor":4278190080,"overlayTextColor":4280024832,"overlayVOffset":2.0,"overlayFScale":2.0,"overlayText":"<<< SPREAD >>>","refActorNameIntl":{"En":"*"},"refActorRequireBuff":true,"refActorBuffId":[3397,3308,3310,3391,3392,3393],"refActorUseBuffTime":true,"refActorBuffTimeMax":8.0,"refActorType":1,"Filled":true}]}
```

[International] Spread: highlights players with debuff if you're standing too close to them. **This is preset for DPS, if you are tank, replace placeholders to `<t2>`, `<h1>` and `<h2>`, if you're healer, replace them to `<t1>`, `<t2>`, `<h2>`**.
```
~Lv2~{"Name":"P7S Other Players Spreads","Group":"P7S","ZoneLockH":[1086],"ElementsL":[{"Name":"3397 - spread","type":1,"radius":6.2,"color":3355508706,"overlayBGColor":4278190080,"overlayTextColor":4280024832,"overlayVOffset":2.0,"overlayFScale":2.0,"thicc":4.0,"overlayText":"<<< SPREAD >>>","refActorPlaceholder":["<d2>","<d3>","<d4>"],"refActorRequireBuff":true,"refActorBuffId":[3397,3308,3310,3391,3392,3393],"refActorUseBuffTime":true,"refActorBuffTimeMax":6.0,"refActorComparisonType":5}],"MaxDistance":6.2,"UseDistanceLimit":true,"DistanceLimitType":1}
```

[International] Purgation stack safe spot. Remembers when first stack was dropped and highlights that spot.
```
~Lv2~{"Name":"P7S Purgation safe spot drop","Group":"P7S","ZoneLockH":[1086],"ElementsL":[{"Name":"","type":1,"radius":3.0,"color":4294573824,"thicc":5.0,"overlayText":"Stacks here","refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3311],"refActorUseBuffTime":true,"refActorBuffTimeMax":0.5,"onlyTargetable":true}],"Freezing":true,"FreezeFor":65.0,"IntervalBetweenFreezes":65.0}
```

[International] Bought of Attis - OUT
```
~Lv2~{"Name":"P7S Bought of Attis - OUT","Group":"P7S","ZoneLockH":[1086,1085],"ElementsL":[{"Name":"","type":1,"offY":18.5,"offZ":-1.0,"radius":20.0,"color":1342177535,"refActorNPCNameID":11374,"refActorRequireCast":true,"refActorCastId":[30753],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true,"AdditionalRotation":5.398303,"Filled":true},{"Name":"","type":1,"offY":18.5,"offZ":-1.0,"radius":20.0,"color":1342177535,"refActorNPCNameID":11374,"refActorRequireCast":true,"refActorCastId":[30753],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true,"AdditionalRotation":0.884882,"Filled":true}]}
```
