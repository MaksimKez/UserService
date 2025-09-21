
using Application.DI;
using Infrastructure.AuthServiceClient;
using Infrastructure.DI;
using Persistence.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<NotificationClientSettings>(
                builder.Configuration.GetSection(NotificationClientSettings.SectionName));
    
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("ReactLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();