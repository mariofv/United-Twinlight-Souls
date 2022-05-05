using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioClips : MonoBehaviour
{
    [Header("Roar")]
    public AudioClip roar;
    public AudioClip roarKingKong;

    [Header("Stun")]
    public AudioClip stun;
    public AudioClip stun2;
    public AudioClip stunRecovery;

    [Header("Slam")]
    public AudioClip slamPreparation;
    public AudioClip slamSwing;
    public AudioClip slamHit;

    [Header("Earthquake")] 
    public AudioClip earthquakePreparation;
    public AudioClip earthquakeSwing;
    public AudioClip earthquakeHit;

    [Header("Punch")]
    public AudioClip punchSwing;
    public AudioClip punchHit;
}
