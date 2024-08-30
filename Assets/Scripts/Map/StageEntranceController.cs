using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageEntranceController : MonoBehaviour
{
    public string stageName;
    public Sprite bossIcon;

    private SpriteRenderer bossIconRenderer;
    private SpriteRenderer capsuleRenderer;

    private void Awake()
    {
        FindRenderers();
    }

    private void Start()
    {
        UpdateBossIcon();
    }

    private void OnValidate()
    {
        FindRenderers();
        UpdateBossIcon();
    }

    private void FindRenderers()
    {
        // Capsuleのレンダラーを取得
        Transform capsuleTransform = transform.Find("Capsule");
        if (capsuleTransform != null)
        {
            capsuleRenderer = capsuleTransform.GetComponent<SpriteRenderer>();
        }

        // BossIconのレンダラーを取得
        Transform bossIconTransform = transform.Find("Capsule/BossIcon");
        if (bossIconTransform != null)
        {
            bossIconRenderer = bossIconTransform.GetComponent<SpriteRenderer>();
        }
    }

    private void UpdateBossIcon()
    {
        if (bossIconRenderer != null)
        {
            bossIconRenderer.sprite = bossIcon;
#if UNITY_EDITOR
            EditorUtility.SetDirty(bossIconRenderer);
#endif
        }
        // Capsuleのスプライトは変更しない
#if UNITY_EDITOR
        SceneView.RepaintAll();
#endif
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            FadeManager.Instance.FadeAndLoadScene(stageName);
        }
    }
}