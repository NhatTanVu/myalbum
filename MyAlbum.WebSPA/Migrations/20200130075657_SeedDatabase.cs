using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MyAlbum.Core.Models;

namespace MyAlbum.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT Categories ON");
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                1, "aeroplane"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                2, "bicycle"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                3, "bird"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                4, "boat"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                5, "bottle"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                6, "bus"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                7, "car"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                8, "cat"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                9, "chair"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                10, "cow"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                11, "diningtable"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                12, "dog"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                13, "horse"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                14, "motorbike"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                15, "person"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                16, "pottedplant"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                17, "sheep"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                18, "sofa"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                19, "train"));
            migrationBuilder.Sql(string.Format("INSERT INTO Categories (Id, Name) VALUES ({0}, '{1}')",
                20, "tvmonitor"));
            migrationBuilder.Sql("SET IDENTITY_INSERT Categories OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE (Id >= 1) AND (Id <= 20)");
        }
    }
}
