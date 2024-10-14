using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Shortner.Web.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Change this to your frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddDbContext<DataStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
