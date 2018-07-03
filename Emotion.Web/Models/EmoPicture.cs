using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Emotion.Web.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }


        public virtual ObservableCollection<EmoFace> Faces { get; set; }
    }
}