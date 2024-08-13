using TEDinc.SnakeTA.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldControlButtons : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        private FieldService _fieldService;

        private void Awake()
        {
            AllServices.Get(out _fieldService);
            _restartButton.onClick.AddListener(_fieldService.StartGame);
        }

        private void OnDestroy() => 
            _restartButton.onClick.RemoveListener(_fieldService.StartGame);
    }
}
