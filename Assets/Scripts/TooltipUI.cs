using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour {

    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform UICanvasRectTransform;

    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;
    private RectTransform backgroundRectTransform;
    private TooltipTimer tooltipTimer;

    private void Awake() {
        Instance = this;

        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();

        Hide();
    }

    private void Update() {
        HandleFollowMouse();

        if(tooltipTimer != null) {
            tooltipTimer.timer -= Time.deltaTime;

            if (tooltipTimer.timer <= 0) {
                Hide();
            }
        }

    }
    private void HandleFollowMouse() {
        Vector2 anchoredPosition = Input.mousePosition / UICanvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > UICanvasRectTransform.rect.width) {
            anchoredPosition.x = UICanvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > UICanvasRectTransform.rect.height) {
            anchoredPosition.y = UICanvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void SetText(string tooltipText) {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(13f, 13f);
        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null) {
        this.tooltipTimer = tooltipTimer;

        SetText(tooltipText);
        gameObject.SetActive(true);

        HandleFollowMouse();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public class TooltipTimer {
        public float timer;
    }
}
