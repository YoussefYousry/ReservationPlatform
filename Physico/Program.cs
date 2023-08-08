using Microsoft.OpenApi.Models;
using Physico.Extensions;
using Physico_DAL.Models;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;



var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
               policy =>
               {
                   //policy.WithOrigins("https://64be5a29d2bf0603b25edc62--enchanting-semifreddo-5ca30c.netlify.app/",
                   policy.SetIsOriginAllowed(origin => true)
                   //    "http://localhost:3000/")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
               });
});



builder.Services.ConfigureLifeTime();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity<User>();
builder.Services.ConfigureIdentity<Doctor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureJwt(builder.Configuration);//call extension method for JWT
builder.Services.AddControllers().AddJsonOptions(
  opt =>
      opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Place to add JWT with Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
{

    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Name = "Bearer",
        },
        new List<string>()
    }
});
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
