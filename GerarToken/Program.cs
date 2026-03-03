using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var secrectKey = builder.Configuration.GetSection("JWT").GetValue<string>("SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//É como se fosse, "Minha aplicação usa
    //autenicação e o tipo padrao é JWT Bearer
    options.DefaultChallengeScheme =
        JwtBearerDefaults
            .AuthenticationScheme; // Isso significa que por padrao o sisttema de autenticação, vai usar token
}).AddJwtBearer(options =>
{
    options.SaveToken = true;//Significa se o token deve ser salvo apos uma autenticaçao bem sucedida
    options.RequireHttpsMetadata = false; //Indica se é preciso HTTPS para transmitir o token OBS: Em produção deve ser true
    
    //Classe que permite configurar os parametros de validaçao do token

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //Siginifica, configurações, validar a validade do Emissor da audiencia e o tempo de vida do token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        
        //Vai validar a assinatura de chave do emissor
        ValidateIssuerSigningKey = true,
        
        //Permite ajustar o tempo entre o servidor de autenticação e aplicaçao
        ClockSkew = TimeSpan.Zero,
        
        //Os dois esta sendo atribuido o valor de audiencia e emissor
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        
        //Gerando a chave, usando a chave simetrica usando a secrectkey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "GerarToken"));
}

app.MapControllers();

app.UseHttpsRedirection();




app.Run();