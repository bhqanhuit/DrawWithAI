using DrawApi.Data;
using DrawApi.Filters;
using DrawWithAI.DrawApi.Services;
using Microsoft.EntityFrameworkCore;
using DrawApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ExceptionHandlerFilter>();
    }
    );
// Register your DbContext with MySQL
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddScoped<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ImageAIService>();
builder.Services.AddSingleton<ImageDriveService>();

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
