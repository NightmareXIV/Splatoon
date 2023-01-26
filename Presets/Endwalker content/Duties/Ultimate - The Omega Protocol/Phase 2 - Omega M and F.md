[International] [Beta] First mechanic clones attack (early display) (v3.2.0.7+ required):
```
~Lv2~{"Name":"P2 - M/F clones attacks","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"Omega-M shield","type":1,"radius":9.0,"Donut":20.0,"thicc":4.0,"refActorDataID":15714,"refActorComparisonType":3,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-F staff","type":3,"refY":40.0,"offY":-40.0,"radius":5.2,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true},{"Name":"Omega-F staff","type":3,"refY":40.0,"offY":-40.0,"radius":5.2,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"AdditionalRotation":1.5707964,"refActorUseTransformation":true},{"Name":"Omega-F feetfighter","type":3,"refX":16.0,"refY":40.0,"offX":16.0,"offY":-40.0,"radius":12.0,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-F feetfighter","type":3,"refX":-16.0,"refY":40.0,"offX":-16.0,"offY":-40.0,"radius":12.0,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-M blade","type":1,"radius":10.2,"color":2013266175,"thicc":3.0,"refActorDataID":15714,"refActorComparisonType":3,"onlyUnTargetable":true,"onlyVisible":true,"Filled":true,"refActorUseTransformation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.5,"Match":"(7635>31550)","MatchDelay":8.2}]}
```

[International] Eye Ray (huge AOE done by eye in background). You must import all 4 presets.
```
~Lv2~{"Name":"P2 Eye ray AC / 眼睛直線激光 A C","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"1","type":2,"refX":100.27825,"refY":125.0,"offX":99.86256,"offY":75.0,"offZ":-5.456968E-12,"radius":8.0,"color":1677787135,"FillStep":0.2}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.5,"Match":"MapEffect: 5, 1, 2","MatchDelay":8.0},{"Type":2,"Duration":3.5,"Match":"MapEffect: 1, 1, 2","MatchDelay":8.0}]}
```
```
~Lv2~{"Name":"P2 Eye ray 13 / 眼睛直線激光 1 3","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"1","type":2,"refX":118.162,"refY":81.36,"offX":84.967,"offY":115.144,"offZ":-5.456968E-12,"radius":8.0,"color":1677787135,"FillStep":0.2}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.5,"Match":"MapEffect: 6, 1, 2","MatchDelay":8.0},{"Type":2,"Duration":3.5,"Match":"MapEffect: 2, 1, 2","MatchDelay":8.0}]}
```
```
~Lv2~{"Name":"P2 Eye ray BD / 眼睛直線激光 B D","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"1","type":2,"refX":80.0,"refY":100.0,"refZ":3.8146918E-06,"offX":120.0,"offY":100.0,"offZ":-5.456968E-12,"radius":8.0,"color":1677787135,"FillStep":0.2}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.5,"Match":"MapEffect: 7, 1, 2","MatchDelay":8.0},{"Type":2,"Duration":3.5,"Match":"MapEffect: 3, 1, 2","MatchDelay":8.0}]}
```
```
~Lv2~{"Name":"P2 Eye ray 24 / 眼睛直線激光 2 4","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"1","type":2,"refX":83.40503,"refY":83.29182,"offX":114.622086,"offY":114.61661,"offZ":-5.456968E-12,"radius":8.0,"color":1677787135,"FillStep":0.2}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.5,"Match":"MapEffect: 8, 1, 2","MatchDelay":8.0},{"Type":2,"Duration":3.5,"Match":"MapEffect: 4, 1, 2","MatchDelay":8.0}]}
```

[International] Playstation chain extender. Displays chain markers above all players until mechanics resolves.
```
~Lv2~{"Name":"P2 - Playstation Extender","Group":"TOP","ZoneLockH":[1122,990],"ElementsL":[{"Name":"","type":1,"radius":0.0,"overlayBGColor":0,"overlayTextColor":4278190259,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":0.0,"overlayText":"","refActorComparisonType":7,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_01c.avfx","refActorVFXMin":7000,"refActorVFXMax":15000},{"Name":"","type":1,"radius":0.0,"overlayBGColor":0,"overlayTextColor":4290904258,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":0.0,"overlayText":"","refActorComparisonType":7,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_03c.avfx","refActorVFXMin":7000,"refActorVFXMax":15000},{"Name":"","type":1,"radius":0.0,"overlayBGColor":0,"overlayTextColor":4294932224,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":0.0,"overlayText":"","refActorComparisonType":7,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_04c.avfx","refActorVFXMin":7000,"refActorVFXMax":15000},{"Name":"","type":1,"radius":0.0,"overlayBGColor":0,"overlayTextColor":4278236428,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":0.0,"overlayText":"","refActorComparisonType":7,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_02c.avfx","refActorVFXMin":7000,"refActorVFXMax":15000}]}
```

[International] Dedicated playstation marker and tether for you + your partner for the whole duration of mechanic. Import all presets. Requires Splatoon v3.2.0.8+.
```
~Lv2~{"Name":"P2 - Playstation Partner ","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"color":4278190259,"overlayBGColor":0,"overlayTextColor":4278190259,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":4.0,"overlayText":"","refActorComparisonType":7,"tether":true,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_01c.avfx","refActorVFXMax":25000}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"VFX vfx/lockon/eff/z3oz_firechain_01c.avfx spawned on me npc"}]}
```
```
~Lv2~{"Name":"P2 - Playstation Partner ","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"color":4290904258,"overlayBGColor":0,"overlayTextColor":4290904258,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":4.0,"overlayText":"","refActorComparisonType":7,"tether":true,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_03c.avfx","refActorVFXMax":25000}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"VFX vfx/lockon/eff/z3oz_firechain_03c.avfx spawned on me npc"}]}
```
```
~Lv2~{"Name":"P2 - Playstation Partner ","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"color":4294932224,"overlayBGColor":0,"overlayTextColor":4294932224,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":4.0,"overlayText":"","refActorComparisonType":7,"tether":true,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_04c.avfx","refActorVFXMax":25000}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"VFX vfx/lockon/eff/z3oz_firechain_04c.avfx spawned on me npc"}]}
```
```
~Lv2~{"Name":"P2 - Playstation Partner ","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"color":4278236428,"overlayBGColor":0,"overlayTextColor":4278236428,"overlayVOffset":1.5,"overlayFScale":3.0,"thicc":4.0,"overlayText":"","refActorComparisonType":7,"tether":true,"refActorVFXPath":"vfx/lockon/eff/z3oz_firechain_02c.avfx","refActorVFXMax":25000}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":25.0,"Match":"VFX vfx/lockon/eff/z3oz_firechain_02c.avfx spawned on me npc"}]}
```

P2 Omega-F Kick back / 吹飛安全点
```
~Lv2~{"Name":"P2 Omega-F Kick back / 吹飛安全点","Group":"Omega / 絶オメガ検証戦","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"31521 - 鋼鉄","type":4,"radius":10.0,"coneAngleMin":-269,"coneAngleMax":90,"color":2516647908,"overlayBGColor":2483027968,"overlayTextColor":4280024832,"overlayVOffset":2.0,"thicc":4.0,"refActorNPCNameID":7633,"FillStep":1.798,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":11.0,"Match":"(7640>31521)","MatchDelay":2.0}]}
```
