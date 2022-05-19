using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenShield : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> piecesBodies;
    private List<Vector3> piecesPositions;
    private List<Quaternion> piecesRotations;

    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float radius;

    private void Awake()
    {
        piecesPositions = new List<Vector3>();
        piecesRotations = new List<Quaternion>();
        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesPositions.Add(piecesBodies[i].transform.position);
            piecesRotations.Add(piecesBodies[i].transform.rotation);
        }
    }

    public void Explode()
    {
        ResetBrokenShield();

        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesBodies[i].gameObject.SetActive(true);
            float explosionForce = Random.Range(minForce, maxForce);
            piecesBodies[i].AddExplosionForce(explosionForce, transform.position, radius);
        }
    }

    public void ResetBrokenShield()
    {
        for (int i = 0; i < piecesBodies.Count; ++i)
        {
            piecesBodies[i].gameObject.SetActive(false);
            piecesBodies[i].transform.position = piecesPositions[i];
            piecesBodies[i].transform.rotation = piecesRotations[i];
        }
    }
}
