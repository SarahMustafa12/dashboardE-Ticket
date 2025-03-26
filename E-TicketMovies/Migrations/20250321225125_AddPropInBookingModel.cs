using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_TicketMovies.Migrations
{
    /// <inheritdoc />
    public partial class AddPropInBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookingItems_Bookings_BookingId",
                table: "bookingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_bookingItems_Movies_MovieId",
                table: "bookingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookingItems",
                table: "bookingItems");

            migrationBuilder.RenameTable(
                name: "bookingItems",
                newName: "BookingItems");

            migrationBuilder.RenameIndex(
                name: "IX_bookingItems_MovieId",
                table: "BookingItems",
                newName: "IX_BookingItems_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingItems_BookingId",
                table: "BookingItems",
                newName: "IX_BookingItems_BookingId");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItems_Bookings_BookingId",
                table: "BookingItems",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItems_Movies_MovieId",
                table: "BookingItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingItems_Bookings_BookingId",
                table: "BookingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingItems_Movies_MovieId",
                table: "BookingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "BookingItems",
                newName: "bookingItems");

            migrationBuilder.RenameIndex(
                name: "IX_BookingItems_MovieId",
                table: "bookingItems",
                newName: "IX_bookingItems_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingItems_BookingId",
                table: "bookingItems",
                newName: "IX_bookingItems_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookingItems",
                table: "bookingItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookingItems_Bookings_BookingId",
                table: "bookingItems",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookingItems_Movies_MovieId",
                table: "bookingItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
