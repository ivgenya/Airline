namespace AirlineService.DTO;

public class PaymentModel
{
    public string CreditCardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string Cvc { get; set; }
}