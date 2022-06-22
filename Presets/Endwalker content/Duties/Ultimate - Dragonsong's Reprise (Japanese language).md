|This page contains triggers localized for Japanese game client|
|---|

# Phase 1 - Knights triggers
Empty/Full Dimension Ring: Places a ring around Ser Grinnaux that displays the edge of Empty/Full Dimension when it is being cast.
```
DSR Empty/Full Dimension Ring~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"thicc":5.0,"refActorName":"聖騎士グリノー","includeHitbox":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":18.5,"Duration":5.0},{"TimeBegin":99.0,"Duration":5.0}],"Phase":1}
```
### Holy Chains (Playstation)
Ser Grinnaux Knockback Helper: Draws a small circle around the Ser Grinnaux's hitbox to help with the knockback.
```
DSR Grinnaux knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"聖騎士グリノー","onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75.0,"Duration":10.0}]}
```
### Shining Blade (Dashes)
Knockback Tether: Tethers Adelphel when he jumps to help locate where you need to get knocked back to.
```
DSR P1 Knockback Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"聖騎士アデルフェル","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":53.0,"Duration":15.0}],"Phase":1}
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

Sanctity DRK Tether. Locates the DRK (Ser Zephirin) with a tether during Sanctity of the Ward for use with the DRK Relative strat:
```
DSR Sanctity DRK Tether~{"ZoneLockH":[968],"Elements":{"1":{"type":1,"radius":0.0,"color":3372158208,"thicc":5.0,"refActorName":"聖騎士ゼフィラン","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":102.0,"Duration":10.0}],"Phase":2}
```

# Phase 3 - Nidhogg
Baits after tower (Geirskogul cast): (Requires Splatoon version 1.0.8.0 or higher)
```
DSR Geirskogul~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":30.0,"radius":4.0,"color":1174405375,"thicc":4.0,"refActorName":"ニーズヘッグ","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.0,"Match":"ニーズヘッグは「ゲイルスコグル」の構え。"}],"Phase":2}
```

Drachenlance:
```
DSR Dranchenlance~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true},"2":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":0.7853982},"3":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":0.5235988},"4":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":0.2617994},"5":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":6.021386},"6":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":5.7595863},"7":{"type":3,"refY":15.0,"radius":0.0,"color":4294967040,"thicc":6.0,"refActorName":"ニーズヘッグ","includeRotation":true,"onlyTargetable":true,"AdditionalRotation":5.497787}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":3.0,"Match":"ニーズヘッグは「ドラッケンランス」の構え。"}],"Phase":2}
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
