using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MejorAppTG2.Modelo;
using SQLite;

namespace MejorAppTG2.Modelo
{
    public class User
    {
        private String nombre;
        private int edad;
        private String sexo;
        private String imagen;


        public User(string nombre, int edad, string sexo, string imagen)
        {
            this.nombre = nombre;
            this.edad = edad;
            this.sexo = sexo;
            this.imagen = imagen;
        }

        public User() { }
    }
}
