namespace ConfigurationExample.Options
{
    public class PollyOptions
    {
        public const string SectionName = "Polly";

        public int RetriesNumber { get; set; }
        public int IntervalBetweenRetries { get; set; }
    }
}
