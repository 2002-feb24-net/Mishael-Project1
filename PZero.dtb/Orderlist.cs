using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POne.dtb
{
    public interface IOrderList
    {
        int CustID { get; set; }
        List<OrderData> Cart { get; set; }
    }
}
