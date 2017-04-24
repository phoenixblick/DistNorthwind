using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWCFServices.DistNorthwindEntities;
using MyWCFServices.DistNorthwindDAL;

namespace MyWCFServices.DistNorthwindLogic
{
    public class ProductLogic
    {
        ProductDAO productDAO = new ProductDAO();
        public ProductEntity GetProduct(int id)
        {
            return productDAO.GetProduct(id);
        }

        public bool UpdateProduct(ref ProductEntity product)
        {
            // first check to see if it is a valid price
            if (product.UnitPrice <= 0)
                return false;
            // ProductName can't be empty
            else if (product.ProductName == null || product.ProductName.Length == 0)
                return false;
            // QuantityPerUnit can't be empty
            else if (product.QuantityPerUnit == null || product.QuantityPerUnit.Length == 0)
                return false;
            // then validate other properties
            else
            {
                ProductEntity productInDB = GetProduct(product.ProductID);
                // invalid product to update
                if (productInDB == null)
                    return false;
                // a product can't be discontinued if there are
                // non-fulfilled orders
                if (product.Discontinued == true && productInDB.UnitsOnOrder > 0)
                    return false;
                else
                    return productDAO.UpdateProduct(ref product);
            }
        }
    }
}
