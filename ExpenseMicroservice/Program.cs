using AutoMapper;
using CustomerMicroservice.Config;
using ExpenseMicroservice.Config;
using ExpenseMicroservice.Context;
using ExpenseMicroservice.Middlewares;
using ExpenseMicroservice.Repositories;
using ExpenseMicroservice.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region  ============== Setting Swagger dengan security ====================
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name:JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Input token : `Bearer {token}`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[] {}
        }
    });
});
#endregion

#region ============== Inject AppDbContext =============
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region =============== Pesan Response (Ambil value dari appsetting.json) =========================
DataProperties.UnauthorizedMessage = builder.Configuration["GlobalMessage:Unauthorized"];
DataProperties.NotFoundMessage = builder.Configuration["GlobalMessage:NotFound"];
DataProperties.SuccessGetDataMessage = builder.Configuration["GlobalMessage:SuccessGetData"];
DataProperties.SuccessDeleteDataMessage = builder.Configuration["GlobalMessage:SuccessDeleteData"];
DataProperties.SuccessUpdateDataMessage = builder.Configuration["GlobalMessage:SuccessUpdateData"];
DataProperties.SuccessCreateDataMessage = builder.Configuration["GlobalMessage:SuccessCreateData"];
#endregion

#region ========================= Dependencies =============================
builder.Services.AddTransient<IExpenseRepository, ExpenseRepository>();
builder.Services.AddTransient<IExpenseDetailRepository, ExpenseDetailRepository>();
#endregion

//  ============== Setting Middleware  ==============
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
// ========================================================

//  ============== Setting Jwt ambil dari class yang sudah dibuatkan  ==============
builder.AddAppAuthentication();
// ==================================================================================

//  ============== Konfigurasi Auto Mapper ==============
IMapper mapper = MappingConfig.RegisterMapper().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// ===================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();