using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using test.Entities;
using test.Models;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly BookStoreContext context;

        public TransactionController(BookStoreContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Transaction>> createTransaction(NewTransaction newTransaction)
        {
            Transaction transaction = new Transaction() 
            {
                QuantitySold= newTransaction.QuantitySold,
                Book= newTransaction.Book,
                TransactionDate= newTransaction.TransactionDate,
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();
            //await sendEmail(transaction.QuantitySold, mail);

            return Ok(transaction);
        }

        private async Task sendEmail(int quantity,string mailTo)
        {
            string body = "<body>" +
                "<h1>Usted compro " + quantity + "libro(s), GRACIAS</h1>" +
                "</body>";

            SmtpClient smtp = new SmtpClient("smt.gmail.com");
            smtp.Credentials = new NetworkCredential("testnicni@gmail.com", "ningunpassword");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("testnicni@gmail.com", "Prueba de NIC.NI");
            mail.To.Add(new MailAddress(mailTo));
            mail.Subject = "Prueba de NIC.NI";
            mail.IsBodyHtml = true;
            mail.Body = body;

            smtp.Send(mail);
        }
    }
}
