----------------------------------------------
                 SpriteMask
          Copyright © 2015 TrueSoft
             support@truesoft.pl
                Version: 1.4
----------------------------------------------

 HELP
----------------------------------------------

1) How to create mask in Unity editor?

    Just add component SpriteMask to GameObject. Next you can add sprites as child's objects for the mask.

2) Is it possible to create mask at runtime?

    Yes of course, just look at scene "Examples\Scenes\05 - Runtime mask.unity".

3) Does the sprite have to be parented to the mask?    

    Theoretically no, but by default only sprites that are under a mask are updated automatically.
    If you want to mask a sprite (or brunche of sprites) that is outside the SpriteMask component, than you have to call this method (at runtime):
    
    GetComponent <SpriteMask> ().updateSprites (transfom); // Where 'transform' belongs to the sprite it self or it's a parent of many sprites
    
4) Can I turn off/on masking at runtime?

    Yes of course, just disable/enable SpriteMask component:
    
        GetComponent <SpriteMask> ().enabled = false; // Turn masking OFF
        
    To turn it on again:
    
        GetComponent <SpriteMask> ().enabled = true; // Turn masking ON
            
5) Can I use my own shader?

    Yes, it is possible, but you should ensure that your shader support Stencil buffer. 
    In your shader source code you should have this:

        Properties
        {
            // [...]
    
            _Stencil ("Stencil Ref", Float) = 0
            _StencilComp ("Stencil Comparison", Float) = 8
        }
    
        SubShader
        {
            Pass
            {
                Stencil
                {
                    Ref [_Stencil]
                    Comp [_StencilComp]
                    Pass Keep
                }
                
                // [...]
            }
        }
    
    For more info see source code of shader "SpriteMask/Default" in file "Resources\SpriteDefault.shader".

6) I've some UI components masked by UI Mask that are located over SpriteMask. Why are my UI components also masked by SpriteMask?

    To get SpriteMask work with UI Mask, Stencil buffer must be cleared before Unity UI is rendered. 
    To do that you must create an GameObject and attach to it ClearStencilBufferComponent. Next you have to set 
    sortingLayer and sortingOrder in that way, that it should be rendered between SpriteMask and Unity UI. 
    You can see how does it work on scene '10 - SpriteMask & UI Mask'. 

 VERSIONS
----------------------------------------------

1.4
    * Fix: Serialization of custom shader
    * SpriteDefault & SpriteDiffuse shaders upgrade to Unity 5.x

1.3
    * Masking parts (see example: 07 - Masking Part)
    * Inverted masking (see example: 08 - Inverted)
    * Flat hierarchy (see example: 09 - Flat hierarchy)
    * Custom sprites shader (e.g. Diffuse)
    * Co-work with Unity UI Mask (see example: 10 - SpriteMask & UI Mask)
    
1.2
    * Backward compatibility with Unity 4.3 and 4.5
    * Fix: Unable to change masks 'Sorting layer' value (for types Rectangle and Texture).
    * Better Undo handling

1.1
    * Performance improvements
    * Shader image quality improvements

1.0 
    * Initial relaease
