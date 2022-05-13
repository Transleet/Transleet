using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transleet.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Components_ComponentId",
                table: "Translations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentId",
                table: "Translations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Locales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Locales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSource",
                table: "Entries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuggestion",
                table: "Entries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TranslationId",
                table: "Entries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Entries_TranslationId",
                table: "Entries",
                column: "TranslationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Translations_TranslationId",
                table: "Entries",
                column: "TranslationId",
                principalTable: "Translations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Components_ComponentId",
                table: "Translations",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Translations_TranslationId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Components_ComponentId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Entries_TranslationId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Locales");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Locales");

            migrationBuilder.DropColumn(
                name: "IsSource",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "IsSuggestion",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "TranslationId",
                table: "Entries");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentId",
                table: "Translations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Components_ComponentId",
                table: "Translations",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id");
        }
    }
}
