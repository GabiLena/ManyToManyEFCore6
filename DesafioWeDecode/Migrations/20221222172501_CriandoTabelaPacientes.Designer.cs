﻿// <auto-generated />
using DesafioWeDecode.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DesafioWeDecode.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221222172501_CriandoTabelaPacientes")]
    partial class CriandoTabelaPacientes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DesafioWeDecode.Model.Medicamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Indicacao")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Mg")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Medicamentos");
                });

            modelBuilder.Entity("DesafioWeDecode.Model.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Idade")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("DesafioWeDecode.Model.PacienteMedicamento", b =>
                {
                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.Property<int>("MedicamentoId")
                        .HasColumnType("int");

                    b.HasKey("PacienteId", "MedicamentoId");

                    b.HasIndex("MedicamentoId");

                    b.ToTable("PacienteMedicamento");
                });

            modelBuilder.Entity("DesafioWeDecode.Model.PacienteMedicamento", b =>
                {
                    b.HasOne("DesafioWeDecode.Model.Medicamento", "Medicamento")
                        .WithMany("PacienteMedicamentos")
                        .HasForeignKey("MedicamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DesafioWeDecode.Model.Paciente", "Paciente")
                        .WithMany("PacienteMedicamentos")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicamento");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("DesafioWeDecode.Model.Medicamento", b =>
                {
                    b.Navigation("PacienteMedicamentos");
                });

            modelBuilder.Entity("DesafioWeDecode.Model.Paciente", b =>
                {
                    b.Navigation("PacienteMedicamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
