using RLP_External_Data_Ingest;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// TODO - Add DB context and other services

var host = builder.Build();
host.Run();
