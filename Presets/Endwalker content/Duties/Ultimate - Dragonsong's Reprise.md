# General Fight triggers
Brightsphere Circles: Places a circle around every active Brightsphere that shows you where they are.

Note: Display conditions set for P1 and P2. Not sure if there are any more instances of Brightspheres later on in the fight or not.
```
DSR Brightsphere Circles~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":9.0,"thicc":4.0,"refActorName":"Brightsphere","refActorObjectLife":true,"refActorLifetimeMin":0.0,"refActorLifetimeMax":1.5},"2":{"type":1,"radius":9.0,"color":503316735,"thicc":4.0,"refActorName":"Brightsphere","refActorObjectLife":true,"refActorLifetimeMin":0.0,"refActorLifetimeMax":1.5,"Filled":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":50.0,"Duration":20.0},{"TimeBegin":110.0,"Duration":7.0}]}
```
# Phase 1 - Knights triggers
Empty/Full Dimension Ring: Places a ring around Ser Grinnaux that displays the edge of Empty/Full Dimension when it is being cast. Additionally shows a red danger zone around Ser Grinnaux if he is casting Full Dimension to tell you to go out.
Note: I want to add some kind of an indicator for Empty Dimension but I can't think of a good one that is actually coherent.
```
DSR P1 Empty/Full Dimension Ring~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"thicc":5.0,"refActorName":"Ser Grinnaux","includeHitbox":true},"2":{"type":1,"radius":2.0,"color":503316735,"thicc":5.0,"refActorName":"Ser Grinnaux","refActorRequireCast":true,"refActorCastId":[25307],"includeHitbox":true,"Filled":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":17.2,"Duration":6.0},{"TimeBegin":98.2,"Duration":6.0}],"Phase":1}
```
### Hyperdimensional Slash (Spreads and Stacks)
Slash Safespots: Places a circle showing roughly how far away you need to be with blue lines for individual positioning to help with uptime.
```
DSR P1 Slash Safespots~{"ZoneLockH":[968],"DCond":5,"Elements":{"Circle":{"refX":100.0,"refY":100.0,"radius":9.5,"thicc":5.0,"refActorType":1},"Pos1":{"type":2,"refX":107.86864,"refY":105.34262,"offX":108.24861,"offY":104.72231,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos2":{"type":2,"refX":104.14739,"refY":108.542496,"refZ":-9.536743E-07,"offX":104.73123,"offY":108.235725,"offZ":4.7683716E-07,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos3":{"type":2,"refX":95.846466,"refY":108.54908,"refZ":4.7683716E-07,"offX":95.26983,"offY":108.24042,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos4":{"type":2,"refX":92.13487,"refY":105.32302,"refZ":4.7683716E-07,"offX":91.76572,"offY":104.722046,"offZ":-4.7683716E-07,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos5":{"type":2,"refX":91.76442,"refY":95.25914,"refZ":-4.7683716E-07,"offX":92.13521,"offY":94.65761,"offZ":4.7683716E-07,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos6":{"type":2,"refX":95.85496,"refY":91.464424,"refZ":-2.3841858E-07,"offX":95.25607,"offY":91.75623,"offZ":2.3841858E-07,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos7":{"type":2,"refX":104.14046,"refY":91.447266,"offX":104.74052,"offY":91.77505,"radius":0.0,"color":3372023808,"thicc":15.0},"Pos8":{"type":2,"refX":107.85812,"refY":94.680984,"refZ":9.536743E-07,"offX":108.23496,"offY":95.28196,"radius":0.0,"color":3372023808,"thicc":15.0}},"UseTriggers":true,"Triggers":[{"TimeBegin":37.0,"Duration":15.0}],"Phase":1}
```
### Shining Blade (Dashes)
Knockback Tether: Tethers Adelphel when he jumps to help locate where you need to get knocked back to.
```
DSR P1 Knockback Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"Ser Adelphel","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":53.6,"Duration":9.0,"ResetOnTChange":false}],"Phase":1}
```
Aetherial Tear Circles: Puts red circles around all of the Aetherial Tears that indicates their death zone.
```
DSR P1 Aetherial Tear AoE~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":9.0,"thicc":4.0,"refActorName":"aetherial tear"},"2":{"type":1,"radius":9.0,"color":503316735,"thicc":4.0,"refActorName":"aetherial tear","Filled":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":44.8,"Duration":25.2}],"Phase":1}
```
### Holy Chains (Playstation)
Ser Grinnaux Knockback Helper: Draws a small circle around the Ser Grinnaux's hitbox to help with the knockback.
```
DSR P1 Grinnaux Knockback~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"color":3370581760,"refActorName":"Ser Grinnaux","onlyTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":75.0,"Duration":10.0}],"Phase":1}
```
### Planar Prison (Transition)
Brightwing Cone: Displays a cone from Charibert towards you that indicates the size of the cone. Disappears when Brightwing hits you.
```
DSR P1 Prison Cone~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":4,"refX":-714.8652,"refY":-644.2318,"refZ":26.868929,"radius":10.0,"coneAngleMin":-15,"coneAngleMax":15,"refActorName":"Ser Charibert","includeRotation":true,"onlyTargetable":true,"Filled":true,"FaceMe":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Match":"You suffer the effect of Planar Imprisonment."},{"Type":3,"Match":"You suffer the effect of Skyblind."}],"Phase":1}
```
Skyblind Circles: Displays a circle around all players who have Skyblind on them. Disappears when Skyblind drops onto the floor.
```
DSR P1 Prison Skyblind~{"DCond":5,"Elements":{"1":{"type":1,"radius":2.0,"thicc":4.0,"refActorRequireBuff":true,"refActorBuffId":[2661],"refActorComparisonType":1}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":60.0,"Match":"You suffer the effect of Planar Imprisonment."}],"Phase":1}
```
# Phase 2 - Thordan triggers
Ascalon's Mercy Move Reminder: Flashes "MOVE" on the screen when Ascalon's Mercy is fully cast to remind you to move. 

(While Splatoon isn't really designed for being general-purpose trigger system, it can be used as such)
```
DSR P2 Move Trigger~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"overlayBGColor":4278190335,"overlayVOffset":3.0,"overlayFScale":8.0,"thicc":0.0,"overlayText":"MOVE","refActorType":1}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":1.0,"Match":"King Thordan readies Ascalon's Mercy Concealed.","MatchDelay":2.6}],"Phase":2}
```
Thordan Jump Tether: Tethers Thordan when he jumps to make it easier to locate him during Strength and Sanctity
```
DSR P2 Thordan Jump Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372158464,"overlayBGColor":4294911744,"overlayVOffset":3.0,"thicc":19.9,"refActorName":"King Thordan","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":48.0,"Duration":3.0},{"TimeBegin":102.0,"Duration":10.0}],"Phase":2}
```
### Strength of the Ward
Divebomb Helper: Shows both divebomb safespots
```
DSR P2 Strength Divebombs~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Vellguine","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Ignasse","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain":{"type":3,"refY":50.0,"radius":4.0,"color":1677721855,"refActorName":"Ser Paulecrain","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":7.0,"Match":"King Thordan readies Strength of the Ward"}],"Phase":2}
```
Divebomb Helper (Alternative): Looks a little nicer but requires more elements.
```
DSR P2 Strength Divebombs~{"ZoneLockH":[968],"DCond":5,"Elements":{"Ser Vellguine 1":{"type":3,"refY":50.0,"radius":4.0,"color":503316735,"refActorName":"Ser Vellguine","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Vellguine 2":{"type":3,"refX":8.0,"refY":50.0,"offX":8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Vellguine","includeRotation":true,"onlyUnTargetable":true},"Ser Vellguine 3":{"type":3,"refX":-8.0,"refY":50.0,"offX":-8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Vellguine","includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse 1":{"type":3,"refY":50.0,"radius":4.0,"color":503316735,"refActorName":"Ser Ignasse","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse 2":{"type":3,"refX":8.0,"refY":50.0,"offX":8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Ignasse","includeRotation":true,"onlyUnTargetable":true},"Ser Ignasse 3":{"type":3,"refX":-8.0,"refY":50.0,"offX":-8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Ignasse","includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain 1":{"type":3,"refY":50.0,"radius":4.0,"color":503316735,"refActorName":"Ser Paulecrain","includeHitbox":true,"includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain 2":{"type":3,"refX":8.0,"refY":50.0,"offX":8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Paulecrain","includeRotation":true,"onlyUnTargetable":true},"Ser Paulecrain 3":{"type":3,"refX":-8.0,"refY":50.0,"offX":-8.0,"radius":0.0,"thicc":5.0,"refActorName":"Ser Paulecrain","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":7.0,"Match":"King Thordan readies Strength of the Ward"}]}
```
Heavy Impact Rings: Places rings around Ser Guerrique indicating the size of the quake rings from Heavy Impact
```
DSR P2 Quake Rings~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":33.0,"Duration":18.0,"Match":"King Thordan readies Strength of the Ward"}],"Phase":2}
```
Heavy Impact Rings: Alternative Triggers
```
DSR Quake markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"Ser Paulecrain readies Spiral Thrust"}],"Phase":2}
```
Sequential Heavy Impact Rings: Displays the quake markers sequentially instead of all at once.
```
DSR P2 Strength Quake 1~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":6.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":0.0,"thicc":5.0,"refActorName":"Ser Guerrique","tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":35.0,"Duration":10.0},{"Type":3,"Match":"Ser Guerrique uses Heavy Impact.","MatchDelay":1.9}],"Phase":2}
```
```
DSR P2 Strength Quake 2~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":12.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":41.0,"Duration":3.8,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":6.0}],"Phase":2}
```
```
DSR P2 Strength Quake 3~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":18.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":43.5,"Duration":3.8,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":8.0}],"Phase":2}
```
```
DSR P2 Strength Quake 4~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":24.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"TimeBegin":45.5,"Duration":1.9,"Match":"Ser Guerrique readies Heavy Impact.","MatchDelay":10.0}],"Phase":2}
```
Sequential Heavy Impact Rings (Alternative): Uses a very jank method to contain all elements within a single layout. Experimental.
```
DSR P2 Strength Quake Circles~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":6.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","refActorObjectLife":true,"refActorLifetimeMin":0.0,"refActorLifetimeMax":7.9,"includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","refActorObjectLife":true,"refActorLifetimeMin":6.0,"refActorLifetimeMax":9.8,"includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","refActorObjectLife":true,"refActorLifetimeMin":7.9,"refActorLifetimeMax":11.7,"includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4278190335,"thicc":4.0,"refActorName":"Ser Guerrique","refActorObjectLife":true,"refActorLifetimeMin":9.8,"refActorLifetimeMax":11.7,"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":35.0,"Duration":20.0}],"Phase":2}
```
Party Positions: Places blue circles on the spots where the party stack, two tankbusters and 3 defam dives should be.
```
DSR P2 Strength Positions~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"offX":6.3,"offY":3.25,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"2":{"type":1,"offX":-6.3,"offY":3.25,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"3":{"type":1,"offY":1.5,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"4":{"type":1,"offY":43.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"5":{"type":1,"offX":20.0,"offY":26.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true},"6":{"type":1,"offX":-20.0,"offY":26.0,"radius":0.3,"color":3372154880,"thicc":4.0,"refActorDataID":12604,"refActorComparisonType":3,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":52.0,"Duration":8.0}]}
```
### Sanctity of the Ward
DRK Tether: Locates the DRK (Ser Zephirin) with a tether during Sanctity of the Ward for use with the DRK Relative strat
```
DSR P2 Sanctity DRK Tether~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.0,"color":3372154880,"thicc":5.0,"refActorName":"Ser Zephirin","onlyVisible":true,"tether":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":9.2}],"Phase":2}
```
DRK Safespots: Indicates the two possible safespots for the DRK Relative strat on both sides of the arena.
```
DSR P2 Sanctity DRK Starting Spots~{"ZoneLockH":[968],"DCond":5,"Elements":{"G1 CCW":{"type":1,"offX":4.0,"offY":-5.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G1 CW":{"type":1,"offX":-4.0,"offY":-5.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G2 CCW":{"type":1,"offX":4.0,"offY":35.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true},"G2 CW":{"type":1,"offX":-4.0,"offY":35.0,"radius":0.5,"color":3372158208,"thicc":5.0,"refActorName":"Ser Zephirin","includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":9.2}],"Phase":2}
```
PLD Facing Arrows: Places an arrow on both PLDs (Ser Adelphel and Ser Janlenoux) that shows which direction they're facing so you know whether to move Clockwise or Counter-Clockwise.
```
DSR P2 Sanctity PLD Arrows~{"ZoneLockH":[968],"DCond":5,"Elements":{"Adelphel":{"type":3,"refY":4.0,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Adelphel 2":{"type":3,"refY":4.0,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Adelphel 3":{"type":3,"refY":4.0,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Adelphel","includeRotation":true,"onlyVisible":true},"Janlenoux":{"type":3,"refY":4.0,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true},"Janlenoux 2":{"type":3,"refY":4.0,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true},"Janlenoux 3":{"type":3,"refY":4.0,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorName":"Ser Janlenoux","includeRotation":true,"onlyVisible":true}},"UseTriggers":true,"Triggers":[{"TimeBegin":100.8,"Duration":7.0}],"Phase":2}
```
# Phase 3 - Nidhogg triggers:
Dive from Grace Numbers: Places coloured dots under all party members that matches their Dive from Grace Number
```
DSR P3 Dive Numbers~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3004],"refActorComparisonType":1,"includeRotation":true,"Filled":true},"2 - 1":{"type":1,"offX":0.5,"radius":0.25,"refActorRequireBuff":true,"refActorBuffId":[3005],"refActorComparisonType":1,"includeRotation":true,"Filled":true},"2 - 2":{"type":1,"offX":-0.5,"radius":0.25,"refActorRequireBuff":true,"refActorBuffId":[3005],"refActorComparisonType":1,"includeRotation":true,"Filled":true},"3 - 1":{"type":1,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"Filled":true},"3 - 2":{"type":1,"offX":0.666,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"includeRotation":true,"Filled":true},"3 - 3":{"type":1,"offX":-0.666,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"includeRotation":true,"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":90.0,"Match":"Nidhogg uses Final Chorus."}],"Phase":2}
```
Dive from Grace Numbers (Solo Variant): Only places the coloured dots under your character.
```
DSR P3 Dive Numbers~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":1,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3004],"refActorComparisonType":1,"refActorType":1,"includeRotation":true,"Filled":true},"2 - 1":{"type":1,"offX":0.5,"radius":0.25,"refActorRequireBuff":true,"refActorBuffId":[3005],"refActorComparisonType":1,"refActorType":1,"includeRotation":true,"Filled":true},"2 - 2":{"type":1,"offX":-0.5,"radius":0.25,"refActorRequireBuff":true,"refActorBuffId":[3005],"refActorComparisonType":1,"refActorType":1,"includeRotation":true,"Filled":true},"3 - 1":{"type":1,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"refActorType":1,"Filled":true},"3 - 2":{"type":1,"offX":0.666,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"refActorType":1,"includeRotation":true,"Filled":true},"3 - 3":{"type":1,"offX":-0.666,"radius":0.25,"color":3372154884,"refActorRequireBuff":true,"refActorBuffId":[3006],"refActorComparisonType":1,"refActorType":1,"includeRotation":true,"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":90.0,"Match":"Nidhogg uses Final Chorus."}],"Phase":2}
```
Dive from Grace Markers: Places circles and arrows under all party members that matches what mechanic they have for Dive from Grace.
```
DSR P3 Dive Circles~{"ZoneLockH":[968],"DCond":5,"Elements":{"Circle":{"type":1,"radius":2.5,"thicc":4.0,"refActorRequireBuff":true,"refActorBuffId":[2755,2756,2757],"refActorComparisonType":1},"Spineshatter 1":{"type":3,"refY":2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"includeRotation":true},"Spineshatter 2":{"type":3,"refY":2.47,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"includeRotation":true},"Spineshatter 3":{"type":3,"refY":2.47,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"includeRotation":true},"Elusive 1":{"type":3,"refY":-2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"includeRotation":true},"Elusive 2":{"type":3,"refY":-2.47,"offX":0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"includeRotation":true},"Elusive 3":{"type":3,"refY":-2.47,"offX":-0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":90.0,"Match":"Nidhogg uses Final Chorus."}],"Phase":2}
```
Dive from Grace Markers (Alternative): Places circles under all party members but only places arrows under your own character.
```
DSR P3 Dive Circles~{"ZoneLockH":[968],"DCond":5,"Elements":{"Circle":{"type":1,"radius":2.5,"thicc":4.0,"refActorRequireBuff":true,"refActorBuffId":[2755,2756,2757],"refActorComparisonType":1},"Spineshatter 1":{"type":3,"refY":2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Spineshatter 2":{"type":3,"refY":2.47,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Spineshatter 3":{"type":3,"refY":2.47,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 1":{"type":3,"refY":-2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 2":{"type":3,"refY":-2.47,"offX":0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 3":{"type":3,"refY":-2.47,"offX":-0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":90.0,"Match":"Nidhogg uses Final Chorus."}],"Phase":2}
```
Dive from Grace Markers (Solo Variant): Only places circles and markers under your character.
```
DSR P3 Dive Circles~{"ZoneLockH":[968],"DCond":5,"Elements":{"Circle":{"type":1,"radius":2.5,"thicc":4.0,"refActorRequireBuff":true,"refActorBuffId":[2755,2756,2757],"refActorComparisonType":1,"refActorType":1},"Spineshatter 1":{"type":3,"refY":2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Spineshatter 2":{"type":3,"refY":2.47,"offX":0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Spineshatter 3":{"type":3,"refY":2.47,"offX":-0.5,"offY":1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2756],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 1":{"type":3,"refY":-2.47,"radius":0.0,"color":3372089344,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 2":{"type":3,"refY":-2.47,"offX":0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true},"Elusive 3":{"type":3,"refY":-2.47,"offX":-0.5,"offY":-1.5,"radius":0.0,"color":3372154880,"thicc":10.0,"refActorRequireBuff":true,"refActorBuffId":[2757],"refActorComparisonType":1,"refActorType":1,"includeRotation":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":90.0,"Match":"Nidhogg uses Final Chorus."}],"Phase":2}
```
Geirskogul Line AoEs: Displays a red line whenever a Geirskogul is cast, showing the AoE.
(Requires Splatoon version 1.0.8.0 or higher)
```
DSR P3 Geirskogul Lines~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":30.0,"radius":4.0,"color":1174405375,"thicc":4.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.0,"Match":"Nidhogg readies Geirskogul."}],"Phase":2}
```
Geirskogul Line AoEs (Alternative): Looks a little nicer but requires more elements.
```
DSR P3 Geirskogul Lines~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":3,"refY":30.0,"radius":4.0,"color":671088895,"thicc":4.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true},"2":{"type":3,"refX":-4.0,"refY":30.0,"offX":-4.0,"radius":0.0,"thicc":10.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true},"3":{"type":3,"refX":4.0,"refY":30.0,"offX":4.0,"radius":0.0,"thicc":10.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true},"4":{"type":3,"refX":4.03,"refY":30.0,"offX":-4.03,"offY":30.0,"radius":0.0,"thicc":10.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true},"5":{"type":3,"refX":4.03,"offX":-4.03,"radius":0.0,"thicc":10.0,"refActorName":"Nidhogg","refActorRequireCast":true,"refActorCastId":[3555,6066,6312,8805,9274,21098,24732,26378,29491],"includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":5.0,"Match":"Nidhogg readies Geirskogul."}],"Phase":2}
```
Drachenlance:
```
DSR P3 Drachenlance Cone~{"ZoneLockH":[968],"DCond":5,"Elements":{"1":{"type":4,"refY":15.0,"radius":13.0,"coneAngleMin":-45,"coneAngleMax":45,"color":4294967040,"thicc":3.0,"refActorName":"Nidhogg","includeRotation":true,"onlyTargetable":true,"Filled":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":3.0,"Match":"Nidhogg readies Drachenlance."}],"Phase":2}
```
# Phase 5 - Alternative timeline Thordan triggers:
The second set of quakes seen in P5:
```
DSR P5 Quake Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Quake marker":{"type":1,"radius":6.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"2":{"type":1,"radius":12.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"3":{"type":1,"radius":18.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true},"4":{"type":1,"radius":24.0,"color":4293721856,"refActorName":"Guerrique","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":15.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```

Dive Markers - these are the dives when the four dooms go out, displaying the safe spots accurately, with correct timings too:
```
DSR P5 DIve Markers~{"ZoneLockH":[968],"DCond":5,"Elements":{"Spear of the Fury":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Ser Zephirin","includeRotation":true,"onlyUnTargetable":true},"Cauterize":{"type":3,"refY":30.0,"offY":-15.0,"radius":10.0,"color":1690288127,"refActorName":"Ser Guerrique","includeRotation":true,"onlyUnTargetable":true},"Twisting Dive":{"type":3,"refY":45.0,"radius":5.0,"color":1690288127,"refActorName":"Vedrfolnir","includeRotation":true,"onlyUnTargetable":true}},"UseTriggers":true,"Triggers":[{"Type":2,"Duration":8.0,"Match":"King Thordan readies Death of the Heavens","MatchDelay":13.0}],"Phase":2}
```

# Phase 6 - Nidhogg and Hraesvelgr
P6 Arena Quarter
Helps with keeping melee uptime when Nidhogg dives one half of the arena in addition to Hraesvelgr cleaving the other half leaving one quarter safe. (exact trigger to be improved):
```
DSR P6 Arena Quarter~{"ZoneLockH":[968],"DCond":5,"Elements":{"P6 Quarter":{"type":2,"refX":100.0,"refY":80.0,"offX":100.0,"offY":120.0,"radius":0.0},"P6 Quarter 2":{"type":2,"refX":80.0,"refY":100.0,"offX":120.0,"offY":100.0,"radius":0.0}},"UseTriggers":true,"Triggers":[{"TimeBegin":660.0,"Duration":240.0}],"Phase":2}
```
