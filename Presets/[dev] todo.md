# this is developer notes, do not copy from here
p2

priority: dont break people's stuff

[EN] Divebomb Helper: Shows both divebomb safespots [Translation required: trigger]
```
DSR P2 Strength Divebombs~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorNPCNameID":3637,"refActorComparisonType":6,"includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":7.0,"MatchIntl":{"En":"King Thordan readies Strength of the Ward."},"MatchDelay":8.0}],"Phase":2}
```



Divebomb Helper (Alternative): Looks a little nicer but requires more elements. - delete, irrelevant


[EN] Heavy Impact Rings: Places rings around Ser Guerrique indicating the size of the quake rings from Heavy Impact [Translation required: trigger]
```
DSR Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":12.0,"MatchIntl":{"En":"Ser Paulecrain readies Spiral Thrust"}}],"Phase":2}
```

Heavy Impact alternative triggers - delete, irrelevant



p3


[International] Geirskogul
justification: scanning object table can't be worse than comparing strings
```
DSR P3 Geirskogul Lines~{"ZoneLockH":[968],"Elements":{"1":{"type":3,"refY":30.0,"radius":4.0,"color":1174405375,"thicc":4.0,"refActorNPCNameID":3458,"refActorRequireCast":true,"refActorCastId":[26378],"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"Phase":2}
```


[International] Drachenlance
```
DSR P3 Drachenlance Cone~{"ZoneLockH":[968],"Elements":{"1":{"type":4,"refY":15.0,"radius":13.0,"coneAngleMin":-45,"coneAngleMax":45,"color":4294967040,"thicc":3.0,"refActorNPCNameID":3458,"refActorRequireCast":true,"refActorCastId":[26379,26380],"refActorComparisonType":6,"includeRotation":true,"onlyTargetable":true,"Filled":true}},"Phase":2}
```


p5

Wrath of the heavens

[EN] Display tethers (make tethers same as red line) and safe spot as blue marker
```
DSR P5 Wrath of the Heavens resolve~{"ZoneLockH":[903,968],"DCond":5,"Elements":{"Right tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":6.2308254},"Left tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":0.05235988},"Blue marker safe spot":{"type":1,"offX":17.42,"offY":12.22,"radius":0.6,"Donut":0.0,"color":4294901787,"thicc":7.6,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Wrath of the Heavens"},"MatchDelay":8.0}],"Phase":2}
```

[EN] Display safespot under Ser Grinnaux
```
DSR P5 Wrath of the Heavens resolve~{"ZoneLockH":[903,968],"DCond":5,"Elements":{"Right tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":6.2308254},"Left tether":{"type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":0.05235988},"Blue marker safe spot":{"type":1,"offX":17.42,"offY":12.22,"radius":0.6,"Donut":0.0,"color":4294901787,"thicc":7.6,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Wrath of the Heavens"},"MatchDelay":8.0}],"Phase":2}
```


[International] Display chain lightning radius around people.
```
DSR P5 Wrath of the Heavens Chain Lightning~{"ZoneLockH":[968],"Elements":{"1":{"type":1,"offY":0.14,"radius":5.0,"Donut":0.0,"color":1694433303,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2833],"Filled":true}}}
```


Death of the heavens

[EN, JP] The second set of quakes seen in P5:
```
DSR P5 Death of the Heavens Quake Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"Donut":0.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":15.0}],"Phase":2}
```
[EN, JP] Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR P5 Death of the Heavens DIve Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3633,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":13.0}],"Phase":2}
```


p6

[EN] Wroth flames stack/spread aoes display
```
DSR P6 spread/stack debuff aoe markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spread":{"type":1,"radius":5.0,"Donut":0.0,"color":838861055,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2758],"Filled":true},"Stack":{"type":1,"radius":5.0,"Donut":0.0,"color":841481984,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2759],"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"MatchIntl":{"En":"Nidhogg readies Wroth Flames"},"MatchDelay":19.5}],"Phase":2}
```
