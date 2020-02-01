using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTextureFetcher : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer flagRenderer;

    private Material flagMaterial;

    private void Start()
    {
        flagMaterial = flagRenderer.material;

        flagMaterial.mainTexture = null;

        StartCoroutine(FetchFlagTexture());
    }

    private IEnumerator FetchFlagTexture()
    {
        while (flagMaterial.mainTexture == null)
        {
            yield return new WaitForSeconds(0.5f);

            var flagTexture = LocaleController.Instance.CurrentFlagTexture;

            if (flagTexture != null)
            {
                flagMaterial.mainTexture = flagTexture;
                yield break;
            }
        }
    }
}