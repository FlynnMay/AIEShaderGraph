using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListUI : MonoBehaviour
{
    public List<Action> actionList;
    public ActionUI actionUI;
    
    IEnumerator Start()
    {
        foreach (Action action in actionList)
        {
            Instantiate(actionUI.gameObject, transform).GetComponent<ActionUI>()?.SetAction(action);
        }

        yield return new WaitForEndOfFrame();

        GetComponent<ContentSizeFitter>().enabled = false;
        GetComponent<LayoutGroup>().enabled = false;
    }

    void Update()
    {
        
    }
}
