using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model.Exceptions;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //UploadSimpleText();
            //UploadTwoFiles();
            //UploadFicheiro2();
            //serializeOperation();
            teste();
        }



        public static void UploadSimpleText()
        {
            const string fileName = "C:\\exemplos\\error.txt";
 
            using (var content = new MultipartFormDataContent())
            using (var fileStream = File.OpenRead(fileName))
            {
                var client = new System.Net.Http.HttpClient();
                var fileContent = new StreamContent(fileStream);
                 
                fileContent.Headers.ContentType = 
                    MediaTypeHeaderValue.Parse("text/plain");
                 
                fileContent.Headers.ContentDisposition = 
                    new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName
                };
                 
                content.Add(fileContent);
                var result = client.PostAsync("http://WLX0113153:61181/api/Upload/UploadFile", content).Result;
                result.EnsureSuccessStatusCode();
            }
        }
        
        public static void UploadTwoFiles()
        {
            const string fileName1 = "C:\\exemplos\\error1.txt";
            const string fileName2 = "C:\\exemplos\\error2.txt";
            
            using (var content = new MultipartFormDataContent())
            using (var file1 = File.OpenRead(fileName1))
            using (var file2 = File.OpenRead(fileName2))
            {
                var client = new System.Net.Http.HttpClient();
                var fileContent1 = new StreamContent(file1);
                fileContent1.Headers.ContentType = 
                    MediaTypeHeaderValue.Parse("text/plain");
 
                fileContent1.Headers.ContentDisposition = 
                    new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName1
                };
 
                var fileContent2 = new StreamContent(file2);
                fileContent2.Headers.ContentType = 
                    MediaTypeHeaderValue.Parse("text/plain");
 
                fileContent2.Headers.ContentDisposition = 
                    new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName2
                };
 
                content.Add(fileContent1);
                content.Add(fileContent2);

                var result = client.PostAsync("http://WLX0113153:61181/api/Upload/UploadFile", content).Result;
                result.EnsureSuccessStatusCode();
            }
        }

        public static void UploadFicheiro()
        {

            Concelho file = new Concelho { Id = 1,  Nome = "Lisboa" };

            var client = new System.Net.Http.HttpClient();
            
            String XmlizedString = null;
            XmlSerializer x = new XmlSerializer(file.GetType());
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            x.Serialize(xmlTextWriter, file);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            UTF8Encoding encoding = new UTF8Encoding();
            XmlizedString = encoding.GetString(memoryStream.ToArray());
            XmlizedString = XmlizedString.Substring(1);


            var content = new StringContent(XmlizedString, Encoding.UTF8, "text/xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //content.Headers.Add("Content-Type", "text/xml");
            
            var result = client.PostAsync("http://localhost:61181/api/Upload/UploadOperations", content).Result;
            result.EnsureSuccessStatusCode();
            
        }
        
        public static void UploadFicheiro2()
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:61181/api/Upload/UploadOperations");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ "
                                  +"\"OperationEntryId\": 1,"
                                  +"\"CodigoOperacao\": 2,"
                                  +"\"IdSeguradora\": 3,"
                                  +"\"Apolice\": \"sample string 4\","
                                  +"\"DataInicioCobertura\": \"2013-11-29T11:15:49.439848+00:00\","
                                  +"\"DataFimCobertura\": \"2013-11-29T11:15:49.439848+00:00\","
                                  +"\"NomeResponsavelSinistros\": \"sample string 7\","
                                  +"\"EmailResponsavelSinistros\": \"sample string 8\","
                                  +"\"TipoContrato\": \"sample string 9\","
                                  +"\"MotivoAnulacao\": \"sample string 10\","
                                  +"\"Matricula\": \"sample string 11\","
                                  +"\"Nacionalidade\": \"sample string 12\","
                                  +"\"Marca\": \"sample string 13\","
                                  +"\"Modelo\": \"sample string 14\","
                                  +"\"AnoConstrucao\": 15,"
                                  +"\"CategoriaVeiculo\": \"sample string 16\","
                                  +"\"ConcelhoCirculacao\": \"sample string 17\","
                                  +"\"NomeTomadorSeguro\": \"sample string 18\","
                                  +"\"MoradaTomadorSeguro\": \"sample string 19\","
                                  +"\"CodigoPostalTomador\": \"sample string 20\","
                                  +"\"CodigoPostalExtTomador\": \"sample string 21\","
                                  +"\"EmailTomadorSeguro\": \"sample string 22\","
                                  +"\"NifTomadorSeguro\": \"sample string 23\","
                                  +"\"TipoIdentificacaoSeguro\": \"sample string 24\","
                                  +"\"NumeroIdentificacaoSeguro\": \"sample string 25\","
                                  +"\"NomeCondutorHabitual\": \"sample string 26\","
                                  +"\"MoradaCondutorHabitual\": \"sample string 27\","
                                  +"\"CodigoPostalCondutorHabitual\": \"sample string 28\","
                                  +"\"CodigoPostalExtCondutorHabitual\": \"sample string 29\","
                                  +"\"EmailCondutorHabitual\": \"sample string 30\","
                                  +"\"NifCondutorHabitual\": \"sample string 31\","
                                  +"\"TipoIdentificacaoCondutorHabitual\": \"sample string 32\","
                                  +"\"NumeroIdentificacaoCondutorHabitual\": \"sample string 33\""
                                +"}";


                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

            }

        }

        public static void serializeOperation()
        {
            EventoStagging file = new EventoStagging();

            var client = new System.Net.Http.HttpClient();

            String XmlizedString = null;
            XmlSerializer x = new XmlSerializer(file.GetType());
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            x.Serialize(xmlTextWriter, file);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            UTF8Encoding encoding = new UTF8Encoding();
            XmlizedString = encoding.GetString(memoryStream.ToArray());
            XmlizedString = XmlizedString.Substring(1);

            var content = new StringContent(XmlizedString, Encoding.UTF8, "text/xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var result = client.PostAsync("http://localhost:61181/api/Upload/UploadOperations", content).Result;

            //Erro: usando PostAsXml não é corretamente recebido na WebAPI (null)
            //var result = client.PostAsXmlAsync("http://localhost:61181/api/Upload/UploadOperations", file).Result;

            result.EnsureSuccessStatusCode();
        }


        public static void teste()
        {
            try
            {
                DomainModels context = new DomainModels();
                context.Database.Connection.ConnectionString = "Data Source=slx01dev20;Initial Catalog=ISP_BD_MATRICULAS;User Id=user_ispmat;Password=admin";

                IQueryable<Ficheiro> query = null;
                query = context.Ficheiros;

                FicheiroRepository ficheiroRepository = new FicheiroRepository();
                

                query = ficheiroRepository.All;
                query = ficheiroRepository.All.Where(f => f.estado == Ficheiro.EstadoFicheiro.pendente);
                int d = 3;
                foreach (Ficheiro file in query.ToList())
                {
                    file.estado = Ficheiro.EstadoFicheiro.emProcessamento;

                    List<ErroFicheiro> errosFicheiro = new List<ErroFicheiro>();
                    try
                    {
                        file.validar();
                    }
                    catch (ErroFicheiroException ex)
                    {
                        file.errosFicheiro = ex.errosFicheiro;
                        file.estado = Ficheiro.EstadoFicheiro.erro;
                        continue;
                    }

                    file.carregaEventos();
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
