# Intermission
[International] Wave Repeater
```
~Lv2~{"Name":"P3 Wave Repeater","Group":"TOP","ZoneLockH":[1122],"ElementsL":[{"Name":"1","type":1,"radius":6.0,"color":2348810495,"refActorNPCNameID":7636,"refActorRequireCast":true,"refActorCastId":[31567],"refActorUseCastTime":true,"refActorCastTimeMax":5.5,"refActorUseOvercast":true,"refActorComparisonType":6,"Filled":true},{"Name":"2","type":1,"radius":6.0,"Donut":6.0,"thicc":5.0,"refActorNPCNameID":7636,"refActorRequireCast":true,"refActorCastId":[31567],"refActorUseCastTime":true,"refActorCastTimeMin":5.5,"refActorCastTimeMax":7.5,"refActorUseOvercast":true,"refActorComparisonType":6},{"Name":"3","type":1,"radius":12.0,"Donut":6.0,"thicc":5.0,"refActorNPCNameID":7636,"refActorRequireCast":true,"refActorCastId":[31567],"refActorUseCastTime":true,"refActorCastTimeMin":7.5,"refActorCastTimeMax":9.5,"refActorUseOvercast":true,"refActorComparisonType":6},{"Name":"4","type":1,"radius":18.0,"Donut":6.0,"thicc":5.0,"refActorNPCNameID":7636,"refActorRequireCast":true,"refActorCastId":[31567],"refActorUseCastTime":true,"refActorCastTimeMin":9.5,"refActorCastTimeMax":11.5,"refActorUseOvercast":true,"refActorComparisonType":6}]}
```

[International] [Untested] Stack/spread indicators
```
~Lv2~{"Name":"P3S Stack/spread debuffs","Group":"TOP","ZoneLockH":[1122],"ElementsL":[{"Name":"stack","type":1,"radius":5.5,"color":1358954495,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3426],"refActorUseBuffTime":true,"refActorBuffTimeMax":15.0,"Filled":true},{"Name":"spread","type":1,"radius":5.5,"color":1358889215,"refActorName":"*","refActorRequireBuff":true,"refActorBuffId":[3425],"refActorUseBuffTime":true,"refActorBuffTimeMax":15.0,"Filled":true}]}
```

[EN] [Beta] Hands explosions, requires Splatoon v3.2.1.0+
```
~Lv2~{"Name":"P3 Hands explosion - 1","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":11.0,"color":1174405375,"refActorNameIntl":{"En":" arm unit"},"Filled":true,"LimitDistance":true,"LimitDistanceInvert":true,"DistanceSourceX":100.0,"DistanceSourceY":100.0,"DistanceMax":1.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"MatchIntl":{"En":"Final analysis. Drastic overhaul of combat logic required."}}],"Freezing":true,"FreezeFor":13.5,"IntervalBetweenFreezes":20.0}
```
```
~Lv2~{"Name":"P3 Hands explosion - 2","Group":"TOP","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":11.0,"color":1174405375,"refActorNameIntl":{"En":" arm unit"},"refActorRequireCast":true,"refActorCastReverse":true,"refActorCastId":[31566],"Filled":true,"LimitDistanceInvert":true,"DistanceSourceX":100.0,"DistanceSourceY":100.0,"DistanceMax":1.0}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"Match":">31566)","MatchDelay":0.5,"FireOnce":true}],"Freezing":true,"FreezeFor":5.0,"IntervalBetweenFreezes":5.0,"FreezeDisplayDelay":1.0}
```

# Battle
[International] [Beta] [Script] Hello world helper. Work in progress. Based on Maxwell toolbox: https://ff14.toolboxgaming.space/?id=073180945764761&preview=1
```
https://github.com/NightmareXIV/Splatoon/raw/master/SplatoonScripts/Duties/Endwalker/The%20Omega%20Protocol/Hello%20World.cs
```

[International] Oversampled Wave Cannon left
```
~Lv2~{"Name":"P3 Oversampled Wave Cannon left","Group":"Omega / 絶オメガ検証戦","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"omega","type":3,"refX":-10.0,"refY":21.76,"offX":-10.0,"offY":-25.02,"radius":10.0,"color":335544575,"thicc":0.6,"refActorNPCID":7636,"FillStep":0.1,"refActorComparisonType":4,"includeRotation":true,"onlyVisible":true},{"Name":"Player left","type":3,"offY":10.0,"radius":20.0,"color":167772415,"thicc":1.0,"refActorRequireBuff":true,"refActorBuffId":[3453],"FillStep":0.1,"refActorComparisonType":3,"includeRotation":true,"AdditionalRotation":1.5707964,"Filled":true},{"Name":"Player left1","type":3,"offY":-10.0,"radius":20.0,"color":167772415,"thicc":1.0,"refActorRequireBuff":true,"refActorBuffId":[3452],"FillStep":0.1,"refActorComparisonType":1,"includeRotation":true,"AdditionalRotation":1.5707964}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"Match":"(7636>31596)"}]}
```

[International] Oversampled Wave Cannon right
```
~Lv2~{"Name":"P3 Oversampled Wave Cannon right","Group":"Omega / 絶オメガ検証戦","ZoneLockH":[1122],"DCond":5,"ElementsL":[{"Name":"omega","type":3,"refX":10.0,"refY":21.76,"offX":10.0,"offY":-25.02,"radius":10.0,"color":335544575,"thicc":0.6,"refActorNPCID":7636,"FillStep":0.1,"refActorComparisonType":4,"includeRotation":true,"onlyVisible":true},{"Name":"Player left","type":3,"refY":-10.0,"radius":20.0,"color":167772415,"thicc":1.0,"refActorRequireBuff":true,"refActorBuffId":[3453],"FillStep":0.1,"refActorComparisonType":3,"includeRotation":true,"AdditionalRotation":1.5707964,"Filled":true},{"Name":"Player left1","type":3,"offY":10.0,"radius":20.0,"color":167772415,"thicc":1.0,"refActorRequireBuff":true,"refActorBuffId":[3452],"FillStep":0.1,"refActorComparisonType":1,"includeRotation":true,"AdditionalRotation":1.5707964}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":10.0,"Match":"(7636>31595)"}]}
```

# Obsolete
Obsolete presets serve as a history and not recommended for use

[International] Hello World
```
~Lv2~{"Name":"P3 Hello World   International","Group":"","ZoneLockH":[1122],"ElementsL":[{"Name":"Remote Code Smell","type":1,"radius":0.0,"overlayBGColor":3355443200,"overlayTextColor":4278255614,"overlayVOffset":1.0,"thicc":0.0,"overlayText":"blueline near","refActorRequireBuff":true,"refActorBuffId":[3441],"refActorUseBuffTime":true,"refActorBuffTimeMax":20.0,"refActorComparisonType":3,"onlyVisible":true},{"Name":"Local Code Smell","type":1,"radius":0.0,"overlayBGColor":3355443200,"overlayTextColor":4278255614,"overlayVOffset":1.0,"thicc":0.0,"overlayText":"Green line away","refActorRequireBuff":true,"refActorBuffId":[3503],"refActorUseBuffTime":true,"refActorBuffTimeMax":20.0,"refActorComparisonType":3},{"Name":"Critical Synchronization Bug ","type":1,"radius":0.0,"overlayTextColor":4278255614,"overlayVOffset":-1.0,"thicc":0.0,"overlayText":"Share","refActorRequireBuff":true,"refActorBuffId":[3524],"refActorUseBuffTime":true,"refActorBuffTimeMax":21.0,"refActorComparisonType":3},{"Name":"Critical Overflow Bug","type":1,"radius":0.0,"overlayTextColor":4278255614,"overlayVOffset":-1.0,"thicc":0.0,"overlayText":"Circle","refActorRequireBuff":true,"refActorBuffId":[3525],"refActorUseBuffTime":true,"refActorBuffTimeMax":21.0,"refActorComparisonType":3},{"Name":"red poison ","type":1,"radius":0.0,"overlayTextColor":4278255614,"thicc":0.0,"overlayText":"GO RED TOWER ","refActorRequireBuff":true,"refActorBuffId":[3526],"refActorUseBuffTime":true,"refActorBuffTimeMin":6.0,"refActorBuffTimeMax":26.0,"refActorComparisonType":3},{"Name":"blue poison ","type":1,"radius":0.0,"overlayTextColor":4278255614,"thicc":0.0,"overlayText":"GO BLUE TOWER ","refActorRequireBuff":true,"refActorBuffId":[3429],"refActorUseBuffTime":true,"refActorBuffTimeMin":6.0,"refActorBuffTimeMax":26.0,"refActorComparisonType":3,"onlyVisible":true},{"Name":"red poison AOE","type":1,"radius":5.0,"color":1677721855,"overlayTextColor":4278255614,"refActorRequireBuff":true,"refActorBuffId":[3526],"refActorUseBuffTime":true,"refActorBuffTimeMax":6.0,"refActorComparisonType":3,"Filled":true},{"Name":"blue poison  AOE","type":1,"radius":5.0,"color":855613952,"overlayTextColor":4278255614,"thicc":1.0,"refActorRequireBuff":true,"refActorBuffId":[3429],"refActorUseBuffTime":true,"refActorBuffTimeMax":6.0,"refActorComparisonType":3,"onlyVisible":true,"Filled":true},{"Name":"Blue line  near","type":1,"radius":0.0,"overlayTextColor":4278255614,"overlayVOffset":1.0,"overlayFScale":1.5,"thicc":0.0,"overlayText":"Near","refActorRequireBuff":true,"refActorBuffId":[3530],"refActorComparisonType":3},{"Name":"Green line away","type":1,"radius":0.0,"overlayTextColor":4278255614,"overlayVOffset":1.0,"overlayFScale":1.5,"thicc":0.0,"overlayText":"Away","refActorRequireBuff":true,"refActorBuffId":[3529],"refActorComparisonType":3,"onlyVisible":true}]}
```
