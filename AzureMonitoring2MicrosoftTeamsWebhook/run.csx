#r "Newtonsoft.Json"
#load "WebHook.csx"

using System;
using System.Net;
using Newtonsoft.Json;


public class Response
{
    [JsonProperty(PropertyName = "summary")]
    public string Summary;

    [JsonProperty(PropertyName = "title")]
    public string Title;

    [JsonProperty(PropertyName = "sections")]
    public List<Section> Sections = new List<Section>();
}
public class Section
{
    [JsonProperty(PropertyName = "activityTitle")]
    public string ActivityTitle;

    [JsonProperty(PropertyName = "facts")]
    public List<Fact> Facts = new List<Fact>();
}
public class Fact
{
    public string name;
    public string value;
}



public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

	//CONVERT INCOMING JSON PAYLOAD
    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject<WebHook.RootObject>(jsonContent);
	
	//STORE PAYLOAD IN ARRAYS
    var resp = new Response { Summary = $"{ data.operation } { data.context.resourceName }", Title = "Azure Monitoring" };
    resp.Sections.Add(new Section { ActivityTitle = $"Service: {data.context.resourceName}" });

    var sect = new Section { };
    sect.Facts.Add(new Fact { name = "Message: ", value = $"{data.context.details}" });
    resp.Sections.Add(sect);

	//CHECK CONTENT
    if (data.context.condition != null)
    {
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.conditionType}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.metricName}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.metricUnit}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.metricValue}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.threshold}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.windowSize}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.timeAggregation}" });
        sect.Facts.Add(new Fact { name = "", value = $"{data.context.condition.@operator}" });
    }
	
	//BUILD MESSAGE_CARD
    var responses = resp;
	
	//CONVERT TO JSON PAYLOAD
    var responseJson = JsonConvert.SerializeObject(
            responses,
            Formatting.Indented
        );
	
	//SEND PAYLOAD TO MSTEAMS WEBHOOK
    var client = new HttpClient();

    await client.PostAsync("https://<MSTEAMS_WEBHOOK>", new StringContent(responseJson, System.Text.Encoding.UTF8, "application/json"));

    return req.CreateResponse(HttpStatusCode.OK);
}
