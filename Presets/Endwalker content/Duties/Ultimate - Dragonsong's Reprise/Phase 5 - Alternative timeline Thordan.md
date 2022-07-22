### Wrath of the heavens
Display tethers (make tethers same as red line) and safe spot for blue marker (**WIP, not precise yet**)
```
DSR Wrath of the Heavens resolve~{"ZoneLockH":[903,968],"DCond":5,"Elements":{"Right tether":{"type":3,"refY":43.0,"radius":0.0,"refActorName":"Ser Ignasse","includeRotation":true,"onlyVisible":true},"Left tether":{"type":3,"refY":43.0,"radius":0.0,"refActorName":"Ser Vellguine","includeRotation":true,"onlyVisible":true},"Blue marker safe spot":{"type":1,"offX":17.32,"offY":11.54,"radius":0.6,"color":4294901787,"thicc":7.6,"refActorName":"Vedrfolnir","includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"King Thordan readies Wrath of the Heavens","MatchDelay":8.0}],"Phase":2}
```

Display safespot under Ser Grinnaux
```
DSR Ser Grinnaux Empty dimension~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"thicc":5.0,"refActorName":"Ser Grinnaux","includeHitbox":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":30.0,"Match":"King Thordan readies Wrath of the Heavens","MatchDelay":10.0}]}
```

Display chain lightning radius around people. Whether it is precise - needs to be confirmed yet.
```
DSR Chain Lightning~{"ZoneLockH":[968],"Elements":{"1":{"type":1,"offY":0.14,"radius":5.0,"color":1694433303,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2833],"Filled":true}}}
```

Display Ascalon's Mercy Revealed on you. Might be not precise.
```
DSR p5 thordan cleave~{"ZoneLockH":[968],"Elements":{"Thordan cleave":{"type":4,"radius":20.0,"coneAngleMin":-20,"coneAngleMax":20,"color":2885746175,"refActorName":"King Thordan","refActorRequireCast":true,"refActorCastId":[25546,25547],"includeRotation":true,"Filled":true,"FaceMe":true}},"Phase":2}
```

### Death of the Heavens

The second set of quakes seen in P5:
```
DSR P5 Quake Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```

Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR P5 DIve Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Ser Zephirin","includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Vedrfolnir","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```
