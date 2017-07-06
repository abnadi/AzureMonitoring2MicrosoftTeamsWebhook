class WebHook
{
    public class Condition
    {
        public string metricName { get; set; }
        public string metricUnit { get; set; }
        public string metricValue { get; set; }
        public string threshold { get; set; }
        public string windowSize { get; set; }
        public string timeAggregation { get; set; }
        public string @operator { get; set; }
    }
    public class Context
    {
        public string timestamp { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string conditionType { get; set; }
        public Condition condition { get; set; }
        public string details { get; set; }
        public string subscriptionId { get; set; }
        public string resourceGroupName { get; set; }
        public string resourceName { get; set; }
        public string resourceType { get; set; }
        public string resourceId { get; set; }
        public string portalLink { get; set; }
        public string resourceRegion { get; set; }
        public string oldCapacity { get; set; }
        public string newCapacity { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(id);
        }
    }

    public class Properties
    {
        public string key1 { get; set; }
        public string key2 { get; set; }
    }

    public class RootObject
    {
        public string version { get; set; }
        public string status { get; set; }
        public string operation { get; set; }
        public Context context { get; set; }
        public Properties properties { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(status);
        }
    }


}
