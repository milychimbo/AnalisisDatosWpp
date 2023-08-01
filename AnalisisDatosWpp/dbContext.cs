using Microsoft.Data.SqlClient;
using System;

namespace AnalisisDatosWpp
{
    
    public class dbContext
    {
        private string connectionString = string.Empty;
        public dbContext()
        {
            // Build the configuration using the appsettings.json file
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string from the configuration
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Chat chat)
        {
            //create db connection sql
            using (var connection = new SqlConnection(connectionString))
            {
                //open connection
                connection.Open();
                //create command
                var command = connection.CreateCommand();
                //set command text
                command.CommandText = "INSERT INTO Chat(fecha, persona, mensaje) VALUES (@fecha, @persona, @mensaje)";
                //add parameters
                command.Parameters.AddWithValue("@fecha", chat.fecha);
                command.Parameters.AddWithValue("@persona", chat.persona);
                command.Parameters.AddWithValue("@mensaje", chat.mensaje);
                //execute
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public Chat ObtenerUltimo()
        {
             using (var connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();
                var command = connection.CreateCommand();

                // Corregir la consulta para seleccionar los campos Fecha, Persona y Mensaje
                command.CommandText = "SELECT TOP 1 Fecha, Persona, Mensaje FROM [Pruebas].[dbo].[Chat] ORDER BY Fecha DESC";

                using (var reader = command.ExecuteReader())
                {
                    // Verificar si se encontró algún registro
                    if (reader.Read())
                    {
                        // Obtener los valores de las columnas Fecha, Persona y Mensaje
                        DateTime fecha = reader.GetDateTime(0);
                        string persona = reader.GetString(1);
                        string mensaje = reader.GetString(2);

                        // Crear y retornar un objeto Chat con los datos obtenidos
                        return new Chat
                        {
                            fecha = fecha,
                            persona = persona,
                            mensaje = mensaje
                        };
                    }
                }

                // Si no se encontró ningún registro, retornar null
                return null;
            }
        }

        public void Update(string mensajeFaltante)
        {
            var chat = ObtenerUltimo();

            if (chat != null)
            {
                // Concatenar el mensaje faltante con el mensaje existente
                string nuevoMensaje = chat.mensaje + " " + mensajeFaltante;

                using (var connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();
                    var command = connection.CreateCommand();

                    // Actualizar el registro en la base de datos con el nuevo mensaje
                    command.CommandText = "UPDATE [Pruebas].[dbo].[Chat] SET Mensaje = @NuevoMensaje WHERE Fecha = @Fecha";
                    command.Parameters.AddWithValue("@NuevoMensaje", nuevoMensaje);
                    command.Parameters.AddWithValue("@Fecha", chat.fecha);

                    command.ExecuteNonQuery();

                    // Cerrar la conexión
                    connection.Close();
                }
            }
        }
    }
}
