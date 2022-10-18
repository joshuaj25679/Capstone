//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
//builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
// builder.Services.AddDbContext<AccountDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("docker_db2")));
// builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
var app = builder.Build();
app.MapControllers();
app.UseCors();

app.MapGet("/", () => "Hello World!");

app.Run();
