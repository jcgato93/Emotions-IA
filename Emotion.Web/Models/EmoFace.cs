using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Emotion.Web.Models
{
    public class EmoFace
    {
        public int Id { get; set; }
        public int EmoPictureId { get; set; }

        #region
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        #endregion

        public virtual EmoPicture Picture { get; set; }

        public virtual ObservableCollection<EmoEmotion> Emotions { get; set; }
    }
}