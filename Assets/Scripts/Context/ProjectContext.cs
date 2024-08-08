using UnityEngine;

namespace TEDinc.SnakeTA.Contexts
{
    internal sealed class ProjectContext : MonoBehaviour
    {
        private void Awake()
        {
            AllServices.AddProcessor(new DisposableServiceProcessor());
            AllServices.Register(new string[4]);
            AllServices.Register(new string('1', 2));
        }

        private void OnDestroy() => 
            AllServices.Clear();
    }
}