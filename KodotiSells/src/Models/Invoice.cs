using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Client Client { get; set; }
        public List<InvoiceDetail> Detail { get; set; }
        public decimal Iva { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public Invoice()
        {
            Detail = new List<InvoiceDetail>();
        }
    }
}
