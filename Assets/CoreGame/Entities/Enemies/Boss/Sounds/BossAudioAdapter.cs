using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioAdapter : MonoBehaviour
{
    [Header("Roar")]
    public AudioSource roar;
    public AudioSource roarKingKong;

    [Header("Stun")]
    public AudioSource stun;
    public AudioSource stun2;
    public AudioSource stunRecovery;

    [Header("Slam")]
    public AudioSource slamPreparation;
    public AudioSource slamSwing;
    public AudioSource slamHit;

    [Header("Earthquake")] 
    public AudioSource earthquakePreparation;
    public AudioSource earthquakeSwing;
    public AudioSource earthquakeHit;
}
