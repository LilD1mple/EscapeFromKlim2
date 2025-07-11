using EFK2.Events.Interfaces;
using UnityEngine;

namespace EFK2.Events.Signals
{
    public readonly struct InputKeyChangedSignal : IEvent
    {
        public InputKeyChangedSignal(string key, KeyCode keyCode)
        {
            Key = key;

            KeyCode = keyCode;
        }

        public string Key { get; }

        public KeyCode KeyCode { get; }
    }
}
