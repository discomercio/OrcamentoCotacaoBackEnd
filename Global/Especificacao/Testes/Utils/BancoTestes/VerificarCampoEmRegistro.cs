using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils.BancoTestes
{

    static class VerificarCampoEmRegistro
    {
        public static void VerificarRegistro<T>(string campo, string valor_desejado, T registro)
        {
            string especial_hora_atual_formato_HoraParaBanco = "especial: hora atual, formato HoraParaBanco";
            string especial_data_atual_com_hora = "especial: data atual, com hora";
            if (valor_desejado == especial_hora_atual_formato_HoraParaBanco || valor_desejado == especial_data_atual_com_hora)
            {
                //neste caso, precisamos testar uma faixa de tempo

                //de 10 segundos antes
                var inicio = DateTime.Now.AddSeconds(-10);
                var fim = DateTime.Now.AddSeconds(1);
                var valor_desejado_nesta_passada = valor_desejado;
                while (inicio < fim)
                {
                    valor_desejado_nesta_passada = UtilsGlobais.Util.HoraParaBanco(inicio);
                    if (valor_desejado == especial_data_atual_com_hora)
                        valor_desejado_nesta_passada = inicio.ToString("yyyy-MM-ddTHH:mm:ss");
                    //se este está igual, paramos por aqui
                    if (VerificarRegistroInterno(campo, valor_desejado_nesta_passada, registro, false, true))
                        return;
                    inicio = inicio.AddSeconds(1);
                }

                //se não achamos nenhum igual, executamos pelo caminho padrão e dá erro
                //fazemos isto para melhorar a mensagem de erro
                valor_desejado = valor_desejado_nesta_passada;
            }

            //verificamos o resto dos campos
            VerificarRegistroInterno(campo, valor_desejado, registro, true, false);
        }

        private static bool VerificarRegistroInterno<T>(string campo, string valor_desejado, T registro, bool registrarErros,
            bool datasSemMilissegundos)
        {
            if (valor_desejado == "especial: data atual, sem hora")
            {
                valor_desejado = DateTime.Now.Date.ToString("yyyy-MM-ddTHH:mm:ss");
                datasSemMilissegundos = true;
            }
            if (valor_desejado.Contains("\\n")) valor_desejado = valor_desejado.Replace("\\n", "\n");

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new DecimalConverter() }
            };
            if (datasSemMilissegundos)
                settings.Converters.Add(new DatetimeConverter());

            //tiramos um clone
            string original = Newtonsoft.Json.JsonConvert.SerializeObject(registro, settings);
            T copia = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(original, settings);
            //por causa do t_pedido:Data_hora (campo computado no sql server), precisamos serializar da cópia porque o campo não é criado através do DeserializeObject
            original = Newtonsoft.Json.JsonConvert.SerializeObject(copia);
            if (copia == null)
                throw new Exception($"copia==nul");
            if (!WhenInformoCampo.InformarCampo(campo, valor_desejado, copia))
                throw new Exception($"Campo {campo} não encontrado em {typeof(T).Name}");
            string desejado = Newtonsoft.Json.JsonConvert.SerializeObject(copia, settings);
            if (desejado != original && registrarErros)
                LogTestes.LogTestes.ErroNosTestes($"ThenTabelaRegistroComCampoVerificarCampo {typeof(T).Name} campo {campo} valor errado, desejado: {desejado}, original: {original}");
            if (registrarErros)
                Assert.Equal(desejado, original);
            return desejado == original;
        }
    }

    #region DecimalConverter
    /*
     * 
    //copiado de https://stackoverflow.com/questions/24051206/handling-decimal-values-in-newtonsoft-json
    precisamos para resolver isto:
                                     ↓ (pos 511)
    Expected: ···lor_Parcela":3440.0000,"Pce_Forma_Pagto_Entrada":0,"Pce_Forma···
    Actual:   ···lor_Parcela":3440.00,"Pce_Forma_Pagto_Entrada":0,"Pce_Forma_P···
                                     ↑ (pos 511)
    Ocorre porque o decimal é deserializado com outra precisão
    */

#nullable disable
    /// <inheritdoc cref="JsonConverter"/>
    /// <summary>
    /// Converts an object to and from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter"/>
    public class DecimalConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// Gets a new instance of the <see cref="DecimalConverter"/>.
        /// </summary>
        public static readonly DecimalConverter Instance = new DecimalConverter();

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        /// <seealso cref="JsonConverter"/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        /// <seealso cref="JsonConverter"/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader2, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            object readerValue = reader2.Value;
            if (readerValue is double valueDouble)
                readerValue = valueDouble.ToString();
            if (readerValue is float valuefloat)
                readerValue = valuefloat.ToString();
            if (readerValue is decimal valuedecimal)
                readerValue = valuedecimal.ToString();

            if (!(readerValue is string value))
            {
                if (objectType == typeof(decimal?))
                {
                    return null;
                }

                return default(decimal);
            }

            if (decimal.TryParse(value, out var result))
            {
                return result;
            }

            if (objectType == typeof(decimal?))
            {
                return null;
            }

            return default(decimal);
        }

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <seealso cref="JsonConverter"/>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var d = default(decimal?);

            if (value != null)
            {
                d = value as decimal?;
                if (d.HasValue)
                {
                    d = new decimal(decimal.ToDouble(d.Value));
                }
            }

            Newtonsoft.Json.Linq.JToken.FromObject(d ?? 0).WriteTo(writer);
        }
#nullable enable

    }
    #endregion

    #region DatetimeConverter
    /*
     * 
    precisamos para resolver isto:
                                     ↓ (pos 1144)
    Expected: ···:"2021-03-30T17:01:28-03:00","Etg_Imediata_Usuario":"USRMAG",···
    Actual:   ···:"2021-03-30T17:01:25.9032042-03:00","Etg_Imediata_Usuario":"···
                                     ↑ (pos 1144)
      Ocorre por causa dos milissegundos. Neste caso específico, queremos serializar SEM os milissegundos.
      Usamos somente quando queremos testar se a data/hora do registro é a atual.
    */

#nullable disable
    /// <inheritdoc cref="JsonConverter"/>
    /// <summary>
    /// Converts an object to and from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter"/>
    public class DatetimeConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// Gets a new instance of the <see cref="DatetimeConverter"/>.
        /// </summary>
        public static readonly DatetimeConverter Instance = new DatetimeConverter();

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        /// <seealso cref="JsonConverter"/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        /// <seealso cref="JsonConverter"/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader2, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            object readerValue = reader2.Value;
            if (readerValue is DateTime valueDateTime)
                readerValue = valueDateTime.ToString();

            if (!(readerValue is string value))
            {
                if (objectType == typeof(DateTime?))
                {
                    return null;
                }

                return default(DateTime);
            }

            if (DateTime.TryParse(value, out var result))
            {
                return result;
            }

            if (objectType == typeof(decimal?))
            {
                return null;
            }

            return default(decimal);
        }

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <seealso cref="JsonConverter"/>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var d = default(DateTime?);

            if (value != null)
            {
                d = value as DateTime?;
                if (d.HasValue)
                {
                    //sem milissegundos
                    d = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, d.Value.Hour, d.Value.Hour, d.Value.Second, d.Value.Kind);
                }
            }

            Newtonsoft.Json.Linq.JToken.FromObject(d).WriteTo(writer);
        }
#nullable enable

    }
    #endregion
}
