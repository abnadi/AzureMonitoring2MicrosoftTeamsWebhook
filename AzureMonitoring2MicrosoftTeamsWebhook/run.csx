#r "Newtonsoft.Json"
#load "WebHook.csx"

using System;
using System.Net;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

		//CONVERT INCOMING JSON PAYLOAD
		string jsonContent = await req.Content.ReadAsStringAsync();
		dynamic data = JsonConvert.DeserializeObject<WebHook.RootObject>(jsonContent);

		//CHECK CONTENT
		if (data == null)
		{
			return req.CreateResponse(HttpStatusCode.BadRequest, new
			{
				error = "Incorrect Payload!!"
			});
		}
		
		//BUILD MESSAGE_CARD
		var response = new
		{
			summary = $"{data.operation} {data.context.resourceName}",
			title = "Azure Monitoring",
			sections = new[] {
					new {
						activityTitle = $"Service: {data.context.resourceName}",
						//activitySubtitle = "Azure Monitoring",
						//activityText = "Message",
						//activityImage = "HTTPS://<YOUR_LOGO>",

						facts = new [] {
							new {name ="Message: ", value = $"{data.context.details}"}
						}
				}
			}
		};
		
		//CONVERT TO JSON PAYLOAD
		var responseJson = JsonConvert.SerializeObject(
				response,
				Formatting.Indented
			);
		
		//SEND PAYLOAD TO MSTEAMS WEBHOOK
		var client = new HttpClient();

		await client.PostAsync("https://<YOUR_MSTEAMS_WEBHOOK>", new StringContent(responseJson, System.Text.Encoding.UTF8, "application/json"));
			

		return req.CreateResponse(HttpStatusCode.OK);
}