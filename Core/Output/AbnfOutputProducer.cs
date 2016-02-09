namespace Core.Output
{
    using Interfaces;

    public class AbnfOutputProducer
    {
        private readonly ISummaryFormatter _formatter;
        public AbnfOutputProducer(ISummaryFormatter formatter)
        {
            _formatter = formatter;
        }

        public string GetSummaryAsText(ISummary summary)
        {
            if (summary == null)
                return string.Empty;

            if (summary.InvalidInput)
                return string.Empty;

            return _formatter.GetFormattedSummary(summary, ' ');
        }
    }
}
