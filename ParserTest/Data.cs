namespace ParserTest
{
    internal class Data
    {
        public Data(string name, string requestsPerHour, string hours, string info)
        {
            Name = name;
            RequestsPerHour = requestsPerHour;
            Hours = hours;
            Info = info;
        }

        public string Name { get; }

        public string RequestsPerHour { get; }

        public string Hours { get; }

        public string Info { get; }
        
        public string SumRequests { get; set; }
    }
}