using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Parthners");

            migrationBuilder.AddColumn<int>(
                name: "PartnerTypeId",
                table: "Parthners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PartnerType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parthners_PartnerTypeId",
                table: "Parthners",
                column: "PartnerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parthners_PartnerType_PartnerTypeId",
                table: "Parthners",
                column: "PartnerTypeId",
                principalTable: "PartnerType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.InsertData(
            table: "PartnerType",
            columns: new[] { "Id", "TypeName" },
            values: new object[,]
            {
                { 1, "ЗАО" },
                { 2, "ООО" },
                { 3, "ПАО" },
                { 4, "ОАО" }
            });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parthners_PartnerType_PartnerTypeId",
                table: "Parthners");

            migrationBuilder.DropTable(
                name: "PartnerType");

            migrationBuilder.DropIndex(
                name: "IX_Parthners_PartnerTypeId",
                table: "Parthners");

            migrationBuilder.DropColumn(
                name: "PartnerTypeId",
                table: "Parthners");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Parthners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
