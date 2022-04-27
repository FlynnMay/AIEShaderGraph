using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    public enum BodyPart
    {
        Root,
        LeftHand,
        RightHand,
        LeftFoot,
        RightFoot,
        Head,
        Chest
    }

    Dictionary<BodyPart, Transform> parts;

    public Transform leftHand;
    public Transform rightHand;
    public Transform leftfoot;
    public Transform rightfoot;
    public Transform head;
    public Transform chest;

    public AnimatedAction currentAction;
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Transform GetBodyPart(BodyPart part)
    {
        if (parts == null)
        {
            parts = new Dictionary<BodyPart, Transform>();
            parts[BodyPart.LeftHand] = leftHand;
            parts[BodyPart.RightHand] = rightHand;
            parts[BodyPart.LeftFoot] = leftfoot;
            parts[BodyPart.RightFoot] = rightfoot;
            parts[BodyPart.Head] = head;
            parts[BodyPart.Chest] = chest;
        }

        if (parts.ContainsKey(part))
            return parts[part];

        return transform;
    }

    public void DoAction(AnimatedAction action)
    {
        currentAction = action;
        animator.SetTrigger(action.animTrigger);

        action.beginFX?.Begin(GetBodyPart(action.beginPart));
    }

    // Called from an animation event
    public void Activate()
    {
        if (currentAction)
        {
            currentAction.Activate(this);
            currentAction = null;
        }
    }
}
