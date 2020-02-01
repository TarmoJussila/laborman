using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFetcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flagRenderer;

    private void Start()
    {
        flagRenderer.sprite = null;

        StartCoroutine(FetchFlagSprite());
    }

    private IEnumerator FetchFlagSprite()
    {
        while (flagRenderer.sprite == null)
        {
            yield return new WaitForSeconds(0.5f);

            var flagSprite = LocaleController.Instance.CurrentFlagSprite;

            if (flagSprite != null)
            {
                flagRenderer.sprite = flagSprite;
            }
        }
    }
}
