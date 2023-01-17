using System;
using System.Collections.Generic;

namespace test.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string Tittle { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int UnitsAvailables { get; set; }

    public decimal? YearOfRelease { get; set; }

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
