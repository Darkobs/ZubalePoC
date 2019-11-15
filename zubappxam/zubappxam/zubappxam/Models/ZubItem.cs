using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace zubappxam.Models
{
    public class ZubItem
    {
        string id;
        string nombre;
        string correo;
        string direccion;
        string empresa;
        string cuestionario;
        bool pregunta_1;
        string pregunta_2;
        string pregunta_3;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "nombre")]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        [JsonProperty(PropertyName = "correo")]
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        [JsonProperty(PropertyName = "direccion")]
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        [JsonProperty(PropertyName = "empresa")]
        public string Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        [JsonProperty(PropertyName = "cuestionario")]
        public string Cuestionario
        {
            get { return cuestionario; }
            set { cuestionario = value; }
        }

        [JsonProperty(PropertyName = "pregunta_1")]
        public bool Pregunta_1
        {
            get { return pregunta_1; }
            set { pregunta_1 = value; }
        }

        [JsonProperty(PropertyName = "pregunta_2")]
        public string Pregunta_2
        {
            get { return pregunta_2; }
            set { pregunta_2 = value; }
        }

        [JsonProperty(PropertyName = "pregunta_3")]
        public string Pregunta_3
        {
            get { return pregunta_3; }
            set { pregunta_3 = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
