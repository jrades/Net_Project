using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISAM4332.Assignment05.Models
{
    public class MortgageApplication
    {
        public Guid ApplicationId { get; set; } = Guid.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime? DateOfBirth { get; set; } = null;

        [Required, EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public decimal? GrossIncomeFY3 { get; set; } = null;

        [Required]
        public decimal? GrossIncomeFY2 { get; set; } = null;

        [Required]
        public decimal? GrossIncomeFY1 { get; set; } = null;

        [Required]
        public bool Veteran { get; set; } = false;

        [Required]
        public bool Married { get; set; } = false;

        public string AdditionalInfo { get; set; } = string.Empty;

        [Required]
        public decimal? LoanAmount { get; set; } = null;

        [Required]
        public decimal? DownPayment { get; set; } = null;

        [Required]
        public decimal InterestRate { get; set; } = 0m;

        [Required]
        public int LoanDuration { get; set; } = 360;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; } = null;

        public decimal MonthlyInterestRate()
        {
            return ((InterestRate / 100m) / 12m);
        }

        public decimal? MonthlyPayment()
        {
            decimal? payment = null;
            try
            {
                decimal mRate = MonthlyInterestRate();
                decimal pv = (LoanAmount ?? 0) - (DownPayment ?? 0);
                decimal n = (mRate * pv * (decimal)(Math.Pow((double)(1 + mRate), LoanDuration)));
                decimal d = (decimal)(Math.Pow((double)(1 + mRate), LoanDuration) - 1);
                payment = (n / d);
            }
            catch (Exception)
            {
                payment = null;
            }
            return payment;
        }

        public IEnumerable<PaymentSchedule> AmortizationSchedule()
        {
            List<PaymentSchedule> schedule = new List<PaymentSchedule>();
            decimal balance = (LoanAmount ?? 0) - (DownPayment ?? 0);
            decimal tInterest = 0;
            decimal? payment = MonthlyPayment();
            decimal mRate = MonthlyInterestRate();
            if (!payment.HasValue)
            {
                return schedule;
            }
            decimal mAmount = payment.Value;
            for (int i = 0; i < LoanDuration; i++)
            {
                decimal startBalance = balance;
                decimal mInterest = (balance * mRate);
                decimal mPrincipal = (mAmount - mInterest);
                balance -= mPrincipal;
                tInterest += mInterest;
                schedule.Add(new PaymentSchedule()
                {
                    PaymentNumber = (i + 1),
                    CurrentBalance = startBalance,
                    MonthlyInterest = mInterest,
                    MonthlyPrincipal = mPrincipal,
                    MonthlyPayment = mAmount,
                    RemainingBalance = balance,
                    TotalInterestPaid = tInterest
                });
            }
            return schedule;
        }

    }
}