using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emotion.Web.Models
{
    public class EmoEmotion
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public int EmoFaceId { get; set; }
        public EmoEmotionEnum EmotionType { get; set; }


        public virtual EmoFace Face { get; set; } 
    }
}