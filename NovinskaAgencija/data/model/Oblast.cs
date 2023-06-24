namespace NovinskaAgencija.data.model
{
    public class Oblast
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ClanakOblast> ClanakOblasti { get; set; }
    }
}
