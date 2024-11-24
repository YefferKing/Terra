using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Terra.Models;
using System.Data;

namespace Terra.Commons
{
    public interface ITERRADB
    {
        JsonDataResult TERRA_QTConsulta(string sql, string conexionString = "");
    }

    public class TERRADB : ITERRADB
    {

        public async Task<string> GetConecctionString()
        {
            return Environment.GetEnvironmentVariable("ConnectionString");
        }

        public static string TNS_FormatCharacterString(string cadena)
        {
            cadena = cadena.Replace("#", " ");
            cadena = cadena.Replace("--", "  ");

            //Se reemplazan etiquetas para evitar el ataque XSS
            cadena = cadena.Replace("<link", " ");
            cadena = cadena.Replace("<a", " ");
            cadena = cadena.Replace("</a>", " ");
            cadena = cadena.Replace("<img", " ");
            cadena = cadena.Replace("<embed", " ");
            cadena = cadena.Replace("<input", " ");
            cadena = cadena.Replace("<button", " ");
            cadena = cadena.Replace("</button>", " ");
            cadena = cadena.Replace("<iframe", " ");
            cadena = cadena.Replace("</iframe>", " ");

            string compare = cadena;

            string[] cadenaArreglo = cadena.Split(',');

            int iterador = 0;

            while (iterador < cadenaArreglo.Length)
            {

                if (!esPDF(cadenaArreglo[iterador]))
                {
                    cadenaArreglo[iterador] = cadenaArreglo[iterador].Replace("\\", "++..--");
                    cadenaArreglo[iterador] = cadenaArreglo[iterador].Replace("++..--", "/");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SELECT");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "UPDATE");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "DROP");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "DELETE");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "INSERT");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "EXEC");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "WHERE");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "FROM");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "AND");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "OR");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "XOR");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "CONVERT");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SLEEP");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SYSCOLUMS");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "LIKE");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "INTO");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "VALUES");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "IIF");
                    //cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "GROUP");
                    //cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "UNION");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "TOP");
                    //cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "CHR");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SHELL");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "ASCII");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SUBSTRING");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "LENGTH");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "VERSION");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "SQLMAPFILE");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "||");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "&&");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "/**/");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "%2b");
                    //cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "%3");
                    //cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "%0");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "#");
                    cadenaArreglo[iterador] = buscarPalabraClave(cadenaArreglo[iterador], "--");

                }

                if (cadenaArreglo[iterador].Equals("+**+"))
                {
                    cadena = "+**+";
                    break;
                }

                iterador++;
            }

            cadena = cadena.Replace("\\", "++..--");
            cadena = cadena.Replace("++..--", "/");

            string cadenaAux = cadena;
            //insertMySqlInjLog(compare, cadenaAux);

            return cadena;
        }

        private static bool esPDF(string cadena)
        {
            return (!cadena.Contains('(') && !cadena.Contains(')') && !cadena.Contains('\'') && cadena.Length > 10000);
        }

        public static string buscarPalabraClave(string cadena, string valor)
        {
            ValidacionInyeccionSql objInyeccion = new ValidacionInyeccionSql(cadena, valor);
            bool esInyeccion = false;
            string cadenaUp = cadena.ToUpper();
            string cadenaReturn = cadena;
            int valida = 0;
            int asciiAnt = 0;
            int asciiPost = 0;

            while (valida != -1)
            {
                if (valida == 0)
                {
                    valida = cadenaUp.IndexOf(valor);
                }
                else if (valida + valor.Length < cadenaUp.Length)
                {
                    valida = cadenaUp.IndexOf(valor, valida + valor.Length);
                }
                else
                {
                    valida = -1;
                }

                if (valida > -1)
                {
                    if (valida > 0)
                    {
                        objInyeccion.caracterPredecesor = cadenaUp.Substring(valida - 1, 1).ToCharArray()[0];
                        asciiAnt = (int)objInyeccion.caracterPredecesor;
                        if ((asciiAnt >= 65 && asciiAnt <= 90) || (asciiAnt >= 97 && asciiAnt <= 122) || (asciiAnt == 95) || (asciiAnt >= 48 && asciiAnt <= 57) || (asciiAnt == 241) || (asciiAnt == 209))
                            objInyeccion.letraDetras = true;
                        if (asciiAnt == 32)
                            objInyeccion.espacioDetras = true;
                    }
                    else
                    {
                        objInyeccion.vacioDetras = true;
                    }

                    if ((valida + valor.Length) < cadenaUp.Length)
                    {
                        objInyeccion.caracterPosterior = cadenaUp.Substring(valida + valor.Length, 1).ToCharArray()[0];
                        asciiPost = (int)objInyeccion.caracterPosterior;
                        if ((asciiPost >= 65 && asciiPost <= 90) || (asciiPost >= 97 && asciiPost <= 122) || (asciiPost == 95) || (asciiPost >= 48 && asciiPost <= 57) || (asciiPost == 241) || (asciiPost == 209))
                            objInyeccion.letraDelante = true;
                        if (asciiPost == 32)
                            objInyeccion.espacioDelante = true;
                    }
                    else
                    {
                        objInyeccion.letraDelante = true;
                    }

                    esInyeccion = objInyeccion.validarCadena();

                    if (esInyeccion)
                    {
                        cadenaReturn = "+**+";
                        return cadenaReturn;
                    }
                }
            }
            return cadenaReturn;
        }
        public JsonDataResult TERRA_QTConsulta(string sql, string conexionString = "")
        {
            if (string.IsNullOrEmpty(conexionString))
            {
                conexionString = Task.Run(async () =>
                {
                    return await GetConecctionString();
                }).GetAwaiter().GetResult();
            }

            JsonDataResult JsonResult = new JsonDataResult() { CONTENIDO = null };
            string sqlAux = sql;
            sql = TNS_FormatCharacterString(sql);

            if (sql.Equals("+**+"))
            {
                JsonResult.SUCCESS = false;
                JsonResult.CONTENIDO = "Datos incorrectos!";
                return JsonResult;
            }
            else
            {
                
                if (sql.Contains("CALL"))
                {
                    using (MySqlConnection conexion = new MySqlConnection(conexionString))
                    {
                        try
                        {
                            conexion.Open();

                            using (MySqlCommand command = new MySqlCommand(sql, conexion))
                            using (MySqlDataReader reader = (MySqlDataReader)command.ExecuteReader())
                            {
                                DataTable vDataTable = new DataTable();
                                vDataTable.Load(reader);

                                JsonResult.CONTENIDO = JsonConvert.SerializeObject(vDataTable);
                                JsonResult.SUCCESS = true;
                            }
                        }
                        catch (Exception e)
                        {

                            JsonResult.CONTENIDO = "...Oops! Datos incorrectos!";
                            JsonResult.SUCCESS = false;
                        }
                    }
                }
                return JsonResult;
            }
        }

    }
}
