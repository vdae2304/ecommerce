namespace Ecommerce.Common.Models.Orders
{
    public enum PaymentStatus
    {
        /// <summary>
        /// Awaiting payment.
        /// </summary>
        AwaitingPayment,
        /// <summary>
        /// Paid.
        /// </summary>
        Paid,
        /// <summary>
        /// Cancelled.
        /// </summary>
        Cancelled,
        /// <summary>
        /// Refunded.
        /// </summary>
        Refunded,
        /// <summary>
        /// Incomplete.
        /// </summary>
        Incomplete
    }
}
