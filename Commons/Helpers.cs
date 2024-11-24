

using System.Globalization;

namespace Terra.Commons
{
    public static class Helpers
    {
       
        public static string FormatNumberToCurrency(dynamic number, string format = "C")
        {
            double numberValue = 0;
            string numberString = "0";

            if (number is null)
                number = "0";

            if (!(number is string))
                return number.ToString(format, CultureInfo.CreateSpecificCulture("es-US"));
            else
            {
                if (number == "")
                    number = "0";
                numberString = number;
            }

            numberValue = FormatCurrencyToNumber<double>(numberString);

            return numberValue.ToString(format, CultureInfo.CreateSpecificCulture("es-US"));
        }

        public static T FormatCurrencyToNumber<T>(string valor, string cultura = "es-US")
        {
            NumberStyles style = NumberStyles.Currency | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultura);

            if (string.IsNullOrEmpty(valor))
                valor = "0";

            if (typeof(T) == typeof(int))
            {
                if (valor.Contains('.') && (valor.Length - valor.IndexOf('.')) > 0)
                    valor = valor.Remove(valor.IndexOf('.'), valor.Length - 1);
                return (T)Convert.ChangeType(int.Parse(valor, style, culture), typeof(T));
            }

            try
            {
                if (typeof(T) == typeof(double))
                    return (T)Convert.ChangeType(double.Parse(valor, style, culture), typeof(T));

                if (typeof(T) == typeof(string))
                {
                    double aux = double.Parse(valor, style, culture);
                    return (T)Convert.ChangeType(aux.ToString("G", culture), typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("El tipo de dato solicitado no es valido. " + ex.Message);
            }

            throw new ArgumentException("El tipo de dato solicitado no es valido.");
        }

        public static string FormatFecha(string fecha)
        {
            if (!string.IsNullOrEmpty(fecha))
            {
                string[] formats = { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "yyyy/MM/dd", "dd/MM/yyyy HH:mm", "dd/MM/yyyy" };
                DateTime dateTime = DateTime.ParseExact(fecha, formats, CultureInfo.CreateSpecificCulture("Es"), DateTimeStyles.None);
                return dateTime.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("Es"));
            }
            return "";
        }

        public static string formatFecha(DateTime? fecha, string formato = "")
        {
            if (fecha != null)
            {
                if (string.IsNullOrEmpty(formato))
                    return fecha?.ToString("yyyy/MM/dd");
                else
                    return fecha?.ToString(formato);
            }
            return "";
        }
    }
}
