|This page contains triggers localized for Japanese game client|
|---|

# Phase 1 - Knights triggers
```
DSR Grinnaux knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"聖騎士グリノー","onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75.0,"Duration":10.0}]}
```

# Phase 2 - Thordan triggers
King Thordan move reminder: (while Splatoon isn't really designed for being general-purpose trigger system, it can be used as such)
```
DSR thordan move reminder~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"overlayBGColor":4278190335,"overlayVOffset":3.0,"overlayFScale":8.0,"thicc":0.0,"overlayText":"MOVE","refActorType":1}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"Match":"騎神トールダンは「インビジブル・アスカロンメルシー」の構え。","MatchDelay":2.6}],"Phase":2}
```

Strength of the Ward - "divebomb" helper (shows safe zone)
```
DSR Strength of the Ward~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"聖騎士ヴェルギーン","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"聖騎士イニアセル","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":80.0,"radius":4.0,"color":1677721855,"refActorName":"聖騎士ポールクラン","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":10.0,"Match":"King Thordan readies Strength of the Ward"}]}
```

Tether on Thordan's jump for towers + jump on gaze - to make it easier to locate him:
```
DSR King Thordan tether on leap~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"騎神トールダン","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":48.0,"Duration":3.0},{"TimeBegin":100.0,"Duration":10.0}],"Phase":2}
```

Strength of the Ward - earthquake radius rings
```
DSR Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":18.0,"Match":"King Thordan readies Strength of the Ward"}]}
```

# Phase 5 - Alternative timeline Thordan triggers:
The second set of quakes seen in P5:
```
DSR Quake P5 markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"騎神トールダンは「至天の陣：死刻」の構え。","MatchDelay":13.0}],"Phase":2}
```

Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR Dive P5 markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"聖騎士ゼフィラン","includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorName":"聖騎士ゲリック","includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"ヴェズルフェルニル","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"騎神トールダンは「至天の陣：死刻」の構え。","MatchDelay":13.0}],"Phase":2}
```
