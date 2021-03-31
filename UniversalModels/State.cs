namespace UniversalModels
{
    /// <summary>
    /// Contains data describing a state.
    /// </summary>
    public class State
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public decimal TaxPercent { get; set; }

        public State(string name, string abbreviation, decimal taxPercent)
        {
            Name = name;
            Abbreviation = abbreviation;
            TaxPercent = taxPercent;
        }
    }
}