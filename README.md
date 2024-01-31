## Note. As of v1.0.17, HDLethalCompany has been added as a dependency  
I recommend that you make the following adjustments to HDLethalCompany config:  
1: EnableAA = true  
2: EnablePP = false  
3: FogQuality = 3  

This should give you good results with reducing the amount of texture and geometry flicker due to Lethal Companies very low internal render resolution (and I don't have to reinvent the wheel implementing rendering changes)  


Added the following settings: (Settings are adjustable in the mod config file. (BepInEx\config))  

Disable stun grenade explosion (default: true)  
Disable stun grenade flashed effect (default: false)  
Disable scan blue flash effect (default: true)  
Disabled flashing red light when getting fired (default: true)  
Disable global notifications (default: false)  
Disable radiation warning (default: true)  
Replace warnings with hint (default: true)  
Disable lightning bolts (default: true)  
Disable lightning bolt explosion (default: true)  
Disable the fear screen filter effect (default: true)  
Disable lightning effect on bees (default: true)  
Disable teleporter beamup effect (default: true)    
Prevent network player flashlight spam (default: true)  
Network player flashlight use cooldown (default: 2s) -This goes with the above setting.  
Disable player monitor blink when changing target (default: true)  
Disable start room fan spinning animation in the facility (default: true)  
Hide the blinking caret on the terminal screen (default: true)  
Hide the spark animation when using the item charger (default: true)  
Hide the turret bullet projectiles. Note doesn't hide the muzzle flash. (default: true)  
Disable the radar booster spinning animation (default: true)  
Disable the radar booster flash, which also stops it working... sorry (default: true)  
Disable landmines, they no longer explode because I couldn't remove the explosion any other way (default: false)  
Disable the FPV helmet as the crack can cause light reflections (default: true)  
Disable all fog (default: true)  
Disable volumetric fog only (default: true) *NOTE* in order for this to work you need to set Disable all fog to {false}  
Disable FPV helmet glass only (default: true) *NOTE* in order for this to work you need to set Disable FPV helemt to {false}   
Disable Critical health message which is also the same effect as when taking damage (default: true)  
Disable volumetric fog movement, this prevents the wind blowing effect which can cause strobing around light sources (default: true).  
### Note. Please set the other fog settings to 'false' as well as any other mods that remove fog if you want to use this or it will not work.  
Disable the custom shader, this is the shader that gives the unique stylised look to lethal company with the outlines and harsh colour gradients (default: true)  
Disable miscellaneous reflections, turns off a few reflections that may cause issues (default: true)  

New in v1.0.18
Attempt to prevent tooltip from flashing (default: true)  
  
  
  
# Shoutout to anEpilepticGamer for testing and many many suggestions, the scope of this mod would not have gotten to where it is without your input.  
## Please go and support her and the epileptic gaming community at: https://www.twitch.tv/AnEpilepticGamer  
  
  
  
## If you are a mod developer and familiar with editing UI elements in unity games, I could really use your help with making the UI colours adjustable.
### Please check out the github repo and pitch in if you are able, thankyou.  
