using Core.Model;

namespace Core.ModelDto.EPSPG;

public class EPSPGPaymentResponseDto
{
    public string MerchantTransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TotalAmount { get; set; } = string.Empty;
    public string TransactionDate { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public string FinancialEntity { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string customerId { get; set; } = string.Empty;
    public string StoreAmount { get; set; } = string.Empty;
    public string customerName { get; set; } = string.Empty;
    public string customerEmail { get; set; } = string.Empty;
    public string customerAddress { get; set; } = string.Empty;
    public string customerAddress2 { get; set; } = string.Empty;
    public string customerCity { get; set; } = string.Empty;
    public string customerState { get; set; } = string.Empty;
    public string customerPostcode { get; set; } = string.Empty;
    public string customerCountry { get; set; } = string.Empty;
    public string customerPhone { get; set; } = string.Empty;
    public string shipmentName { get; set; } = string.Empty;
    public string shipmentAddress { get; set; } = string.Empty;
    public string shipmentAddress2 { get; set; } = string.Empty;
    public string shipmentCity { get; set; } = string.Empty;
    public string shipmentState { get; set; } = string.Empty;
    public string shipmentPostcode { get; set; } = string.Empty;
    public string shipmentCountry { get; set; } = string.Empty;
    public string valueA { get; set; } = string.Empty;
    public string valueB { get; set; } = string.Empty;
    public string valueC { get; set; } = string.Empty;
    public string Valued { get; set; } = string.Empty;
    public string shippingMethod { get; set; } = string.Empty;
    public string noOfItem { get; set; } = string.Empty;
    public string productName { get; set; } = string.Empty;
    public string productProfile { get; set; } = string.Empty;
    public string productCategory { get; set; } = string.Empty;
    public string IsSimplifiedPG { get; set; } = string.Empty;
    public string PaymentReferance { get; set; } = string.Empty;
    public string EPSTransactionId { get; set; } = string.Empty;
}