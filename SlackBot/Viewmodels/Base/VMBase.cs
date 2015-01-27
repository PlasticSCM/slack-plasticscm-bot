using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SlackBot.Viewmodels.Base
{

    public class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var tmpHandler = PropertyChanged;
            if (tmpHandler != null)
            {
                tmpHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
