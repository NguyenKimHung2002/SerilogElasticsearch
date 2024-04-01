using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilder, loggerConfiguration) =>
{
    //var elasticsearchSettings = hostBuilder.Configuration.GetSection(nameof(ElasticsearchSettings)).Get<ElasticsearchSettings>();

    var envName = builder.Environment.EnvironmentName.ToLower().Replace(".", "-");
    var yourAppName = "dev-8.13.0";
    var yourTemplateName = "dev-8.13.0";

    loggerConfiguration
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://183.80.130.240:9200"))
        {
            IndexFormat = $"{yourAppName}-{DateTimeOffset.Now:yyyy-MM}",
            AutoRegisterTemplate = true,
            OverwriteTemplate = true,
            TemplateName = yourTemplateName,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            TypeName = null,
            BatchAction = ElasticOpType.Create,
            ModifyConnectionSettings = x =>
            {
                return x.ApiKeyAuthentication("PpWrlY4BSNINE6GDO2Fd", "1tQ1NHAXTG2Tsb55Y9X7Yw");
            },
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();