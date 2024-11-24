using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terra.Commons;
using Terra.Models;
using ZstdSharp.Unsafe;

namespace Terra.Services
{
    public interface ISessionService
    {
        Task SetSessionValue(string key, dynamic value);
        Task SetSessionValue(List<SessionVariable> variables);
        Task<string> GetSessionValue(string key);
        Task<JObject> GetSessionValue(params string[] keys);
        Task<T> GetSessionObject<T>(string key);
        Task DeleteSessionValue(string key);
        Task DeleteSessionValue(params string[] keys);
        Task<string> GetConecctionString();
    }

    public class SessionService : ISessionService
    {
        public string Key = Environment.GetEnvironmentVariable("EncryptKey");
        private IJSRuntime _jsRuntime;
        private NavigationManager _navigationManager;
        private TERRADB _dbConnection;

        public SessionService(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        public async Task SetSessionValue(string key, dynamic value)
        {
            if (key is null)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(key));

            string stringValue;

            if (value is int || value is double || value is float || value is bool || value is long || value is byte || value is decimal || value is string)
                stringValue = value.ToString();
            else
                stringValue = JsonConvert.SerializeObject(value);

            string keyEncrypt = Seguridad.Encrypt(key, Key);
            string valueEncrypt = Seguridad.Encrypt(stringValue, Key);

            string variableJson = JsonConvert.SerializeObject(new SessionVariable { key = keyEncrypt, value = valueEncrypt });

            await _jsRuntime.InvokeVoidAsync("setLocalVariable", variableJson);
        }

        public async Task SetSessionValue(List<SessionVariable> variables)
        {
            if (variables is null)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(variables));

            string stringValue;

            foreach (var variable in variables)
            {
                dynamic value = variable.value;

                if (value is int || value is double || value is float || value is bool || value is long || value is byte || value is decimal || value is string)
                    stringValue = value.ToString();
                else
                    stringValue = JsonConvert.SerializeObject(value);

                variable.key = Seguridad.Encrypt(variable.key, Key);
                variable.value = Seguridad.Encrypt(stringValue, Key);
            }

            string variableJson = JsonConvert.SerializeObject(variables);

            await _jsRuntime.InvokeVoidAsync("setLocalVariables", variableJson);
        }

        public async Task<string?> GetSessionValue(string key)
        {
            if (key is null)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(key));

            try
            {
                string encryptKey = Seguridad.Encrypt(key);

                string jsonResponse = await _jsRuntime.InvokeAsync<string>("getLocalVariable", encryptKey);

                if (string.IsNullOrEmpty(jsonResponse))
                    return "";

                string value = Seguridad.Decrypt(jsonResponse);

                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(key);
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<JObject> GetSessionValue(params string[] keys)
        {
            if (keys is null || keys.Length == 0)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(keys));

            try
            {
                string[] encryptKeys = new string[keys.Length];

                for (int x = 0; x < keys.Length; x++)
                {
                    encryptKeys[x] = Seguridad.Encrypt(keys[x]);
                }

                string keysJson = JsonConvert.SerializeObject(encryptKeys);

                string jsonResponse = await _jsRuntime.InvokeAsync<string>("getLocalVariables", keysJson);

                if (string.IsNullOrEmpty(jsonResponse))
                    return null;

                JObject response = JObject.Parse(jsonResponse);

                JObject responseDecrypt = new JObject();

                string valueIt = "";
                string itAux = "";

                for (int x = 0; x < keys.Length; x++)
                {
                    itAux = response[encryptKeys[x]].ToString();

                    if (string.IsNullOrEmpty(itAux) || itAux == "null")
                        valueIt = null;
                    else
                        valueIt = Seguridad.Decrypt(response[encryptKeys[x]].ToString());

                    responseDecrypt.Add(keys[x], valueIt);
                }

                return responseDecrypt;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<T?> GetSessionObject<T>(string key)
        {
            if (key is null)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(key));

            try
            {
                string encryptKey = Seguridad.Encrypt(key);

                string jsonResponse = await _jsRuntime.InvokeAsync<string>("getLocalVariable", encryptKey);

                if (string.IsNullOrEmpty(jsonResponse))
                    return default(T);

                string value = Seguridad.Decrypt(jsonResponse);

                T? response = JsonConvert.DeserializeObject<T>(value);

                return response;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task DeleteSessionValue(string key)
        {
            if (key is null)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(key));

            try
            {
                await _jsRuntime.InvokeVoidAsync("deleteLocalVariable", Seguridad.Encrypt(key, Key));
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task DeleteSessionValue(params string[] keys)
        {
            if (keys is null || keys.Length == 0)
                throw new ArgumentException("El parámetro no puede ser nulo", nameof(keys));

            try
            {
                string[] encryptKeys = new string[keys.Length];

                for (int x = 0; x < keys.Length; x++)
                {
                    encryptKeys[x] = Seguridad.Encrypt(keys[x]);
                }

                string keysJson = JsonConvert.SerializeObject(encryptKeys);

                await _jsRuntime.InvokeVoidAsync("deleteLocalVariables", keysJson);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

       

        //IMPORTANTE: Cuando este metodo es llamado y la validacion del token es false
        //se ejecuta el metodo CloseSession que internamente lanza la execepcion OperationCanceledException
        //si esta excepcion es lanzada desde un proceso en un hilo distitnto al principal (Timer o llamadas Task)
        //detendra la ejecucion de codigo, se debe colocar un try cath en el metodo que realizado el llamado
        //Ej: metodo LoadData archivo Comandas.razor.cs
        

        public async Task<string> GetConecctionString()
        {
                return Environment.GetEnvironmentVariable("ConnectionString");;
        }
    }
}
