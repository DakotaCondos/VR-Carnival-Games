using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] List<GameObject> _panels = new List<GameObject>();
    [SerializeField] GameObject _activePanel;

    public void SetActivePanel(GameObject panel)
    {
        bool duplicateFlag = false;
        foreach (var item in _panels)
        {
            if (panel == item)
            {
                if (!duplicateFlag)
                {
                    _activePanel = item;
                    duplicateFlag = true;
                }
                else
                {
                    Debug.LogWarning("Multiple panels match active panel gameobject!");
                }
            }
            item.SetActive(panel == item);
        }
    }

    public GameObject GetActivePanel()
    {
        return _activePanel;
    }

    public void DisableAllPanels()
    {
        foreach (var item in _panels)
        {
            item.SetActive(false);
        }
        _activePanel = null;
    }
}