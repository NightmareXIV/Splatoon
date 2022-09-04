[International] Display danger zone on Gorgons mechanic cast.
```
~Lv2~{"Name":"P8S Out/Gorgons","Group":"P8S-1","ZoneLockH":[1088],"ElementsL":[{"Name":"","type":1,"radius":0.2,"color":2348810495,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[31052],"refActorUseCastTime":true,"refActorCastTimeMax":12.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeHitbox":true,"onlyTargetable":true,"Filled":true}]}
```

[International] Multipreset: displays snakes, birds and tiles AOEs during various mechanics.
```
~Lv2~{"Name":"P8S Snakes_ birds and tiles","Group":"P8S-1","ElementsL":[{"Name":"Arena tiles/光劍畫地板","type":3,"refY":-5.0,"offY":5.0,"radius":5.0,"color":1687393270,"thicc":5.0,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[31015],"refActorComparisonType":6,"includeRotation":true,"Filled":true},{"Name":"Sunforge (bird 1)去中間(畫圖右)","type":3,"refX":14.0,"refY":20.0,"offX":14.0,"offY":-20.0,"radius":7.0,"color":1683540695,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[30993],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true,"LineAddHitboxLengthXA":true},{"Name":"Sunforge (bird 2)去中間(畫圖左)","type":3,"refX":-14.0,"refY":20.0,"offX":-14.0,"offY":-20.0,"radius":7.0,"color":1683540695,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[30993],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true,"LineAddHitboxLengthXA":true},{"Name":"Sunforge (snake)/去兩邊","type":3,"refY":20.0,"offY":-20.0,"radius":7.0,"color":1683540695,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[30992],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true},{"Name":"Creation at command (middle)/分身去兩邊","type":3,"refY":40.0,"radius":7.0,"color":1683540695,"refActorNPCNameID":11405,"refActorRequireCast":true,"refActorCastId":[31058],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true,"DistanceSourceX":101.73789,"DistanceSourceY":113.62963},{"Name":"Creation at command (sides)/分身去中間","type":3,"refY":7.0,"offY":20.0,"radius":40.0,"color":1683540695,"refActorNPCNameID":11405,"refActorRequireCast":true,"refActorCastId":[31059],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true,"DistanceSourceX":101.73789,"DistanceSourceY":113.62963},{"Name":"Fourfold/畫火圈 ","type":1,"radius":24.0,"color":1349909939,"thicc":5.0,"refActorNPCNameID":11404,"refActorRequireCast":true,"refActorCastId":[31054,31013],"refActorComparisonType":6,"Filled":true},{"Name":"跳躍鋼鐵","type":1,"offY":-20.0,"radius":30.0,"color":1358819445,"thicc":5.0,"refActorNPCNameID":11399,"refActorRequireCast":true,"refActorCastId":[31237],"refActorComparisonType":6,"Filled":true}]}
```

[EN] Octaflare, Tetraflare and Diflare storage. Places a sign on a boss indicating which flare it currently stores.
```
~Lv2~{"Name":"P8S Octaflare reminder","Group":"P8S-1","ZoneLockH":[1088],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"overlayBGColor":3355443200,"overlayTextColor":4278255599,"overlayFScale":1.5,"thicc":0.0,"overlayText":"< Stored: SPREAD >","refActorNPCNameID":11399,"refActorUseCastTime":true,"refActorCastTimeMax":60.0,"refActorUseOvercast":true,"refActorRequireBuff":true,"refActorBuffId":[2552],"refActorComparisonType":6,"onlyTargetable":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":60.0,"MatchIntl":{"En":"Hephaistos begins casting Conceptual Octaflare."}}]}
```
```
~Lv2~{"Name":"P8S Tetraflare reminder","Group":"P8S-1","ZoneLockH":[1088],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"overlayBGColor":3355443200,"overlayTextColor":4294965504,"overlayFScale":1.5,"thicc":0.0,"overlayText":"> Stored: 2 STACK <","refActorNPCNameID":11399,"refActorUseCastTime":true,"refActorCastTimeMax":60.0,"refActorUseOvercast":true,"refActorRequireBuff":true,"refActorBuffId":[2552],"refActorComparisonType":6,"onlyTargetable":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":60.0,"MatchIntl":{"En":"Hephaistos begins casting Conceptual Tetraflare."}}]}
```
```
~Lv2~{"Name":"P8S Diflare reminder","Group":"P8S-1","ZoneLockH":[1088],"DCond":5,"ElementsL":[{"Name":"","type":1,"radius":0.0,"overlayBGColor":3355443200,"overlayTextColor":4278190335,"overlayFScale":1.5,"thicc":0.0,"overlayText":">>> Stored: 4 STACK <<<","refActorNPCNameID":11399,"refActorUseCastTime":true,"refActorCastTimeMax":60.0,"refActorUseOvercast":true,"refActorRequireBuff":true,"refActorBuffId":[2552],"refActorComparisonType":6,"onlyTargetable":true}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":60.0,"MatchIntl":{"En":"Hephaistos begins casting Conceptual Diflare."}}]}
```

[International] Gorgons (both): see where gorgons will end up early
```
~Lv2~{"Name":"P8S Early see Gorgons","Group":"P8S-1","ZoneLockH":[1088],"ElementsL":[{"Name":"","type":1,"radius":3.0,"color":3372155125,"thicc":5.0,"refActorNPCNameID":11517,"refActorRequireCast":true,"refActorCastId":[31019],"refActorComparisonType":6,"tether":true,"Filled":true}]}
```

[EN] Gorgons (both): your line of sight. When combined with previous preset: make sure that no purple lines are within red lines and you're good. Sorry, no "don't look" image this time.
```
~Lv2~{"Name":"P8S Player sight to avoid Gorgons","Group":"P8S-1","ZoneLockH":[1088],"DCond":5,"ElementsL":[{"Name":"","type":3,"refY":50.0,"radius":0.0,"thicc":10.0,"refActorType":1,"includeRotation":true,"AdditionalRotation":0.80285144},{"Name":"","type":3,"refY":50.0,"radius":0.0,"thicc":10.0,"refActorType":1,"includeRotation":true,"AdditionalRotation":5.480334}],"UseTriggers":true,"Triggers":[{"Type":2,"Duration":7.5,"MatchIntl":{"En":"The gorgon begins casting Petrifaction."}}]}
```

[International] Second Gorgons - line AOE of illusions
```
~Lv2~{"Name":"P8S 2nd gorgons Illusory lines","Group":"P8S-1","ZoneLockH":[1088],"ElementsL":[{"Name":"","type":3,"refY":40.0,"radius":5.0,"color":1677762815,"refActorNPCNameID":11405,"refActorRequireCast":true,"refActorCastId":[31026],"refActorUseCastTime":true,"refActorCastTimeMax":8.0,"refActorUseOvercast":true,"refActorComparisonType":6,"includeRotation":true,"onlyVisible":true}],"Triggers":[{"Type":2,"MatchIntl":{"En":"Hephaistos uses Into the Shadows."}}]}
```
