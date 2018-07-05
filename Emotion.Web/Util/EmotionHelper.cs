using Emotion.Web.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Emotion.Web.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient emoClient;


        public EmotionHelper(string key)
        {
            emoClient = new EmotionServiceClient(key);
        }


        public async Task<EmoPicture> DetectAndExtracFacesAsync(Stream imageStream)
        {
           Microsoft.ProjectOxford.Common.Contract.Emotion[] emotions= await emoClient.RecognizeAsync(imageStream);

            var emoPicture = new EmoPicture();

            emoPicture.Faces = ExtacFaces(emotions,emoPicture);

            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtacFaces(Microsoft.ProjectOxford.Common.Contract.Emotion[] emotions,
            EmoPicture emoPicture)
        {
            var listaFaces = new ObservableCollection<EmoFace>();

            foreach (var emotion in emotions)
            {
                var emoface = new EmoFace();
                emoPicture.Faces.Add(new EmoFace()
                {
                     X=emotion.FaceRectangle.Left,
                     Y=emotion.FaceRectangle.Top,
                     Width=emotion.FaceRectangle.Width,
                     Height= emotion.FaceRectangle.Height,
                     Picture=emoPicture

                });
                emoface.Emotions = ProcessEmotions(emotion.Scores,emoface);
                listaFaces.Add(emoface);

            }
            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores,
            EmoFace emoFace)
        {
            var emotionList = new ObservableCollection<EmoEmotion>();

            //Extraer las propiedades de los Scores
            //BindingFlags especifica el tipo de propiedades que extraera
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance );

            //optine todas las propiedades de tipo float
            var filterProperties = properties.Where(p => p.PropertyType == typeof(float));

            var emotype = EmoEmotionEnum.Undetermined;
            foreach (var prop in filterProperties)
            {
                //intenta parsear la propiedad que viene del servicio en el tipo enum
                if(!Enum.TryParse<EmoEmotionEnum>(prop.Name,out emotype)){//prop.Name nombre de la propiedad, donde se guardara el resultado
                    emotype = EmoEmotionEnum.Undetermined;
                }


                var emoEmotion = new EmoEmotion();
                emoEmotion.Score =(float) prop.GetValue(scores);
                emoEmotion.EmotionType = emotype;
                emoEmotion.Face = emoFace;

                emotionList.Add(emoEmotion);
            }

            return emotionList;
        }
    }
}