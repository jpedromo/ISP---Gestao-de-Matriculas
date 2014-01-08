using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class OperationEntry
    {
        public int CodigoOperacao;
        public int IdSeguradora;
        public string Apolice;
        public DateTime DataInicioCobertura;
        public DateTime DataFimCobertura;

        public String NomeResponsavelSinistros;
        public String EmailResponsavelSinistros;
        public String TipoContrato;
        public String MotivoAnulacao;

        public String Matricula;
        public String Nacionalidade;
        public String Marca;
        public String Modelo;
        public int AnoConstrucao;
        public String CategoriaVeiculo;
        public String ConcelhoCirculacao;

        public String NomeTomadorSeguro;
        public String MoradaTomadorSeguro;
        public String CodigoPostalTomador;
        public String CodigoPostalExtTomador;
        public String EmailTomadorSeguro;
        public String NifTomadorSeguro;
        public String TipoIdentificacaoSeguro;
        public String NumeroIdentificacaoSeguro;

        public String NomeCondutorHabitual;
        public String MoradaCondutorHabitual;
        public String CodigoPostalCondutorHabitual;
        public String CodigoPostalExtCondutorHabitual;
        public String EmailCondutorHabitual;
        public String NifCondutorHabitual;
        public String TipoIdentificacaoCondutorHabitual;
        public String NumeroIdentificacaoCondutorHabitual;

    }
}