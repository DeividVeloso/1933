using SpaUserControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaUserControl.Infraestructure.Data.Map
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            //Definição do nome da tabela
            ToTable("User");

            //Gera um Guid no um tipo de Guid que o SQL entenda
            //Sempre verificar se o SPL Provider suporta geração de GUID para o Seu banco ex Oracle, SQL SERVER, MySql
            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name)
                .HasMaxLength(160)
                .IsRequired();

            Property(x => x.Email)
                .HasMaxLength(160)
                //GEra um index unico para o email
                //Não teremos email duplicado na base de dados
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_EMAIL", 1) { IsUnique = true})
                )
                .IsRequired();

            //Deixa o valor fixo de 32 caracteres
            Property(x => x.Password)
                .HasMaxLength(32)
                .IsFixedLength();
        }
    }
}
