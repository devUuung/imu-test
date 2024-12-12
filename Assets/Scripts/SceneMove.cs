using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Meta.XR.MultiplayerBlocks.Shared;

namespace Meta.XR.MultiplayerBlocks.Fusion
{
    public class SceneMove : MonoBehaviour
    {
        private NetworkRunner _networkRunner;
        private void OnEnable()
        {
            FusionBBEvents.OnSceneLoadDone += OnLoaded;
        }

        private void OnDisable()
        {
            FusionBBEvents.OnSceneLoadDone -= OnLoaded;
        }

        private void OnLoaded(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
        }
        public void ToMain()
        {
            _networkRunner.LoadScene(SceneRef.FromIndex(0), LoadSceneMode.Additive);
        }

        public void ToPlay()
        {
            _networkRunner.LoadScene(SceneRef.FromIndex(1), LoadSceneMode.Additive);
        }

        public void ToAim()
        {
            _networkRunner.LoadScene(SceneRef.FromIndex(2), LoadSceneMode.Additive);
        }
    }
}