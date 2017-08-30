using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;

namespace Holy_Bible.Domain
{
    public class Verse : INotifyPropertyChanged
    {

        public Int32 number { get; set; }
        public string text { get; set; }
        public Int32 chapterNumber { get; set; }
        public string bookAcronym { get; set; }
        public string bookName { get; set; }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(bookAcronym);
                sb.Append(" ");
                sb.Append(chapterNumber);
                sb.Append(":");
                sb.Append(number);

                return sb.ToString();
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
