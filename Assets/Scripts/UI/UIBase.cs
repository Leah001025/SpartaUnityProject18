using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
