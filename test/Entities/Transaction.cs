using System;
using System.Collections.Generic;

namespace test.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public int QuantitySold { get; set; }

    public int? Book { get; set; }

    public virtual Book? BookNavigation { get; set; }
}
