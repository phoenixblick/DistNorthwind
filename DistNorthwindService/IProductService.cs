﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyWCFServices.DistNorthwindService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        [FaultContract(typeof(ProductFault))]
        Product GetProduct(int id);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateProduct(ref Product product);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "DistNorthwindService.ContractType".
    [DataContract]
    public class Product
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string QuantityPerUnit { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public bool Discontinued { get; set; }

        [DataMember]
        public Byte[] RowVersion { get; set; }
    }

    [DataContract]
    public class ProductFault
    {
        public ProductFault(string msg)
        {
            FaultMessage = msg;
        }
        [DataMember]
        public string FaultMessage;
    }
}
