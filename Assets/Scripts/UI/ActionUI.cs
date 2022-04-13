using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUI : MonoBehaviour
{
    public Action action;

    [Header("Child Components")]
    public Image icon;
    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI descriptionTag;
    Button button;

    void Awake()
    {
        button = GetComponentInChildren<Button>();    
    }

    void Start()
    {
        if (button != null)
            button.onClick.AddListener(() => { Player.DoAction(action); });

        SetAction(action);
    }

    public void SetAction(Action a)
    {
        action = a;
        if(nameTag != null)
            nameTag.text = action.actionName;
        if(descriptionTag != null)
            descriptionTag.text = action.description;
        if (icon != null)
        {
            icon.sprite = action.icon;
            icon.color = action.colour;
        }
    }
}
