namespace Core.ModelDto.EPSPG;

public class EPSPGPaymentRequestDto
{
    public string MerchantId { get; set; } = string.Empty;
    public string StoreId { get; set; } = string.Empty;
    public string CustomerOrderId { get; set; } = string.Empty;
    public string MerchantTransactionId { get; set; } = string.Empty;
    public int TransactionTypeId { get; set; }
    public int FinancialEntityId { get; set; }
    public int TransitionStatusId { get; set; }
    public decimal TotalAmount { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string SuccessUrl { get; set; } = string.Empty;
    public string FailUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;

    // Customer Information
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string CustomerAddress2 { get; set; } = string.Empty;
    public string CustomerCity { get; set; } = string.Empty;
    public string CustomerState { get; set; } = string.Empty;
    public string CustomerPostcode { get; set; } = string.Empty;
    public string CustomerCountry { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    // Shipment Information
    public string ShipmentName { get; set; } = string.Empty;
    public string ShipmentAddress { get; set; } = string.Empty;
    public string ShipmentAddress2 { get; set; } = string.Empty;
    public string ShipmentCity { get; set; } = string.Empty;
    public string ShipmentState { get; set; } = string.Empty;
    public string ShipmentPostcode { get; set; } = string.Empty;
    public string ShipmentCountry { get; set; } = string.Empty;

    // Extra Values
    public string ValueA { get; set; } = string.Empty;
    public string ValueB { get; set; } = string.Empty;
    public string ValueC { get; set; } = string.Empty;
    public string ValueD { get; set; } = string.Empty;

    // Product & Payment Info
    public string ShippingMethod { get; set; } = string.Empty;
    public string NoOfItem { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductProfile { get; set; } = string.Empty;
    public string ProductCategory { get; set; } = string.Empty;
    public int DeviceTypeId { get; set; }
    public string RequestBody { get; set; } = string.Empty;
    public bool IsSimplifiedPG { get; set; }
    public List<ProductDto> ProductList { get; set; }
    // References
    public int PaymentLinkId { get; set; }
    public string PaymentReferance { get; set; } = string.Empty;
}


public class ProductDto
{
    public string ProductName { get; set; }
    public string NoOfItem { get; set; }
    public string ProductProfile { get; set; }
    public string ProductCategory { get; set; }
    public string ProductPrice { get; set; }
}