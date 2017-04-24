using System.Linq;
using MyWCFServices.DistNorthwindEntities;
using System.Data.SqlClient;
using System;

namespace MyWCFServices.DistNorthwindDAL
{
    public class ProductDAO
    {
        public ProductEntity GetProduct(int id)
        {
            ProductEntity productEntity = null;
            using (NORTHWNDEntities NWEntities = new NORTHWNDEntities())
            {
                Product product = (from p in NWEntities.Products
                                   where p.ProductID == id
                                   select p).FirstOrDefault();
                if (product != null)
                {
                    productEntity = new ProductEntity();
                    productEntity.ProductID = product.ProductID;
                    productEntity.ProductName = product.ProductName;
                    productEntity.QuantityPerUnit = product.QuantityPerUnit;
                    productEntity.UnitPrice = product.UnitPrice ?? 0m;
                    productEntity.UnitsInStock = product.UnitsInStock ?? 0;
                    productEntity.UnitsOnOrder = product.UnitsOnOrder ?? 0;
                    productEntity.ReorderLevel = product.ReorderLevel ?? 0;
                    productEntity.Discontinued = product.Discontinued;
                    productEntity.RowVersion = product.RowVersion;
                }
            }
            return productEntity;
        }

        public bool UpdateProduct(ref ProductEntity productEntity)
        {
            using (NORTHWNDEntities NWEntities = new NORTHWNDEntities())
            {
                // save productID in a variable
                int productID = productEntity.ProductID;
                Product productInDB =
                                    (from p
                                    in NWEntities.Products
                                     where p.ProductID == productID
                                     select p).FirstOrDefault();
                // check product
                if (productInDB == null)
                    throw new Exception("No product with ID " + productID);
                var context = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)NWEntities).ObjectContext;
                // first detach the object
                context.Detach(productInDB);
                // update product price
                productInDB.ProductName = productEntity.ProductName;
                productInDB.QuantityPerUnit = productEntity.QuantityPerUnit;
                productInDB.UnitPrice = productEntity.UnitPrice;
                productInDB.Discontinued = productEntity.Discontinued;
                productInDB.RowVersion = productEntity.RowVersion;
                // now attach the object again
                NWEntities.Products.Attach(productInDB);
                context.ObjectStateManager.ChangeObjectState(productInDB, System.Data.Entity.EntityState.Modified);

                NWEntities.SaveChanges();

                //refresh the RowVersion property
                productEntity.RowVersion = productInDB.RowVersion;
            }
            return true;
        }
    }
}
