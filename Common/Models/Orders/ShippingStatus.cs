namespace Ecommerce.Common.Models.Orders
{
    public enum ShippingStatus
    {
        /// <summary>
        /// Awaiting processing.
        /// </summary>
        AwaitingProcessing,
        /// <summary>
        /// Processing.
        /// </summary>
        Processing,
        /// <summary>
        /// Shipped.
        /// </summary>
        Shipped,
        /// <summary>
        /// Out for delivery.
        /// </summary>
        OutForDelivery,
        /// <summary>
        /// Delivered.
        /// </summary>
        Delivered,
        /// <summary>
        /// Returned.
        /// </summary>
        Returned
    }
}
