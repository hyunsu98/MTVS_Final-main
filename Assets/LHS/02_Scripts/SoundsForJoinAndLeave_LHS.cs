// ----------------------------------------------------------------------------
// <copyright file="SoundsForJoinAndLeave.cs" company="Exit Games GmbH">
// Photon Voice Demo for PUN- Copyright (C) 2016 Exit Games GmbH
// </copyright>
// <summary>
// Script to play sound when player joins or leaves room.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

namespace ExitGames.Demos.DemoPunVoice
{
    using Photon.Pun;
    using UnityEngine;
    using Player = Photon.Realtime.Player;

    public class SoundsForJoinAndLeave_LHS : MonoBehaviourPunCallbacks
    {
        public AudioClip JoinClip;
        public AudioClip LeaveClip;
        private AudioSource source;

        // 새로운 플레이어가 참여했을 때
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (this.JoinClip != null)
            {
                if (this.source == null) this.source = FindObjectOfType<AudioSource>();
                this.source.PlayOneShot(this.JoinClip);
            }
        }
        
        // 다른 플레이어가 나갔을 때
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (this.LeaveClip != null)
            {
                if (this.source == null) this.source = FindObjectOfType<AudioSource>();
                this.source.PlayOneShot(this.LeaveClip);
            }
        }
    }
}