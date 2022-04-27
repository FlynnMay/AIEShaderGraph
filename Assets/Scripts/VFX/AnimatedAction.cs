using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualFXSystem;

public abstract class AnimatedAction : ScriptableObject
{
    public string animTrigger;

    [Header("Begin Effects")]
    public VisualFX beginFX;
    public CharacterFX.BodyPart beginPart;
    [Header("Activate Effects")]
    public VisualFX activateFX;
    public CharacterFX.BodyPart activatePart;

    public void Activate(CharacterFX characterFX)
    {
        activateFX?.Begin(characterFX.GetBodyPart(activatePart));
        OnActivate(characterFX);
    }

    public abstract void OnActivate(CharacterFX character);
}
