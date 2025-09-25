using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.DAL;
using TodoApi.Extensions;
using TodoApi.Mappers;
using TodoApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // This stops the automatic ProblemDetails response
        options.SuppressModelStateInvalidFilter = true;
    }); ;

//custom services

////automapper registration.
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);

var app = builder.Build();

//create the database if doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

////custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
