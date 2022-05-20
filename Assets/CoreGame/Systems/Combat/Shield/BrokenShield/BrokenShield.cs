using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenShield : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> piecesBodies;
    private Vector3 originalSBrokenShieldPosition;
    private List<Vector3> piecesPositions;
    private List<Quaternion> piecesRotations;
    private List<MeshRenderer> piecesRenderers;

    [SerializeField] private float explodedTime;
    [SerializeField] [Range(0f,1f)] private float dissolveActivationPercentage;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float radius;

    private bool alreadyExploded = false;
    private float currentTime = 0f;

    private void Awake()
    {
        originalSBrokenShieldPosition = transform.position;
        piecesPositions = new List<Vector3>();
        piecesRotations = new List<Quaternion>();
        piecesRenderers = new List<MeshRenderer>();
        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesPositions.Add(piecesBodies[i].transform.position);
            piecesRotations.Add(piecesBodies[i].transform.rotation);
            piecesRenderers.Add(piecesBodies[i].GetComponent<MeshRenderer>());
        }
    }

    private void Update()
    {
        if (alreadyExploded)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(1f, currentTime / explodedTime);

            if (progress >= dissolveActivationPercentage)
            {
                float fadeProgress = (progress - dissolveActivationPercentage) / (1f - dissolveActivationPercentage);
                for (int i = 0; i < piecesRenderers.Count; ++i)
                {
                    piecesRenderers[i].material.SetFloat("_Disolve", fadeProgress);
                }
            }

            if (progress == 1f)
            {
                alreadyExploded = false;
                ResetBrokenShield();
            }
        }
    }

    public void Explode(Vector3 hitPosition)
    {
        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesBodies[i].gameObject.SetActive(true);
            float explosionForce = Random.Range(minForce, maxForce);
            piecesBodies[i].AddExplosionForce(explosionForce, hitPosition, radius);
        }

        currentTime = 0f;
        alreadyExploded = true;
    }

    public void ResetBrokenShield()
    {
        transform.position = originalSBrokenShieldPosition;
        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesBodies[i].gameObject.SetActive(false);
            piecesBodies[i].velocity = Vector3.zero;
            piecesBodies[i].transform.position = piecesPositions[i];
            piecesBodies[i].transform.rotation = piecesRotations[i];
            piecesRenderers[i].material.SetFloat("_Disolve", 0f);
        }
    }

    public Color GetBrokenShieldColor()
    {
        return piecesRenderers[0].material.GetColor("_FresnelColor");
    }
}
