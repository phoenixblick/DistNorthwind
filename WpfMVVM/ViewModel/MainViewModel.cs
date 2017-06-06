using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMVVM.Command;

namespace WpfMVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private string _productID1;
        public string ProductID1
        {
            get { return _productID1; }
            set
            {
                _productID1 = value;
                OnPropertyChanged("ProductID1");
            }
        }

        private string _productID2;
        public string ProductID2
        {
            get { return _productID2; }
            set
            {
                _productID2 = value;
                OnPropertyChanged("ProductID2");
            }
        }

        //Product1Details
        private string _product1Details;
        public string Product1Details
        {
            get { return _product1Details; }
            set
            {
                _product1Details = value;
                OnPropertyChanged("Product1Details");
            }
        }

        //Product2Details
        private string _product2Details;
        public string Product2Details
        {
            get { return _product2Details; }
            set
            {
                _product2Details = value;
                OnPropertyChanged("Product2Details");
            }
        }

        //NewPrice1
        private string _newPrice1;
        public string NewPrice1
        {
            get { return _newPrice1; }
            set
            {
                _newPrice1 = value;
                OnPropertyChanged("NewPrice1");
            }
        }

        //NewPrice2
        private string _newPrice2;
        public string NewPrice2
        {
            get { return _newPrice2; }
            set
            {
                _newPrice2 = value;
                OnPropertyChanged("NewPrice2");
            }
        }

        //Update1Results
        private string _update1Results;
        public string Update1Results
        {
            get { return _update1Results; }
            set
            {
                _update1Results = value;
                OnPropertyChanged("Update1Results");
            }
        }

        //Update2Results
        private string _update2Results;
        public string Update2Results
        {
            get { return _update2Results; }
            set
            {
                _update2Results = value;
                OnPropertyChanged("Update2Results");
            }
        }
        #endregion

        #region Commands
        private ICommand _getProductCommand;

        public ICommand GetProductCommand
        {
            get { return _getProductCommand; }
        }

        private ICommand _updatePriceCommand;

        public ICommand UpdatePriceCommand
        {
            get { return _updatePriceCommand; }
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            _getProductCommand = new RelayCommand(p => this.GetProduct(), null);
            _updatePriceCommand = new RelayCommand(p => this.UpdatePrice(), null);
        }
        #endregion

        #region Helpers
        private void GetProduct()
        {
            int productID1 = Int32.Parse(ProductID1);
            int productID2 = Int32.Parse(ProductID2);
                       
            Product1Details = _controller.GetProduct1(productID1);
            Product2Details = _controller.GetProduct2(productID2);
        }

        private void UpdatePrice()
        {
            decimal price1 = Decimal.Parse(NewPrice1);
            decimal price2 = Decimal.Parse(NewPrice2);
            var tuple = _controller.UpdatePrices(price1, price2);
            Update1Results = tuple.Item1;
            Update2Results = tuple.Item2;
        }
        #endregion
    }
}
