using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using System.Xml.Serialization;
using ISP.GestaoMatriculas.Model;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using System.Xml;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class FicheiroIsentos
    {
        
        //key
        [Key]
        [Column("CodFicheiroIsentosId_PK")]
        public int ficheiroIsentosId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }
        
        //Atributos do ficheiro
        [Column("NomFicheiro")]
        [Display(Name = "Nome Ficheiro")]
        public string nomeFicheiro { get; set; }
        [Column("XLocalizacao")]
        public string localizacao { get; set; }
        [ForeignKey("estado")]
        [Column("CodEstadoId_FK")]
        public int estadoId { get; set; }
        [Display(Name = "Estado")]
        public virtual ValorSistema estado { get; set; }

        [Column("TotRegistos")]
        public int totalRegistos { get; set; }
        [Column("TotEventosErro")]
        public int numEventosErro { get; set; }
        [Column("TotEventosAviso")]
        public int numEventosAviso { get; set; }

        [Column("DtUpload")]
        [Display(Name = "Data Upload")]
        public DateTime dataUpload { get; set; }
        [Column("DtAlteracao")]
        [Display(Name = "Data Alteração")]
        public DateTime dataAlteracao { get; set; }
        [Column("NomeUtilizador")]
        [Display(Name = "Utilizador")]
        public string userName { get; set; }

        public virtual ICollection<ApoliceIsento> apolicesIsentos { get; set; }

        public FicheiroIsentos()
        {
        }


        public bool validar()
        {

            //Assembly a = Assembly.GetExecutingAssembly();
            //Stream stream = a.GetManifestResourceStream("FNMPAS.xsd");

            //XmlSchema x = XmlSchema.Read(stream, null);
                        
            //XmlSchemaSet schemas = new XmlSchemaSet();
            //schemas.Add(x);

            //XDocument doc = XDocument.Load(this.localizacao);
            //string msg = "";
            //doc.Validate(schemas, (o, e) =>
            //{
            //    msg += e.Message + Environment.NewLine;
            //});


            //throw new ErroFicheiroException();
            //TODO
            return true;
        }

        public bool ficheiroProcessado()
        {
            return false;
        }
    }
}