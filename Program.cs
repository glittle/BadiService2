using Microsoft.OpenApi.Models;
using BadiService2.BadiMain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var logger = LoggerFactory.Create(config =>
{
  config.AddConsole();
}).CreateLogger("Program");

// builder.Services.AddSwaggerGen(options =>
// {
//   options.SwaggerDoc("v1", new OpenApiInfo
//   {
//     Version = "v1",
//     Title = "ToDo API",
//     Description = "An ASP.NET Core Web API for managing ToDo items",
//     TermsOfService = new Uri("https://example.com/terms"),
//     Contact = new OpenApiContact
//     {
//       Name = "Example Contact",
//       Url = new Uri("https://example.com/contact")
//     },
//     License = new OpenApiLicense
//     {
//       Name = "Example License",
//       Url = new Uri("https://example.com/license")
//     }
//   });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// app.UseHttpsRedirection();


BadiDate Run(HttpRequest request, int gYear = 0, int gMonth = 0, int gDay = 0, int fragment = 1)
{
  logger.LogInformation($"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}");
  var gDate = gYear == 0 || gMonth == 0 || gDay == 0 ? DateTime.Today : new DateTime(gYear, gMonth, gDay);
  var bDate = new BadiCalc().GetBadiDate(gDate, (RelationToSunset)fragment);
  return bDate;
}

app.MapGet("/Today/{gYear?}/{gMonth?}/{gDay?}/{fragment?}", Run);
app.MapGet("/{gYear?}/{gMonth?}/{gDay?}/{fragment?}", Run);



// ports, see https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727
// var port = Environment.GetEnvironmentVariable("PORT") ?? "8009";
app.Run();

