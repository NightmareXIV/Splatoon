# Party synergy
[International] First mechanic clones attack (early display) (v3.2.0.7+ required):
```
~Lv2~{"Name":"P2 - M/F clones attacks","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"Omega-M shield","type":1,"radius":10.0,"Donut":20.0,"thicc":4.0,"refActorDataID":15714,"refActorComparisonType":3,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-F staff","type":3,"refY":40.0,"offY":-40.0,"radius":5.2,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true},{"Name":"Omega-F staff","type":3,"refY":40.0,"offY":-40.0,"radius":5.2,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"AdditionalRotation":1.5707964,"refActorUseTransformation":true},{"Name":"Omega-F feetfighter","type":3,"refX":16.0,"refY":40.0,"offX":16.0,"offY":-40.0,"radius":12.0,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-F feetfighter","type":3,"refX":-16.0,"refY":40.0,"offX":-16.0,"offY":-40.0,"radius":12.0,"color":1677721855,"refActorDataID":15715,"refActorComparisonType":3,"includeRotation":true,"onlyUnTargetable":true,"onlyVisible":true,"refActorUseTransformation":true,"refActorTransformationID":4},{"Name":"Omega-M blade","type":1,"radius":10.2,"color":2013266175,"thicc":3.0,"refActorDataID":15714,"refActorComparisonType":3,"onlyUnTargetable":true,"onlyVisible":true,"Filled":true,"refActorUseTransformation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":4.5,"Match":"(7635>31550)","MatchDelay":8.2}]}
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

[International] Optimized Fire III AOE
```
~Lv2~{"Name":"P2 - Optmized Fire III","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"Self","type":1,"radius":7.0,"color":1174470625,"refActorType":1,"Filled":true},{"Name":"Others","type":1,"radius":7.0,"color":3355508705,"thicc":5.0,"refActorPlaceholder":["<2>","<3>","<4>","<5>","<6>","<7>","<8>"],"refActorComparisonType":5}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.0,"Match":"(7635>31550)","MatchDelay":15.0}],"MaxDistance":7.2,"UseDistanceLimit":true,"DistanceLimitType":1}
```

[International] Glitch type reminder
```
~Lv2~{"Name":"P2 - glitch type reminder","Group":"TOP","ZoneLockH":[1122],"ElementsL":[{"Name":"Vulnerability","type":1,"radius":0.0,"overlayBGColor":4278190080,"overlayTextColor":4278255611,"overlayVOffset":2.6,"thicc":0.0,"overlayText":"VULNERABLE","refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3366],"refActorBuffTimeMax":20.0,"refActorType":1},{"Name":"Mid","type":1,"radius":0.0,"overlayBGColor":4278255370,"overlayTextColor":3355443200,"overlayVOffset":3.0,"overlayFScale":1.5,"thicc":0.0,"overlayText":">>> CLOSE <<<","refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3427],"refActorBuffTimeMax":20.0,"refActorType":1},{"Name":"Far","type":1,"radius":0.0,"overlayBGColor":4278190335,"overlayTextColor":3355443200,"overlayVOffset":3.0,"overlayFScale":1.5,"thicc":0.0,"overlayText":"<<< FAR >>>","refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3428],"refActorBuffTimeMax":20.0,"refActorType":1}]}
```

[International] Omega-M aoe attack (stacks). Also serves as relative north finder.
```
~Lv2~{"Name":"P2 - Omega-M aoe","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":10.0,"color":1342215423,"thicc":5.0,"refActorDataID":15714,"refActorComparisonType":3,"onlyVisible":true,"Filled":true},{"Name":"","type":1,"radius":10.0,"color":1677721855,"overlayBGColor":4278190080,"overlayTextColor":4278190335,"overlayFScale":3.0,"thicc":5.0,"overlayText":"NORTH","refActorDataID":15713,"refActorComparisonType":3,"onlyVisible":true,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":12.0,"Match":"(7635>31550)","MatchDelay":20.0}]}
```

[EN] Spots for knockback, depending on glitch type. By default, left is fixed and right adjusts to bottom in mid glitch. Edit mid glitch preset if necessary.
```
~Lv2~{"Name":"P2 - KB spots far","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"Left","type":1,"offX":2.5,"offY":13.0,"radius":1.0,"color":4278255615,"overlayBGColor":0,"overlayTextColor":4278252031,"thicc":5.0,"overlayText":"Left","refActorDataID":15713,"refActorComparisonType":3,"includeRotation":true,"onlyVisible":true},{"Name":"Right","type":1,"offX":-2.5,"offY":13.0,"radius":1.0,"color":4278255615,"overlayBGColor":0,"overlayTextColor":4278252031,"thicc":5.0,"overlayText":"Right","refActorDataID":15713,"refActorComparisonType":3,"includeRotation":true,"onlyVisible":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":12.0,"MatchIntl":{"En":"You suffer the effect of Remote Glitch."},"MatchDelay":14.0}]}
```
```
~Lv2~{"Name":"P2 - KB spots close","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"Bottom","type":1,"offY":15.5,"radius":1.0,"color":4278255615,"overlayBGColor":0,"overlayTextColor":4278252031,"thicc":5.0,"overlayText":"Right","refActorDataID":15713,"refActorComparisonType":3,"includeRotation":true,"onlyVisible":true},{"Name":"Left","type":1,"offX":2.5,"offY":13.0,"radius":1.0,"color":4278255615,"overlayBGColor":0,"overlayTextColor":4278252031,"thicc":5.0,"overlayText":"Left","refActorDataID":15713,"refActorComparisonType":3,"includeRotation":true,"onlyVisible":true},{"Name":"Right","type":1,"Enabled":false,"offX":-2.5,"offY":13.0,"radius":1.0,"color":4278255615,"overlayBGColor":0,"overlayTextColor":4278252031,"thicc":5.0,"overlayText":"Right","refActorDataID":15713,"refActorComparisonType":3,"includeRotation":true,"onlyVisible":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":12.0,"MatchIntl":{"En":"You suffer the effect of Mid Glitch."},"MatchDelay":14.0}]}
```

[International] Optical unit finder. Displays AOE attack coming from the eye, with part of it marked as other color to help to find relative north. Contains optional disabled by default element with tether to optical unit.
```
~Lv2~{"Name":"P2 Optical unit finder - line","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":3,"refY":35.0,"offY":65.0,"radius":8.0,"color":1358954240,"overlayBGColor":0,"overlayTextColor":4278190080,"overlayFScale":7.0,"thicc":5.0,"overlayText":"EYE","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true,"Filled":true},{"Name":"","type":3,"offY":35.0,"radius":8.0,"color":1342177535,"overlayBGColor":0,"overlayTextColor":4278190080,"overlayFScale":7.0,"thicc":5.0,"overlayText":"EYE","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true,"Filled":true},{"Name":"Circle with tether","type":1,"Enabled":false,"offY":27.04,"radius":5.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4278190080,"overlayFScale":7.0,"thicc":5.0,"overlayText":"EYE","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true,"tether":true,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":7.3,"Match":"(7635>31550)","MatchDelay":12.7}]}
```

[International] Earlier optical unit finder. Serves the purpose to find it earlier while not turning screen into a complete mess.
```
~Lv2~{"Name":"P2 Optical unit finder - early circle","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"offY":25.0,"radius":5.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4278190080,"overlayFScale":7.0,"thicc":5.0,"overlayText":"EYE","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true,"Filled":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":20.0,"Match":"(7635>31550)","MatchDelay":5.0}]}
```

## Playstation markers
[EN] For this strat:
- https://ff14.toolboxgaming.space/?id=109976780281761&preview=1#8
- https://ff14.toolboxgaming.space/?id=109976780281761&preview=1#9

Only necessary (far/close) part will be displayed.
```
~Lv2~{"Name":"P2 Playstation spots - Close","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":" right","type":1,"offX":-10.5,"offY":30.0,"radius":1.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4294967040,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-10.5,"offY":40.0,"radius":1.0,"color":4294902015,"overlayBGColor":0,"overlayTextColor":4294902015,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-10.5,"offY":50.0,"radius":1.0,"color":4278190335,"overlayBGColor":0,"overlayTextColor":4278190335,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-10.5,"offY":60.0,"radius":1.0,"color":4278255360,"overlayBGColor":0,"overlayTextColor":4278255360,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":10.5,"offY":30.0,"radius":1.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4294967040,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":10.5,"offY":40.0,"radius":1.0,"color":4294902015,"overlayBGColor":0,"overlayTextColor":4294902015,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":10.5,"offY":50.0,"radius":1.0,"color":4278190335,"overlayBGColor":0,"overlayTextColor":4278190335,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":10.5,"offY":60.0,"radius":1.0,"color":4278255360,"overlayBGColor":0,"overlayTextColor":4278255360,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":13.0,"MatchIntl":{"En":"You suffer the effect of Mid Glitch."},"MatchDelay":3.0}]}
```
```
~Lv2~{"Name":"P2 Playstation spots - far (right flip)","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":" right","type":1,"offX":-12.0,"offY":60.0,"radius":1.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4294967040,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-18.5,"offY":50.0,"radius":1.0,"color":4294902015,"overlayBGColor":0,"overlayTextColor":4294902015,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-18.5,"offY":40.0,"radius":1.0,"color":4278190335,"overlayBGColor":0,"overlayTextColor":4278190335,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" right","type":1,"offX":-12.0,"offY":30.0,"radius":1.0,"color":4278255360,"overlayBGColor":0,"overlayTextColor":4278255360,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":12.0,"offY":30.0,"radius":1.0,"color":4294967040,"overlayBGColor":0,"overlayTextColor":4294967040,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":18.5,"offY":40.0,"radius":1.0,"color":4294902015,"overlayBGColor":0,"overlayTextColor":4294902015,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":18.5,"offY":50.0,"radius":1.0,"color":4278190335,"overlayBGColor":0,"overlayTextColor":4278190335,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true},{"Name":" left","type":1,"offX":12.0,"offY":60.0,"radius":1.0,"color":4278255360,"overlayBGColor":0,"overlayTextColor":4278255360,"overlayFScale":2.0,"thicc":5.0,"overlayText":"","refActorNPCNameID":7640,"refActorComparisonType":6,"includeRotation":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":13.0,"MatchIntl":{"En":"You suffer the effect of Remote Glitch."},"MatchDelay":3.0}]}
```

# Limitless Synergy
[Script] [Beta] Tethers indicator
```
https://github.com/NightmareXIV/Splatoon/raw/master/SplatoonScripts/Duties/Endwalker/The%20Omega%20Protocol/Limitless%20Synergy.cs
```

[International] Optimized Sagittarius Arrow indicator
```
~Lv2~{"Name":"P2 Optimized Sagittarius Arrow","Group":"TOP","ZoneLockH":[1122],"ElementsL":[{"Name":"","type":3,"refY":45.0,"radius":5.0,"color":1342242815,"refActorNPCNameID":7633,"refActorRequireCast":true,"refActorCastId":[31539],"refActorComparisonType":6,"includeRotation":true}]}
```

[Script] Beyond Defense bait indicator. **It's not very reliable unless someone clearly baits it out, I'd not recommend to 100% rely on it**. 
```
https://github.com/NightmareXIV/Splatoon/raw/master/SplatoonScripts/Duties/Endwalker/The%20Omega%20Protocol/Beyond%20Defense.cs
```

# Obsolete
|These presets are outdated and preserved only as a history|
|---|

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
