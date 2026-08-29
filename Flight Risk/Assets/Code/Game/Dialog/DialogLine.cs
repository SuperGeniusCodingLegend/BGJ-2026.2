using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    [System.Serializable]
    public class DialogLine
    {
        [SerializeField] private string speakerName;
        [SerializeField] [TextArea(3, 10)] private string text;
        [SerializeField] private AudioClip lineAudio;

        public string SpeakerName => speakerName;
        public string Text => text;
        public AudioClip LineAudio => lineAudio;
    }
}