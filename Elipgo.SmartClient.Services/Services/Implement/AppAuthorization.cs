using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class AppAuthorization : IAppAuthorization
    {
        private List<AppAuthorizationEntity> _authorization;
        //private Task _initializationTask;
        public AppAuthorization()
        {
            _authorization = new List<AppAuthorizationEntity>();
        }

        public AppAuthorization(string token)
        {
            // 1. Inicializa la lista vacía para evitar NullReferenceException si algo falla
            _authorization = new List<AppAuthorizationEntity>();

            try
            {
                // 2. EL TRUCO: Task.Run mueve la ejecución a un hilo del ThreadPool.
                // .Result bloquea el hilo actual (UI) hasta que termine, pero
                // como el trabajo está en otro hilo, no se muerden la cola (Deadlock).
                var resultado = Task.Run(async () => await Vmon5Service.AppAuthorization(token)).Result;

                if (resultado != null)
                {
                    _authorization = resultado;
                }
            }
            catch (Exception ex)
            {
                // Loguea el error aquí si es necesario
                Console.WriteLine("Error obteniendo autorización: " + ex.Message);
            }
        }

        public async Task InitializeAsync(string token)
        {
            var result = await Vmon5Service.AppAuthorization(token);
            _authorization = result ?? new List<AppAuthorizationEntity>();
        }

        public bool Exist(string CodeAction)
        {
            //await _initializationTask;
            return _authorization.FirstOrDefault(x => x.Key.Contains(CodeAction)) != null;
            //return _authorization.Where(x => x.Key.Contains(CodeAction)).FirstOrDefault() != null;
        }

        public List<AppAuthorizationEntity> GetAll()
        {
            return _authorization;
        }

        AppAuthorizationEntity IAppAuthorization.Contains(string CodeAction)
        {
            return _authorization.Where(x => x.Key.Contains(CodeAction)).FirstOrDefault();
        }

    }
}
