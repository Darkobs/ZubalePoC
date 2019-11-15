using Microsoft.Azure.Mobile.Server;

namespace zubwebback.DataObjects
{
    public class ZubItem : EntityData
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Empresa { get; set; }
        public string Direccion { get; set; }
        public string Cuestionario { get; set; }
        public bool Pregunta_1 { get; set; }
        public string Pregunta_2 { get; set; }
        public string Pregunta_3 { get; set; }
    }
}