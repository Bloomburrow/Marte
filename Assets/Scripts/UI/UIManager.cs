using Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public abstract class UIManager : MonoBehaviourSingleton
{
    #region Components

    [Foldout("Components (UIManager)/External")]
    [SerializeField] protected Canvas canvas;
    [Foldout("Components (UIManager)/External")]
    [Indent(1)][SerializeField] Image backgroundImage;
    [Foldout("Components (UIManager)/External")]
    [Indent(2)][SerializeField] protected RectTransform safeAreaPanelRectTransform;
    [Foldout("Components (UIManager)/External")]
    [ReadOnly][SerializeField] List<Panel> activePanels;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        Rect safeAreaRect = Screen.safeArea;

        Vector2 anchorMin = safeAreaRect.position;

        Vector2 anchorMax = safeAreaRect.position + safeAreaRect.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        safeAreaPanelRectTransform.anchorMin = anchorMin;

        safeAreaPanelRectTransform.anchorMax = anchorMax;
    }

    public void SetBackground(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public void SetBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    public void OpenPanel(Panel panel, bool hideLastPanel = true)
    {
        if (hideLastPanel && activePanels.Count > 0)
        {
            Panel lastPanel = activePanels[activePanels.Count - 1];

            if (panel != lastPanel)
                lastPanel.gameObject.SetActive(false);
        }

        panel.gameObject.SetActive(true);

        panel.OnOpen();

        activePanels.Add(panel);
    }

    public void SwapCurrentPanel(Panel panel)
    {
        if (activePanels.Count > 0)
        {
            Panel lastPanel = activePanels[activePanels.Count - 1];

            if (panel != lastPanel)
            {
                lastPanel.gameObject.SetActive(false);

                activePanels.Remove(lastPanel);
            }
        }

        panel.gameObject.SetActive(true);

        panel.OnOpen();

        activePanels.Add(panel);
    }

    public void CloseLastPanel()
    {
        if (activePanels.Count > 0)
        {
            Panel lastPanel = activePanels[activePanels.Count - 1];

            lastPanel.gameObject.SetActive(false);

            activePanels.Remove(lastPanel);

            if (activePanels.Count > 0)
            {
                lastPanel = activePanels[activePanels.Count - 1];

                lastPanel.gameObject.SetActive(true);

                lastPanel.OnOpen();
            }
        }
    }
}
