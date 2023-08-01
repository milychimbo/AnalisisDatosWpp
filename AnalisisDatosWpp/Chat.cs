namespace AnalisisDatosWpp
{
    public class Chat
    {
        //tengo fecha que es datetime2, tengo persona que es nvarchar(30) y un mensaje que es nvarchar(max)
        public DateTime fecha { get; set; }
        public string? persona { get; set; }

        public string? mensaje { get; set; }
    }
}