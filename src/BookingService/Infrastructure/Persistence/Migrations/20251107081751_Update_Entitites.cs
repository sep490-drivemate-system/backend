    using Microsoft.EntityFrameworkCore.Migrations;

    #nullable disable

    namespace BookingService.Infrastructure.Persistence.Migrations
    {
        /// <inheritdoc />
        public partial class Update_Entitites : Migration
        {
            /// <inheritdoc />
            protected override void Up(MigrationBuilder migrationBuilder)
            {
              
            }

            /// <inheritdoc />
            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.AddColumn<string>(
                    name: "description",
                    table: "RoadType",
                    type: "character varying(256)",
                    maxLength: 256,
                    nullable: true);
            }
        }
    }
