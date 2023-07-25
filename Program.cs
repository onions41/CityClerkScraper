using Scraper.Richmond;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
IConfiguration config = builder.Configuration;

builder.Logging.AddFile(config.GetSection("Logging"));

builder.Services.AddSingleton<IRichmond, Richmond>();

builder.Services.AddHostedService<ScrapingService>();

// builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
// builder.Services.AddSingleton<IMeetingData, MeetingData>();
// builder.Services.AddSingleton<IRecordData, RecordData>();
// builder.Services.AddSingleton(new OpenAIClient(config.GetSection("ApiKeys")["OpenAi"]));
// builder.Services.AddSingleton<IDivideDocumentPrompt, DivideDocumentPrompt>();

IHost host = builder.Build();
host.Run();
