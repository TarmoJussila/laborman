using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTextureFetcher : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer flagRenderer;
    [SerializeField] private bool fetchFlagAngle = true;

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

            if (fetchFlagAngle)
            {
                string windDegreeString = LocaleController.Instance.CurrentWindDegree;

                float windDegree = transform.eulerAngles.y;
                if (float.TryParse(windDegreeString, out windDegree))
                {
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, windDegree, transform.eulerAngles.z);
                }
            }
        }
    }
}