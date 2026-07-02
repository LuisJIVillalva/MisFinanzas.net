using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoApis.Migrations
{
    /// <inheritdoc />
    public partial class LowercaseIdentifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "tasks");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "tasks",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tasks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "tasks",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "tasks",
                newName: "is_completed");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "tasks",
                newName: "ix_tasks_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_tasks",
                table: "tasks",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_users_user_id",
                table: "tasks",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_users_user_id",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tasks",
                table: "tasks");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tasks",
                newName: "Tasks");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Tasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tasks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Tasks",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "is_completed",
                table: "Tasks",
                newName: "IsCompleted");

            migrationBuilder.RenameIndex(
                name: "ix_tasks_user_id",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
