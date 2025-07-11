using System.Collections.Generic;
using UnityEngine;

namespace EFK2.Inputs.Interfaces
{
    public interface IKeyboardUpdateService
    {
        IReadOnlyDictionary<string, KeyCode> KeyValues { get; }

        bool SetKey(string key, KeyCode keyCode);

        void ResetKey(string key, KeyCode defaultKey);
    }
}