using LibraryBookApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB (SQLite)
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? "Data Source=library.db";
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(cs));

var app = builder.Build();

// Auto-apply migrations at startup (optional for dev). Comment if undesired in production.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
