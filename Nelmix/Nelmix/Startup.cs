using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Nelmix.Configuration;

namespace Nelmix
{
    public class Startup
    {
        private IConfiguration Configuration { get; } // Agrega una propiedad de configuración


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // Inicializa la propiedad de configuración
        }

        public void ConfigureServices(IServiceCollection services)
        {

            string connectionString = Configuration.GetConnectionString("Conex");
            services.AddDbContext<CasinoContext>(options =>
                options.UseSqlServer(connectionString));

            // Otros servicios
            services.AddControllers();
            services.GetDependencyInjections();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Casino API Nelmix",
                    Version = "V1",
                    Description = "La API del Casino de Las Vegas es una aplicación que simula la funcionalidad de un casino en línea, inspirado en los casinos de Las Vegas Permite a los usuarios administrar cuentas bancarias, fichas y jugar varios juegos, cumpliendo con una serie de reglas y restricciones específicas. \n" +
                        "\n" +
                        "Datos Relevantes:\n" +
                        "  - Las fichas solo pueden comprarse en dólares y se pueden cambiar a cualquier moneda exceptuando DOLARES.\n" +
                        "  - Al cambiar fichas a Monedas debes introducir el nombre de la moneda destino disponibles:\n" +
                        "  - Al crear la cuenta bancaria o añadir saldo se debe identificar el tipo de moneda que manejara su cuenta mediante el id de la moneda:\n" +
                        "  - Si se pierde 10000 USD cantidad de dinero en un solo juego, el usuario no podrá acceder más a dicho juego.\n" +
                        "  - Si se gana 25000 USD cantidad de dinero, el usuario no podrá acceder más a ningun juego.\n" +
                        "  - Los usuarios sin fichas suficientes para apostar, usuarios menores sin adulto asignado y con limites excedidos NO podran jugar\n" +
                        "\n" +
                        "  - Valor de Fichas:\n" +
                        "    - Fichas Rojas (ID = 1, USD 50)\n" +
                        "    - Fichas Amarillas (ID = 2, USD 100)\n" +
                        "    - Fichas Verdes (ID = 3, USD 500)\n" +
                        "    - Fichas Negras (ID = 4, USD 1000)\n" +
                         "\n" +
                        "  - Identificar de Monedas:\n" +
                        "    - Dólar estadounidense ID = 1\n" +
                        "    - Euro ID = 2\n" +
                        "    - Peso mexicano MXN ID = 3\n" +
                        "    - Yen japonés ID = 4\n" +
                        "    - Franco suizo ID = 5\n" +
                        "    - Peso argentino ID = 6\n" +
                        "    - Real brasileño ID = 7\n" +
                        "    - Peso dominicano ID = 8\n" +
                        "\n" +
                        "Aclaraciones:\n" +
                        "  - Al utilizar el endpoint convertirMonedasDolares se convertiran TODAS las monedas que manejaba la cuenta bancaria a DOLARES.\n" +
                        "  - Al cambiar fichas a cualquier otro tipo de moneda convertira tu saldo actual mas la suma equivalente de cambio de fichas a la moneda destino, es decir cambiara el tipo de moneda de tu cuenta bancaria a la moneda destino.\n" +
                        "  - Los usuarios tienen un estado, el endpoint InhabilitarUsuarios se encarga de cambiarlo\n"

                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}

