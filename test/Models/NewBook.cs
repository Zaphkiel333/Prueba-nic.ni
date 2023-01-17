namespace test.Models
{
    public class NewBook
    {
        public string Tittle { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int UnitsAvailables { get; set; }

        public decimal? YearOfRelease { get; set; }
    }
}
