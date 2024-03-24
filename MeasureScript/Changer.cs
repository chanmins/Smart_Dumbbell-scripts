using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Changer : MonoBehaviour
{
    public Button button;
    public GameObject text;
    public float originalButtonPosY;
    public float originalButtonHeight;
    public float targetButtonPosY;
    public float targetButtonHeight;

    public float originalTextPosY;
    public float originalTextHeight;
    public float targetTextPosY;
    public float targetTextHeight;

    public float animationDuration = 1f;

    private bool isAnimating = false;
    private bool isOriginalState = true;

    private void Start()
    {
        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        RectTransform textTransform = text.GetComponent<RectTransform>();

        // Set initial position Y and height for the button
        originalButtonPosY = buttonTransform.anchoredPosition.y;
        originalButtonHeight = buttonTransform.sizeDelta.y;

        // Set initial position Y and height for the text
        originalTextPosY = textTransform.anchoredPosition.y;
        originalTextHeight = textTransform.sizeDelta.y;

        button.onClick.AddListener(AnimateButton);
    }

    private void AnimateButton()
    {
        if (isAnimating)
            return;

        isAnimating = true;

        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        RectTransform textTransform = text.GetComponent<RectTransform>();

        // Determine the target position and height based on the current state
        float targetButtonPosY = isOriginalState ? this.targetButtonPosY : originalButtonPosY;
        float targetButtonHeight = isOriginalState ? this.targetButtonHeight : originalButtonHeight;
        float targetTextPosY = isOriginalState ? this.targetTextPosY : originalTextPosY;
        float targetTextHeight = isOriginalState ? this.targetTextHeight : originalTextHeight;

        // Animate button position Y
        buttonTransform.DOAnchorPosY(targetButtonPosY, animationDuration);

        // Animate button height
        buttonTransform.DOSizeDelta(new Vector2(buttonTransform.sizeDelta.x, targetButtonHeight), animationDuration);

        // Animate text position Y and height
        textTransform.DOAnchorPosY(targetTextPosY, animationDuration);
        textTransform.DOSizeDelta(new Vector2(textTransform.sizeDelta.x, targetTextHeight), animationDuration)
            .OnComplete(() =>
            {
                isAnimating = false;
                isOriginalState = !isOriginalState;
            });
    }
}
