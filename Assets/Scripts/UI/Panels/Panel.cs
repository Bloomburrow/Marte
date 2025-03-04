using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    #region Variables & Properties

    protected bool _fontColorIsInverted { get; set; }

    #endregion

    public virtual void Initialize() { }

    public virtual void SetUIStyle(UIStyleData UIStyleData) { }

    protected Color InvertColor(Color color)
    {
        Color invertedColor = new Color(Mathf.Abs(color.r - Color.white.r), Mathf.Abs(color.g - Color.white.g), Mathf.Abs(color.b - Color.white.b), 1f);

        return invertedColor;
    }

    public virtual void OnOpen() { }
}
