# API del Casino de Las Vegas 🎰

La **API del Casino de Las Vegas** es una aplicación que simula la funcionalidad de un casino en línea, inspirado en los casinos de Las Vegas. Permite a los usuarios administrar cuentas bancarias, fichas y jugar varios juegos, cumpliendo con una serie de reglas y restricciones específicas.

## Cómo Ejecutarlo ▶️

### Dependencias 📦

Asegúrate de que tu proyecto tenga las siguientes dependencias instaladas mediante el administrador de paquetes NuGet de Visual Studio 2022:

- **Microsoft.EntityFrameworkCore.SqlServer (6.0.23)**: Este paquete proporciona funcionalidades para trabajar con bases de datos SQL Server utilizando Entity Framework Core.

- **Microsoft.EntityFrameworkCore.Tools (6.0.23)**: Herramientas adicionales para Entity Framework Core que facilitan la migración de bases de datos y otras tareas relacionadas con bases de datos.

- **Swashbuckle.AspNetCore (6.2.3)**: Swashbuckle es una biblioteca que simplifica la generación de documentación de API basada en Swagger.

- **System.Data.SqlClient (4.8.5)**: Este paquete proporciona el proveedor de datos de SQL Server necesario para conectarse a bases de datos SQL Server desde la aplicación.

![Imagen de Dependencias](https://media.discordapp.net/attachments/728672417097973834/1166099060269252678/image.png?ex=65494161&is=6536cc61&hm=5f5424f7c3b522723d881cbbe6e40557b15b5c49363166c1c0949ce47b50fe67&=)

## Instalación ⚙️

Si estás utilizando **Visual Studio 2022**, puedes instalar estas dependencias a través del **administrador de paquetes NuGet** siguiendo estos pasos:

1. Abre tu proyecto en **Visual Studio 2022**.

2. Ve al menú **"Herramientas" > "Administrador de paquetes NuGet" > "Consola del Administrador de paquetes".**

3. Utiliza el comando `Install-Package` seguido del nombre de la dependencia para instalarla, por ejemplo:

   ```bash
   Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.23

### Acciones necesarias ✅

Luego de tener todas las dependencias instaladas deberán crear la base de datos, preferiblemente en **MS SQL SERVER** para esto iran a la carpeta llamada BDD en el repositorio y encontrar un archivo ```CBDD.SQL``` ahi encontraran todas las tablas y StoreProcedure utilizados en el proyecto, agregar uno por uno.

Luego de agregar la base de datos deberan modificar la cadena de conexion de la API, tendran que dirijirse al ```appsettings.json``` y modificar la linea **Conex** con el nombre de su servidor, en este caso se utiliza la autentificación de Windows 

![image](https://media.discordapp.net/attachments/728672417097973834/1166090487690690630/image.png?ex=65493965&is=6536c465&hm=889ede9a9d23a69323e4be6f0feb5e01012f0f691de169a9b9efb82be72b178f&=)

#### Cadena con autenticación de Windows
```"data source=TUSERVER;initial catalog=Casinoo;Integrated Security=true; TrustServerCertificate=true; MultipleActiveResultSets=true"```

#### Realizar el Scaffold

Ve a **Visual Studio 2022 > Herramientas > Administrador de paquetes NuGet > Consola del administrador del paquete**, verifica que el proyecto seleccionado sea el correcto en la consola y realiza el scaffold.

![Imagen de Scaffold](https://media.discordapp.net/attachments/728672417097973834/1166098600258969792/image.png?ex=654940f3&is=6536cbf3&hm=9b088d83705da9c8ecd089376cba7b0109cde6e2b8c897640f2612e1a162a7ca&=)

```Scaffold-DbContext "Server=TUSERVER; Database=Casinoo; Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force```

### Recuerda reemplazar los datos por tu propia conexión; en este caso, solo deberías cambiar el nombre del SERVIDOR si estás utilizando autenticación de Windows.

## ¡Todo Listo! 🚀

#### Datos a considerar: Valor de fichas

![Imagen de monedas](https://media.discordapp.net/attachments/1166140795611517018/1166220800320344167/image.png?ex=6549b2c2&is=65373dc2&hm=51b7151a1f7c9aba14d5839838b23f0434418fc0f3ddea97374b5dc0425f7b8c&=&width=855&height=456)

#### Monedas

![Imagen de monedas](https://media.discordapp.net/attachments/1166140795611517018/1166141413663199423/8.png?ex=654968d2&is=6536f3d2&hm=aecf20263ad95110526a8ff00549573da11a9a4ca0b3771bb28deef7aedd7eb2&=&width=811&height=456)

Una vez que ejecutes el proyecto, se abrirá el Swagger, donde podrás probar la API del Casino con sus distintas funcionalidades, que incluyen:


#### Gestión de Usuarios 👤
- Crear Usuario

Se encarga de crear un usuario recibiendo el objeto completo.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141291172745286/1.png?ex=654968b5&is=6536f3b5&hm=4b9f1798926d48be224dc0473ebbea4e8dee107d5af4d2b095305f760aa59bbb&=&width=811&height=456)

- Iniciar Sesión

Se encarga de iniciar sesión mediante el email y contraseña correctas.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141309761900584/2.png?ex=654968ba&is=6536f3ba&hm=921c49de428ca81743e13b81f07aed0efa828ab43d0ed6dc901a6f3db40613b7&=&width=811&height=456)

- Cambiar Contraseña

Se encarga de cambiar la contraseña actual del usuario mediante el email y contraseña correctas.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141326656540835/3.png?ex=654968be&is=6536f3be&hm=4cc8d943e625cae3998817bb3c61d1ec1804f55695bee002acc49274299339ab&=&width=811&height=456)

- Asignar Adulto a un Usuario Menor de Edad

Se encarga de asignar un adulto a un usuario menor de edad.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141353407827968/4.png?ex=654968c4&is=6536f3c4&hm=461daee9350bd09e00fa87bb171a196ff3f2dde5836b30e236152006de8bab0b&=&width=811&height=456)

- Deshabilitar Usuario

Se encarga de deshabilitar un usuario de la base de datos.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141371111972924/5.png?ex=654968c8&is=6536f3c8&hm=778e0641f55e3784e0f9989e17fd653071b19bc560ad57ce6b859aadd6f6059a&=&width=811&height=456)

#### Administración de Cuentas Bancarias 💰

- Crear Cuenta Bancaria (El usuario se asigna en la creación)

Se encarga de crear una cuenta bancaria con un usuario asignado en su creación, además del tipo de moneda y el saldo.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141385095782458/6.png?ex=654968cc&is=6536f3cc&hm=737a1da52b770f44bd2587a05ad46e0e153b00af945f516232bbc5faf08fb400&=&width=811&height=456)

- Eliminar una Cuenta Bancaria

Se encarga de eliminar una cuenta bancaria existente de un usuario

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141427823149096/9.png?ex=654968d6&is=6536f3d6&hm=3d926a22f5e683270c1a09d1d2a0c3326ad8b2b742491b1baafc3b206531a117&=&width=811&height=456)

- Añadir Saldo a Cuenta Existente

Se encarga de añadir saldo extra a una cuenta bancaria

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141446701715568/10.png?ex=654968da&is=6536f3da&hm=a1c727755d3ef4e678fb5caddf07e3a0c33e13b6d507ef1130775d86c24a19e8&=&width=811&height=456)

#### Gestión de Divisas 💱
- Convertir Moneda a Dólares

Se encarga de convertir el tipo de moneda de la cuenta bancaria a dólares, dependiendo el tipo de moneda que tenia se realiza la conversión a dólares

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141453496496218/11.png?ex=654968dc&is=6536f3dc&hm=61b4cf50385146eb50e068e2ac2ce49b1b2dd133716475e9b31c6b6980fc19de&=&width=811&height=456)

- Comprar Fichas (Únicamente con Dólares)

Se encarga de comprar fichas únicamente con dólares, no acepta otra divisa.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141478490345542/12.png?ex=654968e2&is=6536f3e2&hm=68c4c29a3f006e262c6ce41aea6cff92997c267bb75cbe4f408e4a898a06d398&=&width=811&height=456)

- Cambiar Fichas a Cualquier Moneda (Exceptuando Dólares)

Se encarga de cambiar fichas a cualquier moneda exceptuando dólares.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141516624961556/13.png?ex=654968eb&is=6536f3eb&hm=f46386238786ec72a15ef334604ada582424936ccb160695e7c3eaa33eeb81f1&=&width=811&height=456)

#### Estado Financiero 💳
- Obtener Estado Financiero General (Saldo, Ganancias, Pérdidas y Fichas Generales)

Se encarga de obtener el estado financiero general de un usuario a lo largo del casino.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141530600390727/14.png?ex=654968ee&is=6536f3ee&hm=cf0be5ce790c3749fbbcb2e27bf62f2844168f95cfbfb2e2b654ffe30f84822a&=&width=811&height=456)

- Obtener Estado Financiero por Juego (Ganancias, Pérdidas por Juego)

Se encarga de obtener el estado financiero especifico de cada juego de un usuario a lo largo del casino.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141543653068850/15.png?ex=654968f1&is=6536f3f1&hm=90a19fa27edb744b7b83ef9ec1905d7dd221a514ce744e036127a7a7e558d511&=&width=811&height=456)

#### Gestión de Juegos de Casino 🎲
- Jugar Craps

Se encarga de validar si eres elegible para jugar, recibe fichas apostadas y en caso de ganar duplica las fichas, en caso de perder la resta, para ganar es necesario que los dados sumen 7 u 11 para perder que sumen 2, 3 o 12.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141558354083850/16.png?ex=654968f5&is=6536f3f5&hm=832cc2b166f3afada1f67de60c4a409e5483d5c09c7a3469670cf220350f1453&=&width=811&height=456)

- Jugar TragaPerras

Se encarga de validar si eres elegible para jugar, recibe fichas apostadas y en caso de ganar duplica las fichas, en caso de perder la resta, para ganar es necesario que los 3 números salgan iguales.

![Imagen de Scaffold](https://media.discordapp.net/attachments/1166140795611517018/1166141572220473455/17.png?ex=654968f8&is=6536f3f8&hm=c034d947a4cdf7426b165bc4bbac37100a02d12f35cd02462acf4db341be504b&=&width=811&height=456)

- Jugar BlackJack

Se encarga de validar si eres elegible para jugar, recibe fichas apostadas y en caso de ganar duplica las fichas, en caso de perder la resta, para ganar es necesario que tus cartas sumen 21.

![Imagen de Scaffold](https://cdn.discordapp.com/attachments/1166140795611517018/1166141586116202547/18.png?ex=654968fb&is=6536f3fb&hm=ede1ab2178925ab6daaf4ebf0b4e5c4a9c77d085cda5b06f58ae99cb24dc9635&)

¡A disfrutar de la experiencia de juego en la API del Casino de Las Vegas! 🎉

**Nota:** Gracias a **Nelmix** por brindar la oportunidad de aplicar a su programa de pasantías. 👏
