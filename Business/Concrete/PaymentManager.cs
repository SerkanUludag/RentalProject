using Business.Abstract;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        public IResult isPaymentVerified(string cardNumber)
        {
            if(cardNumber.Length == 19)
            {
                return new SuccessResult("Payment verified");
            }
            else
            {
                return new ErrorResult("Payment not verified");
            }
        }
    }
}
