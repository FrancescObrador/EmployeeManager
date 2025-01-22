using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Win32;

namespace HumanResourcesManager.Utilities
{
    public class DatabaseUtilities
    {

        private HumanResourcesManagerContext _context = new HumanResourcesManagerContext();
        private Logger logger;

        public DatabaseUtilities() 
        {
            logger = new Logger("database-utilities");
        }

        public async Task ExecuteSqlScriptAsync()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivo SQL|*.sql";
                if (openFileDialog.ShowDialog() == true)
                {
                    // Leer el archivo SQL
                    string sqlScript = await File.ReadAllTextAsync(openFileDialog.FileName);

                    // Crear una conexión a la base de datos
                    using (var connection = new SqlConnection(HumanResourcesManagerContext.dbConnection))
                    {
                        await connection.OpenAsync();

                        using (var command = new SqlCommand(sqlScript, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    logger.LogInfo("El script SQL se ejecutó correctamente.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error al ejecutar el script SQL: {ex.Message}");
            }
        }

        public async Task PopulateDB()
        {
            await ExecuteSqlScriptAsync();
            await AddEmployeesPictures();
            await _context.DisposeAsync();
            _context = new HumanResourcesManagerContext();

        }

        private async Task AddEmployeesPictures()
        {
            _context.Employees.Load();

            var httpClient = new HttpClient();

            // Listas de nombres masculinos y femeninos -- Tiene que ser igual al de Build.sql 
            var maleNames = new List<string> { "Juan", "Miguel", "Carlos", "David", "Daniel", "Jaime", "Roberto", "Guillermo", "José", "Marc", "Fernando", "Pablo", "Álvaro", "Jesús", "Raúl" };
            var femaleNames = new List<string> { "María", "Emilia", "Jessica", "Sara", "Laura", "Ana", "Sofía", "Olivia", "Emma", "Isabela", "Gracia", "Mía", "Clara", "Lola", "Amelia" };

            var employees = _context.Employees.ToList();

            foreach (var employee in employees)
            {
                // Determinar la URL de la imagen basándose en el primer nombre
                string imageUrl;
                if (maleNames.Contains(employee.first_name, StringComparer.OrdinalIgnoreCase))
                {
                    imageUrl = "https://avatar.iran.liara.run/public/boy";
                }
                else if (femaleNames.Contains(employee.first_name, StringComparer.OrdinalIgnoreCase))
                {
                    imageUrl = "https://avatar.iran.liara.run/public/girl";
                }
                else
                {
                    logger.LogWarning($"No se debería entrar aqui, el nombre que no coincide es: {employee.first_name}");
                    imageUrl = "https://avatar.iran.liara.run/public";
                }

                try
                {
                    // Descargar la imagen
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    employee.picture = imageBytes;
                    _context.Update(employee);
                    _context.SaveChanges();

                    logger.LogInfo($"Imagen asignada para empleado con ID {employee.id}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error al descargar la imagen para el empleado con ID {employee.id}: {ex.Message}");
                }
            }

            logger.LogInfo("Todas las imágenes se han guardado correctamente.");
        }
    }
}
