using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace homebrewAppServerAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterProfiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LacticAcid = table.Column<double>(nullable: false),
                    Gypsum = table.Column<double>(nullable: false),
                    CalciumChloride = table.Column<double>(nullable: false),
                    EpsomSalt = table.Column<double>(nullable: false),
                    NonIodizedSalt = table.Column<double>(nullable: false),
                    BakingSoda = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterProfiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Favourite = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Type = table.Column<short>(nullable: false),
                    WaterProfileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Recipes_WaterProfiles_WaterProfileID",
                        column: x => x.WaterProfileID,
                        principalTable: "WaterProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brews",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    BrewDate = table.Column<DateTime>(nullable: false),
                    TastingNotes = table.Column<string>(maxLength: 1000, nullable: true),
                    ABV = table.Column<double>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    RecipeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Brews_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    RecipeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeStep",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Timer = table.Column<int>(nullable: false),
                    RecipeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeStep", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecipeStep_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WaterProfiles",
                columns: new[] { "ID", "BakingSoda", "CalciumChloride", "EpsomSalt", "Gypsum", "LacticAcid", "Name", "NonIodizedSalt" },
                values: new object[] { 1000, 0.0, 0.0, 0.0, 0.0, 0.0, "APA focused", 0.0 });

            migrationBuilder.InsertData(
                table: "WaterProfiles",
                columns: new[] { "ID", "BakingSoda", "CalciumChloride", "EpsomSalt", "Gypsum", "LacticAcid", "Name", "NonIodizedSalt" },
                values: new object[] { 1001, 0.0, 0.0, 0.0, 0.0, 0.0, "Lager focused", 0.0 });

            migrationBuilder.InsertData(
                table: "WaterProfiles",
                columns: new[] { "ID", "BakingSoda", "CalciumChloride", "EpsomSalt", "Gypsum", "LacticAcid", "Name", "NonIodizedSalt" },
                values: new object[] { 1002, 0.0, 0.0, 0.0, 0.0, 0.0, "Stout focused", 0.0 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Favourite", "Name", "Type", "WaterProfileID" },
                values: new object[] { 2001, "Characterized by floral, fruity, citrus-like, piney, resinous American hops, the American pale ale is a medium-bodied beer with low to medium caramel, and carries with it a toasted maltiness.", true, "Amarillo SMaSH", (short)9, 1000 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Favourite", "Name", "Type", "WaterProfileID" },
                values: new object[] { 2000, "Kolsch is a unique style in that it is fermented with ale yeast, but then finished with lagering. The result is the best of both worlds: A light easy drinking pale that finishes crisp and clean. The most basic way to separate beers into different styles is by yeast. By far, the two largest groups are ales and lagers.", true, "Brothers Kolsch Ripoff", (short)23, 1001 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Favourite", "Name", "Type", "WaterProfileID" },
                values: new object[] { 2003, "Saison (French, \"season,\" French pronunciation: ​[sɛ. zɔ̃]) is a pale ale that is highly carbonated, fruity, spicy, and often bottle conditioned. It was historically brewed with low alcohol levels, but modern productions of the style have moderate to high levels of alcohol.", false, "Pepper Saison", (short)17, 1001 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Favourite", "Name", "Type", "WaterProfileID" },
                values: new object[] { 2004, "Sour beer is beer which has an intentionally acidic, tart, or sour taste. Traditional sour beer styles include Belgian lambics, gueuze, and Flanders red ale.", true, "White Peach Sour", (short)16, 1001 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Favourite", "Name", "Type", "WaterProfileID" },
                values: new object[] { 2002, "A moderate-strength brown beer with a restrained roasty character and bitterness. May have a range of roasted flavors, generally without burnt qualities, and often has a chocolate-caramel-malty profile.", false, "Raspberry Brown Porter", (short)11, 1002 });

            migrationBuilder.InsertData(
                table: "Brews",
                columns: new[] { "ID", "ABV", "BrewDate", "Name", "Rating", "RecipeID", "TastingNotes" },
                values: new object[] { 3002, 4.7000000000000002, new DateTime(2020, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amarillo SMaSH I", 0.0, 2001, "Cool, think I have found a house brew I can easily do and drink :)" });

            migrationBuilder.InsertData(
                table: "Brews",
                columns: new[] { "ID", "ABV", "BrewDate", "Name", "Rating", "RecipeID", "TastingNotes" },
                values: new object[] { 3000, 5.5, new DateTime(2019, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brothers Kolsch Ripoff I", 2.2999999999999998, 2000, "Not a million miles away from the real thing!" });

            migrationBuilder.InsertData(
                table: "Brews",
                columns: new[] { "ID", "ABV", "BrewDate", "Name", "Rating", "RecipeID", "TastingNotes" },
                values: new object[] { 3001, 4.9000000000000004, new DateTime(2019, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brothers Kolsch Ripoff II", 4.7000000000000002, 2000, "Yep, this one isn't great, there is an odd metalic taste associated with it." });

            migrationBuilder.InsertData(
                table: "Brews",
                columns: new[] { "ID", "ABV", "BrewDate", "Name", "Rating", "RecipeID", "TastingNotes" },
                values: new object[] { 3003, 4.7000000000000002, new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brothers Kolsch Ripoff III", 5.0, 2000, "Nice clean flavour with a reasonably strong aroma and good clarity" });

            migrationBuilder.CreateIndex(
                name: "IX_Brews_RecipeID",
                table: "Brews",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeID",
                table: "Ingredients",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_WaterProfileID",
                table: "Recipes",
                column: "WaterProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStep_RecipeID",
                table: "RecipeStep",
                column: "RecipeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brews");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "RecipeStep");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "WaterProfiles");
        }
    }
}
