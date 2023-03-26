using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image progressBarUI;
    [SerializeField] GameObject hasProgressObject ;
    IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressObject.GetComponent<IHasProgress>();
        if(hasProgress == null )
        {
            Debug.LogError("Game Object " + hasProgressObject + " doesn't have a component that implements IHasProgress!");
        }
        hasProgress.OnHasProgressTimeChanged += HasProgress_OnHasProgressTimeChanged;
        Hide();
    }
    private void HasProgress_OnHasProgressTimeChanged(float progressTimeNormalized)
    {
        progressBarUI.fillAmount= progressTimeNormalized;
        
        if(progressTimeNormalized > 0 && progressTimeNormalized < 1) {
            Show();
        }
        else
        {
            Hide();
        }
    }
    void Hide()
    {
            gameObject.SetActive(false);     
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
}
