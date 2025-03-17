using Bogus;
using DatabasePopulator.Models;

namespace DatabasePopulator
{
    public static class DataGenerator
    {
        public static List<CatalogType> GenerateCatalogTypes(int count)
        {
            var faker = new Faker<CatalogType>()
                .RuleFor(ct => ct.Type, f => f.Commerce.Categories(1)[0]);

            return faker.Generate(count);
        }

        public static List<CatalogBrand> GenerateCatalogBrands(int count)
        {
            var faker = new Faker<CatalogBrand>()
                .RuleFor(cb => cb.Brand, f => f.Company.CompanyName());

            return faker.Generate(count);
        }

        public static List<CatalogItem> GenerateCatalogItems(int count, List<CatalogType> catalogTypes, List<CatalogBrand> catalogBrands, string imageFolderPath)
        {
            var faker = new Faker<CatalogItem>()
                .RuleFor(ci => ci.CatalogTypeId, f => f.PickRandom(catalogTypes).Id)
                .RuleFor(ci => ci.CatalogBrandId, f => f.PickRandom(catalogBrands).Id)
                .RuleFor(ci => ci.Description, f => f.Commerce.ProductDescription())
                .RuleFor(ci => ci.Name, f => f.Commerce.ProductName())
                .RuleFor(ci => ci.Price, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(ci => ci.PictureFileName, (f, ci) => $"Normal_Image_{ci.Id}.png")
                .RuleFor(ci => ci.AvailableStock, f => f.Random.Int(1, 100));

            var items = faker.Generate(count);

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var imagePath = System.IO.Path.Combine(imageFolderPath, $"Normal_Image_{i}.png");
                File.Copy($"{imageFolderPath}\\Normal_Image.png", imagePath, true);
            }

            return items;
        }
    }
}