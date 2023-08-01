using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AnalisisDatosWpp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<string> Post(IFormFile datos)
        {
            //contar lineas insertadas
            var lineas = 0;
            //create an array to save the lines 
            var errores = new List<string>();
            //context
            var _context = new dbContext();
            if (datos != null && datos.Length > 0)
            {
                //read each line
                using (var reader = new StreamReader(datos.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = reader.ReadLine();

                        //the date is the line before ] without the [
                        if (line == null)
                        {
                            line = "";
                        }
                        else
                        {
                            var fecha = line.Split("]")[0];
                            try
                            {
                                fecha = fecha.Split("[")[1];

                                //the person is the line after ] and before :
                                var persona = line.Split("]")[1];
                                persona = persona.Split(":")[0];

                                //the message is the line after the thirth : (sum if in the message there is a : it will be a problem)
                                var mensajes = line.Split(":");
                                var mensaje = "";
                                foreach (var item in mensajes)
                                {
                                    if (item != mensajes[0] && item != mensajes[1] && item != mensajes[2])
                                    {
                                        mensaje += item;
                                    }
                                }
                                //create a new chat object
                                var chat = new Chat();
                                //set the values
                                try
                                {
                                    chat.fecha = DateTime.Parse(fecha);
                                    chat.persona = persona;
                                    chat.mensaje = mensaje;

                                    //save to database
                                    _context.Add(chat);
                                    lineas++;
                                }
                                catch
                                {
                                    errores.Add(line);
                                }

                            }
                            catch
                            {
                                _context.Update(line);
                            }

                        }
                    }
                }
            }
            //the filepath is C:\Users\EmilyChimbo\Downloads\errores.txt
            var filePath = "C:\\Users\\EmilyChimbo\\Downloads\\errores.txt";

            System.IO.File.WriteAllLines(filePath, errores);
            // Your code here to return a string or any other type of data
            return $"Se insertaron {lineas} chats";
        }
    }
}