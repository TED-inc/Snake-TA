using System;

namespace TEDinc.SnakeTA.Contexts
{
    internal sealed class DisposableServiceProcessor : IServicesProcessor
    {
        public void Registered(object service) { }

        public void UnRegistered(object service)
        {
            (service as IDisposable)?.Dispose();
        }
    }
}