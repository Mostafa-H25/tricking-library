var builder = WebApplication.CreateBuilder(args);

const string allCors = "all";
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options => 
    options.AddPolicy(allCors,
        policyBuilder => policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(allCors);
app.MapControllers();
    
app.Run();

