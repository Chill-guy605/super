using Microsoft.EntityFrameworkCore;
using super_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных
builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем контроллеры
builder.Services.AddControllers();

var app = builder.Build();

// Настроим маршруты
app.MapControllers();

app.Run();
