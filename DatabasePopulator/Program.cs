using DatabasePopulator;
using DatabasePopulator.Infrastructure;
using Microsoft.Extensions.Configuration;

using (var context = new CatalogDBContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    int catalogTypeCount = 10; 
    int catalogBrandCount = 10;
    int itemCount = 100000;

    var config = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .Build();

    string imageFolderPath = config["Secrets:IMAGE_FOLDER_PATH"];
    Console.WriteLine($"Image folder path: {imageFolderPath}");
    return;

    if (string.IsNullOrEmpty(imageFolderPath))
    {
        Console.WriteLine("Environment variable IMAGE_FOLDER_PATH is not set.");
        return;
    }

    var catalogTypes = DataGenerator.GenerateCatalogTypes(catalogTypeCount);
    var catalogBrands = DataGenerator.GenerateCatalogBrands(catalogBrandCount);

    context.CatalogTypes.AddRange(catalogTypes);
    context.CatalogBrands.AddRange(catalogBrands);
    context.SaveChanges();

    var catalogItems = DataGenerator.GenerateCatalogItems(itemCount, catalogTypes, catalogBrands, imageFolderPath);

    context.CatalogItems.AddRange(catalogItems);
    context.SaveChanges();
    Console.WriteLine($"Database populated with {itemCount} items.");
}

