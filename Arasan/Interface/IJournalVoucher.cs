namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

    public interface IJournalVoucher
    {
    DataTable GetLocation();
    DataTable GetCurrency();
    DataTable GetAcc();
    DataTable GetParty();



}

