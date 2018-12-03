using System.Collections;
using UnityEngine;

public class Warp : MonoBehaviour {

    public Transform warpTarget;

	public BoxCollider2D newBounds;

	private CameraFolow theCam;

    public string destinationMapName;

    private bool isPerforming;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (!isPerforming)
        {
            isPerforming = true;
            StartCoroutine(PerformWarp(other));
            Invoke("TogglePerformingBool", 2f);
        }
    }

    IEnumerator PerformWarp(Collider2D other)
    {
        
		ScreenFader sf = GameObject.FindGameObjectWithTag ("Fader").GetComponent<ScreenFader>();

		yield return StartCoroutine (sf.FadeToBlack());

		theCam = FindObjectOfType<CameraFolow> ();

        other.gameObject.transform.position = warpTarget.position;
        Camera.main.transform.position = warpTarget.position;

		theCam.setNewBounds (newBounds);

        GameManager.instance.gameData.mapName = destinationMapName;

		yield return StartCoroutine (sf.FadeToClear());
    }

    private void TogglePerformingBool()
    {
        isPerforming = !isPerforming;
    }
}
