﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBase
{
    public interface BaseData<T, U> where T : IModel where U : IFilter
    {
        public abstract List<T> PorFiltro(U obj);

        public abstract T Inserir(T obj);
        public abstract T Atualizar(T obj);
        public abstract bool Excluir(T obj);

        //public MySqlConnection conn { get; set; }
        //public DynamicParameters p { get; set; }

        public static string getConnectionString()
        {
            //return _db.Database.GetConnectionString("SoniAcosDatabase");
            throw new NotImplementedException();
        }


        //static BaseData()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder()
        //              .SetBasePath(Directory.GetCurrentDirectory())
        //              .AddJsonFile("appsettings.json");

        //    AppSett = builder.Build();
        //}
    }
}