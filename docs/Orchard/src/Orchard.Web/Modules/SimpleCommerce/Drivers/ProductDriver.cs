using SimpleCommerce.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;

namespace SimpleCommerce.Drivers {
  public class ProductDriver : ContentPartDriver<ProductPart> {
    protected override DriverResult Display(
        ProductPart part, string displayType, dynamic shapeHelper)
    {
      return ContentShape("Parts_Product",
          () => shapeHelper.Parts_Product(
              Sku: part.Sku,
              Price: part.Price));
    }

    //GET
    protected override DriverResult Editor(ProductPart part, dynamic shapeHelper)
    {
      return ContentShape("Parts_Product_Edit",
          () => shapeHelper.EditorTemplate(
              TemplateName: "Parts/Product",
              Model: part,
              Prefix: Prefix));
    }
 
    //POST
    protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper)
    {
      updater.TryUpdateModel(part, Prefix, null, null);
      return Editor(part, shapeHelper);
    }
  }
}
