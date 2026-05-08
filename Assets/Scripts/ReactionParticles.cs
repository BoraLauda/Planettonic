using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ReactionType { Angry, Happy }

public class ReactionParticles : MonoBehaviour
{
    [System.Serializable]
    public struct ReactionSettings
    {
        public string name;
        public Sprite[] sprites;
        public int minParticles;
        public int maxParticles;
        public float animDuration;
        public float explosionForce;
        public float kafaHizasiYOffset;
    }

    [Header("Görseller ve Prefab")]
    public GameObject reactionPrefab;
    public Transform spawnContainer;

    [Header("Efekt Ayarları")]
    public Vector2 iconBoyutu = new Vector2(80f, 80f);

    [Header("Reaksiyon Paketleri")]
    public ReactionSettings[] reactions;

    public void PlayReactionEffect(RectTransform characterKafasi, ReactionType type)
    {
        if (reactionPrefab == null || spawnContainer == null || characterKafasi == null) return;
        if (reactions == null || reactions.Length <= (int)type) return;

        ReactionSettings currentSettings = reactions[(int)type];

        int spawnCount = Random.Range(currentSettings.minParticles, currentSettings.maxParticles);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject vfxObj = Instantiate(reactionPrefab, spawnContainer);
            RectTransform rect = vfxObj.GetComponent<RectTransform>();
            Image img = vfxObj.GetComponent<Image>();

            if (img == null)
            {
                Destroy(vfxObj);
                return;
            }

            rect.localScale = Vector3.one;
            rect.sizeDelta = iconBoyutu;
            img.preserveAspect = true;

            if (currentSettings.sprites != null && currentSettings.sprites.Length > 0)
            {
                img.sprite = currentSettings.sprites[Random.Range(0, currentSettings.sprites.Length)];
            }

            rect.position = characterKafasi.position;
            rect.anchoredPosition += new Vector2(Random.Range(-20f, 20f), currentSettings.kafaHizasiYOffset + Random.Range(0f, 20f));

            Vector2 randomDirection = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(1.0f, 2.5f)).normalized;
            Vector2 targetPosition = rect.anchoredPosition + (randomDirection * currentSettings.explosionForce);

            Vector3 startScale = rect.localScale;
            rect.localScale = Vector3.zero;
            
            rect.DOScale(startScale * Random.Range(0.8f, 1.3f), 0.2f).SetEase(Ease.OutBack);
            rect.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), currentSettings.animDuration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);
            rect.DOAnchorPos(targetPosition, currentSettings.animDuration).SetEase(Ease.OutExpo);

            img.DOFade(0, currentSettings.animDuration).SetEase(Ease.InSine).OnComplete(() =>
            {
                Destroy(vfxObj);
            });
        }
    }
}