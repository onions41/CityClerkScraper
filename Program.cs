using DataAccess.Data;
using DataAccess.DbAccess;

using Scraper.Richmond;

var builder = Host.CreateApplicationBuilder(args);
IConfiguration config = builder.Configuration;

// DI

// Adds logging provider to for logging to file
builder.Logging.AddFile(config.GetSection("Logging"));

// Data Access DI
builder.Services.AddSingleton<IPostgresAccess, PostgresAccess>();
builder.Services.AddSingleton<IMeetingData, MeetingData>();
builder.Services.AddSingleton<IVideoData, VideoData>();

// Scraper for each city
builder.Services.AddSingleton<ISite, Site>();

// Main service
builder.Services.AddHostedService<TestParseAgenda>();

// builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
// builder.Services.AddSingleton<IMeetingData, MeetingData>();
// builder.Services.AddSingleton<IRecordData, RecordData>();
// builder.Services.AddSingleton(new OpenAIClient(config.GetSection("ApiKeys")["OpenAi"]));
// builder.Services.AddSingleton<IDivideDocumentPrompt, DivideDocumentPrompt>();

// Runs the service. Waits for threads to complete
builder.Build().Run();
