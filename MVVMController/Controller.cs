using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMController.ProductServiceProxy;
using System.Transactions;

namespace MVVMController
{
    public class Controller
    {
        Product product1, product2;

        public string GetProduct1(int productID)
        {
            return GetProduct(productID, ref product1);
        }

        public string GetProduct2(int productID)
        {
            return GetProduct(productID, ref product2);
        }

        private string GetProduct(int productID, ref Product product)
        {
            string result = "";
            try
            {
                ProductServiceClient client = new ProductServiceClient();
                product = client.GetProduct(productID);
                StringBuilder sb = new StringBuilder();
                sb.Append("ProductID:" +
                product.ProductID.ToString() + "\n");
                sb.Append("ProductName:" +
                product.ProductName + "\n");
                sb.Append("UnitPrice:" +
                product.UnitPrice.ToString() + "\n");
                sb.Append("RowVersion:");
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }
                result = sb.ToString();
            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message.ToString();
            }
            return result;
        }

        public Tuple<string, string> UpdatePrices(decimal newPrice1, decimal newPrice2)
        {
            string txtUpdate1Results = string.Empty;
            string txtUpdate2Results = string.Empty;
            if (product1 == null)
            {
                txtUpdate1Results = "Get product details first";
            }
            else if (product2 == null)
            {
                txtUpdate2Results = "Get product details first";
            }
            else
            {
                bool update1Result = false, update2Result = false;
                using (TransactionScope ts = new TransactionScope())
                {
                    txtUpdate1Results = UpdatePrice(newPrice1, ref product1, ref update1Result);
                    txtUpdate2Results = UpdatePrice(newPrice2, ref product2, ref update2Result);
                    if (update1Result == true && update2Result == true)
                        ts.Complete();
                }
            }

            return new Tuple<string, string>(txtUpdate1Results, txtUpdate2Results);
        }
        private string UpdatePrice(decimal newPrice, ref Product product, ref bool updateResult)
        {
            //Test
            string result = "";

            try
            {
                product.UnitPrice = newPrice;
                ProductServiceClient client = new ProductServiceClient();
                updateResult = client.UpdateProduct(ref product);
                StringBuilder sb = new StringBuilder();

                if (updateResult == true)
                {
                    sb.Append("Price updated to ");
                    sb.Append(newPrice.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("New RowVersion:");
                }
                else
                {
                    sb.Append("Price not updated to ");
                    sb.Append(newPrice.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("Old RowVersion:");
                }
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }

                result = sb.ToString();
            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message;
            }

            return result;
        }
    }
}
