using Models;
using Services;
using System;
using System.Collections.Generic;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestService.TestConecction();
            //List<Invoice> result = null;            
            InvoiceService servicio = new InvoiceService();
            //result = servicio.GetAll();
            //Invoice result = null;
            //result = servicio.Get(1);

            
            Invoice invoice = null;
            InvoiceDetail detail = null;
            List<InvoiceDetail> details = null;
            details = new List<InvoiceDetail>();
            invoice = new Invoice();
            invoice.ClienteId = 1;

            detail = new InvoiceDetail();
            detail.ProductId = 1;
            detail.Quantity = 5;
            detail.Price = 1500;
            details.Add(detail);

            detail = new InvoiceDetail();
            detail.ProductId = 6 ;
            detail.Quantity = 15;
            detail.Price = 125;
            details.Add(detail);

            invoice.Detail = details;
            servicio.Create(invoice);            

            /*
            Invoice invoice = null;
            InvoiceDetail detail = null;
            List<InvoiceDetail> details = null;
            details = new List<InvoiceDetail>();
            invoice = new Invoice();
            invoice.Id = 4;
            invoice.ClienteId = 1;

            detail = new InvoiceDetail();
            detail.ProductId = 1;
            detail.Quantity = 5;
            detail.Price = 1500;
            details.Add(detail);

            detail = new InvoiceDetail();
            detail.ProductId = 6;
            detail.Quantity = 30;
            detail.Price = 125;
            details.Add(detail);

            invoice.Detail = details;
            servicio.Update(invoice);
            */

            /*
            Invoice invoice = null;            
            invoice = new Invoice();
            invoice.Id = 4;
            
            servicio.Delete(invoice.Id);
            */
            Console.Read();
        }
    }
}
