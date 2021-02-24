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
            if (valor_desejado.Contains("\\n")) valor_desejado = valor_desejado.Replace("\\n", "\n");

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new DecimalConverter() }
            };


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
            if (desejado != original)
                LogTestes.LogTestes.ErroNosTestes($"ThenTabelaRegistroComCampoVerificarCampo {typeof(T).Name} campo {campo} valor errado, {desejado}, {original}");
            Assert.Equal(desejado, original);
        }

    }

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
}
