namespace Invynite.Services.Procurement.DTOs
{
    public record ReceivePurchaseOrderResponse(
        int PurchaseOrderId,
        ICollection<ItemReceivedDto> ItemsReceived
        );
}
