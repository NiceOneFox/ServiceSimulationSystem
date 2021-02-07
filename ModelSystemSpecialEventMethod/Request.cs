namespace ModelSystemSpecialEventMethod
{
    class Request
    {
        public float dateTime { get; set; } = 0f;
        public int numberSource { get; set; }

        public int SerialNumberOfSource { get; set; }

        public float EndTime { get; set; } = 0f;
        public string uniqueID => ":Time: " + dateTime.ToString() + " №source: " + numberSource.ToString() + "-" + SerialNumberOfSource.ToString();

        public Request() { }

        public Request(float dateTime, int numberSource, int serialNumberOfSource)
        {
            this.dateTime = dateTime;
            this.numberSource = numberSource;
            this.SerialNumberOfSource = serialNumberOfSource;
        }
        public Request(float dateTime, int numberSource)
        {
            this.dateTime = dateTime;
            this.numberSource = numberSource;
        }
    }
}
