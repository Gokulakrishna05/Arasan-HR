using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Arasan.Interface 
{
    public interface IPaymentRequest
    {
        DataTable GetSupplier();
        DataTable GetGRN(string id);
        DataTable GetPO(string id);
        DataTable GetGRNDetails(string id);
        DataTable GetPODetails(string id);
        DataTable EditPayment(string id);
        DataTable EditPaymentRequest(string id);
        string  PaymentCRUD(PaymentRequest Cy);
        IEnumerable<PaymentRequest>  GetAllPaymentRequest();
        string PaymentApprove(PaymentRequest Cy);
        string StatusChange(string tag, int id);

        IEnumerable<PaymentRequest>  GetAllApprovePaymentRequest();
        DataTable GetPaymentRequestDetail(string id);
        DataTable GetPaymentRequestDetail1(string id);
        //DataTable GetPaymentRequestDetail2(string id) ;
    }
}
