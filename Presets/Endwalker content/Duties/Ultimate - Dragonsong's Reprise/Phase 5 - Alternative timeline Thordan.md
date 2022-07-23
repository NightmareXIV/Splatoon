### Wrath of the heavens
[EN] Display tethers (make tethers same as red line) and safe spot as blue marker
```
DSR P5 Wrath of the Heavens resolve~{"ZoneLockH":[903,968],"DCond":5,"Elements":{"Right tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":6.2308254},"Left tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":0.05235988},"Blue marker safe spot":{"type":1,"offX":17.42,"offY":12.22,"radius":0.6,"Donut":0.0,"color":4294901787,"thicc":7.6,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Wrath of the Heavens"},"MatchDelay":8.0}],"Phase":2}
```

[EN] Display safespot under Ser Grinnaux
```
DSR P5 Wrath of the heavens - Ser Grinnaux's Empty Dimension safe spot~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"Donut":0.0,"thicc":5.0,"refActorNPCNameID":3639,"refActorComparisonType":6,"includeHitbox":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":22.0,"MatchIntl":{"En":"King Thordan readies Wrath of the Heavens"},"MatchDelay":10.0}]}
```

[International] Display chain lightning radius around people.
```
DSR P5 Wrath of the Heavens Chain Lightning~{"ZoneLockH":[968],"Elements":{"1":{"type":1,"offY":0.14,"radius":5.0,"Donut":0.0,"color":1694433303,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2833],"Filled":true}}}
```

Display Ascalon's Mercy Revealed on you. Might be not precise.
```
DSR P5 thordan cleave~{"ZoneLockH":[968],"Elements":{"Thordan cleave":{"type":4,"radius":20.0,"coneAngleMin":-20,"coneAngleMax":20,"color":2885746175,"refActorName":"King Thordan","refActorRequireCast":true,"refActorCastId":[25546,25547],"includeRotation":true,"Filled":true,"FaceMe":true}},"Phase":2}
```

### Death of the Heavens

[EN, JP] The second set of quakes seen in P5:
```
DSR P5 Death of the Heavens Quake Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":15.0}],"Phase":2}
```

[EN, JP] Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR P5 Death of the Heavens DIve Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3633,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":13.0}],"Phase":2}
```
