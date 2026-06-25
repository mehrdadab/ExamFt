var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var identityDb = postgres.AddDatabase("identitydb");

builder.AddProject<Projects.ExamFT_Identity>("identity")
    .WithHttpHealthCheck("/health")
    .WithReference(identityDb)
    .WaitFor(identityDb);

var apiService = builder.AddProject<Projects.ExamFt_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.ExamFt_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
