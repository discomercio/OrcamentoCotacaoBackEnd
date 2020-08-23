using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Testes.Utils
{
    public static class StatusCodes
    {
        public static void TestarStatusCode(int statusCode, ActionResult res)
        {
            switch (statusCode)
            {
                case 200:
                    if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                        Assert.Equal("", "Tipo não é OkObjectResult");
                    break;

                case 401:
                    if (res.GetType() != typeof(Microsoft.AspNetCore.Mvc.UnauthorizedResult))
                        Assert.Equal("", "Tipo não é UnauthorizedResult");
                    break;

                default:
                    Assert.Equal("", $"{statusCode} desconhecido");
                    break;
            }
        }
    }
}
