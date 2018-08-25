using Emotion.Web.Models;
using Emotion.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Emotion.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        string serverFolderPath;
        EmotionHelper emoHelper;
        string key;
        EmotionWebContext db = new EmotionWebContext();

        public EmoUploaderController()
        {
            
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            key = ConfigurationManager.AppSettings["EMOTION_API_KEY"];
            emoHelper = new EmotionHelper(key);
        }

        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(HttpPostedFileBase file)
        {
            if (file?.ContentLength>0)
            {
                var pictureName = Guid.NewGuid().ToString();
                pictureName += Path.GetExtension(file.FileName);

                var route = Server.MapPath(serverFolderPath);//optiene la ruta absoluta de la carpeta
                if (!Directory.Exists(route))
                {
                    Directory.CreateDirectory(route);
                }
                route = route + "/" + pictureName;

                
                file.SaveAs(@route);

              var emoPicture= await  emoHelper.DetectAndExtractFacesAsync(file.InputStream);

                emoPicture.Name = file.FileName;
                emoPicture.Path = $"{serverFolderPath}/{pictureName}";
                using (EmotionWebContext database = new EmotionWebContext())
                {
                    db.EmoPictures.Add(emoPicture);
                    await db.SaveChangesAsync();
                }
                    

                return RedirectToAction("Details", "EmoPictures", new { Id = emoPicture.Id });
            }
            return View();
        }
    }
}