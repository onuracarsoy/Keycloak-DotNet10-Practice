using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using LearnToKeylcloak.Options;
using LearnToKeylcloak.Services;
using Microsoft.OpenApi;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//KeycloakService dependecy injection ekleme
builder.Services.AddScoped<KeycloakService>();

//OpenAPI de Bearer Authentication ekleme
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes ??=
            new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "JWT Authorization header"
            };

        document.Security ??= new List<OpenApiSecurityRequirement>();

        document.Security.Add(new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecuritySchemeReference("Bearer")
            ] = new List<string>()
        });

        return Task.CompletedTask;
    });
});

//Keycloak Configuration ayarlarını ekleme
builder.Services.Configure<KeycloakConfiguration>(builder.Configuration.GetSection("KeycloakConfiguration"));

//Keycloak Authentication ekleme
builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration.GetSection("Keycloak"));

//Keycloak Authorization ekleme
builder.Services.AddAuthorization(options =>
{
    //Client role bazlı yetkilendirme
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireResourceRoles("realm-admin"));

    //Realm role bazlı yetkilendirme
    //options.AddPolicy("AdminPolicy", policy =>
    //    policy.RequireRealmRoles("realm-admin"));

})
    .AddKeycloakAuthorization(builder.Configuration.GetSection("Keycloak"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   

    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

 

app.MapControllers();

app.Run();
