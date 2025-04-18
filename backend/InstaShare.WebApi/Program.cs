using Microsoft.AspNetCore.Identity;
using InstaShare.Infrastructure;
using InstaShare.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var frontendUrl = builder.Configuration["FrontendUrl"];
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        builder => {
            builder.WithOrigins(frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();  
app.UseCors("AllowFrontend");

app.Run();