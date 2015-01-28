using System.ComponentModel;
using System.Runtime.CompilerServices;

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
