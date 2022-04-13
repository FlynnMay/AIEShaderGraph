using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListUI : MonoBehaviour
{
    public ActionList actionList;
    public List<ActionUI> uis = new List<ActionUI>();
    public ActionUI actionUI;

    LayoutGroup layoutGroup;
    ContentSizeFitter contentSizeFitter;

    void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        layoutGroup = GetComponent<LayoutGroup>();
        StartCoroutine(UpdateUI());
        actionList.onChanged.AddListener(() => { StartCoroutine(UpdateUI()); });
    }

    IEnumerator UpdateUI()
    {
        contentSizeFitter.enabled = true;
        layoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < actionList.actions.Count; i++)
        {
            if (i >= uis.Count)
            {
                uis.Add(Instantiate(actionUI, transform));
            }
            uis[i].gameObject.SetActive(true);
            uis[i].SetAction(actionList.actions[i]);
            uis[i].transform.SetAsLastSibling();
        }

        for (int i = actionList.actions.Count; i < uis.Count; i++)
            uis[i].gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        contentSizeFitter.enabled = false;
        layoutGroup.enabled = false;
    }

    void Update()
    {

    }
}
