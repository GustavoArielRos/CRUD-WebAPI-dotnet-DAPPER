using CrudDapperVideo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//criando aqui
//Os métodos que estão escritos na IUsuarioInterface estarão implementados no UsuarioService
builder.Services.AddScoped<IUsuarioInterface, UsuarioService>();

//com isso colocamos o serviço do AutoMapper na aplicação
builder.Services.AddAutoMapper(typeof(Program));

//mexendo no CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("usuariosApp", builder =>
    {
        //construção da politica
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//mexendo no CORS
app.UseCors("usuariosApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
