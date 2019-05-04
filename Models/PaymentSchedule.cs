namespace ISAM4332.Assignment05.Models
{
    public class PaymentSchedule
    {
        public int PaymentNumber { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal MonthlyInterest { get; set; }

        public decimal MonthlyPrincipal { get; set; }

        public decimal MonthlyPayment { get; set; }

        public decimal RemainingBalance { get; set; }

        public decimal TotalInterestPaid { get; set; }

    }
}