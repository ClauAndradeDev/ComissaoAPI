using System.Text.Json;
using Ademicon.Comissao.Service.ComissaoService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ComissaoService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Ademicom.Comissao.Api");
        opt.RoutePrefix = String.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();