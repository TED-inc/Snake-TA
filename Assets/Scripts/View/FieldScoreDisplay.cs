using TEDinc.SnakeTA.Logic;
using TMPro;
using UnityEngine;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;

        private FieldScoreService _fieldScoreService;

        private void Awake()
        {
            AllServices.Get(out _fieldScoreService);
            _fieldScoreService.ScoreChanged += RefershScoreLabel;

            RefershScoreLabel();
        }

        private void OnDestroy() =>
            _fieldScoreService.ScoreChanged -= RefershScoreLabel;

        private void RefershScoreLabel() => 
            _scoreLabel.text = _fieldScoreService.Score.ToString();
    }
}
