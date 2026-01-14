namespace Konverter
{
    public interface IKonvertatorModule
    {
        string Category { get; }
        IEnumerable<UnitDef> Units { get; }
        double Convert(string from, string to, double value);
    }


    public class UnitDef
    {
        public string Id { get; }
        public string Name { get; }
        public string Symbol { get; }


        public UnitDef(string id, string name, string symbol)
        {
            Id = id;
            Name = name;
            Symbol = symbol;
        }
    }
}