using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaquetesEntrega",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EtiquetaId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EstadoPaquete = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Destino_DireccionCompleta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Destino_Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Destino_Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Registro_TimestampConfirmacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Registro_TipoPrueba = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Registro_UrlEvidencia = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Registro_Latitud = table.Column<double>(type: "double precision", nullable: true),
                    Registro_Longitud = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaquetesEntrega", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RutasDistribucion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstadoRuta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    PersonalEntregaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Almacen_DireccionCompleta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Almacen_Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Almacen_Longitud = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutasDistribucion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PuntosEntrega",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaqueteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Secuencia = table.Column<int>(type: "integer", nullable: false),
                    EstadoPunto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RutaDistribucionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuntosEntrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PuntosEntrega_RutasDistribucion_RutaDistribucionId",
                        column: x => x.RutaDistribucionId,
                        principalTable: "RutasDistribucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paquete_EtiquetaId",
                table: "PaquetesEntrega",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_PuntosEntrega_RutaDistribucionId",
                table: "PuntosEntrega",
                column: "RutaDistribucionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaquetesEntrega");

            migrationBuilder.DropTable(
                name: "PuntosEntrega");

            migrationBuilder.DropTable(
                name: "RutasDistribucion");
        }
    }
}
