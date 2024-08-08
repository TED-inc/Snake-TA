using System;

namespace TEDinc.SnakeTA.IndependentLogic
{
    public sealed class DisposableServiceProcessor : IServicesProcessor
    {
        public void Registered(object service) { }

        public void UnRegistered(object service)
        {
            (service as IDisposable)?.Dispose();
        }
    }
}