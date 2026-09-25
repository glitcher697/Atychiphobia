using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntryPoint.EntryPoints {
    public abstract class EntryPoint : MonoBehaviour {
        public abstract IEnumerator Run();

        public abstract bool _isSceneLoaded { get; protected set; }
    }
}