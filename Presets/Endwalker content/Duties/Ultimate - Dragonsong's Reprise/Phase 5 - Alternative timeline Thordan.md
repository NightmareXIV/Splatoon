### Wrath of the heavens
[EN] Display tethers (make tethers same as red line) and safe spot as blue marker
```
~Lv2~{"Name":"DSR P5 Wrath of the Heavens resolve","Group":"DSR","ZoneLockH":[903,968],"DCond":5,"ElementsL":[{"Name":"Right tether","type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3638,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":6.2308254},{"Name":"Left tether","type":3,"refY":43.0,"radius":0.0,"refActorNPCNameID":3636,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"AdditionalRotation":0.05235988},{"Name":"Blue marker safe spot","type":1,"offX":17.42,"offY":12.22,"radius":0.6,"color":4294901787,"thicc":7.6,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Wrath of the Heavens"},"MatchDelay":8.0}],"Phase":2}
```

[EN] Display safespot under Ser Grinnaux
```
~Lv2~{"Name":"DSR Ser Grinnaux Empty dimension","Group":"DSR","ZoneLockH":[968],"DCond":5,"ElementsL":[{"Name":"1","type":1,"radius":2.0,"thicc":5.0,"refActorName":"Ser Grinnaux","FillStep":1.0,"includeHitbox":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":20.0,"Match":"King Thordan readies Wrath of the Heavens","MatchDelay":10.0}]}
```

[International] Display chain lightning radius around people.
```
~Lv2~{"Name":"DSR P5 Wrath of the Heavens Chain Lightning","Group":"DSR","ZoneLockH":[968],"ElementsL":[{"Name":"1","type":1,"offY":0.14,"radius":5.0,"color":1694433303,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[2833],"Filled":true}]}
```

[International] Display Ascalon's Mercy Revealed on you. Might be not precise.
```
~Lv2~{"Name":"DSR p5 thordan cleave","Group":"DSR","ZoneLockH":[968],"ElementsL":[{"Name":"Thordan cleave","type":4,"radius":20.0,"coneAngleMin":-20,"coneAngleMax":20,"color":2885746175,"refActorNPCNameID":3632,"refActorRequireCast":true,"refActorCastId":[25546,25547],"FillStep":1.0,"refActorComparisonType":6,"includeRotation":true,"Filled":true,"FaceMe":true}],"Phase":2}
```

### Death of the Heavens
[EN, JP] Relative north marker during Death of the Heavens
```
~Lv2~{"Name":"DSR P5 DeathOTH North","Group":"DSR","ZoneLockH":[968],"DCond":5,"ElementsL":[{"Name":"Circle","type":1,"offY":-10.0,"radius":3.53,"color":4278253567,"overlayBGColor":4278253567,"overlayTextColor":4278190080,"overlayFScale":3.0,"thicc":7.8,"overlayText":"North","refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true},{"Name":"Tether","type":3,"refY":5.8,"offY":-6.66,"radius":0.0,"color":4278253567,"overlayBGColor":4294901764,"overlayTextColor":4294967295,"overlayFScale":3.0,"thicc":5.0,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":8.5}]}
```

[EN, JP] The second set of quakes seen in P5:
```
~Lv2~{"Name":"DSR P5 Death of the Heavens Quake Markers","Group":"DSR","ZoneLockH":[968],"DCond":5,"ElementsL":[{"Name":"Quake marker","type":1,"radius":6.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},{"Name":"2","type":1,"radius":12.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},{"Name":"3","type":1,"radius":18.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},{"Name":"4","type":1,"radius":24.0,"color":4293721856,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":15.0}],"Phase":2}
```

[EN, JP] Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
~Lv2~{"Name":"DSR P5 Death of the Heavens DIve Markers","Group":"DSR","ZoneLockH":[968],"DCond":5,"ElementsL":[{"Name":"Spear of the Fury","type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3633,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},{"Name":"Cauterize","type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorNPCNameID":3641,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true},{"Name":"Twisting Dive","type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorNPCNameID":3984,"refActorComparisonType":6,"includeRotation":true,"onlyUnTargetable":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"MatchIntl":{"En":"King Thordan readies Death of the Heavens","Jp":"騎神トールダンは「至天の陣：死刻」の構え。"},"MatchDelay":13.0}],"Phase":2}
```
