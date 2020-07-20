using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Loja.Testes.Automatizados
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [InlineData("1 parametro 1", true)]
        [InlineData("2 parametro 1", true)]
        [InlineData("3 parametro 1", true)]
        public static void TesteTeoria(string texto, bool flag)
        {
            // assert
            Assert.True(true);
        }
        public class UmaClasseComDados
        {
            public string Content { get; set; }
            public string Email { get; set; }
            public string MyProperty1 { get; set; }
        }

        public class SendEmailParameters : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // parameters to test send email success
                yield return new object[]
                {
            new UmaClasseComDados
            {
                Content = "hello world",
                Email = "user@provider.com"
            }
                };

                // parameters to test send email failed
                yield return new object[]
                {
            new UmaClasseComDados
            {
                Content = "hello world",
                Email = "wrong-email"
            }
                };
                // parameters to test send email failed
                yield return new object[]
                {
            new UmaClasseComDados
            {
                Content = "hello world",
                Email = "wrong-email"
            }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(SendEmailParameters))]
        public static void TesteTeoriaDados(UmaClasseComDados texto)
        {
            // assert
            Assert.True(true);
        }

    }
}
