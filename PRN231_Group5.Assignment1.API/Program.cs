using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PRN231_Group5.Assignment1.Repo;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Mappers;
using PRN231_Group5.Assignment1.Repo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
     x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FstoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FStoreDB")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

