namespace Terra.Commons
{
    class ValidacionInyeccionSql
    {

        public bool letraDelante = false;
        public bool letraDetras = false;
        public bool espacioDelante = false;
        public bool espacioDetras = false;
        public bool vacioDelante = false;
        public bool vacioDetras = false;
        public char caracterPosterior;
        public char caracterPredecesor;
        public string cadena;
        public string valor;
        public int[] caracteresEspecialesPermitidos = new int[] { };
        public ValidacionInyeccionSql(string cadena, string valor)
        {
            this.cadena = cadena;
            this.valor = valor;
        }

        public bool validarCadena()
        {
            bool esInyeccion = false;

            //Validacion creada para el parentesis de los procedures cuyo nombre termina con una palabra reservada.
            if (this.cadena.Substring(0, 3).Equals("CALL") && (int)caracterPosterior == 40)
                return esInyeccion;

            //Validacion creada para los grupos de caracteres que inicia con comilla simple
            if (((int)caracterPredecesor == 39 && letraDelante) || ((int)caracterPredecesor == 34 && letraDelante))
                letraDetras = true;

            //Validacion creada para los grupos de caracteres que finaliza con comilla doble
            if (((int)caracterPosterior == 39 && letraDetras) || ((int)caracterPosterior == 34 && letraDetras))
                letraDelante = true;

            if ((int)caracterPredecesor == 58 && (int)caracterPosterior == 62)
            {
                letraDelante = true;
                letraDetras = true;
            }

            esInyeccion = !(letraDelante || letraDetras);

            return esInyeccion;
        }
    }
}
