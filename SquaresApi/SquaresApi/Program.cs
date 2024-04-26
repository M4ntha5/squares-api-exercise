using Microsoft.EntityFrameworkCore;
using Squares.Core;
using Squares.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                              throw new InvalidOperationException("Default connection string is null");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(defaultConnectionString, config => config.MigrationsAssembly("Squares.Data.Migrations"));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ServiceCollectionExtension).Assembly);

builder.Services.AddCoreServices();


var app = builder.Build();
{
    using var scope = app.Services.CreateScope();
    
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.Migrate();
}


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