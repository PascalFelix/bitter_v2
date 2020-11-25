using bitter_v2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bitter_v2.Views
{
    class ListContentView : ContentView
    {
        private BaseListModel _baseListModel = null;
        public BaseListModel BaseListModel
        {
            get { return _baseListModel; }
            set
            {
                _baseListModel = value;
            }
        }

        public ListContentView(BaseListModel baseListModel)
        {
            _baseListModel = baseListModel;
        }

    }
}
