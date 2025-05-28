var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentEmailConfig();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.RegiteredServices(builder);
builder.Services.AddDbContext(builder);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()   // Allows any origin
            .AllowAnyMethod()   // Allows any HTTP method
            .AllowAnyHeader()); // Allows any headers
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
