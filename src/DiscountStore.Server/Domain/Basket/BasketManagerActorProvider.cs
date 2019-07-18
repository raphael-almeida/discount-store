using Akka.Actor;

namespace DiscountStore.Server.Domain.Basket
{
    /// <summary>
    /// Delegate to get the actor manager instance on the Controller class.
    /// </summary>
    /// <returns></returns>
    public delegate IActorRef BasketManagerActorProvider();
}
