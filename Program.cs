using _26._02_sushi_market_back.Models;
using _26._02_sushi_market_back.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;



SQLitePCL.Batteries_V2.Init(); // ? ??? ???????????

var builder = WebApplication.CreateBuilder(args);



//----------------------------------------
if (!File.Exists("menuDB.db"))
{
    using var conn = new SqliteConnection("Data Source=menuDB.db");
    conn.Open();

    string createTables = @"
    CREATE TABLE IF NOT EXISTS Menu (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        BlobUrl TEXT,
        Count TEXT,
        Title TEXT
    );

    CREATE TABLE IF NOT EXISTS MenuInfo (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Category TEXT,
        BlobUrl TEXT,
        Title TEXT,
        About_1 TEXT,
        About_2 TEXT,
        Price TEXT
    );";

    using var cmd = new SqliteCommand(createTables, conn);
    cmd.ExecuteNonQuery();
}
//----------------------------------------




builder.Services.AddDbContext<ApplicationContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
//----------------------------------------




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BlobService>();
builder.Services.AddSingleton<DatabaseMenuService>();
builder.Services.AddScoped<DatabaseMenuInfoService>();

builder.Services.AddScoped<RegisterService>(provider =>
{
    var context = provider.GetRequiredService<ApplicationContext>();  
    var logger = provider.GetRequiredService<ILogger<RegisterService>>(); 

    return new RegisterService(context, logger); 
});

//----------------------------------------
var app = builder.Build();
app.UseCors("AllowReactApp");
app.MapControllers();


app.UseStaticFiles();

//----------------------------------------



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://0.0.0.0:5256");


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



//using _26._02_sushi_market_back.Models;
//using _26._02_sushi_market_back.Services;
//using Microsoft.EntityFrameworkCore;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        // ?????????? ????????? ???? ??????
//        builder.Services.AddDbContext<ApplicationContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//        // ??????????? ????????
//        builder.Services.AddScoped<RegisterService>();

//        // ?????????? ????????????
//        builder.Services.AddControllers();

//        // ?????????? Swagger
//        builder.Services.AddEndpointsApiExplorer();  // ??? ??????????? API
//        builder.Services.AddSwaggerGen();  // ??? ????????? ???????????? Swagger

//        var app = builder.Build();

//        // ???????? Swagger ? ??????????
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseSwagger();
//            app.UseSwaggerUI();
//        }

//        app.Urls.Add("http://0.0.0.0:5256");


//        var summaries = new[]
//        {
//                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//            };

//        app.MapGet("/weatherforecast", () =>
//        {
//            var forecast = Enumerable.Range(1, 5).Select(index =>
//                new WeatherForecast
//                (
//                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                    Random.Shared.Next(-20, 55),
//                    summaries[Random.Shared.Next(summaries.Length)]
//                ))
//                .ToArray();
//            return forecast;
//        })
//        .WithName("GetWeatherForecast")
//        .WithOpenApi();

//        app.Run();
//    }
//          internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//    {
//        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//    }
//}
