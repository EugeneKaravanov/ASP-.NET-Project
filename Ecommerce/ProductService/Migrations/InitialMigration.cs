using FluentMigrator;

namespace ProductService.Migrations
{
    [Migration(1, "Initial Migration")]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"CREATE TABLE Products (Id SERIAL PRIMARY KEY, Name TEXT, Desctription TEXT, Price DECIMAL, Stock INT);");
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE Products;");
        }
    }
}
